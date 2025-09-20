namespace OrderOperations.Application.DTOs.AuthorizationDTOs;

public class LoginResponseViewModel
{
    public string? UserId { get; set; }
    public string? Token { get; set; }
    public DateTime TokenStartDate { get; set; }
    public DateTime TokenEndDate { get; set; }
}
