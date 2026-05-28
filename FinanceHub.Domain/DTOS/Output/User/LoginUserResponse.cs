namespace FinanceHub.Domain.DTOS.Output.User;

public class LoginUserResponse
{
    public string Username { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}