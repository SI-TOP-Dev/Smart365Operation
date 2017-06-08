using Prism.Events;
using Smart365Operations.Common.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart365Operation.Modules.Dashboard.Events
{
    public class AlarmUpdatedEvent : PubSubEvent<AlarmUpdatedEventArg>
    {
    }

    public class AlarmUpdatedEventArg
    {
        public AlarmInfo  Alarm { get; set; }

        public AlarmUpdatedEventArg(AlarmInfo info)
        {
            Alarm = info;
        }
    }
}
