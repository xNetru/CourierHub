using CourierHub.Server.Abstractions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Shared {
    public class AzureSQL : IDatabase {
        private readonly CourierHubContext _context;
        public AzureSQL() {
            _context = new CourierHubContext();
        }

        public Task<bool> checkUser(string email) {
            _context.Database.EnsureCreated();
            // rób coś
            throw new NotImplementedException();
        }

        public Task addUser(IUser user) {
            throw new NotImplementedException();
        }
    }
}
