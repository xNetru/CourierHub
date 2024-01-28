using Azure.Core;
using CourierHub.Shared.Logging.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Xml;

namespace CourierHub.Shared.Logging {
    public class BlobBuilder : IBlobBuilder {
        private int _statusCode = 0;
        private ulong _time = 0;
        private string _request = string.Empty;
        private string _response = string.Empty;
        private StringBuilder _errorsBuilder = new StringBuilder();

        public void AddStatusCode(int statusCode) {
            _statusCode = statusCode;
        }

        public void AddError(object error) {
            _errorsBuilder.AppendLine(Serialize(error));
        }

        public void AddOperationTime(ulong time) {
            _time = time;
        }

        public void AddRequest(object request) {
            _request = Serialize(request);
        }

        public void AddResponse(object response) {
            _response = Serialize(response);
        }

        public string Build() {
            StringBuilder stringBuilder = new();
            if (_statusCode != 0)
                stringBuilder.AppendLine($"StatusCode: {_statusCode}");
            if(_time > 0) 
                stringBuilder.AppendLine($"Operation time: {_time} ms. ({TimeSpan.FromMilliseconds(_time)})");
            if (_request.Length > 0) {
                stringBuilder.AppendLine("Request:");
                stringBuilder.AppendLine(_request);
            }
            if (_response.Length > 0) {
                stringBuilder.AppendLine("Response:");
                stringBuilder.AppendLine(_response);
            }
            if (_errorsBuilder.Length > 0) {
                stringBuilder.AppendLine("Errors:");
                stringBuilder.AppendLine(_errorsBuilder.ToString());
            }
            return stringBuilder.ToString();
        }

        public void Reset() {
            _statusCode = 0;
            _time = 0;
            _request = string.Empty;
            _response = string.Empty;
            _errorsBuilder = new StringBuilder();
        }

        private string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj,
                Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new IgnoreErrorPropertiesResolver()
                });
        }
        public class IgnoreErrorPropertiesResolver : DefaultContractResolver
        {

            protected override Newtonsoft.Json.Serialization.JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                Newtonsoft.Json.Serialization.JsonProperty property = base.CreateProperty(member, memberSerialization);

                if (new List<string>(){ "InputStream",
                    "Filter",
                    "Length",
                    "Position",
                    "ReadTimeout",
                    "WriteTimeout",
                    "LastActivityDate",
                    "LastUpdatedDate",
                    "Session"
                }.Contains(property.PropertyName))
                {
                    property.Ignored = true;
                }
                return property;
            }
        }
    }
}
