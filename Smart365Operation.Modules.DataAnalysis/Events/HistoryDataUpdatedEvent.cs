﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using Smart365Operations.Common.Infrastructure.Models.TO;
using Smart365Operations.Common.Infrastructure.Models;

namespace Smart365Operation.Modules.DataAnalysis.Events
{
    public class HistoryDataUpdatedEvent: PubSubEvent<HistoryDataUpdatedEventArg>
    {
    }

    public class HistoryDataUpdatedEventArg
    {
        public HistoryDataUpdatedEventArg(IEnumerable<HistoryDataDTO> historyDatas,TimeType dataTimeType)
        {
            HistoryDataDtos = historyDatas;
            DataTimeType = dataTimeType;
        }

        public IEnumerable<HistoryDataDTO> HistoryDataDtos { get; set; }
        public TimeType DataTimeType { get; set; }
    }
}
