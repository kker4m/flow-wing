namespace FlowWing.Entities
{
    public class User
    {
        public int Id{ get; set; }
        public string Email{ get; set; }
        public string Password{ get; set; }
        public string Username{ get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual ICollection<ScheduledEmail> ScheduledEmails { get; set; }
        public virtual ICollection<EmailLog> EmailLogs { get; set; }

    }
}
