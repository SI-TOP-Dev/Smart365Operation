using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart365Operations.Common.Infrastructure.Interfaces
{
    public interface IMonitoringDataService
    {
        event EventHandler<MonitoringDataEventArgs> MonitoringDataUpdated;
        event EventHandler<AlarmDataEventArgs> AlarmDataUpdated;
        //void ReceiveData(string identity, object data);

    }
}
