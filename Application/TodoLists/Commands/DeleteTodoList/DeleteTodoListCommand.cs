using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TodoLists.Commands.DeleteTodoList
{
    public class DeleteTodoListCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteTodoListCommandHandler : IRequestHandler<DeleteTodoListCommand,Result>
    {
        private readonly IUnitOfWork _uow;

        public DeleteTodoListCommandHandler(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public async Task<Result> Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
        {
            var entity = this._uow.TodoLists.Find(x => x.Id == request.Id).SingleOrDefault();

            if (entity == null)
            {
                throw new NotFoundException(nameof(TodoList), request.Id);
            }

            this._uow.TodoLists.Remove(entity);

            await this._uow.CompleteAsync(cancellationToken);

            return Result.Success();
        }
    }
}
