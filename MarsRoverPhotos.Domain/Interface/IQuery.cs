using System.Threading.Tasks;

namespace MarsRoverPhotos.Domain.Interface
{
    public interface IQuery<TResult> {}

    public interface IQueryHandlerAsync<TQuery,TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleQuery(TQuery query);
    }

    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        TResult HandleQuery(TQuery query);
    }
}
