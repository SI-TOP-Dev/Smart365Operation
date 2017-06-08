using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Models.TO;
using Smart365Operations.Common.Infrastructure.Utility;

namespace Smart365Operation.Modules.Monitoring.Services
{
    public class MonitoringSummaryService : IMonitoringSummaryService
    {
        public PowerSummaryDTO GetPowerSummary(string customerId)
        {
            DataServiceApi httpServiceApi = new DataServiceApi();
            var request = new RestRequest($"customer/group/power.json?customerId={customerId}", Method.GET);
            var powerSummaryDTO = httpServiceApi.Execute<PowerSummaryDTO>(request);
            return powerSummaryDTO;
        }

        public AlarmSummaryDTO GetAlarmSummary(string customerId)
        {

            DataServiceApi httpServiceApi = new DataServiceApi();
            var request = new RestRequest($"customer/month/group/alarm.json?customerId={customerId}", Method.GET);
            var alarmSummaryDTO = httpServiceApi.Execute<AlarmSummaryDTO>(request);
            return alarmSummaryDTO;
        }

        public IList<TopPowerDTO> GetTopPowerSummary(string customerId, DateTime dateTime)
        {
            DataServiceApi httpServiceApi = new DataServiceApi();
            var request = new RestRequest($"customer/topfive/power.json?customerId={customerId}&time={dateTime.ToString("yyyy-MM")}", Method.GET);
            var topPowerDTOList = httpServiceApi.Execute<List<TopPowerDTO>>(request);
            return topPowerDTOList;
        }

        public IList<DevicePowerInfoDTO> GetDevicePowerInfo(string deviceId, DateTime dateTime)
        {
            DataServiceApi httpServiceApi = new DataServiceApi();
            var request = new RestRequest($"data/equipment/power/list.json?equipmentId={deviceId}&time={dateTime.ToString("yyyy-MM")}", Method.GET);
            var devicePowerInfoList = httpServiceApi.Execute<List<DevicePowerInfoDTO>>(request);
            return devicePowerInfoList;

        }

        public TransformerCapacityDTO GetTransformerCapacity(string customerId)
        {
            DataServiceApi httpServiceApi = new DataServiceApi(); 
             var request = new RestRequest($"data/customer/apparent.json?customerId={customerId}", Method.GET);
            var capacity = httpServiceApi.Execute<TransformerCapacityDTO>(request);
            return capacity;
        }
    }
}
