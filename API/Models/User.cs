using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class User
    {
        [Key]
        public Guid? Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email is invalid")]
        public string Email { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Credits { get; set; }

    }
}
