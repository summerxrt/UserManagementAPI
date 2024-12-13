using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(20)]
        public required string Role { get; set; }
    }
}
