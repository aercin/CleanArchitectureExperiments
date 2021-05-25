using Api.Controllers;
using Application.TodoLists.Commands.CreateTodoList;
using Application.TodoLists.Commands.DeleteTodoList;
using Application.TodoLists.Commands.UpdateTodoList;
using Application.TodoLists.Queries.GetTodos;
using MediatR;
using Moq;
using NUnit.Framework;

namespace Api.UnitTests
{
    [TestFixture]
    public class TodoListsControllerTests
    {
        Mock<ISender> mockSender;
        TodoListsController todoListsControllerObj;

        [SetUp]
        public void Setup()
        {
            //Arrange
            mockSender = new Mock<ISender>();
            todoListsControllerObj = new TodoListsController(mockSender.Object);
        }

        [Test]
        public void Create_SendMethodOfMediatorIsCalled_VerifiedCallOfSendMethod()
        {
            //Act
            var result = todoListsControllerObj.Create(new CreateTodoListCommand());

            //Assert
            mockSender.Verify(m => m.Send(It.IsAny<CreateTodoListCommand>(), default), Times.Once);
        }

        [Test]
        public void Update_SendMethodOfMediatorIsCalled_VerifiedCallOfSendMethod()
        {
            //Act
            var result = todoListsControllerObj.Update(new UpdateTodoListCommand());

            //Assert
            mockSender.Verify(m => m.Send(It.IsAny<UpdateTodoListCommand>(), default), Times.Once);
        }

        [Test]
        public void Delete_SendMethodOfMediatorIsCalled_VerifiedCallOfSendMethod()
        {
            //Act
            var result = todoListsControllerObj.Delete(0);

            //Assert
            mockSender.Verify(m => m.Send(It.IsAny<DeleteTodoListCommand>(), default), Times.Once);
        }

        [Test]
        public void Get_SendMethodOfMediatorIsCalled_VerifiedCallOfSendMethod()
        {
            //Act
            var result = todoListsControllerObj.Get(new GetTodosQuery());

            //Assert
            mockSender.Verify(m => m.Send(It.IsAny<GetTodosQuery>(), default), Times.Once);
        }
    }
}
