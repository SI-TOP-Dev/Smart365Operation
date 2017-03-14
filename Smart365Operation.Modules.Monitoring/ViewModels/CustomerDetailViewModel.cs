using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Mvvm;
using Prism.Regions;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Models;
using RestSharp;
using Com.Shengzuo.RuntimeCore;
using Com.Shengzuo.RuntimeCore.Common;
using LiveCharts;
using LiveCharts.Wpf;
using Smart365Operations.Common.Infrastructure.Models.TO;
using System.Windows.Controls;
using LiveCharts.Configurations;

namespace Smart365Operation.Modules.Monitoring.ViewModels
{
    public class CustomerDetailViewModel : BindableBase, INavigationAware
    {
        private readonly IWiringDiagramService _wiringDiagramService;
        private readonly IMonitoringDataService _monitoringDataService;
        private readonly IMonitoringSummaryService _monitoringSummaryService;
        private readonly ICustomerEquipmentService _customerEquipmentService;
        private UIManager _uiManager;

        public CustomerDetailViewModel(ICustomerEquipmentService customerEquipmentService, IWiringDiagramService wiringDiagramService, IMonitoringDataService monitoringDataService, IMonitoringSummaryService monitoringSummaryService)
        {
            _wiringDiagramService = wiringDiagramService;
            _monitoringDataService = monitoringDataService;
            _monitoringSummaryService = monitoringSummaryService;
            _customerEquipmentService = customerEquipmentService;
            _monitoringDataService.MonitoringDataUpdated += _monitoringDataService_DataUpdated;
            _uiManager = UIManager.Instance;
            _uiManager.Dispatcher = Application.Current.Dispatcher;
            _uiManager.EnableSafeMode = true;

        }

        private void _monitoringDataService_DataUpdated(object sender, MonitoringDataEventArgs e)
        {
            //Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            //{
            var action = new Action(() =>
            {
                _uiManager.UpdateData(e.Key, e.Value);
            });
            action.BeginInvoke(null, null);

            //}));

        }


        private Customer _currentCustomer;
        public Customer CurrentCustomer
        {
            get { return _currentCustomer; }
            set { SetProperty(ref _currentCustomer, value); }
        }

        private ObservableCollection<EquipmentDTO> _deviceList = new ObservableCollection<EquipmentDTO>();
        public ObservableCollection<EquipmentDTO> DeviceList
        {
            get { return _deviceList; }
            set { SetProperty(ref _deviceList, value); }
        }

