namespace FinanceHub.Application.Errors;
using ErrorOr;

public static class AppErrors
{
    public static class Authentication
    {
        public static Error InvalidCredentials =>
            Error.Unauthorized(code: "Auth.InvalidCredentials", description: "Credenciais inválidas.");
    }

    public static class User
    {
        public static Error EmailAlreadyInUse =>
            Error.Conflict(code: "User.EmailInUse", description: "Email já cadastrado.");
        
        public static Error NotFound =>
            Error.NotFound(code: "User.NotFound", description: "Usuário não encontrado.");
    }

    public static class Bill
    {
        public static Error BillNotFound =>
            Error.NotFound(code: "Bill.NotFound", description: "Fatura não encontrada.");
    }
}