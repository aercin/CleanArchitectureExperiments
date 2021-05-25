using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public class TodoItemRepository : GenericRepository<TodoItem>, ITodoItemRepository
    { 
        public TodoItemRepository(ApplicationDbContext context) : base(context)
        { 
        } 
    }
}
