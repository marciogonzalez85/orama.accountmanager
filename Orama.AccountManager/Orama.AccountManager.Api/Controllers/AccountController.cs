using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orama.AccountManager.Application.Queries.BankAccounts;
using Orama.AccountManager.CrossCutting.Common;
using System.Runtime.CompilerServices;

namespace Orama.AccountManager.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IMediator _mediatr;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IMediator mediatr, ILogger<AccountController> logger)
        {
            _mediatr = mediatr;
            _logger = logger;

        }

        [HttpGet("{accountId}/balance")]
        [ProducesResponseType(typeof(Result<GetAccountBalanceQueryResponse>), 200)]
        [ProducesResponseType(typeof(Result<GetAccountBalanceQueryResponse>), 404)]
        public async Task<IActionResult> GetBalance([FromRoute]int accountId, CancellationToken cancellationToken)
        {
            var query = new GetAccountBalanceQuery();
            query.AtribuirId(accountId);

            var resp = await _mediatr.Send(query, cancellationToken);

            return HandlerResponse(resp);
        }

        [HttpGet("{accountId}/statement")]
        [ProducesResponseType(typeof(Result<GetStatementQueryResponse>), 200)]
        public async Task<IActionResult> GetStatement(
            [FromRoute] int accountId, 
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
        CancellationToken cancellationToken)
        {
            var query = new GetStatementQuery(startDate, endDate, accountId);

            var resp = await _mediatr.Send(query, cancellationToken);

            return HandlerResponse(resp);
        }
    }
}
