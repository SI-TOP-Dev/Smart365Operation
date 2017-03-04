using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Smart365Operations.Common.Infrastructure.Models;

namespace Smart365Operation.Modules.Dashboard
{
    public class CustomerMonitoringViewModel : BindableBase
    {
        public CustomerMonitoringViewModel(Customer customer)
        {
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
    }
}
