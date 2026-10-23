using FinanceHub.Domain.DTOS.Input.AccountReceivable;
using FinanceHub.Domain.Enums;
using FluentValidation;

namespace FinanceHub.Domain.Validations.AccountReceivable;

public class CreateAccountReceivableValidation : AbstractValidator<CreateAccountReceivableRequest>
{
    public CreateAccountReceivableValidation()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(100);

        RuleFor(x => x.Value)
            .GreaterThan(0)
            .WithMessage("O valor da conta deve ser maior que R$ 0,00.");

        RuleFor(x => x.ReceiveDate)
            .NotEmpty().WithMessage("A data deve ser informada.");
        
        RuleFor(x => x.ReceiveDate)
            .NotEmpty()
            .WithMessage("A data deve ser informada.");

        
        RuleFor(x => x.ReceiveStatus)
            .IsInEnum()
            .Must(s => s is EReceivableStatus.Pending or EReceivableStatus.Received)
            .WithMessage("Informe um status válido (Pendente ou Recebido).");

        
    }
}