using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Infrastructure.UnitTests
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        IUnitOfWork _uow;
        ApplicationDbContext _dbContext;
        Mock<IDomainEventService> mockDomainEventService;

        [SetUp]
        public void Setup()
        {
            mockDomainEventService = new Mock<IDomainEventService>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options;
            _dbContext = new ApplicationDbContext(options, mockDomainEventService.Object);

            _uow = new UnitOfWork.UnitOfWork(_dbContext);
        }

        [TearDown]
        public void CleanUp()
        {
            _uow.Dispose();
        }

        [Test]
        public async Task UnitOfWork_AddingNewTodoItemToDB_ReturnTrue()
        {
            var newTodoItem = new TodoItem {Title = "dummyTitle", Note = "dummyNote" };

            _uow.TodoItems.Add(newTodoItem);

            await _uow.CompleteAsync(new System.Threading.CancellationToken());

            var storedTodoItem = _uow.TodoItems.Find(x => x.Id == 1).SingleOrDefault();

            Assert.IsTrue(storedTodoItem != null);
        }

        [Test]
        public async Task UnitOfWork_IsFetchSpecificTodoItemFromDB_ReturnTrue()
        {
            var newTodoItem1 = new TodoItem { Title = "dummyTitle1", Note = "dummyNote1" };
            _uow.TodoItems.Add(newTodoItem1);

            var newTodoItem2 = new TodoItem { Title = "dummyTitle2", Note = "dummyNote2" };
            _uow.TodoItems.Add(newTodoItem2);

            await _uow.CompleteAsync(new System.Threading.CancellationToken());

            var result = _uow.TodoItems.Find(x => x.Id == 2).SingleOrDefault();

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Id == 2);
        }

        [Test]
        public async Task UnitOfWork_IsFetchAllTodoItemFromDB_ReturnTrue()
        {
            var newTodoItem1 = new TodoItem { Title = "dummyTitle1", Note = "dummyNote1" };
            _uow.TodoItems.Add(newTodoItem1);

            var newTodoItem2 = new TodoItem { Title = "dummyTitle2", Note = "dummyNote2" };
            _uow.TodoItems.Add(newTodoItem2);

            await _uow.CompleteAsync(new System.Threading.CancellationToken());

            var result = _uow.TodoItems.All();

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Count() == 2);
            Assert.IsTrue(result.First().Title == "dummyTitle1");
            Assert.IsTrue(result.Last().Note == "dummyNote2");
        }

        [Test]
        public async Task UnitOfWork_UpdatingExistedTodoItem_ReturnTrue()
        {
            var newTodoItem1 = new TodoItem { Title = "dummyTitle1", Note = "dummyNote1" };
            _uow.TodoItems.Add(newTodoItem1);

            var newTodoItem2 = new TodoItem { Title = "dummyTitle2", Note = "dummyNote2" };
            _uow.TodoItems.Add(newTodoItem2);

            await _uow.CompleteAsync(new System.Threading.CancellationToken());

            var result = _uow.TodoItems.Find(x => x.Id == 2).Single();
            result.Title = "dummyTitle2Updated";
            result.Note = "dummyNote2Updated";
            await _uow.CompleteAsync(new System.Threading.CancellationToken());

            result = _uow.TodoItems.Find(x => x.Id == 2).Single();
            Assert.IsTrue(result.Title == "dummyTitle2Updated");
            Assert.IsTrue(result.Note == "dummyNote2Updated");
        }

        [Test]
        public async Task UnitOfWork_RemovingExistedTodoITem_ReturnTrue()
        {
            var newTodoItem1 = new TodoItem { Title = "dummyTitle1", Note = "dummyNote1" };
            _uow.TodoItems.Add(newTodoItem1);

            var newTodoItem2 = new TodoItem { Title = "dummyTitle2", Note = "dummyNote2" };
            _uow.TodoItems.Add(newTodoItem2);

            await _uow.CompleteAsync(new System.Threading.CancellationToken());

            var deletingItem = _uow.TodoItems.Find(x => x.Id == 2).Single();
            _uow.TodoItems.Remove(deletingItem);
            await _uow.CompleteAsync(new System.Threading.CancellationToken());

            var storedTodoItems = _uow.TodoItems.All();
            Assert.IsTrue(storedTodoItems.Count() == 1);
            Assert.IsTrue(storedTodoItems.SingleOrDefault(x => x.Id == 2) == null);
        }
    }
}
