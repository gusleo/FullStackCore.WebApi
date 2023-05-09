using Moq;
using Scaffolding.Data.Repo;
using Scaffolding.Data.Test.Entities;
using Scaffolding.Data;
using MockQueryable.Moq;
using FluentAssertions;
using System.Linq.Expressions;

namespace Scaffolding.Tests
{
    [TestFixture]
    public class EntityReadBaseRepositoryTests
    {
        private Mock<IDataContext> _mockDbContext;
        private EntityReadBaseRepository<TestEntity> _repository;
        private List<TestEntity> _entities = GenerateData();

        [SetUp]
        public void SetUp()
        {

            _mockDbContext = new Mock<IDataContext>();

            //2 - build mock by extension
            var mock = _entities.AsQueryable().BuildMockDbSet();

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
            result.Count().Should().Be(entities.Count());
            result.First().Name.Should().Be(entities.First().Name);
            result.Last().Name.Should().Be(entities.Last().Name);
        }

        [Test]
        [TestCase(1, 2)]
        public async Task GetAllAsync_ReturnsPaginationEntity(int pageIndex, int pageSize)
        {
            // Arrange
            var data = GenerateData();

            
            // Act
            var result = await _repository.GetAllAsync(pageIndex, pageSize);

            // Assert
            result.Should().NotBeNull();
            result.Page.Should().Be(pageIndex);
            result.PageSize.Should().Be(pageSize);
            result.TotalCount.Should().Be(data.Count());
            result.Items.Should().HaveCount(pageSize);
        }

        [Test]
        [TestCase(1, 10)]
        public async Task GetAllAsync_WithIncludeProperties_ReturnsExpectedEntities(int page, int pageSize)
        {
            // Arrange
            var entities = GenerateData();
            
            // Arrange include properties
            Expression<Func<TestEntity, object>>[] includeProperties = { x => x.Related };

            // Act
            var result = await _repository.GetAllAsync(1, 10, includeProperties);

            // Assert
            result.Should().NotBeNull();
            result.Page.Should().Be(1);
            result.PageSize.Should().Be(10);
            result.TotalCount.Should().Be(entities.Count());
            result.Items.Should().HaveCount(entities.Count());
        }
    

        [Test]
        public async Task AllIncludingAsync_ReturnAllEntities()
        {
            // Arrange
            var entities = GenerateData().Where(x => x.Name != string.Empty).OrderByDescending(x => x.CreatedDate).ToList();
            // Arrange include properties
            Expression<Func<TestEntity, object>>[] includeProperties = { x => x.Related };

            // Act
            var result = await _repository.AllIncludingAsync(includeProperties);

            // Assert
            result.Count().Should().Be(entities.Count());
            result.First().Name.Should().Be(entities.First().Name);
            result.Skip(1).First().Name.Should().Be(entities.Skip(1).First().Name);
        }

        [Test]
        public async Task GetSingleAsync_ReturnEntity()
        {
            // Arrange
            
            var first = _entities.First();
            // Act
            var result = await _repository.GetSingleAsync(first.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(first.Id);
            result.Name.Should().Be(first.Name);
        }

        [Test]
        public async Task GetSingleAsync_ReturnNull()
        {
            // Arrange
            var entities = GenerateData().Where(x => x.Name != string.Empty).OrderByDescending(x => x.CreatedDate).ToList();
            var first = Guid.NewGuid();

            // Act
            var result = await _repository.GetSingleAsync(first);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task GetSingleAsync_WithPredicate_ReturnEntity()
        {
            // Arrange
            var entities = GenerateData();
            var first = entities.First();

            // Act
            var result = await _repository.GetSingleAsync(x => x.Name == first.Name);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(first.Name);

        }

        [Test]
        public async Task GetSingleAsync_WithPredicate_And_IncludeProperty_ReturnEntity()
        {
            // Arrange
            var entities = GenerateData();
            var first = entities.First();
            Expression<Func<TestEntity, object>>[] includeProperties = { x => x.Related };


            // Act
            var result = await _repository.GetSingleAsync(x => x.Name == first.Name, includeProperties);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(first.Name.ToString());
            result.Related.Should().NotBeNull();

        }

        
        [Test]
        public void GetSingle_WithValidPredicate_ReturnsEntity()
        {
            // Arrange
            var entities = GenerateData();
            var first = entities[0];


            // Act
            var result = _repository.GetSingle(x => x.Name == first.Name);

            // Assert
            result.Name.Should().BeSameAs(first.Name);
        }

        [Test]
        public void GetSingle_WithValidId_ReturnsEntity()
        {
            // Arrange

            var first = _entities.First();

            
            // Act
            var result = _repository.GetSingle(first.Id);

            // Assert
            result.Id.Should().Be(first.Id);
        }

        [Test]
        public async Task FindByAsync_WithPredicate_ReturnsMatchingEntities()
        {
            // Arrange
            var entities = GenerateData().Where(x => x.Name != string.Empty).OrderByDescending(x => x.CreatedDate).ToList();
            

            // Define predicate
            Expression<Func<TestEntity, bool>> predicate = x => x.Name != string.Empty;

            // Act
            var result = await _repository.FindByAsync(predicate);

            // Assert
            result.First().Name.Equals(entities[0].Name);
        }

        [Test]
        public async Task FindByAsync_WithPredicateAndIncludeProperties_ReturnsMatchingEntitiesWithIncludedProperties()
        {
            // Arrange
            var entities = GenerateData().Where(x => x.Name != string.Empty).OrderByDescending(x => x.CreatedDate).ToList();
            var first = entities.First(x => x.Name.Contains("One"));

            // Define predicate
            Expression<Func<TestEntity, bool>> predicate = x => x.Name.Contains("One");

            // Define include properties
            Expression<Func<TestEntity, object>>[] includeProperties =
            {
                x => x.Related
            };

            // Act
            var result = await _repository.FindByAsync(predicate, includeProperties);

            // Assert
            //result.Should().Contain(entities[0]);
            result.First().Related.Name.Should().BeSameAs(first.Related.Name);
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
