using MAUtour.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUtour.Utils.DbConnect.TestData
{
    internal class TestPinsData
    {
        public static UserPins[] CreatePinsData()
        {
            List<UserPins> pins = new List<UserPins>();
            pins.AddRange(
                new[]
                {
                    new UserPins()
                    {
                        Id = 1,
                        Name = "Momuments Alesha",
                        Description = "Such an another momument",
                        Latitude = 10,
                        Longitude = 10,
                        UserId = 1,
                    },
                    new UserPins()
                    {
                        Id = 2,
                        Name = "MASU",
                        Description = "Simple University",
                        Latitude = 9,
                        Longitude = 11,
                        UserId = 2,
                    },
                    new UserPins()
                    {
                        Id = 3,
                        Name = "Murmansk's port", 
                        Description = "Fish heaven",
                        Latitude = 8,
                        Longitude = 12,
                        UserId = 3,
                    },
                    new UserPins()
                    {
                        Id = 4,
                        Name = "Murmansk Mall",
                        Description = "WallMart? No, its a Murmansk Mall",
                        Latitude = 7,
                        Longitude = 13,
                        UserId = 4,
                    }
                });
            return pins.ToArray();
        }
    }
}
