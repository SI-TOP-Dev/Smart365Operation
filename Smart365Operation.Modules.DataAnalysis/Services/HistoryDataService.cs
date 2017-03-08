using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Models;
using Smart365Operations.Common.Infrastructure.Models.TO;
using Smart365Operations.Common.Infrastructure.Utility;

namespace Smart365Operation.Modules.DataAnalysis.Services
{
    public class HistoryDataService: IHistoryDataService
    {
        public IEnumerable<HistoryDataDTO> GetHistoryDataList(string equipmentId, string parameterId, TimeType timeType, string date)
        {
            DataServiceApi httpServiceApi = new DataServiceApi();
            var request = new RestRequest($"data/his/line.json?typeId={parameterId}&timeType={Convert.ToInt32(timeType)}&time={date}&equipmentId={equipmentId}", Method.GET);
            var historyDataList = httpServiceApi.Execute<List<HistoryDataDTO>>(request);
            return historyDataList;
        }
    }
}
