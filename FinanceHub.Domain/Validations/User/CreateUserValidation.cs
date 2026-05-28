using System.Text.RegularExpressions;
using FinanceHub.Domain.DTOS.Input;
using FluentValidation;

namespace FinanceHub.Domain.Validations.User;

public class CreateUserValidation : AbstractValidator<CreateUser>
{
    public CreateUserValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(50).WithMessage("Nome deve ter no máximo 50 caracteres.");

        RuleFor(x => x.SecondName)
            .NotEmpty().WithMessage("Sobrenome é obrigatório.")
            .MaximumLength(50).WithMessage("Sobrenome deve ter no máximo 50 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .EmailAddress().WithMessage("Formato de email inválido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .Must(ValidatePassword)
            .WithMessage("A senha deve conter pelo menos 8 caracteres, incluindo letras maiúsculas, minúsculas, números e caracteres especiais.")
            .MinimumLength(6).WithMessage("Senha deve ter no mínimo 6 caracteres.");

        RuleFor(x => x.DateBirth)
            .NotEmpty().WithMessage("Data de nascimento é obrigatória.")
            .LessThan(DateTime.Now).WithMessage("Data de nascimento deve ser no passado.");
    }

    public bool ValidatePassword(string password)
    { 
       var regex = new Regex(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$");
       return regex.IsMatch(password);
    }
}