namespace FlowWing.API.Helpers;

public class EmailConnectionServices
{
    public string _smtpServer { get; set; }
    public int _smtpPort { get; set; }
    public string _senderEmail { get; set; }
    public string _senderPassword { get; set; }
}