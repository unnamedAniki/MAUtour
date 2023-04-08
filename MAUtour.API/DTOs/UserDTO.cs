using Microsoft.AspNetCore.Identity;

namespace MAUtour.API.DTOs
{
    public class UserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int AvailablePins { get; set; }
        public int AvailableRoutes { get; set; }
        public DateTime StartPaidDate { get; set; }
        public DateTime ExpiredPaidDate { get; set; }
        public RoleDTO RoleName { get; set; }
    }
}
