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
        private readonly IMapper _mapper;
        public GPSController(ILogger<GPSController> logger, ApplicationContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("/pins", Name = "GetPins")]
        public IEnumerable<UserPins>? GetPins()
        {
            var pins = _context.UserPins.ToList();
            if (pins.Any())
            {
                return pins;
            } 
            return null;
        }
        [HttpGet("/routes", Name = "GetRoutes")]
        public List<GetRoutesDTO>? GetRoutes()
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
                TypeAdapterConfig<UserRoutes, GetRoutesDTO>
                    .NewConfig()
                    .Map(dest => dest.Name, scr => scr.Name)
                    .Map(dest => dest.Description, scr => scr.Description)
                    .Map(dest => dest.UserName, scr => scr.Users.Name);

                TypeAdapterConfig<UserPins, PinsDTO>
                    .NewConfig()
                    .Map(dest => dest.Name, scr => scr.Name)
                    .Map(dest => dest.Description, scr => scr.Description)
                    .Map(dest => dest.UserName, scr => scr.Users.Name)
                    .Map(dest => dest.Latitude, scr => scr.Latitude)
                    .Map(dest => dest.Longitude, scr => scr.Longitude)
                    .Map(dest => dest.Type, scr => scr.Type.Name);

                TypeAdapterConfig<PinsRoutes, GetRoutesDTO>
                    .NewConfig()
                    .Map(dest => dest.Name, scr => scr.UserRoutes.Name)
                    .Map(dest => dest.Description, scr => scr.UserRoutes.Description)
                    .Map(dest => dest.UserName, scr => scr.UserRoutes.Users.Name)
                    .Map(dest => dest.Pins, scr => scr.UserPins);

                List<GetRoutesDTO> getRoutes = routes.Adapt<List<GetRoutesDTO>>();
                return getRoutes;
            }
            return null;
        }

        [HttpGet("/users", Name = "GetUsers")]
        public IEnumerable<UserRoles>? GetUsers()
        {
            var users = _context.UserRoles.Include(p => p.Users).Include(p => p.Roles).ToList();
            if (users.Any())
            {
                return users;
            }
            return null;
        }

        [HttpGet("/users/{id}", Name = "FindUser")]
        public UserRoles? FindUsers(int id)
        {
            var users = _context.UserRoles.Include(p=>p.Users).Include(p=>p.Roles).FirstOrDefault(p=>p.UserId == id);
            if (users is not null)
            {
                return users;
            }
            return null;
        }

        [HttpGet("/pins/{name}", Name = "FindPin")]
        public IEnumerable<UserPins>? FindPin(string name)
        {
            var pins = _context.UserPins.Include(p=>p.Users).Where(p => p.Name.Contains(name));
            if (pins is not null)
            {
                return pins;
            }
            return null;
        }
    }
}