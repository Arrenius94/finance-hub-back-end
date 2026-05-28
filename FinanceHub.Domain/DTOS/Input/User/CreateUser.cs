namespace FinanceHub.Domain.DTOS.Input;

public class CreateUser
{
    public required string Name { get; set; }
    public required string SecondName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public DateTime DateBirth { get; set; }
    public decimal? Wallet { get; set; }
}