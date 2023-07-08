using MAUtour.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUtour.Utils.DbConnect.TestData
{
    public class TestPinsData
    {
        public static TypeOfPins[] CreatePinTypes()
        {
            List<TypeOfPins> pins = new List<TypeOfPins>
            {
                new TypeOfPins()
                {
                    Id = 1,
                    Name = "Default"
                }
            };
            return pins.ToArray();
        }
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
                        Latitude = 68.99327F,
                        Longitude = 33.07197F,
                        UserId = 1,
                        TypeId = 1
                    },
                    new UserPins()
                    {
                        Id = 2,
                        Name = "MASU",
                        Description = "Simple University",
                        Latitude = 68.96356F,
                        Longitude = 33.07387F,
                        UserId = 2,
                        TypeId = 1
                    },
                    new UserPins()
                    {
                        Id = 3,
                        Name = "Murmansk's port", 
                        Description = "Fish heaven",
                        Latitude = 68.97642F,
                        Longitude = 33.06396F,
                        UserId = 3,
                        TypeId = 1
                    },
                    new UserPins()
                    {
                        Id = 4,
                        Name = "Murmansk Mall",
                        Description = "WallMart? No, its a Murmansk Mall",
                        Latitude = 68.95737F,
                        Longitude = 33.06986F,
                        UserId = 4,
                        TypeId = 1
                    }
                });
            return pins.ToArray();
        }
    }
}
