using Mapster;

using MAUtour.API.DTOs;
using MAUtour.Utils.Models;

namespace MAUtour.API
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .NewConfig<UserRoutes, GetRoutesDTO>()
                .Map(dest => dest.Name, scr => scr.Name)
                .Map(dest => dest.Description, scr => scr.Description)
                .Map(dest => dest.UserName, scr => scr.Users.Name);

            config
                .NewConfig<UserPins, PinsDTO>()
                .Map(dest => dest.Name, scr => scr.Name)
                .Map(dest => dest.Description, scr => scr.Description)
                .Map(dest => dest.UserName, scr => scr.Users.Name)
                .Map(dest => dest.Latitude, scr => scr.Latitude)
                .Map(dest => dest.Longitude, scr => scr.Longitude)
                .Map(dest => dest.Type, scr => scr.Type.Name);

            config
                .NewConfig<PinsRoutes, GetRoutesDTO>()
                .Map(dest => dest.Name, scr => scr.UserRoutes.Name)
                .Map(dest => dest.Description, scr => scr.UserRoutes.Description)
                .Map(dest => dest.UserName, scr => scr.UserRoutes.Users.Name)
                .Map(dest => dest.Pins, scr => scr.UserPins);
            config
                .NewConfig<Roles, RoleDTO>()
                .Map(dest => dest.Name, scr => scr.Name)
                .Map(dest => dest.Priority, scr => scr.Prority);

            config
                .NewConfig<UserRoles, UserDTO>()
                .Map(dest => dest.Name, scr => scr.Users.Name)
                .Map(dest => dest.RoleName, scr => scr.Roles)
                .Map(dest => dest.Email, scr => scr.Users.Email)
                .Map(dest => dest.StartPaidDate, scr => scr.Users.StartPaidDate)
                .Map(dest => dest.ExpiredPaidDate, scr => scr.Users.ExpiredPaidDate);
        }
    }
}
