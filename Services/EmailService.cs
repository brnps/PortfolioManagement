using System;
using System.Linq;
using System.Collections.Generic;
using PortfolioManagement.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace PortfolioManagement.Services
{
    public class EmailService
    {
        private readonly List<ProdutoFinanceiro> produtos;
        private readonly string adminEmail = "admin@example.com"; // Email do administrador

        public EmailService(List<ProdutoFinanceiro> produtos)
        {
            this.produtos = produtos;
        }

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

        public void NotificarProdutosProximoVencimento()
        {
            var produtosProximoVencimento = produtos.Where(p => (p.DataVencimento - DateTime.Now).TotalDays <= 7).ToList();
            if (produtosProximoVencimento.Any())
            {
                var mensagem = "Os seguintes produtos estão próximos do vencimento:\n";
                foreach (var produto in produtosProximoVencimento)
                {
                    mensagem += $"- {produto.Nome}, Vencimento: {produto.DataVencimento.ToShortDateString()}\n";
                }
                EnviarEmail(adminEmail, "Produtos Próximos do Vencimento", mensagem);
            }
        }
    }
}
