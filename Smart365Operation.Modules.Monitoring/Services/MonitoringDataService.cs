using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Topology;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Utility;
using System.Threading;
using Smart365Operations.Common.Infrastructure.Models;

namespace Smart365Operation.Modules.Monitoring.Services
{
    public class MonitoringDataService : IMonitoringDataService
    {
        public event EventHandler<MonitoringDataEventArgs> MonitoringDataUpdated;
        public event EventHandler<AlarmDataEventArgs> AlarmDataUpdated;

        private IAdvancedBus _bus;
        private IExchange _exchange;
        private IQueue _realTimeDataQueue;
        private IQueue _alarmDataQueue;
        private ICustomerService _customerService;
        
        public MonitoringDataService(ICustomerService customerService)
        {
            _customerService = customerService;
            InitializeBusTaskAsync();
        }

        private void InitializeBusTaskAsync()
        {
            try
            {
                var task = Task.Run(() => InitializeBus());
                task.Wait();
            }
            catch (AggregateException ae)
            {
                foreach (var item in ae.Flatten().InnerExceptions)
                {
                    Debug.WriteLine($"[MonitoringDataService] {item.Message}");
                }
            }
           
        }

        private void InitializeBus()
        {
            try
            {
                _bus = RabbitHutch.CreateBus("host=www.sitech365.com:5672;username=Test;password=123Li456").Advanced;
                _exchange = _bus.ExchangeDeclare("DEFAULT_EXCHANGE", ExchangeType.Topic, passive: true);

                var macString = SystemHelper.GetMACAddress(string.Empty);
                _realTimeDataQueue = _bus.QueueDeclare($"Smart365Client_{macString}_RealTime_Queue", maxLength: 1000);
                _alarmDataQueue = _bus.QueueDeclare($"Smart365Client_{macString}_Alarm_Queue");

                var principal = Thread.CurrentPrincipal as SystemPrincipal;
                var agentId = principal.Identity.Id;
                var customerList = _customerService.GetCustomersBy(agentId);

                var customerIdList = customerList.Select(c => c.Id.ToString()).Distinct();
                var subscriberKeys = customerIdList.Select(i => $"A.C{i}").ToArray();
                SubscriberToAlarmData(subscriberKeys);
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        public void SubscriberToAlarmData(string[] keys)
        {
            foreach (var key in keys)
            {
               _bus.Bind(_exchange, _alarmDataQueue, key);
            }
            _bus.Consume(_alarmDataQueue, (body, properties, info) => Task.Run(() =>
            {
                var message = Encoding.UTF8.GetString(body);
                var key = info.RoutingKey;
                if (keys.Contains(key))
                {
                    HandleAlarmData(message);
                }
            }));
        }

        public void SubscriberToRealData(string[] keys)
        {
            foreach (var key in keys)
            {
                _bus.Bind(_exchange, _realTimeDataQueue, key);
            }
            
            _bus.Consume(_realTimeDataQueue, (body, properties, info) => Task.Run(() =>
            {
                var message = Encoding.UTF8.GetString(body);
                var key = info.RoutingKey.Replace('.', '_');
                HandleMonitoringData(key, message);
            }));
        }



        private void HandleAlarmData(string data)
        {
            if (AlarmDataUpdated != null)
            {
                Delegate[] delegList = AlarmDataUpdated.GetInvocationList();
                //遍历委托列表
                foreach (EventHandler<AlarmDataEventArgs> deleg in delegList)
                {
                    //异步调用委托
                    deleg.BeginInvoke(this, new AlarmDataEventArgs(data), null, null);
                }
                // AlarmDataUpdated(this,new AlarmDataEventArgs(data));
            }
        }
        private void HandleMonitoringData(string key, object obj)
        {
            //Debug.WriteLine($"[{key}]{obj}");
            //if (MonitoringDataUpdated != null)
            //{
            //    MonitoringDataUpdated(this, new MonitoringDataEventArgs(key, obj));
            //}

            if(MonitoringDataUpdated != null)
            {
                Delegate[] delegList = MonitoringDataUpdated.GetInvocationList();
                foreach (EventHandler<MonitoringDataEventArgs> deleg in delegList)
                {
                    //异步调用委托
                    deleg.BeginInvoke(this, new MonitoringDataEventArgs(key, obj), null, null);
                }
            }
        }


    }
}
