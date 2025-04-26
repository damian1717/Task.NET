using MediatR;
using Task.NET.Shared.Entities;
using Task.NET.Shared.Exceptions;

namespace Task.NET.Shared.Application;

public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, Result<TResponse>>
    where TRequest : IRequest<Result<TResponse>>
{
    protected readonly ILogger<BaseRequestHandler<TRequest, TResponse>> _logger;
    protected BaseRequestHandler(ILogger<BaseRequestHandler<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public abstract Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken);

    public async Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await HandleAsync(request, cancellationToken);
        }
        catch(DomainException ex)
        {
            _logger.LogError(ex, $"[Logging] [Domain] {ex.Message}");
            return Result<TResponse>.Failure(ex.Message);
        }
        catch(NotFoundException ex)
        {
            _logger.LogError(ex, $"[Logging] [NotFound] {ex.Message}");
            return Result<TResponse>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[Logging] {ex.Message}");
            return Result<TResponse>.Failure("An unexpected error occurred. Please try again later.");
        }
    }
}
