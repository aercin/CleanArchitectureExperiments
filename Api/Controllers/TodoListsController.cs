using Application.Common.Models;
using Application.TodoLists.Commands.CreateTodoList;
using Application.TodoLists.Commands.DeleteTodoList;
using Application.TodoLists.Commands.UpdateTodoList;
using Application.TodoLists.Queries.GetTodos;
using Application.TodoLists.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListsController : ControllerBase
    {
        private readonly ISender _mediator;

        public TodoListsController(ISender mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Result<int>>> Create(CreateTodoListCommand command)
        {
            return await this._mediator.Send(command);
        }

        [HttpPut()]
        public async Task<ActionResult<Result>> Update(UpdateTodoListCommand command)
        {
            return await this._mediator.Send(command);
        }

        [HttpDelete()]
        public async Task<ActionResult<Result>> Delete(int id)
        {
            return await this._mediator.Send(new DeleteTodoListCommand { Id = id });
        }

        [HttpGet()]
        public async Task<ActionResult<Result<List<TodoListDto>>>> Get([FromQuery] GetTodosQuery query)
        {
            return await this._mediator.Send(query);
        }

    }
}
