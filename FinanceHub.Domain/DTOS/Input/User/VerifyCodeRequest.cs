namespace FinanceHub.Domain.DTOS.Input;

public record VerifyCodeRequest(string Email, string Code);