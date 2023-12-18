using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowWing.Entities
{
    public class User
    {
        [Key]
        public int Id{ get; set; }
        [Required]
        [EmailAddress]
        public string Email{ get; set; }
        [Required]
        public byte[] Password { get; set; }
        [Required]
        public string Username{ get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime CreationDate { get; set; }

    }   
}
