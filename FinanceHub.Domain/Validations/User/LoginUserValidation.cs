using FinanceHub.Domain.DTOS.Input;
using FluentValidation;

namespace FinanceHub.Domain.Validations.User;

public class LoginUserValidation : AbstractValidator<LoginUser>
{
    public  LoginUserValidation()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("O email é obrigatório.");
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("A senha é obrigatória.");
    }
}