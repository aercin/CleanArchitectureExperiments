using Application.Common.Interfaces;
using Application.TodoItems.Commands.UpdateTodoItemDetail;
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
    public class UpdateTodoItemDetailCommandTests
    {
        [Test]
        public void Handle_RequestedTodoItemNotExist_ThrowNotFoundEx()
        {
            //Arrange 
            Mock<IUnitOfWork> mockUow = new Mock<IUnitOfWork>();

            Mock<ITodoItemRepository> mockTodoItemRepo = new Mock<ITodoItemRepository>();
            mockTodoItemRepo.Setup(m => m.Find(It.IsAny<Expression<Func<TodoItem, bool>>>(), false)).Returns(new List<TodoItem>().AsQueryable());

            mockUow.Setup(m => m.TodoItems).Returns(mockTodoItemRepo.Object);

            var updateTodoItemDetailCommandHandlerObj = new UpdateTodoItemDetailCommandHandler(mockUow.Object);

            //Act & Assert
            Assert.ThrowsAsync<Common.Exceptions.NotFoundException>(() => updateTodoItemDetailCommandHandlerObj.Handle(new UpdateTodoItemDetailCommand { Id = 1, ListId = 1, Priority = Domain.Enums.PriorityLevel.High, Note = "dummyNote" }, new System.Threading.CancellationToken()));
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
                    Priority = Domain.Enums.PriorityLevel.High,
                    Note = "dummyNote"
                }
            };
            mockTodoItemRepo.Setup(m => m.Find(It.IsAny<Expression<Func<TodoItem, bool>>>(), false)).Returns(itemTodoList.AsQueryable());

            mockUow.Setup(m => m.TodoItems).Returns(mockTodoItemRepo.Object);

            var updateTodoItemDetailCommandHandlerObj = new UpdateTodoItemDetailCommandHandler(mockUow.Object);

            //Act
            var result = await updateTodoItemDetailCommandHandlerObj.Handle(new UpdateTodoItemDetailCommand { Id = 1, ListId = 2, Priority = Domain.Enums.PriorityLevel.Low, Note = "dummyNoteUpdated" }, new System.Threading.CancellationToken());

            //Assert
            Assert.IsTrue(result.Succeeded);
            Assert.IsTrue(mockTodoItemRepo.Object.Find(x => x.Id == 1).Single().ListId == 2);
            Assert.IsTrue(mockTodoItemRepo.Object.Find(x => x.Id == 1).Single().Priority == Domain.Enums.PriorityLevel.Low);
            Assert.IsTrue(mockTodoItemRepo.Object.Find(x => x.Id == 1).Single().Note == "dummyNoteUpdated");
        }
    }
}
