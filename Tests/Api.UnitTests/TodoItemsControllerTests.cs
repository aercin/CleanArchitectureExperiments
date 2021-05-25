using Api.Controllers;
using Application.TodoItems.Commands.CreateTodoItem;
using Application.TodoItems.Commands.DeleteTodoItem;
using Application.TodoItems.Commands.UpdateTodoItem;
using Application.TodoItems.Commands.UpdateTodoItemDetail;
using Application.TodoItems.Queries.GetTodoItemsWithPagination;
using MediatR;
using Moq;
using NUnit.Framework;

namespace Api.UnitTests
{
    /// <summary>
    /// Test class name convention is Name Of Class which is tested + "Tests"
    /// Test method name convention is Name Of Method which is tested + "_" + scenario + "_" + expected behaviour
    /// </summary>
    [TestFixture]
    public class TodoItemsControllerTests
    {
        Mock<ISender> mockSender;
        TodoItemsController todoItemsControllerObj;

        [SetUp]
        public void Setup()
        {//It is run once before every test's running
            //Arrange
            mockSender = new Mock<ISender>();
            todoItemsControllerObj = new TodoItemsController(mockSender.Object);
        }

        [TearDown]
        public void CleanUp()
        {//It is run once after every test's running

        }

        [Test]
        public void Create_SendMethodOfMediatorIsCalled_VerifiedCallOfSendMethod()
        {
            //Act
            var result = todoItemsControllerObj.Create(new CreateTodoItemCommand());

            //Assert
            mockSender.Verify(m => m.Send(It.IsAny<CreateTodoItemCommand>(), default), Times.Once);
        }

        [Test]
        public void Update_SendMethodOfMediatorIsCalled_VerifiedCallOfSendMethod()
        {
            //Act 
            var result = todoItemsControllerObj.Update(new UpdateTodoItemCommand());

            //Assert
            mockSender.Verify(m => m.Send(It.IsAny<UpdateTodoItemCommand>(), default), Times.Once);
        }

        [Test]
        public void UpdateItemDetails_SendMethodOfMediatorIsCalled_VerifiedCallOfSendMethod()
        {
            //Act 
            var result = todoItemsControllerObj.UpdateItemDetails(new UpdateTodoItemDetailCommand());

            //Assert
            mockSender.Verify(m => m.Send(It.IsAny<UpdateTodoItemDetailCommand>(), default), Times.Once);
        }

        [Test]
        public void Delete_SendMethodOfMediatorIsCalled_VerifiedCallOfSendMethod()
        {
            //Act
            var result = todoItemsControllerObj.Delete(0);

            //Assert
            mockSender.Verify(m => m.Send(It.IsAny<DeleteTodoItemCommand>(), default), Times.Once);
        }

        [Test]
        public void Get_SendMethodOfMediatorIsCalled_VerifiedCallOfSendMethod()
        {
            //Act
            var result = todoItemsControllerObj.Get(new GetTodoItemsWithPaginationQuery());

            //Assert
            mockSender.Verify(m => m.Send(It.IsAny<GetTodoItemsWithPaginationQuery>(), default), Times.Once);
        }
    }
}