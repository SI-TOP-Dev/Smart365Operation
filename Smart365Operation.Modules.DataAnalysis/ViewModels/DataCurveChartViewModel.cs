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
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using Abt.Controls.SciChart.Visuals.Axes;
using Abt.Controls.SciChart.Visuals.PointMarkers;

namespace Smart365Operation.Modules.DataAnalysis.ViewModels
{
    public class DataCurveChartViewModel : BindableBase
    {
        private readonly Prism.Events.IEventAggregator _eventAggregator;
        private readonly IDeviceParameterInfoService _deviceParameterInfoService;
        private readonly CartesianMapper<DateModel> _dateConfig;
        private List<Color> _colors = new List<Color>()
        {
            (Color)ColorConverter.ConvertFromString("#ffffff"),
            (Color)ColorConverter.ConvertFromString("#f16464"),
            (Color)ColorConverter.ConvertFromString("#ffdf06"),
            (Color)ColorConverter.ConvertFromString("#739eff"),
            (Color)ColorConverter.ConvertFromString("#4cc574"),
            (Color)ColorConverter.ConvertFromString("#d862ff")
        };

        public DataCurveChartViewModel(Prism.Events.IEventAggregator eventAggregator, IDeviceParameterInfoService deviceParameterInfoService)
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
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                SeriesViewModels.Clear();

                if (dataList.Count > 0)
                {
                    int index = 0;
                    var colors = _colors;
                    foreach (var historyData in dataList)
                    {
                        var ds0 = new XyDataSeries<DateTime, double>() { SeriesName = historyData.pointName };

                        SeriesViewModels.Add(
                            new ChartSeriesViewModel(ds0, 
                                new FastLineRenderableSeries()
                                {
                                    SeriesColor = colors[index],
                                    StrokeThickness = 2,
                                    //PointMarker = new EllipsePointMarker()
                                    //{
                                    //    Fill = colors[index],
                                    //    Stroke = colors[index],
                                    //    StrokeThickness = 1,
                                    //    Width = 3,
                                    //    Height = 3,
                                    //}
                                }
                                ));
                        index++;
                        List<DatavalueDTO> data = historyData.dataValue.OrderBy(d => d.time).ToList();
                        ds0.Append(data.Select(x => x.time), data.Select(y => double.Parse(y.value)));
                    }

                    ViewportManager.ZoomExtents();
                    YAxis.VisibleRange.GrowBy(0.4, 0.4);

                }

            }));

            //Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    ResetDataSeries();

            //    if (dataList.Count > 0)
            //    {
            //        foreach (var historyData in dataList)
            //        {
            //            var lineSeries = new LineSeries();
            //            lineSeries.Title = historyData.pointName;
            //            lineSeries.Values = new ChartValues<DateModel>();
            //            lineSeries.LabelPoint = value => $"{value.Y} {historyData.unit}";

            //            foreach (var dataValue in historyData.dataValue)
            //            {
            //                lineSeries.Values.Add(new DateModel { Value = double.Parse(dataValue.value), DateTime = dataValue.time });
            //            }
            //            SeriesCollection.Add(lineSeries);
            //        }
            //        string unit = dataList[0]?.unit;
            //        string dateFormat = string.Empty;
            //        switch (timeType)
            //        {
            //            case TimeType.Day:
            //                dateFormat = "HH:mm";
            //                break;
            //            case TimeType.Month:
            //                dateFormat = "d日 HH:mm";
            //                break;
            //            case TimeType.Year:
            //                dateFormat = "MM月-d日 HH:mm";
            //                break;
            //            default:
            //                break;
            //        }

            //        YFormatter = value => $"{value} {unit}";
            //        XFormatter = value =>
            //        {
            //            if (value > 0)
            //            {
            //                var resultValue = new System.DateTime((long)(value * TimeSpan.FromHours(1).Ticks)).ToString(dateFormat);
            //                return resultValue;
            //            }
            //            else
            //                return "0";
            //        };

            //        var colors = CartesianChart.Colors;
            //        int index = 0;
            //        List<SeriesInfo> seriesList = new List<SeriesInfo>();
            //        foreach (var item in SeriesCollection.Chart.View.ActualSeries)
            //        {
            //            var seriesItem = (item as Series);
            //            if (seriesItem != null)
            //            {

            //                var color = colors.ElementAt(index++);

            //                seriesList.Add(new SeriesInfo()
            //                {
            //                    Title = seriesItem.Title,
            //                    Fill = new SolidColorBrush(color),// == null ? (seriesItem as IFondeable).PointForeround : seriesItem.Fill,
            //                    Stroke = seriesItem.Stroke,
            //                    StrokeThickness = seriesItem.StrokeThickness
            //                });


            //            }
            //        }

            //        SeriesList.AddRange(seriesList);

            //    }
            //    DataTypeName = dataTypeName;
            //}));
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


        private readonly double _barTimeFrame = TimeSpan.FromMinutes(5).TotalSeconds;
        public double BarTimeFrame { get { return _barTimeFrame; } }

        private ObservableCollection<IChartSeriesViewModel> _seriesViewModels = new ObservableCollection<IChartSeriesViewModel>();
        public ObservableCollection<IChartSeriesViewModel> SeriesViewModels
        {
            get { return _seriesViewModels; }
            set
            {
                SetProperty(ref _seriesViewModels, value);
            }
        }

        private IViewportManager _viewportManager = new DefaultViewportManager();
        public IViewportManager ViewportManager
        {
            get { return _viewportManager; }
            set
            {
                SetProperty(ref _viewportManager, value);
            }
        }
        private IAxis _yAxis = new NumericAxis() { AutoRange = AutoRange.Once, GrowBy = new DoubleRange(0.4, 0.4) };
        public IAxis YAxis
        {
            get { return _yAxis; }
            set
            {
                SetProperty(ref _yAxis, value);
            }
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
