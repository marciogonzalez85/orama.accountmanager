# orama.accountmanager
Desafio Técnico para Desenvolvedor DotNet - Órama

*Execução:*<br/>
Executar <code>docker-compose up</code> no diretório da solução <br />

O seed está populando a base de dados com dados fake.
Para testar a integração de ponta a ponta, editar o registro da tabela [account_manager].[dbo].[Clientes] modificando o valor do campo WebhookUrl para uma url válida.<br />
Dessa forma, irá receber a notificação da transação nesse endereço. <br /><br />
*Dica: Para testes, utilizo a ferramenta <a href="https://webhook.site/">Webhook.Site</a>. Lá é possível receber uma url válida, realizar o update no campo da tabela informada acima, e ver a notificação chegando na página da ferramenta. <br /><br />
O acesso ao servidor MSSQL se dá através do host: <code>localhost:1500</code>, sa password: <code>oramaDbServerPass@#</code> de acordo com o arquivo docker-compose.yml <br />

O swagger da api se encontra em: <code>http://localhost:8090/swagger/index.html</code><br />

As chamadas devem incluir o header <code>x-api-key</code>.<br /> 
Esse valor é configurado em <code>services.account_manager_api.environment.SECRET_API_KEY</code> no arquivo docker-compose.yml




