using AutoMapper;
using FluentAssertions;
using Moq;
using Scaffolding.Auth;
using Scaffolding.Data.Test.Entities;
using Scaffolding.Service.Infrastructure;
using Scaffoling.Service.Test.Model;

namespace Scaffoling.Service.Test.Infrastructure;
[TestFixture]
public class ReadWriteServiceBaseTests
{
    private Mock<IAuthenticationService> _authServiceMock;
    private Mock<IMapper> _mapperMock;
    private TestReadWriteService _service;

    [SetUp]
    public void Setup()
    {
        _authServiceMock = new Mock<IAuthenticationService>();
        _mapperMock = new Mock<IMapper>();
        _service = new TestReadWriteService(_authServiceMock.Object, _mapperMock.Object);
    }

    [Test]
    public void GetUserId_ShouldReturnUserId_WhenCalled()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _authServiceMock.Setup(a => a.GetUserId()).Returns(userId);

        // Act
        var result = _service.GetUserId();

        // Assert
        result.Should().Be(userId);
    }

    [Test]
    public void IsSuperAdmin_ShouldReturnTrue_WhenSuperAdmin()
    {
        // Arrange
        _authServiceMock.Setup(a => a.IsSuperAdmin()).Returns(true);

        // Act
        var result = _service.IsSuperAdmin();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void InitErrorResponse_GenericModel_ShouldReturnErrorResponse()
    {
        // Act
        var result = _service.TestInitErrorResponse<TestModel>();

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.StatusCode.Should().Be(500);
        result.Message.Should().Be("Error");
    }

    [Test]
    public void InitErrorResponse_NoGenericType_ShouldReturnErrorResponse()
    {
        // Act
        var result = _service.TestInitErrorResponse<TestModel>();

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.StatusCode.Should().Be(500);
        result.Message.Should().Be("Error");
    }

    [Test]
    public void InitErrorListResponse_GenericModel_ShouldReturnErrorListResponse()
    {
        // Act
        var result = _service.TestInitErrorListResponse<TestModel>();

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.StatusCode.Should().Be(500);
        result.Message.Should().Be("Error");
    }

    [Test]
    public void InitErrorListResponse_NoGenericType_ShouldReturnErrorListResponse()
    {
        // Act
        var result = _service.TestInitErrorListResponse<TestModel>();

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.StatusCode.Should().Be(500);
        result.Message.Should().Be("Error");
    }

    [Test]
    public void InitErrorResponse_PageIndexAndPageSize_ShouldReturnErrorResponse()
    {
        // Arrange
        int pageIndex = 1;
        int pageSize = 10;

        // Act
        var result = _service.TestInitErrorResponse<TestModel>(pageIndex, pageSize);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.StatusCode.Should().Be(500);
        result.Message.Should().Be("Error");
        result.Item.Should().NotBeNull();
        result.Item.Page.Should().Be(pageIndex);
        result.Item.PageSize.Should().Be(pageSize);
    }

    [Test]
    public void InitSuccessResponse_ShouldReturnSuccessResponse()
    {
        // Act
        var result = _service.TestInitSuccessResponse<TestModel>("Success");

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.StatusCode.Should().Be(200);
        result.Message.Should().Be("Success");
    }

    [Test]
    public void InitSuccessResponse_GenericModel_ShouldReturnSuccessResponse()
    {
        // Act
        var result = _service.TestInitSuccessResponse<TestModel>("Success");

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.StatusCode.Should().Be(200);
        result.Message.Should().Be("Success");
    }

    [Test]
    public void InitSuccessListResponse_ShouldReturnSuccessResponse()
    {
        // Act
        var result = _service.TestInitSuccessListResponse<TestModel>("Success");

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.StatusCode.Should().Be(200);
        result.Message.Should().Be("Success");
    }

    [Test]
    public void InitSuccessListResponse_GenericModel_ShouldReturnSuccessResponse()
    {
        // Act
        var result = _service.TestInitSuccessListResponse<TestModel>("Success");

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.StatusCode.Should().Be(200);
        result.Message.Should().Be("Success");
    }
}
