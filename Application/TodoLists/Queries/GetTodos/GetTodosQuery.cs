using Application.Common.Interfaces;
using Application.Common.Models;
using Application.TodoLists.Queries.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TodoLists.Queries.GetTodos
{
    public class GetTodosQuery : IRequest<Result<List<TodoListDto>>>
    {
    }

    public class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, Result<List<TodoListDto>>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetTodosQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this._uow = uow;
            this._mapper = mapper;
        }

        public Task<Result<List<TodoListDto>>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
        {
            var result = this._uow.TodoLists.All(isNoTracking: true)
                                            .ProjectTo<TodoListDto>(this._mapper.ConfigurationProvider);

            var actual_result = result.ToList();
                                       
            return Task.FromResult(Result<List<TodoListDto>>.Success(actual_result));
        }
    }
}
