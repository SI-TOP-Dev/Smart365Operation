using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart365Operations.Common.Infrastructure.Models.TO
{
    public class InspectionStatisticsDTO
    {
        public int customerId { get; set; }
        public int completeInspectionCount { get; set; }
        public string customerName { get; set; }
    }
}
