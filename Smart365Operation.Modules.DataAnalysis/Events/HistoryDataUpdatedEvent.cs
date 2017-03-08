using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using Smart365Operations.Common.Infrastructure.Models.TO;

namespace Smart365Operation.Modules.DataAnalysis.Events
{
    public class HistoryDataUpdatedEvent: PubSubEvent<HistoryDataUpdatedEventArg>
    {
    }

    public class HistoryDataUpdatedEventArg
    {
        public HistoryDataUpdatedEventArg(IEnumerable<HistoryDataDTO> historyDatas)
        {
            HistoryDataDtos = historyDatas;
        }

        public IEnumerable<HistoryDataDTO> HistoryDataDtos { get; set; }
    }
}
