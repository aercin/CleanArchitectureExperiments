using Application.Common.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Application.UnitTests.Models
{
    [TestFixture]
    public class PaginatedListTests
    {
        [TestCase(1, 6, 6)]
        [TestCase(2, 6, 4)]
        [TestCase(4, 3, 1)]
        public void Create_IsPaginationOfSourceWorkCorrectly_ReturnTrue(int pageIndex, int pageSize, int expectedPageItemCount)
        {
            //Arrange  
            var mockQuerables = new List<DummyEntity>
            {
                new DummyEntity { Number = 0, Name = "Name1" },
                new DummyEntity { Number = 1, Name = "Name2" },
                new DummyEntity { Number = 2, Name = "Name3" },
                new DummyEntity { Number = 3, Name = "Name4" },
                new DummyEntity { Number = 4, Name = "Name5" },
                new DummyEntity { Number = 5, Name = "Name6" },
                new DummyEntity { Number = 6, Name = "Name7" },
                new DummyEntity { Number = 7, Name = "Name8" },
                new DummyEntity { Number = 8, Name = "Name9" },
                new DummyEntity { Number = 9, Name = "Name10" }
            }.AsQueryable();

            //Act
            var result = PaginatedList<DummyEntity>.Create(mockQuerables, pageIndex, pageSize);

            //Assert
            Assert.IsTrue(result.Items.Count == expectedPageItemCount);
        }

        private class DummyEntity
        {
            public int Number { get; set; }
            public string Name { get; set; }
        }
    }
}
