using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TodoItems.Commands.UpdateTodoItemDetail
{
    public class UpdateTodoItemDetailCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public int ListId { get; set; }
        public PriorityLevel Priority { get; set; }
        public string Note { get; set; }
    }

    public class UpdateTodoItemDetailCommandHandler : IRequestHandler<UpdateTodoItemDetailCommand,Result>
    {
        private readonly IUnitOfWork _uow;

        public UpdateTodoItemDetailCommandHandler(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public async Task<Result> Handle(UpdateTodoItemDetailCommand request, CancellationToken cancellationToken)
        {
            var entity = this._uow.TodoItems.Find(x => x.Id == request.Id).SingleOrDefault();
            if (entity == null)
            {
                throw new NotFoundException(nameof(TodoItem), request.Id);
            }

            entity.ListId = request.ListId;
            entity.Priority = request.Priority;
            entity.Note = request.Note;

            await this._uow.CompleteAsync(cancellationToken);

            return Result.Success();
        }
    }
}
