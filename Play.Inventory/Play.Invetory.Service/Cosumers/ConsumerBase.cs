using MassTransit;
using Play.Common.Models;
using Play.Common.Repository;

namespace Play.Invetory.Service.Cosumers
{
    //public class ConsumerBase<T> : IConsumer<T> where T : IEntity
    //{
    //    private readonly IRepository<T> repo;

    //    public ConsumerBase(IRepository<T> repo,IMapper)
    //    {
    //        this.repo = repo;
    //    }
    //    public enum Method{
    //        Create,
    //        Update,
    //        Delete
    //    }
    //    public ConsumerBase()
    //    {

    //    }
    //    public virtual void Create()
    //    {

    //    }
    //    public virtual void Update()
    //    {

    //    }
    //    public virtual void Delete()
    //    {

    //    }

    //    public Task Consume(ConsumeContext<T> context)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
