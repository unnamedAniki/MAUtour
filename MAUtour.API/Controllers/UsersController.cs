using Mapster;

using MAUtour.API.DTOs;
using MAUtour.API.Utils;
using MAUtour.Utils.DbConnect;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MAUtour.API.Controllers
{
    [Route("[controller]/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private UsersUtils UsersUtils;
        public UsersController(ApplicationContext context)
        {
            _context = context;
            UsersUtils = new UsersUtils(_context);
        }
        [HttpGet("/all", Name = "GetUsers")]
        public async Task<IEnumerable<UserDTO>?> GetUsersAsync()
        {
            var test = HttpContext.Request.Headers.Keys.FirstOrDefault(p=> p == "api-v1-key");
            if(test == null)
            {
                return null;
            }
            return await UsersUtils.GetUsersAsync();
        }

        [HttpGet("/{id}", Name = "FindUser")]
        public async Task<UserDTO?> FindUsersAsync(int id)
        {
            return await UsersUtils.FindUsersAsync(id);
        }
    }
}
