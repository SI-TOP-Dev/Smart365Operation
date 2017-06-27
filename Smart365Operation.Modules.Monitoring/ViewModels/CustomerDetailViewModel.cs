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
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Visuals.RenderableSeries;

namespace Smart365Operation.Modules.Monitoring.ViewModels
{
    public class CustomerDetailViewModel : BindableBase, INavigationAware
    {

        private readonly IMonitoringSummaryService _monitoringSummaryService;
        private readonly ICustomerEquipmentService _customerEquipmentService;


        public CustomerDetailViewModel(ICustomerEquipmentService customerEquipmentService, IMonitoringSummaryService monitoringSummaryService)
        {

            _monitoringSummaryService = monitoringSummaryService;
            _customerEquipmentService = customerEquipmentService;
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

        private EquipmentDTO _selectedPowerDevice;
        public EquipmentDTO SelectedPowerDevice
        {
            get { return _selectedPowerDevice; }
            set
            {
                if (value != _selectedPowerDevice)
                {
                    GetDevicePowerInfoTaskAsync(value.equipmentId.ToString(), SelectedPowerDate);
                }
                SetProperty(ref _selectedPowerDevice, value);
            }
        }

        private EquipmentDTO _selectedPowerFactorDevice;
        public EquipmentDTO SelectedPowerFactorDevice
        {
            get { return _selectedPowerFactorDevice; }
            set
            {
                if (value != _selectedPowerFactorDevice)
                {
                    GetDevicePowerFactorTaskAsync(value.equipmentId.ToString(), SelectedPowerFactorDate);
                }
                SetProperty(ref _selectedPowerFactorDevice, value);
            }
        }

        private DateTime _selectedPowerDate = DateTime.Now;
        public DateTime SelectedPowerDate
        {
            get { return _selectedPowerDate; }
            set
            {
                if (value != _selectedPowerDate)
                {
                    GetDevicePowerInfoTaskAsync(SelectedPowerDevice.equipmentId.ToString(), value);
                }
                SetProperty(ref _selectedPowerDate, value);
            }
        }
        private DateTime _selectedPowerFactorDate = DateTime.Now;
        public DateTime SelectedPowerFactorDate
        {
            get { return _selectedPowerFactorDate; }
            set
            {
                if (value != _selectedPowerFactorDate)
                {
                    GetDevicePowerFactorTaskAsync(SelectedPowerFactorDevice.equipmentId.ToString(), value);
                }
                SetProperty(ref _selectedPowerFactorDate, value);
            }
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

        private List<string> _devicePowerXLabels = new List<string>();
        public List<string> DevicePowerXLabels
        {
            get { return _devicePowerXLabels; }
            set { SetProperty(ref _devicePowerXLabels, value); }
        }

        private Func<double, string> _devicePowerYFormatter;
        public Func<double, string> DevicePowerYFormatter
        {
            get { return _devicePowerYFormatter; }
            set { SetProperty(ref _devicePowerYFormatter, value); }
        }

        private double _transformerCapacity;
        public double TransformerCapacity
        {
            get { return _transformerCapacity; }
            set { SetProperty(ref _transformerCapacity, value); }
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

                    GetTransformerCapacityTaskAsync(customerId);
                    GetDefaultDevicePowerInfoTaskAsync(customerId);
                    GetAlarmSummaryInfoTaskAsync(customerId);
                    GetPowerSummaryInfoTaskAsync(customerId);
                    // GetTopPowerSummaryInfoListTaskAsync(customerId);
                }
                else
                {
                    if (CurrentCustomer.Id != customer.Id)
                    {
                        var customerId = CurrentCustomer.Id.ToString();
                        GetTransformerCapacityTaskAsync(customerId);
                        GetDefaultDevicePowerInfoTaskAsync(customerId);
                        GetAlarmSummaryInfoTaskAsync(customerId);
                        GetPowerSummaryInfoTaskAsync(customerId);
                        //GetTopPowerSummaryInfoListTaskAsync(customerId);
                    }
                }

            }
        }

