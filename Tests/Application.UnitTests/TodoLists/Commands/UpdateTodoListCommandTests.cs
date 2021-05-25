using Application.Common.Interfaces;
using Application.TodoLists.Commands.UpdateTodoList;
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
    public class UpdateTodoListCommandTests
    {
        [Test]
        public void Handle_RequestedTodoListNotExist_ThrowNotFoundEx()
        {
            //Arrange 
            Mock<IUnitOfWork> mockUow = new Mock<IUnitOfWork>();

            Mock<ITodoListRepository> mockTodoListRepo = new Mock<ITodoListRepository>();
            mockTodoListRepo.Setup(m => m.Find(It.IsAny<Expression<Func<TodoList, bool>>>(), false)).Returns(new List<TodoList>().AsQueryable());

            mockUow.Setup(m => m.TodoLists).Returns(mockTodoListRepo.Object);

            var updateTodoListCommandHandlerObj = new UpdateTodoListCommandHandler(mockUow.Object);

            //Act & Assert
            Assert.ThrowsAsync<Common.Exceptions.NotFoundException>(() => updateTodoListCommandHandlerObj.Handle(new UpdateTodoListCommand { Id = 1, Title = "dummyTitleUpdated" }, new System.Threading.CancellationToken()));
        }

        [Test]
        public async Task Handle_EntityPropertiesIsSuccessfullyUpdated_VerifyEntitySuccessfullyUpdated()
        {
            //Arrange 
            Mock<IUnitOfWork> mockUow = new Mock<IUnitOfWork>();

            Mock<ITodoListRepository> mockTodoListRepo = new Mock<ITodoListRepository>();
            var dummyTodoListStore = new List<TodoList>
            {
                new TodoList
                {
                    Id = 1,
                    Title = "dummyTitle",
                }
            };
            mockTodoListRepo.Setup(m => m.Find(It.IsAny<Expression<Func<TodoList, bool>>>(), false)).Returns(dummyTodoListStore.AsQueryable());

            mockUow.Setup(m => m.TodoLists).Returns(mockTodoListRepo.Object);

            var updateTodoListCommandHandlerObj = new UpdateTodoListCommandHandler(mockUow.Object);

            //Act
            var result = await updateTodoListCommandHandlerObj.Handle(new UpdateTodoListCommand { Id = 1, Title = "dummyTitleUpdated" }, new System.Threading.CancellationToken());

            //Assert
            Assert.IsTrue(result.Succeeded);
            Assert.IsTrue(mockTodoListRepo.Object.Find(x => x.Id == 1).Single().Title == "dummyTitleUpdated"); 
        }
    }
}
