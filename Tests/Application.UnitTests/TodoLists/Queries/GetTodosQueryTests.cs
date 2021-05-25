using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.TodoLists.Queries.GetTodos;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UnitTests.TodoLists.Queries
{
    [TestFixture]
    public class GetTodosQueryTests
    {
        [Test]
        public async Task Handle_IsGettingAllTodoListSuccessfully_VerifyGettingCorrectResult()
        {
            //Arrange
            var mockGetTodosQuery = Mock.Of<GetTodosQuery>();
            Mock<IUnitOfWork> mockUow = new Mock<IUnitOfWork>();
            Mock<ITodoListRepository> mockTodoListRepo = new Mock<ITodoListRepository>();
            var dummyTodoListStore = new List<TodoList>
            {
                new TodoList { Id=1,Title="dummyTitle1", Colour = Colour.Red},
                new TodoList { Id=2,Title="dummyTitle2", Colour = Colour.Orange},
                new TodoList { Id=3,Title="dummyTitle3", Colour = Colour.White}
            };
            mockTodoListRepo.Setup(m => m.All(true)).Returns(dummyTodoListStore.AsQueryable());
            mockUow.Setup(m => m.TodoLists).Returns(mockTodoListRepo.Object);

            var mappingProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            IMapper mapper = new Mapper(configuration);

            var getTodosQueryHandlerObj = new GetTodosQueryHandler(mockUow.Object, mapper);

            //Act
            var result = await getTodosQueryHandlerObj.Handle(mockGetTodosQuery, new System.Threading.CancellationToken());

            //Assert
            Assert.IsTrue(result.Succeeded);
            Assert.IsTrue(result.Data.First().Colour == Colour.Red.ToString());
            Assert.IsTrue(result.Data.Last().Colour == Colour.White.ToString());
        }
    }
}
