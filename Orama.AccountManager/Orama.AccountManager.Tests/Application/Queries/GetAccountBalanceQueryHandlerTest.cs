using Moq;
using NUnit.Framework;
using Orama.AccountManager.Application.Queries.BankAccounts;
using Orama.AccountManager.Model.Repositories;
using Xunit;

namespace Orama.AccountManager.Tests.Application.Queries
{
    public class GetAccountBalanceQueryHandlerTest : IClassFixture<Fixture>
    {
        private readonly Fixture _fixture;
        private readonly Mock<IBankAccountRepository> _bankAccountRepositoryMock;
        private readonly GetAccountBalanceQueryHandler _handler;

        public GetAccountBalanceQueryHandlerTest()
        {
            _fixture = new Fixture();

            _bankAccountRepositoryMock = new Mock<IBankAccountRepository>();
            _bankAccountRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns<int, CancellationToken>((id, cancelationToken) =>
                {
                    var account = _fixture.BankAccounts.SingleOrDefault(a => a.Id == id);
                    return Task.FromResult(account);
                });

            _handler = new GetAccountBalanceQueryHandler(_bankAccountRepositoryMock.Object);
        }

        [Test]
        [Fact(DisplayName = "Dado accountId válido, deve retornar Account selecionada")]
        public void DadoAccountIdValidoDeveRetornarAccountSelecionada()
        {
            // arrange
            var accountId = DadosExemplo.Accounts.First().Id;
            var query = new GetAccountBalanceQuery();
            query.AtribuirId(accountId);

            // act
            var result = _handler.Handle(query, CancellationToken.None).Result;

            // assert
            _bankAccountRepositoryMock.Verify(x => x.GetAsync(accountId, CancellationToken.None), Times.Once());
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }

        [Test]
        [Fact(DisplayName = "Dado accountId inválido, deve retornar null")]
        public void DadoAccountIdInvalidoDeveRetornarNull()
        {
            // arrange
            var accountId = Int32.MaxValue;
            var query = new GetAccountBalanceQuery();
            query.AtribuirId(accountId);

            // act
            var result = _handler.Handle(query, CancellationToken.None).Result;

            // assert
            _bankAccountRepositoryMock.Verify(x => x.GetAsync(accountId, CancellationToken.None), Times.Once());
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Null(result.Data);
        }
    }
}
