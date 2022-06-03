using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Repository;

namespace Play.Catalog.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        public ItemRepository repo { get; }

        public ItemsController(ItemRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok((await this.repo.GetAll()).Select(x => x.AsDto()));

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute]Guid Id) 
        {
            var item = await this.repo.GetById(Id);
            return item == null ? BadRequest() : Ok(item.AsDto());
        }
        [HttpPost]
        public async Task<IActionResult> PostItem(CreateItemDto createItemDto)
        {
            var item = new Models.Item(createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.UtcNow);
            await this.repo.Create(item);
            return CreatedAtAction(nameof(GetById),new {id = item.Id}, item.AsDto());
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put([FromRoute]Guid Id,[FromBody]UpdateItemDto updateItemDto)
        {
            var item = await this.repo.GetById(Id);
            if (item == null) return BadRequest();
            item.Name = updateItemDto.Name;
            item.Description = updateItemDto.Description;
            item.Price = updateItemDto.Price;
            item = await this.repo.Update(item);
            return item == null? BadRequest() : Ok(item);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid Id)
        {
            return await this.repo.Delete(Id) ? Ok() : BadRequest();
        }

    }
}
