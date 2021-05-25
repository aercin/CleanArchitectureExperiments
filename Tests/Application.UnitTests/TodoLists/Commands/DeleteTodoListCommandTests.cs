using Application.Common.Interfaces;
using Application.TodoLists.Commands.DeleteTodoList;
using Domain.Entities;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.UnitTests.TodoLists.Commands
{
    [TestFixture]
    public class DeleteTodoListCommandTests
    {
        [Test]
        public void Handle_RequestedTodoListNotExist_ThrowNotFoundEx()
        {
            //Arrange 
            Mock<IUnitOfWork> mockUow = new Mock<IUnitOfWork>();

            Mock<ITodoListRepository> mockTodoListRepo = new Mock<ITodoListRepository>();
            mockTodoListRepo.Setup(m => m.Find(It.IsAny<Expression<Func<TodoList, bool>>>(), false)).Returns(new List<TodoList>().AsQueryable());

            mockUow.Setup(m => m.TodoLists).Returns(mockTodoListRepo.Object);

            var deleteTodoListCommandHandlerObj = new DeleteTodoListCommandHandler(mockUow.Object);

            //Act & Assert
            Assert.ThrowsAsync<Common.Exceptions.NotFoundException>(() => deleteTodoListCommandHandlerObj.Handle(new DeleteTodoListCommand { Id = 1 }, new System.Threading.CancellationToken()));
        }

        [Test]
        public async Task Handle_RequestedTodoListExist_CommandResultCodeMustBeTrue()
        {
            //Arrange 
            Mock<IUnitOfWork> mockUow = new Mock<IUnitOfWork>();

            Mock<ITodoListRepository> mockTodoListRepo = new Mock<ITodoListRepository>();
            mockTodoListRepo.Setup(m => m.Find(It.IsAny<Expression<Func<TodoList, bool>>>(), false)).Returns(new List<TodoList> { new TodoList { Id = 1, Title = "dummyTitle" } }.AsQueryable());

            mockUow.Setup(m => m.TodoLists).Returns(mockTodoListRepo.Object);

            var deleteTodoListCommandHandlerObj = new DeleteTodoListCommandHandler(mockUow.Object);

            //Act
            var result = await deleteTodoListCommandHandlerObj.Handle(new DeleteTodoListCommand { Id = 1 }, new System.Threading.CancellationToken());

            //Assert
            Assert.IsTrue(result.Succeeded);
        }
    }
}
