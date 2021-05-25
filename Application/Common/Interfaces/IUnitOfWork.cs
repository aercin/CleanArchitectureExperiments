using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IUnitOfWork :IDisposable
    {
        ITodoItemRepository TodoItems { get; }
        ITodoListRepository TodoLists { get; }
        Task<int> CompleteAsync(CancellationToken cancellationToken);
    }
}
