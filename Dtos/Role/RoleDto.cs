using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnlineStore.Dtos.Role
{
    public class RoleDto
    {
        [Required]
        public string Name { get; set; } = null!;
    }

    public enum enRoles { Admin, User, Manager}
    public class UserRoleDto
    {
        [Required(ErrorMessage ="UserId is required")]
        public string UserId { get; set; } = null!;
        
        [Required]
        public ICollection<enRoles> Roles { get; set; } = new List<enRoles>();
    }
}
