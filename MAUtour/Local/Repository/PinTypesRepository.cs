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
    internal class PinTypesRepository : Repository<PinTypes>, IPinTypesRepository
    {
        public PinTypesRepository(LocalContext _context) : base(_context) { }
    }
}
