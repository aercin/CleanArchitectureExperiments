using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            this._context = context;
            TodoItems = new TodoItemRepository(_context);
            TodoLists = new TodoListRepository(_context);
        }

        public ITodoItemRepository TodoItems { get; private set; }

        public ITodoListRepository TodoLists { get; private set; }

        public async Task<int> CompleteAsync(CancellationToken cancellationToken)
        {
            return await this._context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            this._context.Dispose();
        }
    }
}
