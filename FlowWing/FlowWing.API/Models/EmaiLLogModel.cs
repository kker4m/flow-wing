namespace FlowWing.API.Models;

public class EmailLogModel
{
    public string RecipientsEmail { get; set; }
    public string EmailSubject { get; set; }
    public string EmailBody { get; set; }
    public List<string> Attachments { get; set; }
}