using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUtour.Local.Models
{
    internal class Pins
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PinTypes PinType { get; set; }
        public int PinTypeId { get; set; }
    }
}
