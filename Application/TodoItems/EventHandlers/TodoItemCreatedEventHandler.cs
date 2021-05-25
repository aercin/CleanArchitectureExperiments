using Application.Common.Models;
using Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TodoItems.EventHandlers
{
    public class TodoItemCreatedEventHandler : INotificationHandler<DomainEventNotification<TodoItemCreatedEvent>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<TodoItemCreatedEventHandler> _logger;

        public TodoItemCreatedEventHandler(IHttpContextAccessor  httpContextAccessor, ILogger<TodoItemCreatedEventHandler> logger)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._logger = logger;
        }

        public Task Handle(DomainEventNotification<TodoItemCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            this._logger.LogInformation($"TrackId: { this._httpContextAccessor.HttpContext.Request.Headers["TrackId"]} Domain Event: {domainEvent.GetType().Name}");

            return Task.CompletedTask;
        }
    }
}
