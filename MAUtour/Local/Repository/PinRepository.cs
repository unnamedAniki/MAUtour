using MAUtour.Local.DBConnect;
using MAUtour.Local.Models;
using MAUtour.Local.Repository.Interfaces;

namespace MAUtour.Local.Repository
{
    internal class PinRepository : Repository<Pins>, IPinRepository
    {
        public PinRepository(LocalContext _context) : base(_context) 
        { 
            
        }
    }
}
