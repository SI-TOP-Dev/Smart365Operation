using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Wpf;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Smart365Operation.Modules.DataAnalysis.Events;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Models.TO;

namespace Smart365Operation.Modules.DataAnalysis.ViewModels
{
    public class DataCurveChartViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDeviceParameterInfoService _deviceParameterInfoService;

        public DataCurveChartViewModel(IEventAggregator eventAggregator, IDeviceParameterInfoService deviceParameterInfoService)
        {
            _eventAggregator = eventAggregator;
            _deviceParameterInfoService = deviceParameterInfoService;
            _eventAggregator.GetEvent<HistoryDataUpdatedEvent>().Subscribe(UpdateHistoryDataSeriesCollection);
            _eventAggregator.GetEvent<SelectedEquipmentChangedEvent>().Subscribe(ResetHistoryDataSeriesCollection);
        }

        private void ResetHistoryDataSeriesCollection(SelectedEquipmentChangedEventArg obj)
        {
            SeriesCollection.Clear();
            Labels.Clear();
        }

        private void UpdateHistoryDataSeriesCollection(HistoryDataUpdatedEventArg arg)
        {
            var dataList = arg.HistoryDataDtos.ToList();
            SeriesCollection.Clear();
            Labels.Clear();
            if (dataList.Count > 0)
            {
                foreach (var historyData in dataList)
                {
                    var lineSeries = new LineSeries();
                    lineSeries.Title = historyData.pointName;
                    lineSeries.Values = new ChartValues<double>();
                    foreach (var dataValue in historyData.dataValue)
                    {
                        lineSeries.Values.Add(double.Parse(dataValue.value));
                    }
                    SeriesCollection.Add(lineSeries);
                }

                var labelList = new List<string>();
                dataList[0].dataValue.ForEach(d => Labels.Add(d.time));
            }
        }

        private SeriesCollection _seriesCollection = new SeriesCollection();
        public SeriesCollection SeriesCollection
        {
            get { return _seriesCollection; }
            set { SetProperty(ref _seriesCollection, value); }
        }

        private ObservableCollection<string> _labels = new ObservableCollection<string>();
        public ObservableCollection<string> Labels
        {
            get { return _labels; }
            set { SetProperty(ref _labels, value); }
        }
    }
}
