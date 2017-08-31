using Smart365Operations.Common.Infrastructure.Models.TO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart365Operation.Modules.Dashboard.Interfaces
{
    public interface IDataStatisticsService
    {
        IList<CustomerIncrementsDTO> GetCustomerIncrementsInfo();
        IList<CustomerIndustryCategoryDTO> GetCustomerIndustryCategoryInfo();
        IList<AlarmStatisticsDTO> GetAlarmStatisticsInfo();
        IList<InspectionStatisticsDTO> GetInspectionStatisticsInfo();
    }
}
