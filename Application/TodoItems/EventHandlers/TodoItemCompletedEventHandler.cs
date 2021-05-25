using Application.Common.Models;
using Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TodoItems.EventHandlers
{
    public class TodoItemCompletedEventHandler : INotificationHandler<DomainEventNotification<TodoItemCompletedEvent>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<TodoItemCompletedEventHandler> _logger;

        public TodoItemCompletedEventHandler(IHttpContextAccessor httpContextAccessor, ILogger<TodoItemCompletedEventHandler> logger)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._logger = logger;
        }

        public Task Handle(DomainEventNotification<TodoItemCompletedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            this._logger.LogInformation($"TrackId: {this._httpContextAccessor.HttpContext.Request.Headers["TrackId"]} CleanArchitecture Domain Event: {domainEvent.GetType().Name}");

            return Task.CompletedTask;
        }
    }
}
