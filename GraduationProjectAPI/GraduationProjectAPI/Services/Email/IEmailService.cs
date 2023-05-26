namespace GraduationProjectAPI.Services.Email
{

    public interface IEmailService
    {
        public Task<bool> SendEmailFromGmail(string toEmail, string subject, string body);
    }

}
