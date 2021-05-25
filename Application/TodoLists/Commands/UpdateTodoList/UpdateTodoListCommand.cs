using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TodoLists.Commands.UpdateTodoList
{
    public class UpdateTodoListCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class UpdateTodoListCommandHandler : IRequestHandler<UpdateTodoListCommand, Result>
    {
        private readonly IUnitOfWork _uow;

        public UpdateTodoListCommandHandler(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public async Task<Result> Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
        {
            var entity = this._uow.TodoLists.Find(x => x.Id == request.Id).SingleOrDefault();
            if (entity == null)
            {
                throw new NotFoundException(nameof(TodoList), request.Id);
            }

            entity.Title = request.Title;

            await this._uow.CompleteAsync(cancellationToken);

            return Result.Success();
        }
    }
}
