using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Shared.Data
{
    public class RoleContainer
    {
        public Dictionary<string, bool> Roles { get; set; } = new Dictionary<string, bool>
        {
            { "NotAuthorized", false},
            { "Client", false},
            { "OfficeWorker", false},
            { "Courier", false}
        };
    }
}
