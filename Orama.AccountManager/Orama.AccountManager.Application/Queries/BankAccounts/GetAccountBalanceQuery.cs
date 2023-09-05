using MediatR;
using Orama.AccountManager.CrossCutting.Common;
using Orama.AccountManager.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orama.AccountManager.Application.Queries.BankAccounts
{
    public class GetAccountBalanceQuery : IRequest<Result<GetAccountBalanceQueryResponse>>
    {
        public int AccountId { get; private set; }

        public void AtribuirId(int id) => AccountId = id;
    }

    public class GetAccountBalanceQueryResponse
    {
        public decimal Balance { get; set; }
    }

    public class GetAccountBalanceQueryHandler : IRequestHandler<GetAccountBalanceQuery, Result<GetAccountBalanceQueryResponse>>
    {
        private readonly IBankAccountRepository _bankAccountRepository;

        public GetAccountBalanceQueryHandler(IBankAccountRepository bankAccountRepository)
        {
            _bankAccountRepository = bankAccountRepository;
        }

        public async Task<Result<GetAccountBalanceQueryResponse>> Handle(GetAccountBalanceQuery request, CancellationToken cancellationToken)
        {
            var bankAccount = await _bankAccountRepository.GetAsync(request.AccountId, cancellationToken);

            return bankAccount == null ?
                Result<GetAccountBalanceQueryResponse>.NotFound("Account", "Account not found")
                : Result<GetAccountBalanceQueryResponse>.Ok(new GetAccountBalanceQueryResponse
                {
                    Balance = bankAccount.Balance
                });
        }
    }
}
 