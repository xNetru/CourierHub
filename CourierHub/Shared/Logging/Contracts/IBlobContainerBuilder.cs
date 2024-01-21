using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Shared.Logging.Contracts
{
    public interface IBlobContainerBuilder
    {
        void AddLogs();
        string? Build();
        void Reset();
    }
}
