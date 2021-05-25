using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitTests
{
    [TestFixture]
    public class ApplicationDbContextTests
    {
        ApplicationDbContext _dbContext;
        Mock<IDomainEventService> mockDomainEventService;

        [SetUp]
        public void Setup()
        {
            mockDomainEventService = new Mock<IDomainEventService>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options;
            _dbContext = new ApplicationDbContext(options, mockDomainEventService.Object);
        }

        [TearDown]
        public void CleanUp()
        {
            _dbContext.Dispose();
        }

        [Test]
        public async Task DispatchEvents_WhenEntitiesInCollectionInheritedWithIHasDomainEvent_MustPublishEntitiesInheritedIHasDomainEvent()
        {
            //Arrange  
            var newTodoItem = new TodoItem
            {
                Id = 1,
                ListId = 1,
                Title = "dummyTodoItem",
                Done = false
            };
            var newTodoItemCreatedEvent = new TodoItemCreatedEvent(newTodoItem);
            newTodoItem.DomainEvents.Add(newTodoItemCreatedEvent);

            _dbContext.TodoItems.Add(newTodoItem);

            //Act
            var result = await _dbContext.SaveChangesAsync();

            //Assert
            Assert.IsTrue(newTodoItem.DomainEvents.First().IsPublished);
            mockDomainEventService.Verify(m => m.Publish(newTodoItemCreatedEvent));

        }
    }
}
