using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowWing.Entities
{
    public class EmailLog
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime SentDateTime { get; set; }
        public int UserID { get; set; }
        public string RecipientEmail { get; set; }
        public string SenderEmail { get; set; }
        public string EmailSubject { get; set; }
        public string SentEmailBody { get; set; }

        public virtual User User { get; set; }
    }
}
