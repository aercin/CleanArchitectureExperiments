using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.TodoItems.Queries.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TodoItems.Queries.GetTodoItemsWithPagination
{
    public class GetTodoItemsWithPaginationQuery : IRequest<PaginatedList<TodoItemDto>>
    {
        public int ListId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetTodoItemsWithPaginationQueryHandler : IRequestHandler<GetTodoItemsWithPaginationQuery, PaginatedList<TodoItemDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetTodoItemsWithPaginationQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this._uow = uow;
            this._mapper = mapper;
        }

        public Task<PaginatedList<TodoItemDto>> Handle(GetTodoItemsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var result = this._uow.TodoItems.Find(x => x.ListId == request.ListId, isNoTracking: true)
                                                  .ProjectTo<TodoItemDto>(this._mapper.ConfigurationProvider)
                                                  .MappedPaginatedList<TodoItemDto>(request.PageNumber, request.PageSize);

            return Task.FromResult(result);
        }
    }
}
