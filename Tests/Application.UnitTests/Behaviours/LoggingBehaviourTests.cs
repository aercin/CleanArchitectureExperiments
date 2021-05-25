using Application.Common.Behaviours;
using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Application.UnitTests.Behaviours
{
    [TestFixture]
    public class LoggingBehaviourTests
    {
        Mock<ILogger<IRequest<Result>>> mockLogger;
        Mock<IHttpContextAccessor> mockHttpContextAccessor;
        Mock<RequestHandlerDelegate<Result>> mockRequestHandlerDelegate;
        LoggingBehaviour<IRequest<Result>, Result> logginBehaviourObj;

        [SetUp]
        public void Setup()
        {
            //Arrange
            mockLogger = new Mock<ILogger<IRequest<Result>>>();

            mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(m => m.HttpContext).Returns(new DefaultHttpContext());

            mockRequestHandlerDelegate = new Mock<RequestHandlerDelegate<Result>>();

            logginBehaviourObj = new LoggingBehaviour<IRequest<Result>, Result>(mockLogger.Object, mockHttpContextAccessor.Object);
        }

        [Test]
        public async Task Handle_LogInformationMethodIsCalledAtLeastOnce_VerifyCallingOfLogInformationMethod()
        {
            //Act
            var result = await logginBehaviourObj.Handle(null, new System.Threading.CancellationToken(), mockRequestHandlerDelegate.Object);

            //Assert
            mockLogger.Verify(m => m.Log(
                  It.Is<LogLevel>(x => x == LogLevel.Information),
                  It.IsAny<EventId>(),
                  It.Is<It.IsAnyType>((v, t) => true),
                  It.IsAny<Exception>(),
                  It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.AtLeastOnce);
        }

        [Test]
        public async Task Handle_DelegateFuncIsCalledOnce_VerifyCallingOfDelegateFunc()
        {
            //Act
            var result = await logginBehaviourObj.Handle(null, new System.Threading.CancellationToken(), mockRequestHandlerDelegate.Object);

            //Assert
            mockRequestHandlerDelegate.Verify(m => m(), Times.Once);
        }

        [Test]
        public async Task Handle_RequestHeadersHasTrackId_ReturnTrue()
        {
            //Act
            await logginBehaviourObj.Handle(null, new System.Threading.CancellationToken(), mockRequestHandlerDelegate.Object);

            //Assert 
            Assert.IsTrue(mockHttpContextAccessor.Object.HttpContext.Request.Headers.ContainsKey("TrackId"));
        }
    }
}
