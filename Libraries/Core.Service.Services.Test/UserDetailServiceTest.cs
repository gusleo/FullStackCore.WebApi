using AutoMapper;
using Core.Data.Entities;
using Core.Data.Infrastructure;
using FluentAssertions;
using Moq;
using Scaffolding.Auth;
using Scaffolding.Auth.Entities;
using Scaffolding.Data.Repo;
using Scaffolding.Data;
using Scaffolding.Service.Infrastructure;
using MockQueryable.Moq;
using Scaffolding.Data.Entities;
using Core.Service.Models;
using Core.Data.Infrastructure.Repositories;
using Core.Data.Repositories;
using System.Linq.Expressions;

namespace Core.Service.Services.Test;

[TestFixture]
public class UserDetailServiceTests
{
    private Mock<ICoreUnitOfWork> _mockUnitOfWork;
    private Mock<IAuthenticationService> _mockAuthService;
    private Mock<IMapper> _mockMapper;
    private Mock<IDataContext> _mockDbContext;
    private Mock<ICoreContext> _mockCoreContext;

    private UserDetailService _userDetailService;
    private IList<UserDetail> _entities = GenerateData();

    [SetUp]
    public void Setup()
    {
        _mockDbContext = new Mock<IDataContext>();
        _mockCoreContext = new Mock<ICoreContext>();
        var mock = _entities.AsQueryable().BuildMockDbSet();
        _mockDbContext.Setup(c => c.Set<UserDetail>()).Returns(mock.Object);
        _mockCoreContext.Setup(c => c.Set<UserDetail>()).Returns(mock.Object);

        _mockUnitOfWork = new Mock<ICoreUnitOfWork>();
        _mockUnitOfWork
            .Setup(x => x.Set<UserDetail>()).Returns(new GenericRepository<UserDetail>(_mockDbContext.Object));

        var mockUserDetailRepository = new Mock<IUserDetailRepository>();

        _mockUnitOfWork.Setup(x => x.UserDetailRepository).Returns(new UserDetailRepository(_mockCoreContext.Object));

        _mockAuthService = new Mock<IAuthenticationService>();
        _mockMapper = new Mock<IMapper>();

        _userDetailService = new UserDetailService(_mockUnitOfWork.Object, _mockAuthService.Object, _mockMapper.Object);
    }

    [Test]
    public async Task FindUserAsync_WithClue_ShouldReturnSuccessResponseWithItems()
    {
        // Arrange
        var clue = "Test";
        var page = 1;
        var pageSize = 20;
        var expectedCountSearch = _entities.Where(x => x.FirstName.Contains(clue) || x.LastName.Contains(clue)).Count();

        var filteredEntities = _entities
        .Where(x => x.FirstName.Contains(clue) || x.LastName.Contains(clue))
        .ToList();

        var pagedEntities = new PaginationEntity<UserDetail>
        {
            Page = page,
            PageSize = pageSize,
            Items = filteredEntities,
            TotalCount = expectedCountSearch
        };

        // because Ef.Like can't solve with mockable entities
        _mockUnitOfWork.Setup(x => x.UserDetailRepository.FindByAsync(
            It.IsAny<Expression<Func<UserDetail, bool>>>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<Expression<Func<UserDetail, object>>[]>()))
            .ReturnsAsync(pagedEntities);

        var pagedModels = new PaginationSet<UserDetailModel> { Items = pagedEntities.Items.Select(x => new UserDetailModel
        {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
        }), Page = page, PageSize = pageSize, TotalCount = expectedCountSearch};

        _mockMapper.Setup(x => x.Map<PaginationEntity<UserDetail>>(It.IsAny<PaginationSet<UserDetailModel>>())).Returns(pagedEntities);
        _mockMapper.Setup(x => x.Map<PaginationSet<UserDetailModel>>(It.IsAny<PaginationEntity<UserDetail>>())).Returns(pagedModels);

        // Act
        var response = await _userDetailService.FindUserAsync(clue, page, pageSize);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Message.Should().Be(MessageConstant.Load);
        response.Item.Should().NotBeNull();
        response.Item.TotalCount.Should().Be(expectedCountSearch);
    }

    [Test]
    public async Task GetDetailAsync_WithExistingUserId_ShouldReturnSuccessResponseWithItem()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userDetailEntity = new UserDetail { User = new ApplicationUser { Id = userId } };
        

        // Act
        var response = await _userDetailService.GetDetailAsync(userId);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Message.Should().Be(MessageConstant.Load);
        response.Item.Should().NotBeNull();
    }

    [Test]
    public async Task GetDetailAsync_WithNonExistingUserId_ShouldReturnNotFoundResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
       

        // Act
        var response = await _userDetailService.GetDetailAsync(userId);

        // Assert
        response.Should().NotBeNull();
        response.Success.Should().BeFalse();
        response.Message.Should().Be(MessageConstant.NotFound);
        response.Item.Should().BeNull();
    }

    #region Private
    private static IList<UserDetail> GenerateData()
    {
        var entities = new List<UserDetail>()
        {
            new UserDetail { User = new ApplicationUser {Id = Guid.NewGuid()}, Id = Guid.NewGuid(), FirstName = "Test", LastName = "One" },
            new UserDetail { User = new ApplicationUser {Id = Guid.NewGuid()}, Id = Guid.NewGuid(), FirstName = "Test", LastName = "Two" },
            new UserDetail { User = new ApplicationUser {Id = Guid.NewGuid()}, Id = Guid.NewGuid(), FirstName = "Test", LastName = "Three" },
        };
        return entities;
    }
    #endregion

}
