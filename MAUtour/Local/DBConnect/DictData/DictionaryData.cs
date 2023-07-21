using MAUtour.Local.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUtour.Local.DBConnect.DictData
{
    internal class DictionaryData
    {
        internal static PinTypes[] GetPinTypes()
        {
            List<PinTypes> pinTypes = new List<PinTypes>();
            pinTypes.AddRange(
                new[]
                {
                    new PinTypes()
                    {
                        Id = 1,
                        Name = "Опасное место"
                    },
                    new PinTypes()
                    {
                        Id = 2,
                        Name = "Животное"
                    },
                    new PinTypes()
                    {
                        Id = 3,
                        Name = "Место отдыха"
                    },
                    new PinTypes()
                    {
                        Id = 4,
                        Name = "Достопримечательность"
                    },
                    new PinTypes()
                    {
                        Id = 5,
                        Name = "Кемпинг"
                    }
                });
            return pinTypes.ToArray();
        }

        internal static RouteTypes[] GetRouteTypes()
        {
            List<RouteTypes> routeTypes = new List<RouteTypes>();
            routeTypes.AddRange(
                new[]
                {
                    new RouteTypes()
                    {
                        Id = 1,
                        Name = "Туристический маршрут"
                    },
                    new RouteTypes()
                    {
                        Id = 2,
                        Name = "Обычный маршрут"
                    },
                    new RouteTypes()
                    {
                        Id = 3,
                        Name = "Путь до места"
                    }
                });
            return routeTypes.ToArray();
        }
    }
}
