namespace FlowWing.API.Models;
public class ForwardedEmailLogModel
{
    public string RecipientsEmail { get; set; }
    public string EmailBody { get; set; }
    public int ForwardedEmailId { get; set; }
    public IEnumerable<IFormFile>? Files { get; set; }
}