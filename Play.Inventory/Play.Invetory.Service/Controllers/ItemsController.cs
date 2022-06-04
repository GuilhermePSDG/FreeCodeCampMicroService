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
        private readonly IRepository<InventoryItem> inventoryRepo;
        private readonly IRepository<CatalogItem> catalogRepo;

        public ItemsController(IRepository<InventoryItem> inventoryRepo,IRepository<CatalogItem> catalogRepo)
        {
            this.inventoryRepo = inventoryRepo;
            this.catalogRepo = catalogRepo;
        }
        [HttpGet]   
        public async Task<IActionResult> GetAll(Guid UserId)
        {
            if (UserId == Guid.Empty) return Unauthorized();
            var inventoryItems = await this.inventoryRepo.GetAllAsync(x => x.UserId == UserId);

            var catalogItemIds = inventoryItems.Select(x => x.CatalogItemId);
            var catalogItems = await this.catalogRepo.GetAllAsync(x => catalogItemIds.Contains(x.Id));
            
            var dtos = inventoryItems.Select(x =>
            {
                var catalogItem = catalogItems.Single(ci => ci.Id == x.CatalogItemId);
                return x.AsDto(catalogItem.Name, catalogItem.Description);
            });
            return Ok(dtos);
        }
        [HttpPost]
        public async Task<IActionResult> Post(GrantItemsDto item)
        {
            var itemR = await this.inventoryRepo.GetAsync(x => x.UserId == item.UserId && x.CatalogItemId == item.CatalogItemId);
            if (itemR == null)
            {
                itemR = new InventoryItem(item.UserId, item.CatalogItemId, item.Quantity);
                await this.inventoryRepo.CreateAsync(itemR);
            }
            else
            {
                itemR.Quantity += item.Quantity;
                itemR = await this.inventoryRepo.UpdateAsync(itemR);
            }
            return Ok(itemR);
        }

    }
}
