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
        public static UserRoutes[] CreateRoutesData()
        {
            List<UserRoutes> routes = new List<UserRoutes>
            {
                new UserRoutes()
                {
                    Id = 1,
                    Name = "Momuments Alesha tour",
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
                    RouteId = 1,
                    PinsId = 1
                },
                new PinsRoutes()
                {
                    RouteId = 1,
                    PinsId = 2
                },
                new PinsRoutes()
                {
                    RouteId = 1,
                    PinsId = 3
                },
                new PinsRoutes()
                {
                    RouteId = 1,
                    PinsId = 4
                }
            };
            return pinsRoutes.ToArray();
        }
    }
}
