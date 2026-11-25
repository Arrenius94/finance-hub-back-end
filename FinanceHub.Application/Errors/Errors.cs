namespace FinanceHub.Application.Errors;

using ErrorOr;

public static class AppErrors
{
    public static class Authentication
    {
        public static Error InvalidCredentials =>
            Error.Unauthorized(code: "Auth.InvalidCredentials", description: "Credenciais inválidas.");
        public static Error RequiresEmailVerification => Error.Custom(
            type: 403,
            code: "Auth.RequiresEmailVerification",
            description: "Primeiro acesso detectado. Enviamos um código de 4 dígitos para o seu e-mail para liberar a conta.");

        public static Error InvalidCode => Error.Validation(
            code: "Auth.InvalidCode",
            description: "Código de verificação incorreto. Confira no seu e-mail e tente novamente.");

        public static Error CodeExpired => Error.Validation(
            code: "Auth.CodeExpired",
            description: "O código expirou! Solicite um novo e-mail pra você.");
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
        public static Error NameRequired =>
            Error.Validation(code: "Category.NameRequired", description: "Informe o nome da categoria.");

        public static Error NameInUse =>
            Error.Conflict(code: "Category.NameInUse", description: "Já existe uma categoria com esse nome.");

        public static Error NatureRequired =>
            Error.Validation(code: "Category.NatureRequired", description: "Informe se a categoria é de despesa ou receita.");

        public static Error WrongNature =>
            Error.Validation(code: "Category.WrongNature", description: "Esta categoria não pertence a este tipo de conta.");

        public static Error NotFound =>
            Error.NotFound(code: "Category.NotFound", description: "Categoria não encontrada.");

        public static Error Unauthorized =>
            Error.Unauthorized(code: "Category.Unauthorized", description: "Acesso negado para esta categoria.");

        public static Error SameCategory =>
            Error.Validation(code: "Category.SameCategory", description: "Escolha uma categoria diferente para receber as contas.");

        public static Error HasBills(string categoryName) =>
            Error.Validation(
                code: "Category.HasBills",
                description: $"A categoria '{categoryName}' possui contas vinculadas. Mova essas contas antes de excluir.",
                metadata: new Dictionary<string, object>
                {
                    { "categoryName", categoryName }
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
            Error.NotFound(code: "Bill.DifferentList", description: "Alguma Conta não foi encontrada.");

        public static Error NoBillsToDelete =>
            Error.Validation(code: "Bill.NoBillsToDelete", description: "Nenhuma conta selecionada para exclusão.");
    }

    public static class AccountReceivable
    {
        public static Error NotFound =>
            Error.NotFound(code: "AccountReceivable.NotFound", description: "Contas a receber não encontrada.");

        public static Error Void =>
            Error.NotFound(code: "AccountReceivable.Void", description: "Nenhuma conta selecionada");

        public static Error UserNotAuthorized =>
            Error.Validation(code: "AccountReceivable.UserNotAuthorized", description: "Usuário sem permissão para ver essas contas!");

        public static Error DifferentList =>
            Error.NotFound(code: "AccountReceivable.DifferentList", description: "Algum pagamento não foi encontrado.");

        public static Error NoAccountsToDelete =>
            Error.Validation(code: "AccountReceivable.NoAccountsToDelete", description: "Nenhuma conta selecionada para exclusão.");

        public static Error DifferentListAccounts =>
            Error.NotFound(code: "AccountReceivable.DifferentList", description: "Alguma conta a receber não foi encontrada.");
    }
}