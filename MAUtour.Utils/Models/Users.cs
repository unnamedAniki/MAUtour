using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUtour.Utils.Models
{
    [PrimaryKey("Id")]
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual ICollection<UserPins> Pins { get; set; } = new List<UserPins>();
        public virtual ICollection<UserRoles> Roles { get; set; } = new List<UserRoles>(); 
        public virtual ICollection<UserRoutes> Routes { get; set; } = new List<UserRoutes>();
    }
}
