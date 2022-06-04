using Play.Common.Models;

namespace Play.Invetory.Service.Models
{
    public class CatalogItem : IEntity
    {
        public CatalogItem()
        {

        }
        public CatalogItem(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
