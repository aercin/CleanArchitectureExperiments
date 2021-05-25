using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TodoItems.Commands.UpdateTodoItem
{
    public class UpdateTodoItemCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Done { get; set; }
    }

    public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTodoItemCommand,Result>
    {
        private readonly IUnitOfWork _uow;

        public UpdateTodoItemCommandHandler(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public async Task<Result> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
        {
            var entity = this._uow.TodoItems.Find(x => x.Id == request.Id).SingleOrDefault();

            if (entity == null)
            {
                throw new NotFoundException(nameof(TodoItem), request.Id);
            }

            entity.Title = request.Title;
            entity.Done = request.Done;

            await this._uow.CompleteAsync(cancellationToken);

            return Result.Success();
        }
    }
}
