﻿using MAUtour.Local.DBConnect;
using MAUtour.Local.Models;
using MAUtour.Local.Repository.Interfaces;

namespace MAUtour.Local.Repository
{
    internal class PinTypesRepository : Repository<PinTypes>, IPinTypesRepository
    {
        public PinTypesRepository(LocalContext _context) : base(_context) { }
    }
}
