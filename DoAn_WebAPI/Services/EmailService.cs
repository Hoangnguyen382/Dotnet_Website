using MailKit.Security;
using MailKit.Net.Smtp;
using MimeKit;
using DoAn_WebAPI.Interfaces.IService;

namespace DoAn_WebAPI.Services
{
    public class EmailService : IEmailService
    {
        // biến load các biến mỗi trường form appsettings json
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            // Khởi tạo email mime
            var email = new MimeMessage();
            // set thông tin người gửi
            email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:From"]));
            // set thông tin người nhận
            email.To.Add(MailboxAddress.Parse(to));
            // set tiêu đề email
            email.Subject = subject;
            // set nội dung email
            var builder = new BodyBuilder{
                HtmlBody = body
            };
            email.Body = builder.ToMessageBody();
            // Khởi tạo smtp client
            var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(_configuration["EmailSettings:SmtpServer"],
            int.Parse(_configuration["EmailSettings:Port"]),
            SecureSocketOptions.StartTls);

            // đăng nhập vào tài khoản email
            await smtpClient.AuthenticateAsync(_configuration["EmailSettings:Username"],
            _configuration["EmailSettings:Password"]);
            // gửi email
            await smtpClient.SendAsync(email);
            // ngắt kết nối
            await smtpClient.DisconnectAsync(true);
        }
    }
}