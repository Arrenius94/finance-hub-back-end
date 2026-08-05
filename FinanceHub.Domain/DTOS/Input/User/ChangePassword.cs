namespace FinanceHub.Domain.DTOS.Input;

public class ChangePassword
{
    public string NewPassword { get; set; } =  string.Empty;
    public string NewPasswordConfirmation { get; set; } = string.Empty;
}