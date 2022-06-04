using MassTransit;
using Play.Catalog.Contracts;
using Play.Common.Repository;
using Play.Invetory.Service.Models;

namespace Play.Invetory.Service.Cosumers
{
    public class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
    {
        public IRepository<CatalogItem> Repository { get; }

        public CatalogItemDeletedConsumer(IRepository<CatalogItem> repository)
        {
            Repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {
            context.Message.Deconstruct(out Guid Id);
            if (await Repository.GetByIdAsync(Id) == null) return;
            await this.Repository.DeleteAsync(Id);
        }
    }
}
