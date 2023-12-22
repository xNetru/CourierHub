using CourierHub.Shared.Abstractions;

namespace CourierHub.Shared.Data {
    public class ApiContainer {
        public IEnumerable<IWebApi> WebApis { get; }

        public ApiContainer(params IWebApi[] apis) {
            var list = new List<IWebApi>();
            list.AddRange(apis);
            WebApis = list;
        }
    }
}
