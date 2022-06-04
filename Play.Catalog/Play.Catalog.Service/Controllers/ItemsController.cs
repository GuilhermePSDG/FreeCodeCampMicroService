using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Contracts;
using Play.Catalog.Service.Models;
using Play.Common.Repository;

namespace Play.Catalog.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> repo;
        private readonly IPublishEndpoint publishEndpoint;
        public ItemsController(IRepository<Item> repo, IPublishEndpoint publishEndpoint)
        {
            this.repo = repo;
            this.publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok((await repo.GetAllAsync()).Select(x => x.AsDto()));

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var item = await repo.GetByIdAsync(Id);
            return item == null ? BadRequest() : Ok(item.AsDto());
        }
        [HttpPost]
        public async Task<IActionResult> PostItem(CreateItemDto createItemDto)
        {
            var item = new Models.Item(createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.UtcNow);
            await repo.CreateAsync(item);
            await publishEndpoint.Publish(new CatalogItemCreated(item.Id, item.Name, item.Description));
            
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item.AsDto());
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put([FromRoute] Guid Id, [FromBody] UpdateItemDto updateItemDto)
        {
            var item = await repo.GetByIdAsync(Id);
            if (item == null) return BadRequest();
            item.Name = updateItemDto.Name;
            item.Description = updateItemDto.Description;
            item.Price = updateItemDto.Price;
            item = await repo.UpdateAsync(item);
            if(item != null)
            {
                await publishEndpoint.Publish(new CatalogItemUpdated(item.Id, item.Name, item.Description));
                return Ok(item);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid Id)
        {
            if(await repo.DeleteAsync(Id))
            {
                await publishEndpoint.Publish(new CatalogItemDeleted(Id));
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
