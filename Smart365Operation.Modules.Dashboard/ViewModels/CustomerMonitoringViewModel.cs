using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Smart365Operations.Common.Infrastructure;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Models;
using Newtonsoft.Json;

namespace Smart365Operation.Modules.Dashboard
{
    public class CustomerMonitoringViewModel : BindableBase
    {
        private readonly IShellService _shellService;
        private readonly IMonitoringDataService _monitoringDataService;
        private readonly IRegionManager _regionManager;
        private readonly Customer _customer;

        public CustomerMonitoringViewModel(IShellService shellService, IRegionManager regionManager, IMonitoringDataService monitoringDataService, Customer customer)
        {
            _shellService = shellService;
            _regionManager = regionManager;
            _monitoringDataService = monitoringDataService;
            _monitoringDataService.AlarmDataUpdated += _monitoringDataService_AlarmDataUpdated;
            _customer = customer;
            _customerId = customer.Id.ToString();
            _customerName = customer.Name;
            _city = customer.City;
            _agency = customer.Agency;
            _latitude = string.IsNullOrEmpty(customer.Latitude) ? 0d : double.Parse(customer.Latitude);
            _longitude = string.IsNullOrEmpty(customer.Longitude) ? 0d : double.Parse(customer.Longitude);
        }

        private void _monitoringDataService_AlarmDataUpdated(object sender, AlarmDataEventArgs e)
        {
            var alarmStr = e.Data as string;
            if (!string.IsNullOrEmpty(alarmStr))
            {
                var alarmInfo = JsonConvert.DeserializeObject<AlarmInfo>(alarmStr);
                if (alarmInfo != null)
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (!string.IsNullOrEmpty(CustomerId) && CustomerId == alarmInfo.CustomerId.ToString())
                        {
                            HasAlarm = true;
                            AlarmLevel = alarmInfo.Level;
                        }
                    }));
                }
            }
        }

        private string _customerId;
        public string CustomerId
        {
            get { return _customerId; }
            set { SetProperty(ref _customerId, value); }
        }

        private string _customerName;
        public string CustomerName
        {
            get { return _customerName; }
            set { SetProperty(ref _customerName, value); }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set { SetProperty(ref _city, value); }
        }

        private string _agency;
        public string Agency
        {
            get { return _agency; }
            set { SetProperty(ref _agency, value); }
        }

        private bool _hasAlarm = false;
        public bool HasAlarm
        {
            get { return _hasAlarm; }
            set { SetProperty(ref _hasAlarm, value); }
        }
        private int _alarmLevel = 0;
        public int AlarmLevel
        {
            get { return _alarmLevel; }
            set { SetProperty(ref _alarmLevel, value); }
        }

        private double _latitude;
        public double Latitude
        {
            get { return _latitude; }
            set { SetProperty(ref _latitude, value); }
        }

        private double _longitude;


        public double Longitude
        {
            get { return _longitude; }
            set { SetProperty(ref _longitude, value); }
        }

        public DelegateCommand CustomerSelectedCommand => new DelegateCommand(CustomerSelected, CanCustomerSelected);
        public DelegateCommand<object> SelectCustomerCommand => new DelegateCommand<object>(SelectCustomer);

        private void SelectCustomer(object obj)
        {
            CustomerMonitoringViewModel customerViewModel = obj as CustomerMonitoringViewModel;
            if (customerViewModel != null)
            {
                var parameters = new NavigationParameters();
                parameters.Add("Customer", customerViewModel._customer);
                _shellService.ShowShell("Monitoring", parameters);
            }
        }
        private bool CanCustomerSelected()
        {
            return true;
        }

        private void CustomerSelected()
        {
            var parameters = new NavigationParameters();
            parameters.Add("Customer", _customer);
            _shellService.ShowShell("Monitoring", parameters);
            // _regionManager.RequestNavigate(KnownRegionNames.MainRegion, "Monitoring", parameters);
        }
    }
}
