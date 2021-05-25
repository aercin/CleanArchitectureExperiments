using Application.Common.Interfaces;
using Application.TodoItems.Commands.UpdateTodoItem;
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
    public class UpdateTodoItemCommandTests
    {
        [Test]
        public void Handle_RequestedTodoItemNotExist_ThrowNotFoundEx()
        {
            //Arrange 
            Mock<IUnitOfWork> mockUow = new Mock<IUnitOfWork>();

            Mock<ITodoItemRepository> mockTodoItemRepo = new Mock<ITodoItemRepository>();
            mockTodoItemRepo.Setup(m => m.Find(It.IsAny<Expression<Func<TodoItem, bool>>>(), false)).Returns(new List<TodoItem>().AsQueryable());

            mockUow.Setup(m => m.TodoItems).Returns(mockTodoItemRepo.Object);

            var updateTodoItemCommandHandlerObj = new UpdateTodoItemCommandHandler(mockUow.Object);

            //Act & Assert
            Assert.ThrowsAsync<Common.Exceptions.NotFoundException>(() => updateTodoItemCommandHandlerObj.Handle(new UpdateTodoItemCommand { Id = 1, Title = "dummyTitleUpdated", Done = false }, new System.Threading.CancellationToken()));
        }

        [Test]
        public async Task Handle_EntityPropertiesIsSuccessfullyUpdated_VerifyEntitySuccessfullyUpdated()
        {
            //Arrange 
            Mock<IUnitOfWork> mockUow = new Mock<IUnitOfWork>();

            Mock<ITodoItemRepository> mockTodoItemRepo = new Mock<ITodoItemRepository>();
            var itemTodoList = new List<TodoItem>
            {
                new TodoItem
                {
                    Id = 1,
                    ListId = 1,
                    Title = "dummyTitle",
                    Done = false
                }
            };
            mockTodoItemRepo.Setup(m => m.Find(It.IsAny<Expression<Func<TodoItem, bool>>>(), false)).Returns(itemTodoList.AsQueryable());

            mockUow.Setup(m => m.TodoItems).Returns(mockTodoItemRepo.Object);

            var updateTodoItemCommandHandlerObj = new UpdateTodoItemCommandHandler(mockUow.Object);

            //Act
            var result = await updateTodoItemCommandHandlerObj.Handle(new UpdateTodoItemCommand { Id = 1, Title = "dummyTitleUpdated", Done = true }, new System.Threading.CancellationToken());

            //Assert
            Assert.IsTrue(result.Succeeded);
            Assert.IsTrue(mockTodoItemRepo.Object.Find(x => x.Id == 1).Single().Title == "dummyTitleUpdated");
            Assert.IsTrue(mockTodoItemRepo.Object.Find(x => x.Id == 1).Single().Done == true);
        }
    }
}
