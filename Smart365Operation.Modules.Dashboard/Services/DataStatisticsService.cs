using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Smart365Operation.Modules.Dashboard.Interfaces;
using Smart365Operations.Common.Infrastructure.Models.TO;
using Smart365Operations.Common.Infrastructure.Utility;

namespace Smart365Operation.Modules.Dashboard.Services
{
    public class DataStatisticsService:IDataStatisticsService
    {
        public IList<AlarmStatisticsDTO> GetAlarmStatisticsInfo()
        {
            List<AlarmStatisticsDTO> alarmStatistics = new List<AlarmStatisticsDTO>();

            DataServiceApi httpServiceApi = new DataServiceApi();
            var request = new RestRequest($"data/alarm_type_ratio.json", Method.GET);
            alarmStatistics = httpServiceApi.Execute<List<AlarmStatisticsDTO>>(request);

            return alarmStatistics;
        }

        public IList<CustomerIncrementsDTO> GetCustomerIncrementsInfo()
        {
            List<CustomerIncrementsDTO> customerIncrements = new List<CustomerIncrementsDTO>();

            DataServiceApi httpServiceApi = new DataServiceApi();
            var request = new RestRequest($"customer/statistics/count_per_month.json", Method.GET);
            customerIncrements = httpServiceApi.Execute<List<CustomerIncrementsDTO>>(request);
            
            return customerIncrements;
        }

        public IList<CustomerIndustryCategoryDTO> GetCustomerIndustryCategoryInfo()
        {
            List<CustomerIndustryCategoryDTO> customerIndustryCategory = new List<CustomerIndustryCategoryDTO>();

            DataServiceApi httpServiceApi = new DataServiceApi();
            var request = new RestRequest($"customer/statistics/type_ratio.json", Method.GET);
            customerIndustryCategory = httpServiceApi.Execute<List<CustomerIndustryCategoryDTO>>(request);

            return customerIndustryCategory;
        }

        public IList<InspectionStatisticsDTO> GetInspectionStatisticsInfo()
        {
            List<InspectionStatisticsDTO> inspectionStatistics = new List<InspectionStatisticsDTO>();

            DataServiceApi httpServiceApi = new DataServiceApi();
            var request = new RestRequest($"customer/statistics/complete_inspection_count.json?count=5", Method.GET);
            inspectionStatistics = httpServiceApi.Execute<List<InspectionStatisticsDTO>>(request);

            return inspectionStatistics;
        }
    }
}
