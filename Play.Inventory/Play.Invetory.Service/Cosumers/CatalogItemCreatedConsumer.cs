using MassTransit;
using Play.Catalog.Contracts;
using Play.Common.Repository;
using Play.Invetory.Service.Models;

namespace Play.Invetory.Service.Cosumers
{
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {
        public IRepository<CatalogItem> Repository { get; }

        public CatalogItemCreatedConsumer(IRepository<CatalogItem> repository)
        {
            Repository = repository;
        }
        public async Task Consume(ConsumeContext<CatalogItemCreated> context)
        {
            context.Message.Deconstruct(out Guid Id, out string Name, out string Decription);
            var item = await this.Repository.GetByIdAsync(Id);
            var newItem = new CatalogItem(Id, Name, Decription);
            if (item == null)
            {
                await this.Repository.CreateAsync(newItem);
            }
            else if (!item.Equals(newItem))
            {
                await this.Repository.UpdateAsync(newItem);
            }

        }
    }
}
