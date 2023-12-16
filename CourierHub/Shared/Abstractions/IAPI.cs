using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Shared.Abstractions {
    internal interface IAPI {
    }
}
namespace CourierHub.Shared.Abstractions {
    public interface IWebAPI {
        /* tutaj musisz Kamilu dać 
         * POST /inquires {} -> /inquires/id
         * GET /offers/id -> {}
         * albo połączyć te dwa w jedno
         * oraz
         * POST /orders {} -> orders/id
         * PATCH /orders/id {}
         * GET /orders/id -> {}
         */
    }
}