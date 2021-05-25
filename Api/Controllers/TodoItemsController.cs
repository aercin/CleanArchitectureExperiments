using Application.Common.Models;
using Application.TodoItems.Commands.CreateTodoItem;
using Application.TodoItems.Commands.DeleteTodoItem;
using Application.TodoItems.Commands.UpdateTodoItem;
using Application.TodoItems.Commands.UpdateTodoItemDetail;
using Application.TodoItems.Queries.GetTodoItemsWithPagination;
using Application.TodoItems.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ISender _mediator;

        public TodoItemsController(ISender mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Result<int>>> Create(CreateTodoItemCommand command)
        { 
            return await this._mediator.Send(command);
        } 

        [HttpPut()]
        public async Task<ActionResult<Result>> Update(UpdateTodoItemCommand command)
        {
            return await this._mediator.Send(command); 
        }

        [HttpPut("[action]")]
        public async Task<ActionResult<Result>> UpdateItemDetails(UpdateTodoItemDetailCommand command)
        {
            return await this._mediator.Send(command); 
        }

        [HttpDelete()]
        public async Task<ActionResult<Result>> Delete(int id)
        {
            return await this._mediator.Send(new DeleteTodoItemCommand { Id = id }); 
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<TodoItemDto>>> Get([FromQuery]GetTodoItemsWithPaginationQuery query)
        {
            return await this._mediator.Send(query);
        }
    }
}
