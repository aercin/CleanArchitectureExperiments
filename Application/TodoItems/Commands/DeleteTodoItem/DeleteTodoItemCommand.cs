using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TodoItems.Commands.DeleteTodoItem
{
    public class DeleteTodoItemCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand, Result>
    {
        private readonly IUnitOfWork _uow;

        public DeleteTodoItemCommandHandler(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public async Task<Result> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
        {
            var entity = this._uow.TodoItems.Find(x => x.Id == request.Id).SingleOrDefault();

            if (entity == null)
            {
                throw new NotFoundException(nameof(TodoItem), request.Id);
            }

            this._uow.TodoItems.Remove(entity);

            await this._uow.CompleteAsync(cancellationToken);

            return Result.Success();
        }
    }
}
