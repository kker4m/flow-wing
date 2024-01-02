namespace FlowWing.API.Models;

public class ScheduledEmailLogModel
{
    public EmailLogModel EmailLog { get; set; }
    public DateTime? NextSendingDate { get; set; }

}