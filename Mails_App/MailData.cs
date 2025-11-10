namespace Mails_App
{
    public class MailData
    {
        public string? EmailToId { get; set; }
        public string? EmailToName { get; set; }
        public string? EmailSubject { get; set; }
        public string? EmailBody { get; set; }
         public string? FileName { get; set; }
        public bool? withAttatch { get; set; }
        public byte[] pdfBytes { get; set; }
        public string EmailToPhone { get; set; }
        public List<string> ccEmails { get; set; } = new List<string>();
    }
}
