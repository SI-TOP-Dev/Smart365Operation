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
using LiveCharts.Configurations;

namespace Smart365Operation.Modules.DataAnalysis.ViewModels
{
    public class DataCurveChartViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDeviceParameterInfoService _deviceParameterInfoService;
        private readonly CartesianMapper<DateModel> _dateConfig;

        public DataCurveChartViewModel(IEventAggregator eventAggregator, IDeviceParameterInfoService deviceParameterInfoService)
        {
            _eventAggregator = eventAggregator;
            _deviceParameterInfoService = deviceParameterInfoService;
            _dateConfig = Mappers.Xy<DateModel>()
                .X(m => (double)m.DateTime.Ticks / TimeSpan.FromHours(1).Ticks)
                .Y(m => m.Value);
            SeriesCollection = new SeriesCollection(_dateConfig);
            _eventAggregator.GetEvent<HistoryDataUpdatedEvent>().Subscribe(UpdateHistoryDataSeriesCollection);
            _eventAggregator.GetEvent<SelectedEquipmentChangedEvent>().Subscribe(ResetHistoryDataSeriesCollection);
        }

        private void ResetHistoryDataSeriesCollection(SelectedEquipmentChangedEventArg obj)
        {
            SeriesCollection.Clear();
        }

        private void UpdateHistoryDataSeriesCollection(HistoryDataUpdatedEventArg arg)
        {
            var dataList = arg.HistoryDataDtos.ToList();
            SeriesCollection.Clear();
            if (dataList.Count > 0)
            {
                foreach (var historyData in dataList)
                {
                    var lineSeries = new LineSeries();
                    lineSeries.Title = historyData.pointName;
                    lineSeries.Values = new ChartValues<DateModel>();
                    foreach (var dataValue in historyData.dataValue)
                    {
                        lineSeries.Values.Add(new DateModel { Value = double.Parse(dataValue.value), DateTime = dataValue.time });
                    }
                    SeriesCollection.Add(lineSeries);
                }
                string unit = dataList[0]?.unit;
                var labelList = new List<string>();
                XFormatter = value => new System.DateTime((long)(value * TimeSpan.FromHours(1).Ticks)).ToString("yyyy-MM-dd HH:mm");
                YFormatter = value => $"{value}{unit}";
            }
        }

        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get { return _seriesCollection; }
            set { SetProperty(ref _seriesCollection, value); }
        }


        private Func<double, string> _xFormatter;
        public Func<double, string> XFormatter
        {
            get { return _xFormatter; }
            set
            {
                SetProperty(ref _xFormatter, value);
            }
        }


        private Func<double, string> _yFormatter;
        public Func<double, string> YFormatter
        {
            get { return _yFormatter; }
            set
            {
                SetProperty(ref _yFormatter, value);
            }
        }
    }

    public class DateModel
    {
        public DateTime DateTime { get; set; }
        public double Value { get; set; }
    }
}
