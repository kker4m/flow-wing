using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace FlowWing.Entities
{
    public class ScheduledEmail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime SentDateTime { get; set; }
        [Required]
        public string RecipientsEmail { get; set; }
        [Required]
        public string SenderEmail { get; set; }
        [Required]
        public string EmailSubject { get; set; }
        [Required]
        public string SentEmailBody { get; set; }
        public bool Status { get; set; }
        public virtual RepeatingMail RepeatingMail { get; set; }
    }
}
