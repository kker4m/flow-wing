using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowWing.Entities
{
    public class RepeatingMail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string RecipientsEmail { get; set; }
        [Required]
        public string SenderEmail { get; set; }
        [Required]
        public string EmailSubject { get; set; }
        [Required]
        public string SentEmailBody { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime RepeatInterval { get; set; }
        public DateTime NextScheduledDateTime { get; set; }
        public DateTime EndingDate { get; set; }
    }
}