        private EquipmentDTO _selectedDevice;
        public EquipmentDTO SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                if (value != _selectedDevice)
                {
                    GetDevicePowerInfoTaskAsync(value.equipmentId.ToString(), SelectedDate);
                }
                SetProperty(ref _selectedDevice, value);
            }
        }

        private DateTime _selectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (value.Year != _selectedDate.Year || value.Month != _selectedDate.Month)
                {
                    GetDevicePowerInfoTaskAsync(SelectedDevice.equipmentId.ToString(), value);
                }
                SetProperty(ref _selectedDate, value);
            }
        }

        private FrameworkElement _wiringDiagramUI;
        public FrameworkElement WiringDiagramUI
        {
            get { return _wiringDiagramUI; }
            set { SetProperty(ref _wiringDiagramUI, value); }
        }

        private AlarmSummaryDTO _alarmSummaryInfo;
        public AlarmSummaryDTO AlarmSummaryInfo
        {
            get { return _alarmSummaryInfo; }
            set { SetProperty(ref _alarmSummaryInfo, value); }
        }

        private PowerSummaryDTO _powerSummaryInfo;
        public PowerSummaryDTO PowerSummaryInfo
        {
            get { return _powerSummaryInfo; }
            set { SetProperty(ref _powerSummaryInfo, value); }
        }

        private ObservableCollection<TopPowerDTO> _topPowerSummaryList;
        public ObservableCollection<TopPowerDTO> TopPowerSummaryList
        {
            get { return _topPowerSummaryList; }
            set { SetProperty(ref _topPowerSummaryList, value); }
        }

        private SeriesCollection _topPowerSummaryInfoSeriesCollection = new SeriesCollection();
        public SeriesCollection TopPowerSummaryInfoSeriesCollection
        {
            get { return _topPowerSummaryInfoSeriesCollection; }
            set { SetProperty(ref _topPowerSummaryInfoSeriesCollection, value); }
        }

        private ObservableCollection<string> _topPowerSummaryInfoLabels = new ObservableCollection<string>();
        public ObservableCollection<string> TopPowerSummaryInfoLabels
        {
            get { return _topPowerSummaryInfoLabels; }
            set { SetProperty(ref _topPowerSummaryInfoLabels, value); }
        }

        private Func<double, string> _topPowerSummaryInfoFormatter;
        public Func<double, string> TopPowerSummaryInfoFormatter
        {
            get { return _topPowerSummaryInfoFormatter; }
            set { SetProperty(ref _topPowerSummaryInfoFormatter, value); }
        }

        private Func<string, string> _topPowerSummaryInfoDeviceFormatter;
        public Func<string, string> TopPowerSummaryInfoDeviceFormatter
        {
            get { return _topPowerSummaryInfoDeviceFormatter; }
            set { SetProperty(ref _topPowerSummaryInfoDeviceFormatter, value); }
        }

        private SeriesCollection _devicePowerSeriesCollection;
        public SeriesCollection DevicePowerSeriesCollection
        {
            get { return _devicePowerSeriesCollection; }
            set { SetProperty(ref _devicePowerSeriesCollection, value); }
        }

        private Func<double, string> _devicePowerXFormatter;
        public Func<double, string> DevicePowerXFormatter
        {
            get { return _devicePowerXFormatter; }
            set { SetProperty(ref _devicePowerXFormatter, value); }
        }

        private Func<double, string> _devicePowerYFormatter;
        public Func<double, string> DevicePowerYFormatter
        {
            get { return _devicePowerYFormatter; }
            set { SetProperty(ref _devicePowerYFormatter, value); }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var customer = navigationContext.Parameters["Customer"] as Customer;
            if (customer != null)
            {
                if (CurrentCustomer == null)
                {
                    CurrentCustomer = customer;
                    var customerId = CurrentCustomer.Id.ToString();
                    SetWiringDiagramUITaskAsync(customer.Id.ToString());

                    GetDefaultDevicePowerInfoTaskAsync(customerId);
                    GetAlarmSummaryInfoTaskAsync(customerId);
                    GetPowerSummaryInfoTaskAsync(customerId);
                    GetTopPowerSummaryInfoListTaskAsync(customerId);
                }
                else
                {
                    SetWiringDiagramUITaskAsync(customer.Id.ToString());
                    if (CurrentCustomer.Id != customer.Id)
                    {
                        var customerId = CurrentCustomer.Id.ToString();
                        GetDefaultDevicePowerInfoTaskAsync(customerId);
                        GetAlarmSummaryInfoTaskAsync(customerId);
                        GetPowerSummaryInfoTaskAsync(customerId);
                        GetTopPowerSummaryInfoListTaskAsync(customerId);
                    }
                }

            }
        }

        public Task GetDefaultDevicePowerInfoTaskAsync(string customerId) => Task.Run(() =>
        {
            GetDefaultDevicePowerInfo(customerId);
        });
        private void GetDefaultDevicePowerInfo(string customerId)
        {
            var deviceSummaryList = _customerEquipmentService.GetCustomerEquipmentTable(customerId);
            List<EquipmentDTO> deviceList = new List<EquipmentDTO>();
            deviceSummaryList.areaList.ForEach(a => a.switchingRoomList.ForEach(r => deviceList.AddRange(r.equipmentList)));
            deviceList = deviceList.Distinct(new EquipmentIdComparer()).ToList();
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                DeviceList.Clear();
                DeviceList.AddRange(deviceList);
                SelectedDevice = DeviceList.Count != 0 ? DeviceList[0] : null;
            }));
            if (SelectedDevice != null)
                GetDevicePowerInfoTaskAsync(SelectedDevice.equipmentId.ToString(), DateTime.Now);
        }

        public Task GetDevicePowerInfoTaskAsync(string deviceId, DateTime dateTime) => Task.Run(() =>
        {
            GetDevicePowerInfo(deviceId, dateTime);
        });

        private void GetDevicePowerInfo(string deviceId, DateTime dateTime)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (DevicePowerSeriesCollection == null)
                {
                    var _dateConfig = Mappers.Xy<DateModel>()
                    .X(m => (double)m.DateTime.Ticks / TimeSpan.FromHours(1).Ticks)
                    .Y(m => m.Value);
                    DevicePowerSeriesCollection = new SeriesCollection(_dateConfig);
                }
                else
                {
                    DevicePowerSeriesCollection?.Clear();
                }
            }));
            var devicePowerInfoList = _monitoringSummaryService.GetDevicePowerInfo(deviceId, dateTime);
            if (devicePowerInfoList.Count == 0)
            {
                return;
            }
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                var lineSeries = new LineSeries();
                lineSeries.Title = "电量";
                lineSeries.Values = new ChartValues<DateModel>();

                foreach (var devicePowerItem in devicePowerInfoList)
                {
                    lineSeries.Values.Add(new DateModel { Value = devicePowerItem.value, DateTime = devicePowerItem.time });
                    DevicePowerSeriesCollection.Add(lineSeries);
                }
                DevicePowerXFormatter = value => new System.DateTime((long)(value * TimeSpan.FromHours(1).Ticks)).ToString("yyyy-MM-dd");
                DevicePowerYFormatter = value => $"{value} kWh";
            }));
        }

        public Task GetTopPowerSummaryInfoListTaskAsync(string s) => Task.Run(() => GetTopPowerSummaryInfoList(s));

        private void GetTopPowerSummaryInfoList(string customerId)
        {
            var topPowerSummaryList = _monitoringSummaryService.GetTopPowerSummary(customerId,
                    DateTime.Now);
            List<string> labels = new List<string>();
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
          {
              var topPowerSummarySeries = new RowSeries();
              topPowerSummarySeries.Title = "电量";
              topPowerSummarySeries.Values = new ChartValues<double>();
              foreach (var topPowerSummary in topPowerSummaryList)
              {
                  topPowerSummarySeries.Values.Add(double.Parse(topPowerSummary.value));
                  labels.Add(topPowerSummary.equipment);
              }

              TopPowerSummaryInfoSeriesCollection.Add(topPowerSummarySeries);
              TopPowerSummaryInfoLabels.AddRange(labels);
              TopPowerSummaryInfoFormatter = value => $"{value.ToString()} kWh";
              TopPowerSummaryInfoDeviceFormatter = value => $"{value} kWh";
          }));
        }


        public Task<PowerSummaryDTO> GetPowerSummaryInfoTaskAsync(string s) => Task.Run(() => PowerSummaryInfo = GetPowerSummaryInfo(s));
        private PowerSummaryDTO GetPowerSummaryInfo(string customerId)
        {
            return _monitoringSummaryService.GetPowerSummary(customerId);
        }

        public Task<AlarmSummaryDTO> GetAlarmSummaryInfoTaskAsync(string s) => Task.Run(() => AlarmSummaryInfo = GetAlarmSummaryInfo(s));
        private AlarmSummaryDTO GetAlarmSummaryInfo(string customerId)
        {
            return _monitoringSummaryService.GetAlarmSummary(customerId);
        }

        public Task SetWiringDiagramUITaskAsync(string s) => Task.Run(() =>
        {
            var ui = GetWiringDiagramUI(s);
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                WiringDiagramUI = ui;
            }));
        });

        private FrameworkElement GetWiringDiagramUI(string customerId)
        {
            FrameworkElement wiringDiagramUI = null;

            var wiringDiagramConfig = _wiringDiagramService.GetWiringDiagramConfig(customerId);
            var mainDiagram = wiringDiagramConfig.FirstOrDefault(d => d.isMain == 1);
            if (mainDiagram != null)
            {
                Uri uri = new Uri(mainDiagram.filePath);
                var fileName = uri.Segments[uri.Segments.Length - 1];
                var dataBuffer = GetWiringDiagram(uri);
                XamlUI xamlUI = null;
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    xamlUI = _uiManager.Load(dataBuffer, fileName);
                    if (xamlUI != null)
                    {
                        var viewBox = new Viewbox();
                        viewBox.Stretch = System.Windows.Media.Stretch.Fill;
                        if (xamlUI.UI.Parent != null)
                        {
                            (xamlUI.UI.Parent as Viewbox).Child = null;
                        }
                        viewBox.Child = xamlUI.UI;
                        wiringDiagramUI = viewBox;
                    }

                }));

            }
            return wiringDiagramUI;
        }

        private byte[] GetWiringDiagram(Uri diagramUri)
        {
            var httpClient = new RestClient(diagramUri);
            var request = new RestRequest();
            var response = httpClient.Execute(request);
            if (response.ErrorMessage != null)
            {
            }
            return Encoding.UTF8.GetBytes(response.Content);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var customer = navigationContext.Parameters["Customer"] as Customer;
            if (customer != null)
                return CurrentCustomer != null && CurrentCustomer.Id == customer.Id;
            else
                return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }


    }
    public class DateModel
    {
        public DateTime DateTime { get; set; }
        public double Value { get; set; }
    }

    class EquipmentIdComparer : IEqualityComparer<EquipmentDTO>
    {
        public bool Equals(EquipmentDTO x, EquipmentDTO y)
        {
            if (x == null)
                return y == null;
            return x.equipmentId == y.equipmentId;
        }


        public int GetHashCode(EquipmentDTO obj)
        {
            if (obj == null)
                return 0;
            return obj.equipmentId.GetHashCode(); ;
        }
    }
}
