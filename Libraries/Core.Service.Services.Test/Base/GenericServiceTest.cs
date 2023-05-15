using AutoMapper;
using Core.Data.Infrastructure;
using Core.Service.Services.Base;
using Core.Service.Services.Test.Models;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Scaffolding.Auth;
using Scaffolding.Data;
using Scaffolding.Data.Entities;
using Scaffolding.Data.Repo;
using Scaffolding.Data.Test.Entities;
using Scaffolding.Service.Infrastructure;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace Core.Service.Services.Test.Base;

[TestFixture]
public class GenericServiceTests
{
    private GenericService<TestModel, TestEntity> _genericService;
    private Mock<ICoreUnitOfWork> _mockUnitOfWork;
    private Mock<IAuthenticationService> _mockAuthService;
    private Mock<IMapper> _mockMapper;
    private Mock<IDataContext> _mockDbContext;
    private List<TestEntity> _entities = GenerateData();

    [SetUp]
    public void Setup()
    {
        _mockDbContext = new Mock<IDataContext>();
        var mock = _entities.AsQueryable().BuildMockDbSet();
        _mockDbContext.Setup(c => c.Set<TestEntity>()).Returns(mock.Object);

        _mockUnitOfWork = new Mock<ICoreUnitOfWork>();
        _mockUnitOfWork
            .Setup(x => x.Set<TestEntity>()).Returns(new GenericRepository<TestEntity>(_mockDbContext.Object));

        _mockAuthService = new Mock<IAuthenticationService>();
        _mockMapper = new Mock<IMapper>();

        _genericService = new GenericService<TestModel, TestEntity>(_mockUnitOfWork.Object, _mockAuthService.Object, _mockMapper.Object);
    }

    [Test]
    public void Create_WithValidModel_ShouldReturnSuccessResponse()
    {
        // Arrange
        var modelToCreate = new TestModel();
        var entityFromModel = new TestEntity();

        _mockMapper.Setup(x => x.Map<TestEntity>(modelToCreate)).Returns(entityFromModel);
        _mockMapper.Setup(x => x.Map<TestModel>(entityFromModel)).Returns(modelToCreate);

        // Act
        var response = _genericService.Create(modelToCreate);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Message.Should().Be(MessageConstant.Create);
        response.Item.Should().Be(modelToCreate);
    }

    [Test]
    public async Task DeleteAsync_WithExistingId_ShouldReturnSuccessResponse()
    {
        // Arrange
        var entityToDelete = _entities.First();
        var modelToCreate = new TestModel();

        _mockMapper.Setup(x => x.Map<TestEntity>(modelToCreate)).Returns(entityToDelete);
        _mockMapper.Setup(x => x.Map<TestModel>(entityToDelete)).Returns(modelToCreate);
        // Act
        var response = await _genericService.DeleteAsync(entityToDelete.Id);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Message.Should().Be(MessageConstant.Delete);
        response.Item.Should().NotBeNull();
    }

    [Test]
    public async Task DeleteAsync_WithNonExistingId_ShouldReturnNotFoundResponse()
    {
        // Arrange
        var id = Guid.NewGuid();
       
        // Act
        var response = await _genericService.DeleteAsync(id);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeFalse();
        response.Message.Should().Be(MessageConstant.NotFound);
        response.Item.Should().BeNull();
    }

    [Test]
    public async Task EditAsync_WithExistingModel_ShouldReturnSuccessResponse()
    {
        // Arrange
        const string name = "TEST";
        var entityFromModel = _entities.First();
        var modelToEdit = new TestModel { Id = entityFromModel.Id, Name = name };


        // Act
        var response = await _genericService.EditAsync(modelToEdit);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Message.Should().Be(MessageConstant.Delete);
        response.Item.Should().Be(modelToEdit);
        response.Item.Name.Should().Be(name);
    }

    [Test]
    public async Task EditAsync_WithNonExistingModel_ShouldReturnNotFoundResponse()
    {
        // Arrange
        const string name = "TEST";
        var modelToEdit = new TestModel { Id = Guid.NewGuid(), Name = name  };

        // Act
        var response = await _genericService.EditAsync(modelToEdit);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeFalse();
        response.Message.Should().Be(MessageConstant.NotFound);
        response.Item.Should().BeNull();
    }

    [Test]
    public async Task GetAllAsync_WithValidPageIndexAndPageSize_ShouldReturnSuccessResponseWithItems()
    {
        // Arrange
        var pageIndex = 1;
        var pageSize = 20;
        var models = _entities.Select(x => new TestModel
        {
            Name = x.Name,
            Id = x.Id,
        }).ToList();

        var pagedEntities = new PaginationEntity<TestEntity>()
        {
            Page = pageIndex,
            PageSize = pageSize,
            Items = _entities
        };

        var pagedModels = new PaginationSet<TestModel>()
        {
            Page = pageIndex,
            PageSize = pageSize,
            Items = models
        };

        _mockMapper.Setup(x => x.Map<PaginationEntity<TestEntity>>(It.IsAny<PaginationSet<TestModel>>())).Returns(pagedEntities);
        _mockMapper.Setup(x => x.Map<PaginationSet<TestModel>>(It.IsAny<PaginationEntity<TestEntity>>())).Returns(pagedModels);

        // Act
        var response = await _genericService.GetAllAsync(pageIndex, pageSize);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Message.Should().Be(MessageConstant.Load);
        response.Item.Should().NotBeNull();
        response.Item.Items.Should().HaveCount(_entities.Count);
    }

    [Test]
    public async Task GetAllAsync_WithoutPageIndexAndPageSize_ShouldReturnSuccessListResponseWithItems()
    {
        // Arrange
        var models = _entities.Select(x => new TestModel
        {
            Name = x.Name,
            Id = x.Id,
        }).ToList();
        _mockMapper.Setup(x => x.Map<IList<TestEntity>>(models)).Returns(_entities);
        _mockMapper.Setup(x => x.Map<IList<TestModel>>(_entities)).Returns(models);
        // Act
        var response = await _genericService.GetAllAsync();

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Message.Should().Be(MessageConstant.Load);
        response.Item.Should().NotBeNull();
        response.Item.Should().HaveCount(_entities.Count);
    }

    [Test]
    public async Task GetSingleAsync_WithExistingId_ShouldReturnSuccessResponseWithItem()
    {
        // Arrange
        var id = _entities.First().Id;
        var entity = _entities.First();
        var model = new TestModel();

        _mockMapper.Setup(x => x.Map<TestEntity>(model)).Returns(entity);
        _mockMapper.Setup(x => x.Map<TestModel>(entity)).Returns(model);

        // Act
        var response = await _genericService.GetSingleAsync(id);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Message.Should().Be(MessageConstant.Load);
        response.Item.Should().NotBeNull();
    }

    [Test]
    public async Task GetSingleAsync_WithNonExistingId_ShouldReturnNotFoundResponse()
    {
        // Arrange
        var id = Guid.NewGuid();
      
        // Act
        var response = await _genericService.GetSingleAsync(id);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeFalse();
        response.Message.Should().Be(MessageConstant.NotFound);
        response.Item.Should().BeNull();
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




