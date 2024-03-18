using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowWing.Entities
{
    public class Attachment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public int EmailLogId{ get; set; }
        
        public string FileName { get; set; }
        public long FileSize { get; set; }
        
        public string ContentType { get; set; }
        
        public byte[] Data { get; set; }
        
        [ForeignKey("EmailLogId")]
        public virtual EmailLog EmailLog { get; set; }
    }
}