using OrderOperations.CustomExceptions.Common;

namespace OrderOperations.CustomExceptions.Exceptions.AuthExceptions;

public class UserAlreadyExistException : BaseCustomExceptions
{
    public UserAlreadyExistException(string message, string param1 = "", string param2 = "", string param3 = "", int statusCode = 400) : base(message, param1, param2, param3, statusCode)
    { }
}
