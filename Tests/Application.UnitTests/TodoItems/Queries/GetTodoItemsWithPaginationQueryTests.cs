using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.TodoItems.Queries.GetTodoItemsWithPagination;
using AutoMapper;
using Domain.Entities;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.UnitTests.TodoItems.Queries
{
    [TestFixture]
    public class GetTodoItemsWithPaginationQueryTests
    {
        [TestCase(2, 2, 2, 1, 3)]
        [TestCase(1, 2, 4, 3, 7)]
        public async Task Handle_IsGettingCorrectResultForGivenCriteria_VerifyGettingCorrectResultForGivenCriteria(int listId, int pageNumber, int pageSize, int pageItemCount, int totalItemCount)
        {
            //Arrange
            Mock<IUnitOfWork> mockUow = new Mock<IUnitOfWork>();
            Mock<ITodoItemRepository> mockTodoItemRepo = new Mock<ITodoItemRepository>();
            var dummyTodoItemList = new List<TodoItem>
            {//7 adet ListId 1 ve 3 adet ListId 2
                new TodoItem { Id=1,ListId=1,Title="dummyTitle1",Note="dummyNote1"},
                new TodoItem { Id=2,ListId=1,Title="dummyTitle2",Note="dummyNote2"},
                new TodoItem { Id=3,ListId=1,Title="dummyTitle3",Note="dummyNote3"},
                new TodoItem { Id=4,ListId=1,Title="dummyTitle4",Note="dummyNote4"},
                new TodoItem { Id=5,ListId=1,Title="dummyTitle5",Note="dummyNote5"},
                new TodoItem { Id=6,ListId=1,Title="dummyTitle6",Note="dummyNote6"},
                new TodoItem { Id=7,ListId=1,Title="dummyTitle7",Note="dummyNote7"},
                new TodoItem { Id=8,ListId=2,Title="dummyTitle8",Note="dummyNote8"},
                new TodoItem { Id=9,ListId=2,Title="dummyTitle9",Note="dummyNote9"},
                new TodoItem { Id=10,ListId=2,Title="dummyTitle10",Note="dummyNote10"}
            };
            mockTodoItemRepo.Setup(m => m.Find(It.IsAny<Expression<Func<TodoItem, bool>>>(), true)).Returns(dummyTodoItemList.Where(x => x.ListId == listId).AsQueryable());
            mockUow.Setup(m => m.TodoItems).Returns(mockTodoItemRepo.Object);

            var mappingProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            IMapper mapper = new Mapper(configuration);

            var getTodoItemsWithPaginationQueryHandlerObj = new GetTodoItemsWithPaginationQueryHandler(mockUow.Object, mapper);

            //Act
            var result = await getTodoItemsWithPaginationQueryHandlerObj.Handle(new GetTodoItemsWithPaginationQuery { ListId = listId, PageNumber = pageNumber, PageSize = pageSize }, new System.Threading.CancellationToken());

            //Assert
            Assert.IsTrue(result.Succeeded);
            Assert.IsTrue(result.Items.Count == pageItemCount);
            Assert.IsTrue(result.TotalCount == totalItemCount);
        }
    }
}
