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
    }
}
