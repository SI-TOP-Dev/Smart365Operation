using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart365Operations.Common.Infrastructure.Models.TO
{
    
    public class CustomerIncrementsDTO
    {
        public string date { get; set; }
        public int existing { get; set; }
        public int increased { get; set; }
    }

}
