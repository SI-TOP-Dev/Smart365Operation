using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart365Operations.Common.Infrastructure.Models
{
    public class AlarmInfo
    {
        public string Content { get; set; }
        public int CustomerId { get; set; }
        public int EquipmentId { get; set; }
        public int Level { get; set; }
        public string Time { get; set; }
        public int Type { get; set; }
    }

}
