using FinanceHub.Domain.DTOS.Input.Bill;
using FluentValidation;

namespace FinanceHub.Domain.Validations.Bill;

public class CreateBillValidation : AbstractValidator<CreateBill>
{
    public CreateBillValidation()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição da conta é obrigatória.")
            .MaximumLength(100).WithMessage("A descrição deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Value)
            .GreaterThan(0).WithMessage("O valor da conta deve ser maior que zero.");

        RuleFor(x => x.DateDue)
            .NotEmpty().WithMessage("A data de vencimento é obrigatória.");
    } 
}