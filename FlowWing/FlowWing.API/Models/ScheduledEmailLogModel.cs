namespace FlowWing.API.Models;

public class ScheduledEmailLogModel
{
    public DateTime SentDateTime { get; set; }
    public string RecipientsEmail { get; set; }
    public string EmailSubject { get; set; }
    public string EmailBody { get; set; }

}