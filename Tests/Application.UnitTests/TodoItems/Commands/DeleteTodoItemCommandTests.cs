using Application.Common.Interfaces;
using Application.TodoItems.Commands.DeleteTodoItem;
using Domain.Entities;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.UnitTests.TodoItems.Commands
{
    [TestFixture]
    public class DeleteTodoItemCommandTests
    {
        [Test]
        public void Handle_RequestedTodoItemNotExist_ThrowNotFoundEx()
        {
            //Arrange 
            Mock<IUnitOfWork> mockUow = new Mock<IUnitOfWork>();

            Mock<ITodoItemRepository> mockTodoItemRepo = new Mock<ITodoItemRepository>();
            mockTodoItemRepo.Setup(m => m.Find(It.IsAny<Expression<Func<TodoItem, bool>>>(), false)).Returns(new List<TodoItem>().AsQueryable());

            mockUow.Setup(m => m.TodoItems).Returns(mockTodoItemRepo.Object);

            var deleteTodoItemCommandHandlerObj = new DeleteTodoItemCommandHandler(mockUow.Object);

            //Act & Assert
            Assert.ThrowsAsync<Common.Exceptions.NotFoundException>(() => deleteTodoItemCommandHandlerObj.Handle(new DeleteTodoItemCommand { Id = 1 }, new System.Threading.CancellationToken()));
        }

        [Test]
        public async Task Handle_RequestedTodoItemExist_CommandResultCodeMustBeTrue()
        {
            //Arrange 
            Mock<IUnitOfWork> mockUow = new Mock<IUnitOfWork>();

            Mock<ITodoItemRepository> mockTodoItemRepo = new Mock<ITodoItemRepository>();
            mockTodoItemRepo.Setup(m => m.Find(It.IsAny<Expression<Func<TodoItem, bool>>>(), false)).Returns(new List<TodoItem> { new TodoItem { Id = 1, Title = "dummyTitle", ListId = 1 } }.AsQueryable());

            mockUow.Setup(m => m.TodoItems).Returns(mockTodoItemRepo.Object);

            var deleteTodoItemCommandHandlerObj = new DeleteTodoItemCommandHandler(mockUow.Object);

            //Act
            var result = await deleteTodoItemCommandHandlerObj.Handle(new DeleteTodoItemCommand { Id = 1 },new System.Threading.CancellationToken());

            //Assert
            Assert.IsTrue(result.Succeeded);
        }
    }
}
