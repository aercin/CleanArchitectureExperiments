using Application.Common.Behaviours;
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
    public class UnhandledExceptionBehaviourTests
    {
        [Test]
        public async Task Handle_WhenExceptionIsOccuredIsLogErrorCalled_VerifyLogErrorIsCalled()
        {
            //Arrange
            Mock<ILogger<IRequest<Result>>> mockLogger = new Mock<ILogger<IRequest<Result>>>();

            Mock<IHttpContextAccessor> mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());

            Mock<RequestHandlerDelegate<Result>> mockRequestHandlerDelegate = new Mock<RequestHandlerDelegate<Result>>();
            mockRequestHandlerDelegate.Setup(x => x()).Throws<Exception>();

            var unhandledExceptionBehaviourObj = new UnhandledExceptionBehaviour<IRequest<Result>, Result>(mockLogger.Object, mockHttpContextAccessor.Object);

            //Act
            try
            {
                await unhandledExceptionBehaviourObj.Handle(null, new System.Threading.CancellationToken(), mockRequestHandlerDelegate.Object);
            }
            catch
            {

            }

            //Assert
            mockLogger.Verify(m => m.Log(
                  It.Is<LogLevel>(x => x == LogLevel.Error),
                  It.IsAny<EventId>(),
                  It.Is<It.IsAnyType>((v, t) => true),
                  It.IsAny<Exception>(),
                  It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
        }
    }
}
