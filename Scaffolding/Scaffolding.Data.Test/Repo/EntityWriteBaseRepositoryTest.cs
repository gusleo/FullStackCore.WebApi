using MockQueryable.Moq;
using Moq;
using Scaffolding.Data.Repo;
using Scaffolding.Data.Repo.Test;
using Scaffolding.Data.Test.Entities;

namespace Scaffolding.Data.Test.Repo
{
    [TestFixture]
    public class EntityWriteBaseRepositoryTests
    {
        private Mock<IDataContext> _contextMock;
        private EntityWriteBaseRepository<TestEntity> _repository;

        [SetUp]
        public void SetUp()
        {
            _contextMock = new Mock<IDataContext>();

            // set DbSet<TestEntity>
            var data = new List<TestEntity>()
            {
                new TestEntity {Id = Guid.NewGuid()},
            };
            var mock = data.AsQueryable().BuildMockDbSet();
            //assign mock to context
            _contextMock.Setup(x => x.Set<TestEntity>()).Returns(mock.Object);

            _repository = new EntityWriteBaseRepository<TestEntity>(_contextMock.Object);
        }

        [Test]
        public void Add_AddsEntityToContext()
        {
            // Arrange
            var entity = new TestEntity();

            // Act
            _repository.Add(entity);

            // Assert
            _contextMock.Verify(x => x.Set<TestEntity>().Add(entity), Times.Once);
        }

        [Test]
        public void AddRange_AddsEntitiesToContext()
        {
            // Arrange
            var entities = new List<TestEntity> { new TestEntity(), new TestEntity() };

            // Act
            _repository.AddRange(entities);

            // Assert
            _contextMock.Verify(x => x.Set<TestEntity>().AddRange(entities), Times.Once);
        }

        [Test]
        public void Edit_UpdatesEntityInContext()
        {
            // Arrange
            var entity = new TestEntity();

            // Act
            _repository.Edit(entity);

            // Assert
            _contextMock.Verify(x => x.Updated<TestEntity>(entity), Times.Once);
        }

        [Test]
        public void Delete_RemovesEntityFromContext()
        {
            // Arrange
            var entity = new TestEntity();

            // Act
            _repository.Delete(entity);

            // Assert
            _contextMock.Verify(x => x.Set<TestEntity>().Remove(entity), Times.Once);
        }

        [Test]
        public void DeleteRange_RemovesEntitiesFromContext()
        {
            // Arrange
            var entities = new List<TestEntity> { new TestEntity(), new TestEntity() };

            // Act
            _repository.DeleteRange(entities);

            // Assert
            _contextMock.Verify(x => x.Set<TestEntity>().RemoveRange(entities), Times.Once);
        }

        [Test]
        public void Dispose_CallsDataContextDispose()
        {
            // Act
            _repository.Dispose();

            // Assert
            _contextMock.Verify(x => x.Dispose(), Times.Once);
        }


    }
}
