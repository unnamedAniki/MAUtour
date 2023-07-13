using MAUtour.Local.DBConnect;
using MAUtour.Local.Models;
using MAUtour.Local.Repository.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUtour.Local.Repository
{
    internal class PinRepository : Repository<Pins>, IPinRepository
    {
        public PinRepository(LocalContext _context) : base(_context) 
        { 
            
        }
    }
}