        public Task GetTransformerCapacityTaskAsync(string customerId) => Task.Run(() =>
        {
            var capacity = _monitoringSummaryService.GetTransformerCapacity(customerId);
            if (capacity == null)
            {
                return;
            }
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                TransformerCapacity = capacity.value;
            }));
        });

        public Task GetDefaultDevicePowerInfoTaskAsync(string customerId) => Task.Run(() =>
        {
            GetDefaultDevicePowerInfo(customerId);
        });
        private void GetDefaultDevicePowerInfo(string customerId)
        {
            var deviceSummaryList = _customerEquipmentService.GetCustomerEquipmentTable(customerId);
            if (deviceSummaryList == null)
            {
                return;
            }
            List<EquipmentDTO> deviceList = new List<EquipmentDTO>();
            deviceSummaryList.areaList.ForEach(a => a.switchingRoomList.ForEach(r =>
            {
                var result = r.equipmentList.Where(e => e.equipmentType != 0);
                deviceList.AddRange(result);
            }));
            deviceList = deviceList.Distinct(new EquipmentIdComparer()).ToList();
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                DeviceList.Clear();
                DeviceList = new ObservableCollection<EquipmentDTO>(deviceList);
                SelectedPowerDevice = DeviceList.Count != 0 ? DeviceList[0] : null;
                SelectedPowerFactorDevice = DeviceList.Count != 0 ? DeviceList[0] : null;
            }));
            if (SelectedPowerDevice != null)
            {
                GetDevicePowerInfoTaskAsync(SelectedPowerDevice.equipmentId.ToString(), DateTime.Now);
            }
            if (SelectedPowerFactorDevice != null)
            {
                GetDevicePowerFactorTaskAsync(SelectedPowerFactorDevice.equipmentId.ToString(), DateTime.Now);
            }
        }

        public Task GetDevicePowerFactorTaskAsync(string deviceId, DateTime dateTime) => Task.Run(() =>
        {
            GetDevicePowerFactor(deviceId, dateTime);
        });

        private void GetDevicePowerFactor(string deviceId, DateTime dateTime)
        {
            var devicePowerFactorList = _monitoringSummaryService.GetPowerFactorInfo(deviceId, dateTime);
            if (devicePowerFactorList == null || devicePowerFactorList.Count == 0)
            {
                return;
            }
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                DevicePowerFactorSeries.Clear();

                var ds0 = new XyDataSeries<DateTime, double>();

                DevicePowerFactorSeries.Add(new ChartSeriesViewModel(ds0, new FastLineRenderableSeries() { StrokeThickness = 2 }));
                List<PowerFactorDTO> data = devicePowerFactorList.OrderBy(d => d.time).ToList();
                ds0.Append(data.Select(x => x.time), data.Select(y => double.Parse(y.value)));
                PowerFactorLowerThreshold = double.Parse(devicePowerFactorList[0].downLimit);
            }));
        }

        public Task GetDevicePowerInfoTaskAsync(string deviceId, DateTime dateTime) => Task.Run(() =>
        {
            GetDevicePowerInfo(deviceId, dateTime);
        });

        private void GetDevicePowerInfo(string deviceId, DateTime dateTime)
        {
            var devicePowerInfoList = _monitoringSummaryService.GetDevicePowerInfo(deviceId, dateTime);
            if (devicePowerInfoList == null || devicePowerInfoList.Count == 0)
            {
                return;
            }
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                DevicePowerSeries.Clear();

                var ds0 = new XyDataSeries<DateTime, double>();

                DevicePowerSeries.Add(new ChartSeriesViewModel(ds0, new FastLineRenderableSeries() { StrokeThickness = 2 }));
                List<DevicePowerInfoDTO> data = devicePowerInfoList.OrderBy(d => d.time).ToList();
                ds0.Append(data.Select(x => x.time), data.Select(y => y.value));
               
            }));
        }

        public Task GetTopPowerSummaryInfoListTaskAsync(string s) => Task.Run(() => GetTopPowerSummaryInfoList(s));

        private void GetTopPowerSummaryInfoList(string customerId)
        {
            var topPowerSummaryList = _monitoringSummaryService.GetTopPowerSummary(customerId,
                    DateTime.Now);
            if (topPowerSummaryList == null)
            {
                return;
            }
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
              /*  TopPowerSummaryInfoDeviceFormatter = value => $"{value} kWh"*/
              ;
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


        private ObservableCollection<IChartSeriesViewModel> _devicePowerSeries = new ObservableCollection<IChartSeriesViewModel>();
        public ObservableCollection<IChartSeriesViewModel> DevicePowerSeries
        {
            get { return _devicePowerSeries; }
            set
            {
                SetProperty(ref _devicePowerSeries, value);
            }
        }

        private ObservableCollection<IChartSeriesViewModel> _devicePowerFactorSeries = new ObservableCollection<IChartSeriesViewModel>();
        public ObservableCollection<IChartSeriesViewModel> DevicePowerFactorSeries
        {
            get { return _devicePowerFactorSeries; }
            set
            {
                SetProperty(ref _devicePowerFactorSeries, value);
            }
        }

         private double _powerFactorLowerThreshold = 0.0;
        public double PowerFactorLowerThreshold
        {
            get { return _powerFactorLowerThreshold; }
            set
            {
                SetProperty(ref _powerFactorLowerThreshold, value);
            }
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
