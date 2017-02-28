using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Smart365Operation.Modules.Dashboard.Interfaces;

namespace Smart365Operation.Modules.Dashboard.ViewModels
{
    public class OverviewMapViewModel:BindableBase
    {
        private readonly IDataStatisticsService _dataStatisticsService;

        public OverviewMapViewModel(IDataStatisticsService dataStatisticsService)
        {
            _dataStatisticsService = dataStatisticsService;
            StatisticsViewModel = new DataStatisticsViewModel(_dataStatisticsService);
        }

        private DataStatisticsViewModel _statisticsViewModel;
        public DataStatisticsViewModel StatisticsViewModel
        {
            get { return _statisticsViewModel; }
            set { SetProperty(ref _statisticsViewModel, value); }
        }
    }
}
