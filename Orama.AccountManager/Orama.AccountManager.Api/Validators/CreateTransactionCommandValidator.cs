using FluentValidation;
using Orama.AccountManager.Application.Commands.Transactions;

namespace Orama.AccountManager.Api.Validators
{
    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator() 
        { 
            RuleFor(t => t.AssetID).NotEmpty().WithErrorCode("assetId").WithMessage("Ativo não informado");
            RuleFor(t => t.AccountID).NotEmpty().WithErrorCode("accountId").WithMessage("Conta não informada");
            RuleFor(t => t.Quantity).NotEmpty().WithErrorCode("quantity").WithMessage("Quantidade não informada");
            RuleFor(t => t.TransactionType).NotNull().WithErrorCode("transactionType").WithMessage("Tipo de transação não informada");
            RuleFor(t => t.ExternalReference).NotNull().WithErrorCode("externalReference").WithMessage("Código de referência da transação não informado");
        }
    }
}
