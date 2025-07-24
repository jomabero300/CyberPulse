using CyberPulse.Shared.Responses;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;

namespace CyberPulse.Backend.Helpers;

public class MailHelper : IMailHelper
{
    private IConfiguration _configuration;

    public MailHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<ActionResponse<string>> SendMail(string toName, string toEmail, string subject, string body, string language)
    {
        try
        {
            var from = _configuration["Mail:From"];

            var name = _configuration["Mail:NameEn"];

            if (language == "es")
            {
                name = _configuration["Mail:NameEs"];
            }

            var smtp = _configuration["Mail:Smtp"];

            var port = _configuration["Mail:Port"];

            var password = _configuration["Mail:Password"];

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(name, from));

            message.To.Add(new MailboxAddress(toName, toEmail));

            message.Subject = subject;

            // Añadir headers importantes
            message.Headers.Add("X-Mailer", "CyberPulse");
            message.Headers.Add("X-Priority", "3"); // Normal priority


            BodyBuilder bodyBuilder = new BodyBuilder
            {
                HtmlBody = body,
                TextBody=HtmlUtilities.StripTags(body),
                //TextBody = TextToHtml.StripTags(body) // Añadir versión texto plano
            };

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                // Configuración más robusta para Outlook
                client.Timeout = 30000; // 30 segundos timeout
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                // Conexión segura
                await client.ConnectAsync(smtp, int.Parse(port!),MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);
//                client.Connect(smtp, int.Parse(port!), false);

                await client.AuthenticateAsync(from, password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return new ActionResponse<string> { WasSuccess = true };
        }
        catch (Exception ex)
        {
            return new ActionResponse<string>
            {
                WasSuccess = false,
                Message = ex.Message,
            };
        }
    }
}
