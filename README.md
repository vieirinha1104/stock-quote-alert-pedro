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

Monitoring PETR4...
Alert if price < 31,7 or > 31,9

Current stock price is: 31,94
Price is greater than the upper bound.

Current stock price is: 31,95
Price is greater than the upper bound.
Sell alert already sent. Waiting for cooldown.

Current stock price is: 31,89

Current stock price is: 31,69
Price is lower than the lower bound.

Current stock price is: 31,68
Price is lower than the lower bound.
Buy alert already sent. Waiting for cooldown

Arquivo de Configuração JsonConfigFile.json:

{
    "SmtpHost": "smtp.gmail.com", // "smtp.seuprovedor.com"
    "HostEmail": "stockalertpedro@gmail.com", // seuemail@provedor.com"
    "HostPassword": "xrcp heiy cfpy nvtb", // sua senha
    "EmailTo": "peteramvs@gmail.com", // endereço de email que vai receber o alerta
    "Port": 587, // porta default do smtp gmail
    "UseSsl": true
}

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

