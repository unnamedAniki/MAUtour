using MAUtour.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUtour.Utils.DbConnect.TestData
{
    public class TestUserData
    {
        public static Users[] CreateUserData()
        {
            List<Users> users = new List<Users>();
            users.AddRange(
                new[] 
                {
                    new Users()
                    {
                        Id = 1,
                        Email = "test1@mail.ru",
                        Name = "Denis",
                        Password = "123"
                    },
                    new Users()
                    {
                        Id = 2,
                        Email = "test2@mail.ru",
                        Name = "Alexey",
                        Password = "123"
                    },
                    new Users()
                    {
                        Id = 3,
                        Email = "test3@mail.ru",
                        Name = "Danila",
                        Password = "123"
                    },
                    new Users()
                    {
                        Id = 4,
                        Email = "test4@mail.ru",
                        Name = "Artem",
                        Password = "123"
                    } 
                });
            return users.ToArray();
        }
    }
}
