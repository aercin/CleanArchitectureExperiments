using Application.TodoItems.Queries.Models;
using System.Collections.Generic;

namespace Application.TodoLists.Queries.Models
{
    public class TodoListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Colour { get; set; }
        public IList<TodoItemDto> Items { get; set; }
    }
}
