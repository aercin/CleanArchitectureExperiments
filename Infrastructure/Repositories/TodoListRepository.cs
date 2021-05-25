using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public class TodoListRepository : GenericRepository<TodoList>, ITodoListRepository
    {
        public TodoListRepository(ApplicationDbContext context) : base(context)
        { 
        }
    }
}
