namespace OrderOperations.WebApi.DTOs;

public class ResponseDTO
{
    public ResponseDTO(string status, string message, object data)
    {
        Status = status;
        Message = message;
        Data = data;
    }

    public string Status { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
}