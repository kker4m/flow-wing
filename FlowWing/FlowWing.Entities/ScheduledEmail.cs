using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace FlowWing.Entities
{
    public class ScheduledEmail
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Status { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }

        public virtual User User { get; set; }
    }
}
