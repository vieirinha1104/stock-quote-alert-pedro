# Stock Quote Alert

Este projeto monitora o preço de uma ação da B3 (Bolsa de Valores brasileira) e envia alertas por e-mail com base em limites definidos pelo usuário.

## Pré-requisitos

- [.NET SDK 8.0 ou superior](https://dotnet.microsoft.com/download)
- Conexão com a internet
- Arquivo `JsonConfigFile.json` com a configuração de e-mail preenchida corretamente

##  Como compilar o projeto

1. Abra o terminal (PowerShell, CMD ou bash).
2. Navegue até a raiz do projeto (onde está o `.csproj` ou `.sln`):

powershell
cd caminho/para/Stock-Quote-Alert-main

3. Compile o projeto com:

dotnet build

Isso irá gerar os arquivos em:
bin\Debug\net8.0\

4. Após o build, navegue até a pasta onde o executável foi gerado:

cd .\bin\Debug\net8.0

5. Execute o programa com o seguinte formato:

.\stock-quote-alert-pedro.exe <TICKER> <LIMITE_SUPERIOR> <LIMITE_INFERIOR>

Ex:

.\stock-quote-alert-pedro.exe PETR4 31.90 31.70

Exemplo de saída no terminal:

Monitoring PETR4:
Alert if price < 32,05 or > 32,05

[25/07/2025 - 15:07:03] Ticker: PETR4 , Price: 32,06

[25/07/2025 - 15:07:03] [WARNING] Sell Alert. Price is greater than the upper bound.

[25/07/2025 - 15:07:09] Ticker: PETR4 , Price: 32,06

[25/07/2025 - 15:07:12] Ticker: PETR4 , Price: 32,06\

...

Arquivo de Configuração JsonConfigFile.json:
Este arquivo JSON contém as configurações necessárias para o envio de alertas por e-mail e o comportamento do monitoramento. O conteúdo padrão do arquivo é o seguinte:
{
  "SmtpHost": "smtp.gmail.com",
  "HostEmail": "stockalertpedro@gmail.com",
  "HostPassword": "xrcp heiy cfpy nvtb",
  "EmailTo": "peteramvs@gmail.com",
  "Port": 587,
  "UseSsl": true,
  "CoolDown": 300,
  "StopProgramIfMarketIsClosed": false
}
Descrição dos Campos:
SmtpHost: Endereço do servidor SMTP usado para enviar os e-mails (ex: smtp.gmail.com).

HostEmail: Conta de e-mail usada para envio dos alertas.

HostPassword: Senha ou código de aplicativo da conta de e-mail do remetente.

EmailTo: Endereço de e-mail que irá receber os alertas.

Port: Porta do servidor SMTP (ex: 587 para TLS).

UseSsl: Define se o envio de e-mails deve usar SSL (default true).

CoolDown: Intervalo mínimo (em segundos) entre dois alertas do mesmo tipo.
Por exemplo, se o preço ultrapassar o limite de venda (upper bound) e um e-mail for enviado, o programa aguardará pelo menos 300 segundos (5 minutos) antes de enviar outro alerta de venda, mesmo que a condição continue sendo atendida. Esse valor pode ser ajustado livremente pelo usuário.

StopProgramIfMarketIsClosed:

    - Se true, o programa será encerrado automaticamente caso o mercado esteja fechado.

    - Se false, o programa continuará rodando mesmo fora do horário de negociação.

- Importante: Certifique-se de que o arquivo JsonConfigFile.json esteja na mesma pasta do executável, ou especifique corretamente o caminho no código.

Observações:
1-) Se o SMTP Host for o Gmail (smtp.gmail.com), a senha que deve constar no json de configurações do programa não se trata da sua senha padrão do Gmail, portanto realize os seguintes passos:

- Ativar verificação em duas etapas na conta Google

- Gerar uma senha de app em: https://myaccount.google.com/apppasswords

- Usar essa senha no json de configuração do programa

2-) Se o SMTP Host for o Outlook (smtp.office365.com), o suporte a autenticação com usuário e senha simples está sendo descontinuado. Nesse caso, você também deve usar:

- Ativar verificação em duas etapas 
- Usar a senha de aplicativo

3-) No caso de o programa ativar a Exception: 'System.IO.FileNotFoundException: 'Could not find file 'C:\Users\pvieira\source\repos\stock-quote-alert-pedro\stock-quote-alert-pedro\bin\Debug\net8.0\JsonConfigFile.json'.':

- No Visual Studio, clique com botão direito do mouse no arquivo JsonConfigFile.json no Solution Explorer

- Vá para a janela Propriedades

- Altere as propriedades:

Copy to Output Directory → Copy if newer (ou Copy always)

Build Action → Content

