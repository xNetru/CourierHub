using CourierHub.Shared.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Shared.Data
{
    public class OrderContainer
    {
        public List<ApiOrder> Inquires { get; set; } = new List<ApiOrder>();
    }
}
