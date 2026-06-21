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
    }
    
    public static class Bill
    {
        public static Error BillNotFound =>
            Error.NotFound(code: "Bill.NotFound", description: "Fatura não encontrada.");
    }
}