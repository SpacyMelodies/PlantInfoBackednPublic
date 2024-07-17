namespace PlantInfoBackend.Services
{
    public interface IMailService
    {
        public bool SendMail(MailData mailData);
    }
}

