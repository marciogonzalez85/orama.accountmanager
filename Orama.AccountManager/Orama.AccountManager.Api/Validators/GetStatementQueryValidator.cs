using FluentValidation;
using Orama.AccountManager.Application.Queries.BankAccounts;

namespace Orama.AccountManager.Api.Validators
{
    public class GetStatementQueryValidator : AbstractValidator<GetStatementQuery>
    {
        public GetStatementQueryValidator()
        {
            RuleFor(t => t.AccountID).NotEmpty().WithErrorCode("accountId").WithMessage("Conta não informada");
            RuleFor(t => t.StartDate).NotEmpty().WithErrorCode("startDate").WithMessage("Data de início não informada");
            RuleFor(t => t.EndDate).NotEmpty().WithErrorCode("endDate").WithMessage("Data de fim não informada");
        }
    }
}
