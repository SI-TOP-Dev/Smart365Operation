using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smart365Operations.Common.Infrastructure.Models.TO;

namespace Smart365Operations.Common.Infrastructure.Interfaces
{
    public interface IMonitoringSummaryService
    {
        PowerSummaryDTO GetPowerSummary(string customerId);
        AlarmSummaryDTO GetAlarmSummary(string customerId);
        IList<TopPowerDTO> GetTopPowerSummary(string customerId, DateTime dateTime);
        IList<DevicePowerInfoDTO> GetDevicePowerInfo(string deviceId, DateTime dateTime);
    }
}
