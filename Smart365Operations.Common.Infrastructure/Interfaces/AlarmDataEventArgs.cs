using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart365Operations.Common.Infrastructure.Interfaces
{
    public class AlarmDataEventArgs: EventArgs
    {
        public AlarmDataEventArgs(string alarmData)
        {
            Data = alarmData;
        }

        public string Data { get; set; }
    }
}
