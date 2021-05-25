using Application.Common.Interfaces;
using Application.TodoLists.Commands.CreateTodoList;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Application.UnitTests.TodoLists.Commands
{
    [TestFixture]
    public class CreateTodoListCommandTests
    {
        [Test]
        public async Task Handle_CreationOfTodoListSuccessfully_ResultCodeMustBeTrue()
        {
            //Arrange
            Mock<IUnitOfWork> mockUow = new Mock<IUnitOfWork>();

            Mock<ITodoListRepository> mockTodoListRepo = new Mock<ITodoListRepository>();
            mockUow.Setup(m => m.TodoLists).Returns(mockTodoListRepo.Object);

            var mockCreateTodoListCommand = Mock.Of<CreateTodoListCommand>();

            var createTodoListCommandHandlerObj = new CreateTodoListCommandHandler(mockUow.Object);

            //Act
            var result = await createTodoListCommandHandlerObj.Handle(mockCreateTodoListCommand, new System.Threading.CancellationToken());

            //Assert
            Assert.IsTrue(result.Succeeded);
        }
    }
}
