using CourierHub.Shared.Logging.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Shared.Logging
{
    public class BlobData: IBlobData
    {
        private BlobPathBuilder _blobPathBuilder = new BlobPathBuilder();
        private BlobContainerBuilder _blobContainerBuilder = new BlobContainerBuilder();
        private BlobBuilder _blobBuilder = new BlobBuilder();

        public IBlobPathBuilder PathBuilder { get => _blobPathBuilder; }

        public IBlobContainerBuilder ContainerBuilder { get => _blobContainerBuilder; }

        public IBlobBuilder BlobBuilder { get => _blobBuilder; }

        public string? Path => _blobPathBuilder.Build();

        public string? Container => _blobContainerBuilder.Build();

        public string Blob => _blobBuilder.Build();
    }
}
