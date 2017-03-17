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
using Prism.Commands;
using System.Windows.Media;
using System.Windows;
using LiveCharts.Wpf.Components;
using Smart365Operations.Common.Infrastructure.Models;

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
            ResetDataSeries();
        }

        private void UpdateHistoryDataSeriesCollection(HistoryDataUpdatedEventArg arg)
        {
            var dataList = arg.HistoryDataDtos.ToList();
            var timeType = arg.DataTimeType;
            var dataTypeName = arg.DataTypeName;

            ResetDataSeries();

            if (dataList.Count > 0)
            {
                foreach (var historyData in dataList)
                {
                    var lineSeries = new LineSeries();
                    lineSeries.Title = historyData.pointName;
                    lineSeries.Values = new ChartValues<DateModel>();
                    lineSeries.LabelPoint = value => $"{value.Y} {historyData.unit}";

                    foreach (var dataValue in historyData.dataValue)
                    {
                        lineSeries.Values.Add(new DateModel { Value = double.Parse(dataValue.value), DateTime = dataValue.time });
                    }
                    SeriesCollection.Add(lineSeries);
                }
                string unit = dataList[0]?.unit;
                string dateFormat = string.Empty;
                switch (timeType)
                {
                    case TimeType.Day:
                        dateFormat = "yyyy-MM-dd HH:mm";
                        break;
                    case TimeType.Month:
                        dateFormat = "yyyy-MM-dd";
                        break;
                    case TimeType.Year:
                        dateFormat = "yyyy-MM";
                        break;
                    default:
                        break;
                }
                XFormatter = value => new System.DateTime((long)(value * TimeSpan.FromHours(1).Ticks)).ToString(dateFormat);
                YFormatter = value => $"{value} {unit}";
                var colors = CartesianChart.Colors;
                int index = 0;
                foreach (var item in SeriesCollection.Chart.View.ActualSeries)
                {
                    var seriesItem = (item as Series);
                    if (seriesItem != null)
                    {

                        var color = colors.ElementAt(index++);
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            SeriesList.Add(new SeriesInfo()
                            {
                                Title = seriesItem.Title,
                                Fill = new SolidColorBrush(color),// == null ? (seriesItem as IFondeable).PointForeround : seriesItem.Fill,
                                Stroke = seriesItem.Stroke,
                                StrokeThickness = seriesItem.StrokeThickness
                            });
                        }));

                    }
                }
            }
            DataTypeName = dataTypeName;
        }

        private void ResetDataSeries()
        {
            SeriesCollection.Clear();
            SeriesList.Clear();
            DataTypeName = string.Empty;
        }

        private string _dataTypeName = string.Empty;
        public string DataTypeName
        {
            get { return _dataTypeName; }
            set { SetProperty(ref _dataTypeName, value); }
        }

        private ObservableCollection<SeriesInfo> _seriesList = new ObservableCollection<SeriesInfo>();
        public ObservableCollection<SeriesInfo> SeriesList
        {
            get { return _seriesList; }
            set
            {
                SetProperty(ref _seriesList, value);
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


        public DelegateCommand<object> CheckCommand => new DelegateCommand<object>(CheckVisible, CanCheckVisible);
 
        private void CheckVisible(object obj)
        {
            string title = obj as string;
            var series = SeriesCollection.FirstOrDefault(s => s.Title == title);
            if (series != null)
            {
                var element = (series as System.Windows.FrameworkElement);
                if (element != null)
                {
                    element.Visibility = element.Visibility == System.Windows.Visibility.Visible ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
                }
            }
        }

        private bool CanCheckVisible(object arg)
        {
            return true;
        }
    }

    public class DateModel
    {
        public DateTime DateTime { get; set; }
        public double Value { get; set; }
    }

    public class SeriesInfo
    {
        public string Title { get; set; }
        public Brush Stroke { get; set; }
        public double StrokeThickness { get; set; }
        public Brush Fill { get; set; }
    }
}
