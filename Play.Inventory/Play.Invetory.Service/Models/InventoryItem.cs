using Play.Common.Models;

namespace Play.Invetory.Service.Models
{
    public class InventoryItem : IEntity
    {
        public InventoryItem()
        {

        }
        public InventoryItem(Guid userId, Guid catalogItemId, int quantity)
        {
            this.Id = Guid.NewGuid();
            AcquiredDate = DateTimeOffset.UtcNow;
            UserId = userId;
            CatalogItemId = catalogItemId;
            Quantity = quantity;
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CatalogItemId { get; set; }
        public int Quantity { get; set; }
        public DateTimeOffset AcquiredDate { get; set; }




    }
}
