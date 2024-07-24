# Sistema de Gestão de Portfólio de Investimentos

## Visão Geral

Este projeto é um sistema de gestão de portfólio de investimentos que permite gerenciar produtos financeiros e realizar transações de compra e venda. Além disso, a aplicação envia notificações diárias por e-mail sobre produtos com vencimento próximo.

## Requisitos

Antes de executar a aplicação, certifique-se de que você tem os seguintes requisitos instalados:

- **.NET 8 SDK**: [Baixar e instalar o .NET SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Editor de Código**: Recomendado [Visual Studio Code](https://code.visualstudio.com/) com a extensão C# instalada.
- **Servidor SMTP**: Necessário para o envio de e-mails. Configure seu servidor SMTP (por exemplo, Gmail, Outlook, etc.).

## Passos para Executar a Aplicação

### 1. Clonar o Repositório

Clone o repositório contendo o código da aplicação para o seu ambiente local:

```bash
git clone https://github.com/usuario/nome-do-repositorio.git
cd nome-do-repositorio
```

### 2. Restaurar Dependências

Restaure as dependências do projeto usando o comando `dotnet restore`:

```bash
dotnet restore
```

### 3. Configurar o Servidor SMTP

Modifique a configuração do servidor SMTP no arquivo `Services/EmailService.cs` para corresponder às suas credenciais e configuração do servidor de e-mail:

```csharp
public void EnviarEmail(string email, string assunto, string mensagem)
{
    var message = new MimeMessage();
    message.From.Add(new MailboxAddress("Portfolio Management", "noreply@example.com"));
    message.To.Add(new MailboxAddress("", email));
    message.Subject = assunto;
    message.Body = new TextPart("plain")
    {
        Text = mensagem
    };

    using (var client = new SmtpClient())
    {
        client.Connect("smtp.example.com", 587, false); // Configure o servidor SMTP
        client.Authenticate("your-email@example.com", "your-email-password"); // Autenticação

        client.Send(message);
        client.Disconnect(true);
    }
}
```

Substitua "smtp.example.com", "your-email@example.com" e "your-email-password" pelos detalhes do seu servidor SMTP.

### 4. Executar a Aplicação

Compile e execute a aplicação usando o comando `dotnet run`:

```bash
dotnet run
```

A aplicação será iniciada e estará disponível no http://localhost:5000 ou https://localhost:5001 por padrão.
