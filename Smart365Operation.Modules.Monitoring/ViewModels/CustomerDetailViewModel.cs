using System;
using System.Collections.Generic;
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

namespace Smart365Operation.Modules.Monitoring.ViewModels
{
    public class CustomerDetailViewModel:BindableBase, INavigationAware
    {
        private readonly IWiringDiagramService _wiringDiagramService;
        private readonly IMonitoringDataService _monitoringDataService;

        public CustomerDetailViewModel(IWiringDiagramService wiringDiagramService, IMonitoringDataService monitoringDataService)
        {
            _wiringDiagramService = wiringDiagramService;
            _monitoringDataService = monitoringDataService;
            _monitoringDataService.DataUpdated += _monitoringDataService_DataUpdated;
            _uiManager = UIManager.Instance;
            _uiManager.Dispatcher = Application.Current.Dispatcher;
            _uiManager.EnableSafeMode = true;
        }

        private void _monitoringDataService_DataUpdated(object sender, MonitoringDataEventArgs e)
        {
            _uiManager.UpdateData(e.Key, e.Value);
        }

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


        private FrameworkElement _wiringDiagramUI;
        private UIManager _uiManager;

        public FrameworkElement WiringDiagramUI
        {
            get { return _wiringDiagramUI; }
            set { SetProperty(ref _wiringDiagramUI, value); }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var customer = navigationContext.Parameters["Customer"] as Customer;
            if (customer != null)
            {
                CurrentCustomer = customer;
                var wiringDiagramConfig = _wiringDiagramService.GetWiringDiagramConfig(customer.Id.ToString());
                var mainDiagram = wiringDiagramConfig.FirstOrDefault(d => d.isMain == 1);
                if (mainDiagram != null)
                {
                    Uri uri = new Uri(mainDiagram.filePath);
                    var fileName = uri.Segments[uri.Segments.Length - 1];
                    var dataBuffer = GetWiringDiagram(uri);
                    var xamlUI = _uiManager.Load(dataBuffer, fileName);
                    WiringDiagramUI = xamlUI.UI;
                }
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
    }
}
