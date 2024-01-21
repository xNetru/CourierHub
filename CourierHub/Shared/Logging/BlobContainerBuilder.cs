using CourierHub.Shared.Logging.Contracts;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Shared.Logging
{
    public class BlobContainerBuilder : IBlobContainerBuilder
    {
        private string _blobContainerName = string.Empty;
        public void AddLogs()
        {
            _blobContainerName = new string("logs");
        }
        public string? Build()
        {
            return _blobContainerName.Length > 0 ? new string(_blobContainerName) : default;
        }
        public void Reset()
        {
            _blobContainerName = string.Empty;
        }
    }
}
