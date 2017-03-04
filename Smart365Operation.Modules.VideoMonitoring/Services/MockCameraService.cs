using Smart365Operations.Common.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smart365Operations.Common.Infrastructure.Models;

namespace Smart365Operation.Modules.VideoMonitoring.Services
{
    public class MockCameraService : ICameraService
    {
        public IList<Camera> GetCamerasBy(int customerId)
        {
            return new List<Camera>(); 
        }
    }
}
