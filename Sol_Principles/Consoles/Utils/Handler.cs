namespace Consoles.Utils;

public interface IHandler<TRequest, TResponse>
    where TRequest : class
    where TResponse : class
{
    Task<TResponse> HandleAsync(TRequest request);
}