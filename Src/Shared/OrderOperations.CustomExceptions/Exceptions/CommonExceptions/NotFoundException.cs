using OrderOperations.CustomExceptions.Common;

namespace OrderOperations.CustomExceptions.Exceptions.CommonExceptions;

public class NotFoundException : BaseCustomExceptions
{
    public NotFoundException(string message, string param1 = "", string param2 = "", string param3 = "", int statusCode = 404) : base(message, param1, param2, param3, statusCode)
    {
    }
}
