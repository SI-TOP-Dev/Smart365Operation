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

namespace Smart365Operation.Modules.Dashboard
{
    public class DataStatisticsViewModel : BindableBase
    {
        private readonly IDataStatisticsService _dataStatisticsService;

        public DataStatisticsViewModel(IDataStatisticsService dataStatisticsService)
        {
            _dataStatisticsService = dataStatisticsService;
            Initialize();
        }

        private void Initialize()
        {
            GetCustomerIncrementsInfo();
            GetCustomerIndustryCategoryInfo();
            GetAlarmStatisticsInfo();
        }


        #region Customer Increments Statistics

        private SeriesCollection _customerIncrementsInfoSeriesCollection = new SeriesCollection();
        public SeriesCollection CustomerIncrementsInfoSeriesCollection
        {
            get { return _customerIncrementsInfoSeriesCollection; }
            set { SetProperty(ref _customerIncrementsInfoSeriesCollection, value); }
        }
        private List<string> _customerIncrementsLabels = new List<string>();
        public List<string> CustomerIncrementsLabels
        {
            get { return _customerIncrementsLabels; }
            set { SetProperty(ref _customerIncrementsLabels, value); }
        }

        private Func<int, string> _customerIncrementsFormatter;
        public Func<int, string> CustomerIncrementsFormatter
        {
            get { return _customerIncrementsFormatter; }
            set { SetProperty(ref _customerIncrementsFormatter, value); }
        }
        private void GetCustomerIncrementsInfo()
        {
            var customerIncrementsInfo = _dataStatisticsService.GetCustomerIncrementsInfo();
            List<int> oldCustomers = new List<int>();
            List<int> addCustomers = new List<int>();
            List<string> labels = new List<string>();
            foreach (var customerIncrementsDto in customerIncrementsInfo)
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

            CustomerIncrementsInfoSeriesCollection.Add(oldCustomersColumnSeries);
            CustomerIncrementsInfoSeriesCollection.Add(addCustomersColumnSeries);
            CustomerIncrementsLabels = new List<string>(labels);

            CustomerIncrementsFormatter = value => value.ToString()+"家";
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
            foreach (var customerIndustryCategoryDto in customerIndustryCategoryInfo)
            {
                var industryCategoryInfo = new PieSeries
                {
                    Values = new ChartValues<int>() { customerIndustryCategoryDto.count },
                    Title = customerIndustryCategoryDto.typeName,
                    LabelPoint = chartPoint => string.Format("{0}家 ({1:P})", chartPoint.Y, chartPoint.Participation),
                    DataLabels = true
                };
                CustomerIndustryCategorySeriesCollection.Add(industryCategoryInfo);
            }
        }

        #endregion

        #region Customer Alarm Statistics

        private SeriesCollection _alarmStatisticsSeriesCollection = new SeriesCollection();
        public SeriesCollection AlarmStatisticsSeriesCollection
        {
            get { return _alarmStatisticsSeriesCollection; }
            set { SetProperty(ref _alarmStatisticsSeriesCollection, value); }
        }
        private List<string> _alarmStatisticsLabels = new List<string>();
        public List<string> AlarmStatisticsLabels
        {
            get { return _alarmStatisticsLabels; }
            set { SetProperty(ref _alarmStatisticsLabels, value); }
        }

        private Func<int, string> _alarmStatisticsFormatter;
        public Func<int, string> AlarmStatisticsFormatter
        {
            get { return _alarmStatisticsFormatter; }
            set { SetProperty(ref _alarmStatisticsFormatter, value); }
        }
        private void GetAlarmStatisticsInfo()
        {
            var alarmStatisticsInfo = _dataStatisticsService.GetAlarmStatisticsInfo();
            List<int> alarmCountList = new List<int>();
            List<int> untreatedCountList = new List<int>();
            List<string> labels = new List<string>();
            foreach (var alarmStatisticsDto in alarmStatisticsInfo)
            {
                alarmCountList.Add(alarmStatisticsDto.alarmCount);
                untreatedCountList.Add(alarmStatisticsDto.untreatedCount);
                labels.Add(alarmStatisticsDto.date);
            }
            var alarmCountColumnSeries = new StackedColumnSeries
            {
                Title = "告警总数",
                Values = new ChartValues<int>(alarmCountList),
                StackMode = StackMode.Values, // this is not necessary, values is the default stack mode
                DataLabels = true
            };
            var untreatedCountColumnSeries = new StackedColumnSeries
            {
                Title = "未处理告警数",
                Values = new ChartValues<int>(untreatedCountList),
                StackMode = StackMode.Values, // this is not necessary, values is the default stack mode
                DataLabels = true
            };

            AlarmStatisticsSeriesCollection.Add(alarmCountColumnSeries);
            AlarmStatisticsSeriesCollection.Add(untreatedCountColumnSeries);
            AlarmStatisticsLabels = new List<string>(labels);

            AlarmStatisticsFormatter = value => value.ToString() + "次";
        }

        #endregion
    }
}
