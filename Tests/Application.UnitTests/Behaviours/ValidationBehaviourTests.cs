using Application.Common.Behaviours;
using Application.Common.Models;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace Application.UnitTests.Behaviours
{
    [TestFixture]
    public class ValidationBehaviourTests
    {
        Mock<IRequest<Result>> mockRequest;
        Mock<RequestHandlerDelegate<Result>> mockRequestHandlerDelegate;
        Mock<IValidator<IRequest<Result>>> mockValidator;
        ValidationBehaviour<IRequest<Result>, Result> validationBehaviourObj;

        [SetUp]
        public void Setup()
        {
            //Arrange 
            mockRequest = new Mock<IRequest<Result>>();

            mockRequestHandlerDelegate = new Mock<RequestHandlerDelegate<Result>>();

            mockValidator = new Mock<IValidator<IRequest<Result>>>();
        }

        [Test]
        public void Handle_RequestHasValidationButDontSupplyRequirementsOfValidation_IsThrowValidationException()
        {
            //Arrange 
            mockValidator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<IRequest<Result>>>(), new System.Threading.CancellationToken())).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("dummyProperty", "dummyError") }));

            validationBehaviourObj = new ValidationBehaviour<IRequest<Result>, Result>(new List<IValidator<IRequest<Result>>> { mockValidator.Object });

            //Act & Assert
            Assert.ThrowsAsync<Common.Exceptions.ValidationException>(() => validationBehaviourObj.Handle(mockRequest.Object, new System.Threading.CancellationToken(), mockRequestHandlerDelegate.Object));
        }

        [Test]
        public void Handle_RequestHasValidationAndSupplyAllRequirementOfValidation_DoesntThrowAnyValidationException()
        {
            //Arrange 
            mockValidator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<IRequest<Result>>>(), new System.Threading.CancellationToken())).ReturnsAsync(new ValidationResult());

            validationBehaviourObj = new ValidationBehaviour<IRequest<Result>, Result>(new List<IValidator<IRequest<Result>>> { mockValidator.Object });

            //Act & Assert
            Assert.DoesNotThrowAsync(() => validationBehaviourObj.Handle(mockRequest.Object, new System.Threading.CancellationToken(), mockRequestHandlerDelegate.Object));
        }

        [Test]
        public void Handle_RequestHasntAnyValidation_DoesntThrowAnyValidationException()
        { 
            validationBehaviourObj = new ValidationBehaviour<IRequest<Result>, Result>(new List<IValidator<IRequest<Result>>>());

            //Act & Assert
            Assert.DoesNotThrowAsync(() => validationBehaviourObj.Handle(mockRequest.Object, new System.Threading.CancellationToken(), mockRequestHandlerDelegate.Object));
        }
    }
}
