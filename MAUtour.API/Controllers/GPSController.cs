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
using MAUtour.API.Utils;

namespace MAUtour.API.Controllers
{
    [ApiController]
    [Route("[controller]/gps")]
    public class GPSController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private GPSUtils _GPSUtils;
        private readonly ILogger<GPSController> _logger;
        public GPSController(ILogger<GPSController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
            _GPSUtils = new GPSUtils(_context);
        }

        [HttpGet("/pins", Name = "GetPins")]
        public async Task<IEnumerable<PinsDTO>?> GetPinsAsync()
        {
            return await _GPSUtils.GetAllPinsAsync();
        }

        [HttpGet("/pins/{name}", Name = "FindPin")]
        public async Task<IEnumerable<PinsDTO>?> FindPin(string name)
        {
            return await _GPSUtils.FindPinAsync(name);
        }

        [HttpGet("/routes", Name = "GetRoutes")]
        public async Task<IEnumerable<GetRoutesDTO>?> GetRoutes()
        {
            return await _GPSUtils.GetRoutesAsync();
        }
    }
}