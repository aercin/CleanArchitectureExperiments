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
    public class TodoItemCompletedEventHandlerTests
    {

        [Test]
        public async Task Handle_LogInformationMethodIsCalledOnce_VerifyCallingOfLogInformationMethod()
        {
            //Arrange
            var mockNotification = new Mock<DomainEventNotification<TodoItemCompletedEvent>>(Mock.Of<TodoItemCompletedEvent>());
            var mockLogger = new Mock<ILogger<TodoItemCompletedEventHandler>>();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var dummyHttpContext = new DefaultHttpContext();
            dummyHttpContext.Request.Headers["TrackId"] = "dummyTrackId";

            mockHttpContextAccessor.Setup(m => m.HttpContext).Returns(dummyHttpContext);

            var todoItemCompletedEventHandlerObj = new TodoItemCompletedEventHandler(mockHttpContextAccessor.Object, mockLogger.Object);

            //Act
            await todoItemCompletedEventHandlerObj.Handle(mockNotification.Object, new System.Threading.CancellationToken());

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
