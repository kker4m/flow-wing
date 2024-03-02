using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowWing.Entities
{
    public class EmailLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        
        public DateTime? SentDateTime { get; set; }
        [Required]
        public string RecipientsEmail { get; set; }
        [Required]
        public string SenderEmail { get; set; }
        [Required]
        public string EmailSubject { get; set; }
        [Required]
        public string SentEmailBody { get; set; }
        
        public string? AttachmentIds { get; set; }
        
        [Required]
        public bool Status { get; set; }
        [Required]
        public bool IsScheduled { get; set; }
        
        public String? Answers { get; set; }
        
        [ForeignKey("UserId")] 
        public virtual User User { get; set; }

    }
}
