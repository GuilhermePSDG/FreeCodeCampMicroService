using MassTransit;
using Play.Catalog.Contracts;
using Play.Common.Repository;
using Play.Invetory.Service.Models;

namespace Play.Invetory.Service.Cosumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
    {
        public IRepository<CatalogItem> Repository { get; }

        public CatalogItemUpdatedConsumer(IRepository<CatalogItem> repository)
        {
            Repository = repository;
        }


        public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
        {
            context.Message.Deconstruct(out Guid Id, out string Name, out string Decription);
            var item = await this.Repository.GetByIdAsync(Id);
            var newItem = new CatalogItem(Id, Name, Decription);
            if (item != null)
            {
                await this.Repository.UpdateAsync(newItem);
            }
            else 
            {
                await this.Repository.CreateAsync(newItem);
            }
        }
    }
}
