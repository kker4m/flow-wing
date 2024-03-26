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
        [Required]
        public int EmailLogId { get; set; }
        
        [Required]
        public bool IsRepeating { get; set; }
        
        public DateTime? LastSendingDate { get; set; }
        public DateTime? NextSendingDate { get; set; }
        public String? RepeatInterval { get; set; }
        public DateTime? RepeatEndDate { get; set; }

        [ForeignKey("EmailLogId")] 
        public virtual EmailLog EmailLog { get; set; }
        
    }
}
