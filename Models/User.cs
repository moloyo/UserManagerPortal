using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class User : IEntity
    {
        [Key]
        public Guid? Id { get; set; }

        [Required]
        [Length(1, 50)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email is invalid")]
        public string Email { get; set; }

        [Required]
        [Range(0, uint.MaxValue)]
        public uint Credits { get; set; }

    }
}
