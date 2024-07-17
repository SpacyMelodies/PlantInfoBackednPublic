namespace PlantInfoBackend
{
    public class MailData
    {
        public string EmailToId { get; set; }
        public string EmailToName { get; set; }
        public string EmailSubject { get; set; } 
        public string EmailBody { get; set; }

        public MailData(string emailID, string emailName, string subject, string body) 
        {
           EmailToId = emailID;
           EmailToName = emailName;
           EmailSubject = subject;
           EmailBody = body;
        }
    }
}
