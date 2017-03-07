using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Models.TO;
using Smart365Operations.Common.Infrastructure.Utility;

namespace Smart365Operation.Modules.DataAnalysis.Services
{
    public class DeviceParameterInfoService : IDeviceParameterInfoService
    {
        public IEnumerable<DeviceParameterInfoDTO> GetDeviceParameterList(string deviceId)
        {
            DataServiceApi httpServiceApi = new DataServiceApi();
            var request = new RestRequest($"data/equipment/point.json?equipmentId={deviceId}", Method.GET);
            var deviceParameterList = httpServiceApi.Execute<List<DeviceParameterInfoDTO>>(request);
            return deviceParameterList;
        }
    }
}
