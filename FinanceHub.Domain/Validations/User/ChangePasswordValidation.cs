using System.Text.RegularExpressions;
using FinanceHub.Domain.DTOS.Input;
using FluentValidation;

namespace FinanceHub.Domain.Validations.User;

public class ChangePasswordValidation : AbstractValidator<ChangePassword>
{
    public ChangePasswordValidation()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("A nova senha é obrigatória.")
            .Must(ValidatePassword)
            .WithMessage("A senha deve conter pelo menos 8 caracteres, incluindo letras maiúsculas, minúsculas, números e caracteres especiais.")
            .MinimumLength(6).WithMessage("A nova senha deve ter pelo menos 6 caracteres.");

        RuleFor(x => x.NewPasswordConfirmation)
            .NotEmpty().WithMessage("A confirmação da nova senha é obrigatória.")
            .Equal(x => x.NewPassword).WithMessage("As senhas não coincidem.");
        
        
    }
    
    public bool ValidatePassword(string password)
    { 
        var regex = new Regex(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$");
        return regex.IsMatch(password);
    }
}