using Mapster;

using MAUtour.API.DTOs;
using MAUtour.Utils.DbConnect;
using Microsoft.EntityFrameworkCore;

namespace MAUtour.API.Utils
{
    public class GPSUtils
    {
        private readonly ApplicationContext _context;

        internal GPSUtils(ApplicationContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<PinsDTO>?> GetAllPinsAsync()
        {
            var pins = await _context.UserPins.Include(p => p.Users).Include(p => p.Type).ToListAsync();
            if (pins.Any())
            {
                List<PinsDTO> getPins = pins.Adapt<List<PinsDTO>>();
                return getPins;
            }
            return null;
        }

        public async Task<IEnumerable<PinsDTO>?> FindPinAsync(string name)
        {
            var pins = await _context.UserPins.Include(p => p.Users).Where(p => p.Name.Contains(name)).ToListAsync();
            if (pins is not null)
            {
                List<PinsDTO> getPins = pins.Adapt<List<PinsDTO>>();
                return getPins;
            }
            return null;
        }

        public async Task<IEnumerable<GetRoutesDTO>?> GetRoutesAsync()
        {
            var routes = await _context.PinRoutes
                            .Include(p => p.UserRoutes)
                            .ThenInclude(p => p.Users)
                            .Include(p => p.UserPins)
                            .ThenInclude(p => p.Type)
                            .Include(p => p.UserPins)
                            .ThenInclude(p => p.Users)
                            .AsSplitQuery().ToListAsync();

            if (routes.Any())
            {
                List<GetRoutesDTO> getRoutes = routes.Adapt<List<GetRoutesDTO>>();
                return getRoutes;
            }
            return null;
        }
    }
}
