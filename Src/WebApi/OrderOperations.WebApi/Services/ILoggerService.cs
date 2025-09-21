namespace OrderOperations.WebApi.Services;

public interface ILoggerService
{
    public void Write(params object[] messageParams);
}
