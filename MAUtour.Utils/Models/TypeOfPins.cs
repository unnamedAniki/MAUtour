﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUtour.Utils.Models
{
    [PrimaryKey("Id")]
    public class TypeOfPins
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserPins> Pins { get; set; }
    }
}
