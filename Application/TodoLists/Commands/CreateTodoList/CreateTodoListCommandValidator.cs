using Application.Common.Interfaces;
using FluentValidation;
using System.Linq;

namespace Application.TodoLists.Commands.CreateTodoList
{
    public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
    {
        private readonly IUnitOfWork _uow;

        public CreateTodoListCommandValidator(IUnitOfWork uow)
        {
            this._uow = uow;

            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required")
                                 .MaximumLength(200).WithMessage("Title must not exceed 200 characters")
                                 .Must(BeUniqueTitle).WithMessage("The specified title already exists");
        }

        public bool BeUniqueTitle(string title)
        {
            return !this._uow.TodoLists.Find(x => x.Title == title, isNoTracking: true).Any();
        }
    }
}
