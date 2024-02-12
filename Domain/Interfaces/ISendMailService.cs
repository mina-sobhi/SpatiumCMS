namespace Domain.Interfaces
{
    public interface ISendMailService
    {
        public Task<bool> SendMail(string MailTo, string Subject, string Body);
    }
}
