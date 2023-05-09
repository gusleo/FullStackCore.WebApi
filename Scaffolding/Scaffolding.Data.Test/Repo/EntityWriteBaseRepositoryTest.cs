using MockQueryable.Moq;
using Moq;
using Scaffolding.Data.Repo;
using Scaffolding.Data.Test.Entities;

namespace Scaffolding.Data.Test.Repo
{
    [TestFixture]
    public class EntityWriteBaseRepositoryTests
    {
        private Mock<IDataContext> _mockDbContext;
        private EntityWriteBaseRepository<TestEntity> _repository;
        private List<TestEntity> _entities = GenerateData();

        [SetUp]
        public void SetUp()
        {
            _mockDbContext = new Mock<IDataContext>();

            //2 - build mock by extension
            var mock = _entities.AsQueryable().BuildMockDbSet();

            _mockDbContext.Setup(c => c.Set<TestEntity>()).Returns(mock.Object);
            _repository = new EntityWriteBaseRepository<TestEntity>(_mockDbContext.Object);
        }

        [Test]
        public void Add_AddsEntityToContext()
        {
            // Arrange
            var entity = new TestEntity()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
            };

            // Act
            _repository.Add(entity);

            // Assert
            _mockDbContext.Verify(x => x.Set<TestEntity>().Add(entity), Times.Once);
        }

        [Test]
        public void AddRange_AddsEntitiesToContext()
        {
            // Arrange
            var entities = new List<TestEntity> { new TestEntity(), new TestEntity() };

            // Act
            _repository.AddRange(entities);

            // Assert
            _mockDbContext.Verify(x => x.Set<TestEntity>().AddRange(entities), Times.Once);
        }

        [Test]
        public void Edit_UpdatesEntityInContext()
        {
            // Arrange
            var entity = new TestEntity();

            // Act
            _repository.Edit(entity);

            // Assert
            _mockDbContext.Verify(x => x.Updated<TestEntity>(entity), Times.Once);
        }

        [Test]
        public void Delete_RemovesEntityFromContext()
        {
            // Arrange
            var entity = new TestEntity();

            // Act
            _repository.Delete(entity);

            // Assert
            _mockDbContext.Verify(x => x.Set<TestEntity>().Remove(entity), Times.Once);
        }

        [Test]
        public void DeleteRange_RemovesEntitiesFromContext()
        {
            // Arrange
            var entities = new List<TestEntity> { new TestEntity(), new TestEntity() };

            // Act
            _repository.DeleteRange(entities);

            // Assert
            _mockDbContext.Verify(x => x.Set<TestEntity>().RemoveRange(entities), Times.Once);
        }

        [Test]
        public void Dispose_CallsDataContextDispose()
        {
            // Act
            _repository.Dispose();

            // Assert
            _mockDbContext.Verify(x => x.Dispose(), Times.Once);
        }


        #region Private
        private static List<TestEntity> GenerateData()
        {
            var relatedEntity = new TestEntityRelation
            {
                Id = Guid.NewGuid(),
                Name = "RelatedEntity"
            };

            // Arrange
            return new List<TestEntity>
            {
                new TestEntity { Id = Guid.NewGuid(), Name = "One", CreatedDate = DateTime.UtcNow, Related = relatedEntity },
                new TestEntity {Id = Guid.NewGuid(), Name = "Two", CreatedDate = DateTime.UtcNow, Related = relatedEntity },
                new TestEntity {Id = Guid.NewGuid(), Name = "Three", CreatedDate = DateTime.UtcNow,Related = relatedEntity },
            };
        }
        #endregion


    }
}
