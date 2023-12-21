using CourierHub.Shared.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Shared.Data
{
    public class InquireContainer
    {
        public List<ApiInquire> Inquires { get; set; } = new List<ApiInquire>();
    }
}
