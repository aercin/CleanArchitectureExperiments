using Application.Common.Interfaces;
using Application.TodoItems.Commands.CreateTodoItem;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Application.UnitTests.TodoItems.Commands
{
    [TestFixture]
    public class CreateTodoItemCommandTests
    {
        [Test]
        public async Task Handle_CreationOfTodoItemSuccessfully_ResultCodeMustBeTrue()
        {
            //Arrange
            Mock<IUnitOfWork> mockUow = new Mock<IUnitOfWork>(); 

            Mock<ITodoItemRepository> mockTodoItemRepo = new Mock<ITodoItemRepository>(); 
            mockUow.Setup(m => m.TodoItems).Returns(mockTodoItemRepo.Object); 

            var createTodoItemCommandHandlerObj = new CreateTodoItemCommandHandler(mockUow.Object);

            //Act
            var result = await createTodoItemCommandHandlerObj.Handle(new CreateTodoItemCommand { ListId = 1, Title = "dummyTitle" }, new System.Threading.CancellationToken());

            //Assert
            Assert.IsTrue(result.Succeeded);
        }
    }
}
