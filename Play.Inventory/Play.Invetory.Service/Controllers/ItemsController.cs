using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Play.Common.Repository;
using Play.Invetory.Service.Clients;
using Play.Invetory.Service.Dtos;
using Play.Invetory.Service.Models;
using System.Diagnostics;

namespace Play.Invetory.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<InventoryItem> repo;
        private readonly CatalogClient client;

        public ItemsController(IRepository<InventoryItem> repo,CatalogClient client)
        {
            this.repo = repo;
            this.client = client;
        }
        [HttpGet]   
        public async Task<IActionResult> GetAll(Guid UserId)
        {
            if (UserId == Guid.Empty) return Unauthorized();
            var items = await this.repo.GetAllAsync(x => x.UserId == UserId);
            var catalogItems = (await this.client.GetAllAsync());
            var dtos = items.Select(x =>
            {
                var catalogItem = catalogItems.Single(ci => ci.Id == x.CatalogItemId);
                return x.AsDto(catalogItem.Name, catalogItem.Description);
            });
            return Ok(dtos);
        }
        [HttpPost]
        public async Task<IActionResult> Post(GrantItemsDto item)
        {
            var itemR = await this.repo.GetAsync(x => x.UserId == item.UserId && x.CatalogItemId == item.CatalogItemId);
            if (itemR == null)
            {
                itemR = new InventoryItem(item.UserId, item.CatalogItemId, item.Quantity);
                await this.repo.CreateAsync(itemR);
            }
            else
            {
                itemR.Quantity += item.Quantity;
                itemR = await this.repo.UpdateAsync(itemR);
            }
            return Ok(itemR);
        }

    }
}
