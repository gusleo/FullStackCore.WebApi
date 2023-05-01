using Moq;
using Microsoft.EntityFrameworkCore;
using Scaffolding.Data.Repo;
using Scaffolding.Data.Test.Entities;
using Scaffolding.Data;
using Scaffolding.Data.Test.Repo;
using Scaffolding.Data.Repo.Test;
using MockQueryable.Moq;

namespace Scaffolding.Tests
{
    [TestFixture]
    public class EntityReadBaseRepositoryTests
    {
        private Mock<IDataContext> _mockDbContext;
        private EntityReadBaseRepository<TestEntity> _repository;

        [SetUp]
        public void SetUp()
        {
            var entities = GenerateData();

            _mockDbContext = new Mock<IDataContext>();

            //2 - build mock by extension
            var mock = entities.AsQueryable().BuildMockDbSet();

            _mockDbContext.Setup(c => c.Set<TestEntity>()).Returns(mock.Object);
            _repository = new EntityReadBaseRepository<TestEntity>(_mockDbContext.Object);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllEntities()
        {
            // Arrange
            var entities = GenerateData();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.AreEqual(entities.Count(), result.Count());
            Assert.AreEqual(entities.First().Name, result.First().Name);
            Assert.AreEqual(entities.Last().Name, result.Last().Name);
        }

        [Test]
        public async Task AllIncludingAsync_ReturnAllEntities()
        {
            // Arrange
            var entities = GenerateData().Where(x => x.Name != string.Empty).OrderByDescending(x => x.CreatedDate).ToList();

            // Act
            var result = await _repository.AllIncludingAsync(x => x.Name != String.Empty);

            // Assert
            Assert.AreEqual(entities.First().Name, result.First().Name);
            Assert.AreEqual(entities.Skip(1).First().Name, result.Skip(1).First().Name);
        }

        [Test]
        public async Task GetAllAsync_ReturnEntitiesWithPagination()
        {
            // Arrange
            var entities = GenerateData().OrderByDescending(x => x.CreatedDate).ToList();
            var page = 1;
            var pageSize = 2;

            // Act
            var result = await _repository.GetAllAsync(page, pageSize);

            // Assert
            Assert.AreEqual(pageSize, result.Count);
            Assert.AreEqual(entities.Skip(pageSize * (page - 1)).First().Name, result.Items.First().Name);
        }


        #region Private

        private static List<TestEntity> GenerateData()
        {

            // Arrange
            return new List<TestEntity>
            {
                new TestEntity { Id = Guid.NewGuid(), Name = "One", CreatedDate = DateTime.Now },
                new TestEntity {Id = Guid.NewGuid(), Name = "Two", CreatedDate = DateTime.Now },
                new TestEntity {Id = Guid.NewGuid(), Name = "Three", CreatedDate = DateTime.Now },
            };
        }

        #endregion
    }
}
