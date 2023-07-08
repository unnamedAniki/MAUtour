using MAUtour.Utils.Models;

namespace MAUtour.Utils.DbConnect.TestData
{
    public class TestRolesData
    {
        public static Roles[] CreateRolesData()
        {
            List<Roles> roles = new List<Roles>();
            roles.AddRange(
                new[]
                {
                    new Roles()
                    {
                        Id = 1,
                        Name = "Default",
                        Prority = 0
                    },
                    new Roles()
                    {
                        Id = 2,
                        Name = "Verified",
                        Prority = 1
                    },
                    new Roles()
                    {
                        Id = 3,
                        Name = "Company",
                        Prority = 2
                    },
                    new Roles()
                    {
                        Id = 4,
                        Name = "Admin",
                        Prority = int.MaxValue
                    }
                });
            return roles.ToArray();
        }
        public static UserRoles[] CreateUserRolesData()
        {
            List<UserRoles> userRoles = new List<UserRoles>();
            userRoles.AddRange(
                new[]
                {
                    new UserRoles()
                    {
                        Id = 1,
                        UserId = 1,
                        RoleId = 1
                    },
                    new UserRoles()
                    {
                        Id = 2,
                        UserId = 2,
                        RoleId = 2
                    },
                    new UserRoles()
                    {
                        Id = 3,
                        UserId = 3,
                        RoleId = 3
                    },
                    new UserRoles()
                    {
                        Id = 4,
                        UserId = 4,
                        RoleId = 4
                    }
                });
            return userRoles.ToArray();
        }
    }
}
