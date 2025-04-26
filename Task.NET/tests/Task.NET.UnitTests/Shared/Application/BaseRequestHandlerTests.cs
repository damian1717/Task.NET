using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Task.NET.Shared.Application;
using Task.NET.Shared.Entities;
using Task.NET.Shared.Exceptions;

namespace Task.NET.UnitTests.Shared.Application;

public class BaseRequestHandlerTests
{
    private readonly Mock<ILogger<BaseRequestHandler<TestRequest, string>>> _loggerMock;

    public BaseRequestHandlerTests()
    {
        _loggerMock = new Mock<ILogger<BaseRequestHandler<TestRequest, string>>>();
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ShouldReturnSuccessResult_WhenNoExceptionOccurs()
    {
        // Arrange
        var handler = new TestRequestHandler(_loggerMock.Object);
        var request = new TestRequest();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Success", result.Value);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ShouldReturnFailureResult_WhenDomainExceptionOccurs()
    {
        // Arrange
        var handler = new TestRequestHandler(_loggerMock.Object, throwDomainException: true);
        var request = new TestRequest();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Domain exception occurred", result.Error);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ShouldReturnFailureResult_WhenNotFoundExceptionOccurs()
    {
        // Arrange
        var handler = new TestRequestHandler(_loggerMock.Object, throwNotFoundException: true);
        var request = new TestRequest();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Not found exception occurred", result.Error);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ShouldReturnFailureResult_WhenGeneralExceptionOccurs()
    {
        // Arrange
        var handler = new TestRequestHandler(_loggerMock.Object, throwGeneralException: true);
        var request = new TestRequest();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("An unexpected error occurred. Please try again later.", result.Error);
    }

    public class TestRequest : IRequest<Result<string>> { }

    private class TestRequestHandler : BaseRequestHandler<TestRequest, string>
    {
        private readonly bool _throwDomainException;
        private readonly bool _throwNotFoundException;
        private readonly bool _throwGeneralException;

        public TestRequestHandler(
            ILogger<BaseRequestHandler<TestRequest, string>> logger,
            bool throwDomainException = false,
            bool throwNotFoundException = false,
            bool throwGeneralException = false)
            : base(logger)
        {
            _throwDomainException = throwDomainException;
            _throwNotFoundException = throwNotFoundException;
            _throwGeneralException = throwGeneralException;
        }

        public override async Task<Result<string>> HandleAsync(TestRequest request, CancellationToken cancellationToken)
        {
            if (_throwDomainException)
                throw new DomainException("Domain exception occurred");

            if (_throwNotFoundException)
                throw new NotFoundException("Not found exception occurred");

            if (_throwGeneralException)
                throw new Exception("General exception occurred");

            return await System.Threading.Tasks.Task.FromResult(Result<string>.Success("Success"));
        }
    }
}