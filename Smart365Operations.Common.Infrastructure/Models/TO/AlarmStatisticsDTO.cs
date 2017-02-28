using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart365Operations.Common.Infrastructure.Models.TO
{
    public class AlarmStatisticsDTO
    {
        public string date { get; set; }
        public int alarmCount { get; set; }
        public int untreatedCount { get; set; }
    }

}
