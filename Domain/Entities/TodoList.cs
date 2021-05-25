using Domain.Common;
using Domain.ValueObjects;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class TodoList : AuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Colour Colour { get; set; } = Colour.Orange;

        public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();//C# 6 da gelen yetenek - Auto property initializer 
    }
}
