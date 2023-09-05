using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orama.AccountManager.Application.Commands.Transactions;
using Orama.AccountManager.CrossCutting.Common;

namespace Orama.AccountManager.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TransactionController : BaseController
    {
        private readonly IMediator _mediatr;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(IMediator mediatr, ILogger<TransactionController> logger)
        {
            _mediatr = mediatr;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Result<CreateTransactionCommandResponse>), 200)]
        [ProducesResponseType(typeof(Result<CreateTransactionCommandResponse>), 400)]
        public async Task<IActionResult> CreateTransaction(
            [FromBody] CreateTransactionCommand command,
            CancellationToken cancellationToken)
        {
            var resp = await _mediatr.Send(command, cancellationToken);

            return HandlerResponse(resp);
        }
    }
}
