using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Shared.Abstractions {
    public interface IUser {
        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
    }
}
