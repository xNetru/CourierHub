using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CourierHub.Shared.Logging.Contracts;

namespace CourierHub.Shared.Logging
{
    public class BlobBuilder : IBlobBuilder
    {
        private int _statusCode = 0;
        private string _time = string.Empty;
        private string _request = string.Empty;
        private string _response = string.Empty;
        private StringBuilder _errorsBuilder = new StringBuilder();

        public void AddStatusCode(int statusCode)
        {
            _statusCode = statusCode;
        }
        public void AddError(object error)
        {
            _errorsBuilder.AppendLine(JsonSerializer.Serialize(error));
        }

        public void AddOperationTime(TimeSpan time)
        {
            _time = time.ToString();
        }

        public void AddRequest(object request)
        {
            _response = JsonSerializer.Serialize(request);
        }

        public void AddResponse(object response)
        {
            _response = JsonSerializer.Serialize(response);
        }

        public string Build()
        {
            StringBuilder stringBuilder = new();
            if(_statusCode != 0)
                stringBuilder.AppendLine($"StatusCode: {_statusCode}");
            if(_time.Length > 0)
                stringBuilder.AppendLine($"Operation time: {_time}");
            if(_request.Length > 0)
            {
                stringBuilder.AppendLine("Request:");
                stringBuilder.AppendLine(_request);
            }
            if(_response.Length > 0) 
            {
                stringBuilder.AppendLine("Response:");
                stringBuilder.AppendLine(_response);
            }
            if(_errorsBuilder.Length > 0) 
            {
                stringBuilder.AppendLine("Errors:");
                stringBuilder.AppendLine(_errorsBuilder.ToString());
            }
            return stringBuilder.ToString();
        }

        public void Reset()
        {
            _statusCode = 0;
            _time = string.Empty;
            _request = string.Empty;
            _response = string.Empty;
            _errorsBuilder = new StringBuilder();
        }
    }
}
