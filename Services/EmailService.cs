using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

public class EmailService
{
    private readonly string _smtpServer = "smtp.gmail.com"; 
    private readonly int _smtpPort = 587; 
    private readonly string _smtpUsername = "yourmeditechcompany@gmail.com"; 
    private readonly string _smtpPassword = "kfuj yhwq rofo adxx";

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("MediTech", _smtpUsername));
        email.To.Add(new MailboxAddress("", toEmail));
        email.Subject = subject;

        email.Body = new TextPart("plain") { Text = body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_smtpUsername, _smtpPassword);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
