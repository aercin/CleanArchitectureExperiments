using Application.Common.Models;
using Application.TodoItems.EventHandlers;
using Domain.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Application.UnitTests.TodoItems.EventHandlers
{
    [TestFixture]
    public class TodoItemCreatedEventHandlerTests
    {
        [Test]
        public async Task Handle_LogInformationMethodIsCalledOnce_VerifyCallingOfLogInformationMethod()
        {
            //Arrange
            var mockNotification = new Mock<DomainEventNotification<TodoItemCreatedEvent>>(Mock.Of<TodoItemCreatedEvent>());
            var mockLogger = new Mock<ILogger<TodoItemCreatedEventHandler>>();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var dummyHttpContext = new DefaultHttpContext();
            dummyHttpContext.Request.Headers["TrackId"] = "dummyTrackId";

            mockHttpContextAccessor.Setup(m => m.HttpContext).Returns(dummyHttpContext);

            var todoItemCreatedEventHandlerObj = new TodoItemCreatedEventHandler(mockHttpContextAccessor.Object, mockLogger.Object);

            //Act
            await todoItemCreatedEventHandlerObj.Handle(mockNotification.Object, new System.Threading.CancellationToken());

            //Assert
            mockLogger.Verify(m => m.Log(
                  It.Is<LogLevel>(x => x == LogLevel.Information),
                  It.IsAny<EventId>(),
                  It.Is<It.IsAnyType>((v, t) => true),
                  It.IsAny<Exception>(),
                  It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
        }
    }
}
