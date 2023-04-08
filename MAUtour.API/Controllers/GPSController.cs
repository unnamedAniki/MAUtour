using Microsoft.AspNetCore.Mvc;
using MAUtour.Utils.Models;
using MAUtour.Utils.DbConnect;
using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MAUtour.API.DTOs;
using MapsterMapper;
using Mapster;

namespace MAUtour.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GPSController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<GPSController> _logger;
        public GPSController(ILogger<GPSController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("/pins", Name = "GetPins")]
        public IEnumerable<PinsDTO>? GetPins()
        {
            var pins = _context.UserPins.Include(p => p.Users).Include(p=>p.Type).ToList();
            if (pins.Any())
            {
                List<PinsDTO> getPins = pins.Adapt<List<PinsDTO>>();
                return getPins;
            } 
            return null;
        }

        [HttpGet("/pins/{name}", Name = "FindPin")]
        public IEnumerable<PinsDTO>? FindPin(string name)
        {
            var pins = _context.UserPins.Include(p => p.Users).Where(p => p.Name.Contains(name));
            if (pins is not null)
            {
                List<PinsDTO> getPins = pins.Adapt<List<PinsDTO>>();
                return getPins;
            }
            return null;
        }

        [HttpGet("/routes", Name = "GetRoutes")]
        public IEnumerable<GetRoutesDTO>? GetRoutes()
        {
            var routes = _context.PinRoutes
                .Include(p=>p.UserRoutes)
                .ThenInclude(p=>p.Users)
                .Include(p=>p.UserPins)
                .ThenInclude(p=>p.Type)
                .Include(p=>p.UserPins)
                .ThenInclude(p=>p.Users)
                .AsSplitQuery().ToList();

            if (routes.Any())
            {
                List<GetRoutesDTO> getRoutes = routes.Adapt<List<GetRoutesDTO>>();
                return getRoutes;
            }
            return null;
        }

        [HttpGet("/users", Name = "GetUsers")]
        public IEnumerable<UserDTO>? GetUsers()
        {
            var users = _context.UserRoles.Include(p => p.Users).Include(p => p.Roles).ToList();
            if (users.Any())
            {
                List<UserDTO> getUsers = users.Adapt<List<UserDTO>>();
                return getUsers;
            }
            return null;
        }

        [HttpGet("/users/{id}", Name = "FindUser")]
        public UserDTO? FindUsers(int id)
        {
            var users = _context.UserRoles.Include(p=>p.Users).Include(p=>p.Roles).FirstOrDefault(p=>p.UserId == id);
            if (users is not null)
            {
                UserDTO getUsers = users.Adapt<UserDTO>();
                return getUsers;
            }
            return null;
        }
    }
}