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
            GetCustomerIncrementsInfo();
            GetCustomerIndustryCategoryInfo();

        }


        #region Customer Increments Statistics

        private SeriesCollection _customerIncrementsInfoSeriesCollection = new SeriesCollection();
        public SeriesCollection CustomerIncrementsInfoSeriesCollection
        {
            get { return _customerIncrementsInfoSeriesCollection; }
            set { SetProperty(ref _customerIncrementsInfoSeriesCollection, value); }
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

        private void GetCustomerIncrementsInfo()
        {
            var customerIncrementsInfo = _dataStatisticsService.GetCustomerIncrementsInfo();
            List<int> oldCustomers = new List<int>();
            List<int> addCustomers = new List<int>();
            List<string> labels = new List<string>();
            var maxItem = customerIncrementsInfo.Max(i => i.existing + i.increased);
            var infoList = customerIncrementsInfo.OrderByDescending(i => DateTime.Parse(i.date));
            foreach (var customerIncrementsDto in infoList)
            {
                oldCustomers.Add(customerIncrementsDto.existing);
                addCustomers.Add(customerIncrementsDto.increased);
                labels.Add(customerIncrementsDto.date);
            }
            var oldCustomersColumnSeries = new StackedColumnSeries
            {
                Title = "累计客户",
                Values = new ChartValues<int>(oldCustomers),
                StackMode = StackMode.Values, // this is not necessary, values is the default stack mode
                DataLabels = true
            };
            var addCustomersColumnSeries = new StackedColumnSeries
            {
                Title = "新增客户",
                Values = new ChartValues<int>(addCustomers),
                StackMode = StackMode.Values, // this is not necessary, values is the default stack mode
                DataLabels = true
            };

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                CustomerIncrementsInfoSeriesCollection.Add(oldCustomersColumnSeries);
                CustomerIncrementsInfoSeriesCollection.Add(addCustomersColumnSeries);
                CustomerIncrementsLabels = new ObservableCollection<string>(labels);
                MaxCustormerNumber = maxItem;
                CustomerIncrementsFormatter = value => value.ToString() + "家";
            }));

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
                    LabelPoint = chartPoint => string.Format("{0}家 ({1:P})", chartPoint.Y, chartPoint.Participation),
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

        private SeriesCollection _alarmStatisticsSeriesCollection = new SeriesCollection();
        public SeriesCollection AlarmStatisticsSeriesCollection
        {
            get { return _alarmStatisticsSeriesCollection; }
            set { SetProperty(ref _alarmStatisticsSeriesCollection, value); }
        }
        private ObservableCollection<string> _alarmStatisticsLabels = new ObservableCollection<string>();
        public ObservableCollection<string> AlarmStatisticsLabels
        {
            get { return _alarmStatisticsLabels; }
            set { SetProperty(ref _alarmStatisticsLabels, value); }
        }

        private Func<double, string> _alarmStatisticsFormatter;
        public Func<double, string> AlarmStatisticsFormatter
        {
            get { return _alarmStatisticsFormatter; }
            set { SetProperty(ref _alarmStatisticsFormatter, value); }
        }
        private void GetAlarmStatisticsInfo()
        {
            var alarmStatisticsInfo = _dataStatisticsService.GetAlarmStatisticsInfo();
            if (alarmStatisticsInfo == null)
            {
                return;
            }
            List<int> alarmCountList = new List<int>();
            List<int> untreatedCountList = new List<int>();
            List<string> labels = new List<string>();
            var infoList = alarmStatisticsInfo.OrderByDescending(i => DateTime.Parse(i.date));
            foreach (var alarmStatisticsDto in infoList)
            {
                alarmCountList.Add(alarmStatisticsDto.alarmCount);
                untreatedCountList.Add(alarmStatisticsDto.untreatedCount);
                labels.Add(alarmStatisticsDto.date);
            }
            var alarmCountColumnSeries = new StackedColumnSeries
            {
                Title = "每月发生次数",
                Values = new ChartValues<int>(alarmCountList),
                StackMode = StackMode.Values, // this is not necessary, values is the default stack mode
                DataLabels = true
            };
            var untreatedCountColumnSeries = new StackedColumnSeries
            {
                Title = "未处理报警次数",
                Values = new ChartValues<int>(untreatedCountList),
                StackMode = StackMode.Values, // this is not necessary, values is the default stack mode
                DataLabels = true
            };

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                AlarmStatisticsSeriesCollection.Add(alarmCountColumnSeries);
                AlarmStatisticsSeriesCollection.Add(untreatedCountColumnSeries);
                AlarmStatisticsLabels = new ObservableCollection<string>(labels);

                AlarmStatisticsFormatter = value => value.ToString() + "次";
            }));

        }

        #endregion
    }
}
