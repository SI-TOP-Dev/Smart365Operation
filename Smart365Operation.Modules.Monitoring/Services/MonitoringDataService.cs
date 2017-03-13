using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Topology;
using Smart365Operations.Common.Infrastructure.Interfaces;

namespace Smart365Operation.Modules.Monitoring.Services
{
    public class MonitoringDataService : IMonitoringDataService
    {
        public event EventHandler<MonitoringDataEventArgs> MonitoringDataUpdated;
        public event EventHandler<AlarmDataEventArgs> AlarmDataUpdated;

        private IAdvancedBus _bus;
        private IExchange _exchange;
        private IQueue _queue;
        public MonitoringDataService()
        {
            InitializeBusTaskAsync();
        }

        private void InitializeBusTaskAsync() => Task.Run(() => InitializeBus());

        private void InitializeBus()
        {
            _bus = RabbitHutch.CreateBus("host=114.215.94.141;username=Test;password=123456").Advanced;
            _exchange = _bus.ExchangeDeclare("DEFAULT_EXCHANGE", ExchangeType.Topic, passive: true);
            _queue = _bus.QueueDeclare();
            _bus.Bind(_exchange, _queue, "#");
            _bus.Consume(_queue, (body, properties, info) => Task.Factory.StartNew(() =>
            {
                var message = Encoding.UTF8.GetString(body);
                if (String.Equals(info.RoutingKey, "Alarm"))
                {
                    HandleAlarmData(message);
                }
                else
                {
                    HandleMonitoringData(info.RoutingKey, message);
                }
            }));
        }

        private void HandleAlarmData(string data)
        {
            if (AlarmDataUpdated != null)
            {
                AlarmDataUpdated(this,new AlarmDataEventArgs(data));
            }
        }
        private void HandleMonitoringData(string key, object obj)
        {
            //Debug.WriteLine($"[{key}]{obj}");
            if (MonitoringDataUpdated != null)
            {
                MonitoringDataUpdated(this, new MonitoringDataEventArgs(key, obj));
            }
        }
        
    }
}
