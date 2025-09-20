using OrderOperations.CustomExceptions.Common;

namespace OrderOperations.CustomExceptions.Exceptions.AuthExceptions;

public class LoginFailedException : BaseCustomExceptions
{
    public LoginFailedException(string message, string param1 = "", string param2 = "", string param3 = "", int statusCode = 401) : base(message, param1, param2, param3, statusCode)
    {
    }
}