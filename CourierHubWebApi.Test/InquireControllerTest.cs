using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Moq.EntityFrameworkCore;
using CourierHub.Shared.Data;
using CourierHubWebApi.Controllers;
using CourierHub.Shared.Models;

namespace CourierHubWebApi.Test
{
    internal class InquireControllerTest
    {
        Mock<CourierHubDbContext> _mockContext;
        InquireController _controller;

        public InquireControllerTest()
        {
            _mockContext = new Mock<CourierHubDbContext>();
            IList<Inquire> inquires = new List<Inquire>();
            _mockContext.Setup(x => x.Inquires).ReturnsDbSet(inquires);

            
        }
    }
}
