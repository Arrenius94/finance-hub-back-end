namespace FinanceHub.Domain.DTOS.Output.User;

public record UserPerfilView
(
    string Name,
    string SecondName,
    string Email,
    DateOnly  BirthDate
);