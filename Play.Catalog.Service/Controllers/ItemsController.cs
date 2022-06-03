using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Models;
using Play.Common.Repository;

namespace Play.Catalog.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        public IRepository<Item> repo { get; }

        public ItemsController(IRepository<Item> repo)
        {
            this.repo = repo;
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
            return item == null ? BadRequest() : Ok(item);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid Id)
        {
            return await repo.DeleteAsync(Id) ? Ok() : BadRequest();
        }

    }
}
