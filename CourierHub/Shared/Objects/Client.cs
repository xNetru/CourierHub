using CourierHub.Server.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Shared.Objects
{
    public class Client : IUser
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public Client(string email, string name, string surname)
        {
            Email = email;
            Name = name;
            Surname = surname;
        }
    }
}
