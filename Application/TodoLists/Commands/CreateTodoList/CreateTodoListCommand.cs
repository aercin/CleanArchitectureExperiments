using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TodoLists.Commands.CreateTodoList
{
    public class CreateTodoListCommand : IRequest<Result<int>>
    {
        public string Title { get; set; }
    }

    public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, Result<int>>
    {
        private readonly IUnitOfWork _uow;

        public CreateTodoListCommandHandler(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public async Task<Result<int>> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
        {
            var entity = new TodoList();

            entity.Title = request.Title;

            this._uow.TodoLists.Add(entity);

            await this._uow.CompleteAsync(cancellationToken);

            return Result<int>.Success(entity.Id);
        }
    }
}
