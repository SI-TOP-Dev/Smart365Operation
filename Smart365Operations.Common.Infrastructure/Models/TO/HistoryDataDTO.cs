using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart365Operations.Common.Infrastructure.Models.TO
{

    public class HistoryDataDTO
    {
        public string dataType { get; set; }
        public List<DatavalueDTO> dataValue { get; set; }
        public int id { get; set; }
        public int isShow { get; set; }
        public string pointName { get; set; }
        public string pointShortName { get; set; }
        public int sequence { get; set; }
        public string unit { get; set; }
    }

    public class DatavalueDTO
    {
        public string time { get; set; }
        public string value { get; set; }
    }

}
