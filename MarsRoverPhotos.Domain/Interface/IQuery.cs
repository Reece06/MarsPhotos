using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRoverPhotos.Domain.Interface
{
    public interface IQuery<TResult> {}

    public interface IQueryHandler<TQuery,TResult> where TQuery : IQuery<TResult>
    {
        TResult HandleQuery(TQuery query);
    }
}
