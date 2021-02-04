using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarsRoverPhotos.Domain.Interface
{
    public interface ICommand<TResult>
    {
    }

    public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult> 
    {
        Task<TResult> HandleProcess(TCommand command);
    }

}
