using FluentValidation;
using MediatR;
using Orama.AccountManager.Application.DTO;
using Orama.AccountManager.CrossCutting.Common;
using Orama.AccountManager.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orama.AccountManager.Application.Queries.BankAccounts
{
    public class GetStatementQuery : IRequest<Result<GetStatementQueryResponse>>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AccountID { get; set; }

        public GetStatementQuery(DateTime startDate, DateTime endDate, int accountId)
        {
            StartDate = startDate;
            EndDate = endDate;
            AccountID = accountId;
        }
    }

    public class GetStatementQueryResponse
    {
        public List<TransactionDTO> Transactions { get; set; }
    }

    public class GetStatementQueryHandler : IRequestHandler<GetStatementQuery, Result<GetStatementQueryResponse>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IValidator<GetStatementQuery> _validator;

        public GetStatementQueryHandler(ITransactionRepository transactionRepository, IValidator<GetStatementQuery> validator)
        {
            _transactionRepository = transactionRepository;
            _validator = validator;
        }

        public async Task<Result<GetStatementQueryResponse>> Handle(GetStatementQuery query, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(query);

            if (!validationResult.IsValid)
                return Result<GetStatementQueryResponse>.BadRequest(validationResult.Errors.Select(a => (a.ErrorCode, a.ErrorMessage)).ToList());

            var transactions = (await _transactionRepository.GetAllAsync(cancellationToken))
                .Where(a => a.BankAccountId == query.AccountID && a.Date >= query.StartDate && a.Date <= query.EndDate).Select(a => new TransactionDTO
                {
                    Id = a.Id,
                    AccountID = a.BankAccountId,
                    AssetID = a.FinancialAssetId,
                    AssetName = a.FinancialAsset.Name,
                    Date = a.Date,
                    Quantity = a.Quantity,
                    TransactionType = a.TransactionType,
                    TotalValue = a.TotalValue
                }).ToList();

            return Result<GetStatementQueryResponse>.Ok(new GetStatementQueryResponse { Transactions = transactions });
        }
    }
}
