using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart365Operations.Common.Infrastructure.Models.TO
{
   

    public class AlarmSummaryDTO
    {
        public int untreatedCount { get; set; }
        public int treatedCount { get; set; }
        public int communicationCount { get; set; }
        public int commonCount { get; set; }
        public int abnormalCount { get; set; }
        public int faultCount { get; set; }
    }

}
