using System;

namespace Smart365Operations.Common.Infrastructure.Interfaces
{
    public class MonitoringDataEventArgs : EventArgs
    {
        public MonitoringDataEventArgs(string key,object value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public object Value { get; set; }
    }
}