using OrderOperations.CustomExceptions.Common;

namespace OrderOperations.CustomExceptions.Exceptions.APIExceltions;

public class OperationFailException : BaseCustomExceptions
{
    public OperationFailException(string message, string param1 = "", string param2 = "", string param3 = "", int statusCode = 500) : base(message, param1, param2, param3, statusCode)
    {
    }
}
