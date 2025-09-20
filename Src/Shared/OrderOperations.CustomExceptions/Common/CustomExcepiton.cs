namespace OrderOperations.CustomExceptions.Common;
public abstract class BaseCustomExceptions : Exception
{
    public readonly int _statusCode;
    public readonly string _message;
    public readonly string _param1 = null;
    public readonly string _param2 = null;
    public readonly string _param3 = null;

    public BaseCustomExceptions(string message, string param1, string param2, string param3, int statusCode) : base(message)
    {
        _statusCode = statusCode;
        _message = message;
        _param1 = param1;
        _param2 = param2;
        _param3 = param3;
    }
}