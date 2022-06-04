using Play.Invetory.Service.Models;
using Play.Invetory.Service.Dtos;

namespace Play.Invetory.Service
{
    public static class Extensions
    {
        public static InventoryItemDto AsDto(this InventoryItem item,string Name,string Description) => new InventoryItemDto(item.CatalogItemId, Name,Description,item.Quantity, item.AcquiredDate);
    }
}
