using Application.TodoItems.Queries.Models;
using Application.TodoLists.Queries.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TodoItem, TodoItemDto>().ForMember(dest => dest.Priority, opt => opt.MapFrom(src => (int)src.Priority));
            CreateMap<TodoList, TodoListDto>().ForMember(dest => dest.Colour, opt => opt.MapFrom(src => src.Colour.Code));
        }
    }
}
