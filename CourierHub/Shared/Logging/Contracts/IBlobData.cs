using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Shared.Logging.Contracts
{
    public interface IBlobData
    {
        IBlobPathBuilder PathBuilder {  get; }
        IBlobContainerBuilder ContainerBuilder { get; }
        IBlobBuilder BlobBuilder { get; }
        string? Path { get; }
        string? Container { get; }
        string Blob { get; }
    }
}
