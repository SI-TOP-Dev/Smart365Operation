using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Smart365Operation.Modules.Dashboard.Interfaces;
using System.Windows;
using System.Collections.ObjectModel;
using Smart365Operations.Common.Infrastructure.Models.TO;

namespace Smart365Operation.Modules.Dashboard
{
    public class DataStatisticsViewModel : BindableBase
    {
        private readonly IDataStatisticsService _dataStatisticsService;

        public DataStatisticsViewModel()
        {

        }

        public DataStatisticsViewModel(IDataStatisticsService dataStatisticsService)
        {
            _dataStatisticsService = dataStatisticsService;
            Initialize();
        }

        private void Initialize()
        {
            GetAlarmStatisticsInfo();
            GetInspectionInfos();
            GetCustomerIndustryCategoryInfo();

        }


        #region Inspection Statistics

        private SeriesCollection _inspectionStatisticsInfoSeriesCollection = new SeriesCollection();
        public SeriesCollection InspectionStatisticsInfoSeriesCollection
        {
            get { return _inspectionStatisticsInfoSeriesCollection; }
            set { SetProperty(ref _inspectionStatisticsInfoSeriesCollection, value); }
        }
        private ObservableCollection<string> _customerIncrementsLabels = new ObservableCollection<string>();
        public ObservableCollection<string> CustomerIncrementsLabels
        {
            get { return _customerIncrementsLabels; }
            set { SetProperty(ref _customerIncrementsLabels, value); }
        }

        private Func<double, string> _customerIncrementsFormatter;
        public Func<double, string> CustomerIncrementsFormatter
        {
            get { return _customerIncrementsFormatter; }
            set { SetProperty(ref _customerIncrementsFormatter, value); }
        }
        private double _maxCustormerNumber;
        public double MaxCustormerNumber
        {
            get { return _maxCustormerNumber; }
            set { SetProperty(ref _maxCustormerNumber, value); }
        }

        private void GetInspectionInfos()
        {
            //var customerIncrementsInfo = _dataStatisticsService.GetInspectionStatisticsInfo();
            //List<int> addCustomers = new List<int>();
            //List<string> labels = new List<string>();
            //var maxItem = customerIncrementsInfo.Max(i => i.completeInspectionCount);
            //foreach (var customerIncrementsDto in customerIncrementsInfo)
            //{
            //    //oldCustomers.Add(customerIncrementsDto.existing);
            //    addCustomers.Add(customerIncrementsDto.completeInspectionCount);
            //    labels.Add(customerIncrementsDto.customerName);
            //}

            //var addCustomersColumnSeries = new StackedColumnSeries
            //{
            //    Title = "次",
            //    Values = new ChartValues<int>(addCustomers),
            //    StackMode = StackMode.Values, // this is not necessary, values is the default stack mode
            //    DataLabels = true
            //};

            //Application.Current.Dispatcher.Invoke(new Action(() =>
            //{
            //    //CustomerIncrementsInfoSeriesCollection.Add(oldCustomersColumnSeries);
            //    InspectionStatisticsInfoSeriesCollection.Add(addCustomersColumnSeries);
            //    CustomerIncrementsLabels = new ObservableCollection<string>(labels);
            //    MaxCustormerNumber = maxItem;
            //    CustomerIncrementsFormatter = value => value.ToString();// + "家";
            //}));
            InspectionInfos.Clear();

            var inspectionInfos = _dataStatisticsService.GetInspectionStatisticsInfo();
            InspectionInfos.AddRange(inspectionInfos);
        }

        private ObservableCollection<InspectionStatisticsDTO> _inspectionInfos = new ObservableCollection<InspectionStatisticsDTO>();
        public ObservableCollection<InspectionStatisticsDTO> InspectionInfos
        {
            get { return _inspectionInfos; }
            set { SetProperty(ref _inspectionInfos, value); }
        }

        #endregion


        #region Customer Industry Category Statistics

