using Application.Common.Behaviours;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Application.UnitTests.Behaviours
{
    [TestFixture]
    public class PerformanceBehaviourTests
    {
        Mock<ILogger<IRequest<Result>>> mockLogger;
        Mock<IHttpContextAccessor> mockHttpContextAccessor;
        Mock<RequestHandlerDelegate<Result>> mockRequestHandlerDelegate;
        Mock<ITimer> mockTimer;

        [SetUp]
        public void Setup()
        {
            //Arrange
            mockLogger = new Mock<ILogger<IRequest<Result>>>();

            mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(m => m.HttpContext).Returns(new DefaultHttpContext());

            mockRequestHandlerDelegate = new Mock<RequestHandlerDelegate<Result>>();

            mockTimer = new Mock<ITimer>();
        }

        [Test]
        public async Task Handle_ExecutionTimeIsExceedTo500Miliseconds_VerifyLogWarningIsCalled()
        {
            //Arrange
            mockTimer.Setup(timer => timer.Stop()).Returns(501);

            var performanceBehaviourObj = new PerformanceBehaviour<IRequest<Result>, Result>(mockLogger.Object, mockHttpContextAccessor.Object, mockTimer.Object);

            //Act
            await performanceBehaviourObj.Handle(null, new System.Threading.CancellationToken(), mockRequestHandlerDelegate.Object);

            //Assert
            mockLogger.Verify(m => m.Log(
                 It.Is<LogLevel>(x => x == LogLevel.Warning),
                 It.IsAny<EventId>(),
                 It.Is<It.IsAnyType>((v, t) => true),
                 It.IsAny<Exception>(),
                 It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
        }

        [Test]
        public async Task Handle_ExecutionTimeLowerThan500Miliseconds_VerifyLogWarningIsNeverCalled()
        {
            //Arrange
            mockTimer.Setup(timer => timer.Stop()).Returns(499);

            var performanceBehaviourObj = new PerformanceBehaviour<IRequest<Result>, Result>(mockLogger.Object, mockHttpContextAccessor.Object, mockTimer.Object);

            //Act
            await performanceBehaviourObj.Handle(null, new System.Threading.CancellationToken(), mockRequestHandlerDelegate.Object);

            //Assert
            mockLogger.Verify(m => m.Log(
                 It.Is<LogLevel>(x => x == LogLevel.Warning),
                 It.IsAny<EventId>(),
                 It.Is<It.IsAnyType>((v, t) => true),
                 It.IsAny<Exception>(),
                 It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Never);
        }
    }
}
