using CourierHub.Shared.Logging.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Shared.Logging
{
    public class BlobPathBuilder : IBlobPathBuilder
    {
        private string _application = string.Empty;
        private string _controller = string.Empty;
        private string _date = string.Empty;
        private string _method = string.Empty;
        private string _time = string.Empty;

        public void AddApplication(Applications application)
        {
            _application = application switch
            {
                Applications.CourierHub => "back",
                Applications.API => "api",
                _ => ""
            };
        }

        public void AddController(string controllerName)
        {
            _controller = controllerName;
        }

        public void AddDate(DateOnly date)
        {
            _date = date.ToString("dd.MM.yyyy");
        }

        public void AddMethod(string methodName)
        {
            _method = methodName;
        }

        public void AddTime(TimeOnly time)
        {
            _time = time.ToString("hh:mm:ss");
        }

        public string? Build()
        {
            if (_application.Length > 0 && _controller.Length > 0 && _date.Length > 0
                && _method.Length > 0 && _time.Length > 0)
                return $"{_application}/{_controller}/{_date}/{_method}_{_time}.txt";
            return default;
        }

        public void Reset()
        {
            _application = string.Empty;
            _controller = string.Empty;
            _date = string.Empty;
            _method = string.Empty;
            _time = string.Empty;
        }
    }
}