        private SeriesCollection _customerIndustryCategorySeriesCollection = new SeriesCollection();
        public SeriesCollection CustomerIndustryCategorySeriesCollection
        {
            get { return _customerIndustryCategorySeriesCollection; }
            set { SetProperty(ref _customerIndustryCategorySeriesCollection, value); }
        }
        private void GetCustomerIndustryCategoryInfo()
        {
            var customerIndustryCategoryInfo = _dataStatisticsService.GetCustomerIndustryCategoryInfo();
            List<PieSeries> pieSeriesList = new List<PieSeries>();
            foreach (var customerIndustryCategoryDto in customerIndustryCategoryInfo)
            {
                var industryCategoryInfo = new PieSeries
                {
                    Values = new ChartValues<int>() { customerIndustryCategoryDto.count },
                    Title = customerIndustryCategoryDto.typeName,
                    LabelPoint = chartPoint => string.Format("{0}kVA {1:P})", chartPoint.Y, chartPoint.Participation),
                    DataLabels = true
                };
                pieSeriesList.Add(industryCategoryInfo);
            }
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                CustomerIndustryCategorySeriesCollection.AddRange(pieSeriesList);
            }));
        }

        #endregion

        #region Customer Alarm Statistics


        private void GetAlarmStatisticsInfo()
        {
            var alarmStatisticsInfo = _dataStatisticsService.GetAlarmStatisticsInfo();
            List<PieSeries> pieSeriesList = new List<PieSeries>();
            foreach (var alarmStatisticsDto in alarmStatisticsInfo)
            {
                var industryCategoryInfo = new PieSeries
                {
                    Values = new ChartValues<int>() { alarmStatisticsDto.count },
                    Title = alarmStatisticsDto.name,
                    LabelPoint = chartPoint => string.Format("{0}次 {1:P})", chartPoint.Y, chartPoint.Participation),
                    DataLabels = true
                };
                pieSeriesList.Add(industryCategoryInfo);
            }
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                AlarmStatisticsSeriesCollection.AddRange(pieSeriesList);
            }));
        }

        private SeriesCollection _alarmStatisticsSeriesCollection = new SeriesCollection();
        public SeriesCollection AlarmStatisticsSeriesCollection
        {
            get { return _alarmStatisticsSeriesCollection; }
            set { SetProperty(ref _alarmStatisticsSeriesCollection, value); }
        }
        //private ObservableCollection<string> _alarmStatisticsLabels = new ObservableCollection<string>();
        //public ObservableCollection<string> AlarmStatisticsLabels
        //{
        //    get { return _alarmStatisticsLabels; }
        //    set { SetProperty(ref _alarmStatisticsLabels, value); }
        //}

        //private Func<double, string> _alarmStatisticsFormatter;
        //public Func<double, string> AlarmStatisticsFormatter
        //{
        //    get { return _alarmStatisticsFormatter; }
        //    set { SetProperty(ref _alarmStatisticsFormatter, value); }
        //}
        //private void GetAlarmStatisticsInfo()
        //{
        //    var alarmStatisticsInfo = _dataStatisticsService.GetAlarmStatisticsInfo();
        //    if (alarmStatisticsInfo == null)
        //    {
        //        return;
        //    }
        //    List<int> alarmCountList = new List<int>();
        //    List<int> untreatedCountList = new List<int>();
        //    List<string> labels = new List<string>();
        //    var infoList = alarmStatisticsInfo.OrderByDescending(i => DateTime.Parse(i.date));
        //    foreach (var alarmStatisticsDto in infoList)
        //    {
        //        alarmCountList.Add(alarmStatisticsDto.alarmCount);
        //        untreatedCountList.Add(alarmStatisticsDto.untreatedCount);
        //        labels.Add(alarmStatisticsDto.date);
        //    }
        //    var alarmCountColumnSeries = new StackedColumnSeries
        //    {
        //        Title = "每月发生次数",
        //        Values = new ChartValues<int>(alarmCountList),
        //        StackMode = StackMode.Values, // this is not necessary, values is the default stack mode
        //        DataLabels = true
        //    };
        //    var untreatedCountColumnSeries = new StackedColumnSeries
        //    {
        //        Title = "未处理报警次数",
        //        Values = new ChartValues<int>(untreatedCountList),
        //        StackMode = StackMode.Values, // this is not necessary, values is the default stack mode
        //        DataLabels = true
        //    };

        //    Application.Current.Dispatcher.Invoke(new Action(() =>
        //    {
        //        AlarmStatisticsSeriesCollection.Add(alarmCountColumnSeries);
        //        AlarmStatisticsSeriesCollection.Add(untreatedCountColumnSeries);
        //        AlarmStatisticsLabels = new ObservableCollection<string>(labels);

        //        AlarmStatisticsFormatter = value => value.ToString() + "次";
        //    }));

        //}

        #endregion
    }
}
