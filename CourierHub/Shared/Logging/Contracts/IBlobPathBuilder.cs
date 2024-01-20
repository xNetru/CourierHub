using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Shared.Logging.Contracts
{
    public enum Applications
    {
        CourierHub,
        API
    }
    public interface IBlobPathBuilder
    {
        void AddApplication(Applications application);
        void AddDate(DateOnly date);
        void AddController(string controllerName);
        void AddMethod(string methodName);
        void AddTime(TimeSpan time);
        void Reset();
        string? Build();
    }
}
