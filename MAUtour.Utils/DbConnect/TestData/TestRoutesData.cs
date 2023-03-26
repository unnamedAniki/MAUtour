using MAUtour.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace MAUtour.Utils.DbConnect.TestData
{
    internal class TestRoutesData
    {
        public static TypeOfRoutes[] CreateRouteTypes()
        {
            List<TypeOfRoutes> routes = new List<TypeOfRoutes>() 
            {
                new TypeOfRoutes()
                {
                    Id = 1,
                    Name = "Tour"
                }
            };
            return routes.ToArray();
        }

        public static UserRoutes[] CreateRoutesData()
        {
            List<UserRoutes> routes = new List<UserRoutes>
            {
                new UserRoutes()
                {
                    Id = 1,
                    Name = "Momuments Alesha tour",
                    TypeId = 1,
                    Description = "Such an another momument"
                }
            };
            return routes.ToArray();
        }
        public static PinsRoutes[] CreatePinRoutesData()
        {
            List<PinsRoutes> pinsRoutes = new List<PinsRoutes>
            {
                new PinsRoutes()
                {
                    Id = 1,
                    RouteId = 1,
                    PinId = 1
                },
                new PinsRoutes()
                {
                    Id = 2,
                    RouteId = 1,
                    PinId = 2
                },
                new PinsRoutes()
                {
                    Id = 3,
                    RouteId = 1,
                    PinId = 3
                },
                new PinsRoutes()
                {
                    Id = 4,
                    RouteId = 1,
                    PinId = 4
                }
            };
            return pinsRoutes.ToArray();
        }
    }
}
