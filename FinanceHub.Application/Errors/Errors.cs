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
        
        public static Error PasswordsDoNotMatch =>
            Error.Validation(code: "User.PasswordsDoNotMatch", description: "As senhas não coincidem.");
        
        public static Error InsufficientBalance =>
            Error.Validation(code: "User.InsufficientBalance", description: "Saldo insuficiente.");
    }

    public static class Category
    {
        public static Error NameError =>
            Error.Conflict(code: "Category.NameInUse", description: "Nome da categoria é obrigatorio.");
        
        public static Error TypeCategoryError =>
            Error.Conflict(code: "Category.TypeError", description: "Tipo da categoria é obrigatorio.");
        
        public static Error NotFound =>
            Error.NotFound(code: "Category.NotFound", description: "Categoria não encontrado.");
        
        public static Error Unauthorized =>
            Error.Unauthorized(code: "Category.Unauthorized", description: "Acesso negado para esta categoria.");

        public static Error HasPendingBills(string categoryName) =>
            Error.Validation(
                code: "Category.HasPendingBills",
                description: $"A categoria '{categoryName}' possui contas pendentes.",
                metadata: new Dictionary<string, object>
                {
                    {"categoryName", categoryName}
                }
            );
    }

    public static class Bill
    {
        public static Error NotFound =>
            Error.NotFound(code: "Bill.NotFound", description: "Contas não encontrada.");
        
        public static Error NoBillsToPay =>
            Error.Validation(code: "Bill.NoBillsToPay", description: "Nenhuma conta selecionada para pagamento.");
        
        public static Error DifferentList =>
            Error.NotFound(code: "Bill.DifferentList", description: "Contas não tem a mesma lista.");
        
        public static Error NoBillsToDelete =>
            Error.Validation(code: "Bill.NoBillsToDelete", description: "Nenhuma conta selecionada para exclusão.");
    }
}