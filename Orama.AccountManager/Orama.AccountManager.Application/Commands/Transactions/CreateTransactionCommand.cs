using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using Orama.AccountManager.CrossCutting.Common;
using Orama.AccountManager.Infrastructure.RMQ;
using Orama.AccountManager.Model.Code;
using System.Text.Json.Serialization;

namespace Orama.AccountManager.Application.Commands.Transactions
{
    public class CreateTransactionCommand : IRequest<Result<CreateTransactionCommandResponse>>
    {
        [JsonPropertyName("accountId")]
        public int AccountID { get; set; }

        [JsonPropertyName("assetId")]
        public int AssetID { get; set; }

        [JsonPropertyName("transactionType")]
        public TransactionType TransactionType { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("externalReference")]
        public string ExternalReference { get; set; }
    }

    public class CreateTransactionCommandResponse
    {
        public bool Sent { get; set; }
    }

    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Result<CreateTransactionCommandResponse>>
    {
        private readonly IValidator<CreateTransactionCommand> _validator;

        public CreateTransactionCommandHandler(IValidator<CreateTransactionCommand> validator)
        {
            _validator = validator;
        }

        public async Task<Result<CreateTransactionCommandResponse>> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                return Result<CreateTransactionCommandResponse>.BadRequest(validationResult.Errors.Select(a => (a.ErrorCode, a.ErrorMessage)).ToList());

            MsgProducer.PublishTransactionMessage(JsonConvert.SerializeObject(command));

            return Result<CreateTransactionCommandResponse>.Ok(new CreateTransactionCommandResponse { Sent = true });
        }
    }
}
