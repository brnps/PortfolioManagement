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

### 5. Verificar o Envio de E-mails

Para verificar se o envio de e-mails está funcionando corretamente, você pode:

- **Verificar os Logs**: Acompanhe os logs do console para mensagens de sucesso ou erro durante o envio de e-mails.
- **Consultar o Endereço de E-mail**: Verifique o endereço de e-mail do administrador configurado para ver se as notificações são recebidas.

### 6. Testar os Endpoints da API

Use uma ferramenta de API, como o [Postman](https://www.postman.com/) ou o [Swagger UI](http://localhost:5000/swagger), para testar os endpoints disponíveis:

- **POST /api/transacoes/comprar**: Realiza uma compra.
  - **Exemplo de Requisição:**

    ```bash
    curl -X POST "http://localhost:5000/api/transacoes/comprar" -H "Content-Type: application/json" -d '{"clienteId": "cliente1", "produtoId": 1, "quantidade": 10}'
    ```

  - **Parâmetros de Requisição (JSON):**

    ```json
    {
      "clienteId": "string",
      "produtoId": "int",
      "quantidade": "int"
    }
    ```

  - **Resposta:**

    ```json
    {
      "success": true,
      "message": "Compra realizada com sucesso."
    }
    ```

- **POST /api/transacoes/vender**: Realiza uma venda.
  - **Exemplo de Requisição:**

    ```bash
    curl -X POST "http://localhost:5000/api/transacoes/vender" -H "Content-Type: application/json" -d '{"clienteId": "cliente1", "produtoId": 1, "quantidade": 5}'
    ```

  - **Parâmetros de Requisição (JSON):**

    ```json
    {
      "clienteId": "string",
      "produtoId": "int",
      "quantidade": "int"
    }
    ```

  - **Resposta:**

    ```json
    {
      "success": true,
      "message": "Venda realizada com sucesso."
    }
    ```

- **GET /api/transacoes/{clienteId}**: Consulta o extrato de um cliente.
  - **Exemplo de Requisição:**

    ```bash
    curl -X GET "http://localhost:5000/api/transacoes/cliente1"
    ```

  - **Resposta:**

    ```json
    {
      "clienteId": "cliente1",
      "transacoes": [
        {
          "produtoId": 1,
          "quantidade": 10,
          "tipo": "compra",
          "data": "2024-07-23T12:34:56Z"
        },
        {
          "produtoId": 1,
          "quantidade": 5,
          "tipo": "venda",
          "data": "2024-07-24T08:30:00Z"
        }
      ]
    }
    ```

### 7. Configurar o Agendamento de Tarefas

A aplicação utiliza o `Quartz` para agendar o envio diário de e-mails. Certifique-se de que a configuração do agendador no arquivo `Program.cs` está correta para sua necessidade.

### 8. Encerrar a Aplicação

Para parar a aplicação, pressione `Ctrl+C` no terminal onde a aplicação está sendo executada.

## Documentação Adicional

- **[Documentação do .NET](https://docs.microsoft.com/en-us/dotnet/core/)**: Para mais informações sobre o .NET.
- **[Documentação do MailKit](https://github.com/jstedfast/MailKit)**: Para detalhes sobre o uso do MailKit.
- **[Documentação do Quartz](https://www.quartz-scheduler.net/)**: Para mais detalhes sobre agendamento de tarefas com Quartz.

## Resolução de Problemas

- **Não Consigo Enviar E-mails:**
  - Verifique as credenciais do servidor SMTP e a configuração da porta.
  - Certifique-se de que seu servidor SMTP está acessível a partir do ambiente onde a aplicação está executando.

- **Endpoints Não Funcionam:**
  - Verifique se a aplicação está em execução e escutando na porta correta (`http://localhost:5000` por padrão).
  - Verifique os logs da aplicação para mensagens de erro.

