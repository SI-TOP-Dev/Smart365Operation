using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Regions;
using Smart365Operations.Common.Infrastructure.Models;

namespace Smart365Operation.Modules.Monitoring.ViewModels
{
    public class CustomerDetailViewModel:BindableBase, INavigationAware
    {


        private string _companyName;
        public string CompanyName
        {
            get { return _companyName; }
            set { SetProperty(ref _companyName, value); }
        }

        private string _companyProfile;
        public string CompanyProfile
        {
            get { return _companyProfile; }
            set { SetProperty(ref _companyProfile, value); }
        }

        private string _contacts;
        public string Contacts
        {
            get { return _contacts; }
            set { SetProperty(ref _contacts, value); }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }

        private string _industryType;
        public string IndustryType
        {
            get { return _industryType; }
            set { SetProperty(ref _industryType, value); }
        }

        private Customer _currentCustomer;
        public Customer CurrentCustomer
        {
            get { return _currentCustomer; }
            set { SetProperty(ref _currentCustomer, value); }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var customer = navigationContext.Parameters["Customer"] as Customer;
            if (customer != null)
                CurrentCustomer = customer;
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
}
