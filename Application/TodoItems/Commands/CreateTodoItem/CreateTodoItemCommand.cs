using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TodoItems.Commands.CreateTodoItem
{
    public class CreateTodoItemCommand : IRequest<Result<int>>
    {
        public int ListId { get; set; }
        public string Title { get; set; }
    }

    public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, Result<int>>
    {
        private readonly IUnitOfWork _uow;

        public CreateTodoItemCommandHandler(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public async Task<Result<int>> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
        {
            var entity = new TodoItem
            {
                ListId = request.ListId,
                Title = request.Title,
                Done = false
            }; 

            entity.DomainEvents.Add(new TodoItemCreatedEvent(entity));

            this._uow.TodoItems.Add(entity);

            await this._uow.CompleteAsync(cancellationToken);

            var result = Result<int>.Success(entity.Id);

            return result;
        }
    }
}
