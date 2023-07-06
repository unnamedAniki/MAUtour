using Mapster;

using MAUtour.API.DTOs;
using MAUtour.Utils.DbConnect;

using Microsoft.EntityFrameworkCore;

namespace MAUtour.API.Utils
{
    public class UsersUtils
    {
        private readonly ApplicationContext _context;

        internal UsersUtils(ApplicationContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<UserDTO>?> GetUsersAsync()
        {
            var users = await _context.UserRoles.Include(p => p.Users).Include(p => p.Roles).ToListAsync();
            if (users.Any())
            {
                List<UserDTO> getUsers = users.Adapt<List<UserDTO>>();
                return getUsers;
            }
            return null;
        }

        public async Task<UserDTO?> FindUsersAsync(int id)
        {
            var users = await _context.UserRoles.Include(p => p.Users).Include(p => p.Roles).FirstOrDefaultAsync(p => p.UserId == id);
            if (users is not null)
            {
                UserDTO getUsers = users.Adapt<UserDTO>();
                return getUsers;
            }
            return null;
        }
    }
}
