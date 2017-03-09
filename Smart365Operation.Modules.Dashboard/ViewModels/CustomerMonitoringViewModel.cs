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

namespace Smart365Operation.Modules.Dashboard
{
    public class CustomerMonitoringViewModel : BindableBase
    {
        private readonly IShellService _shellService;
        private readonly IRegionManager _regionManager;
        private readonly Customer _customer;

        public CustomerMonitoringViewModel(IShellService shellService, IRegionManager regionManager, Customer customer)
        {
            _shellService = shellService;
            _regionManager = regionManager;
            _customer = customer;
            _customerId = customer.Id.ToString();
            _customerName = customer.Name;
            _latitude = string.IsNullOrEmpty(customer.Latitude) ? 0d : double.Parse(customer.Latitude);
            _longitude = string.IsNullOrEmpty(customer.Longitude) ? 0d : double.Parse(customer.Longitude);
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
