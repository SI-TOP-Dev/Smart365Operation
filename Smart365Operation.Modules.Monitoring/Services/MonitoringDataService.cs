using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using Smart365Operations.Common.Infrastructure.Interfaces;

namespace Smart365Operation.Modules.Monitoring.Services
{
    public class MonitoringDataService : IMonitoringDataService
    {
        public event EventHandler<MonitoringDataEventArgs> DataUpdated;

        private IBus _bus;
        public MonitoringDataService()
        {
            InitializeBus();

        }

        private void InitializeBus()
        {
            //_bus = RabbitHutch.CreateBus("host=192.168.8.143;username=Test;password=123456");
            //_bus.Subscribe<string>("test", HandleTextMessage);
        }

        private void HandleTextMessage(string obj)
        {
            Debug.WriteLine($"++++++++{obj}+++++++++");
            if (DataUpdated != null)
            {
                DataUpdated(this, new MonitoringDataEventArgs("Test", obj));
            }
        }

        #region UIManager
        //private UIManager _uiManager;
        #endregion
    }
}
