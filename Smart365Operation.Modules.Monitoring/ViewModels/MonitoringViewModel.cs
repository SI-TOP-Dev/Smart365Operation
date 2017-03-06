using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Com.Shengzuo.RuntimeCore;
using Prism.Commands;
using Prism.Mvvm;
using Smart365Operations.Common.Infrastructure.Interfaces;
using System.Windows;
using Prism.Regions;
using RestSharp;
using Smart365Operations.Common.Infrastructure.Models;
using Smart365Operations.Common.Infrastructure.Models.TO;
using Smart365Operations.Common.Infrastructure.Prism;

namespace Smart365Operation.Modules.Monitoring
{
    public class MonitoringViewModel : BindableBase, INavigationAware, IRegionManagerAware
    {
        private readonly ICustomerService _customerService;
        private readonly IWiringDiagramService _wiringDiagramService;
        //private readonly IMonitoringDataService _monitoringDataService;
        //private UIManager _uiManager;

        public MonitoringViewModel(ICustomerService customerService, IWiringDiagramService wiringDiagramService)
        {
            _customerService = customerService;
            _wiringDiagramService = wiringDiagramService;
            //_monitoringDataService = monitoringDataService;
            //_monitoringDataService.DataUpdated += _monitoringDataService_DataUpdated;
        }


        private ObservableCollection<Customer> _customerList = new ObservableCollection<Customer>();
        public ObservableCollection<Customer> CustomerList
        {
            get { return _customerList; }
            set { SetProperty(ref _customerList, value); }
        }

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                SetProperty(ref _selectedCustomer, value);
                NavigateTo("CustomerDetailRegion", "CustomerDetail", _selectedCustomer);
            }
        }


        private FrameworkElement _wiringDiagramUI;
        public FrameworkElement WiringDiagramUI
        {
            get { return _wiringDiagramUI; }
            set { SetProperty(ref _wiringDiagramUI, value); }
        }

        public DelegateCommand InitializeCommand => new DelegateCommand(Initialize, CanInitialize);

        private void Initialize()
        {

            var principal = Thread.CurrentPrincipal as SystemPrincipal;
            var agentId = principal.Identity.Id;
            var customers = _customerService.GetCustomersBy(agentId);
            CustomerList.AddRange(customers);


            //_uiManager = UIManager.Instance;
            //_uiManager.Dispatcher = Application.Current.Dispatcher;
            //_uiManager.EnableSafeMode = true;

        }



        private bool CanInitialize()
        {
            return true;
        }

        //private void _monitoringDataService_DataUpdated(object sender, MonitoringDataEventArgs e)
        //{
        //    _uiManager.UpdateData(e.Key, e.Value);
        //}

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var customer = navigationContext.Parameters["Customer"] as Customer;
            if (customer != null)
            {
                SelectedCustomer = customer;
            }

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public IRegionManager RegionManager { get; set; }

        private void NavigateTo(string regionName, string viewName, object value)
        {
            var parameters = new NavigationParameters();
            parameters.Add("Customer", value);
            RegionManager.RequestNavigate("CustomerDetailRegion", "CustomerDetail", parameters);
        }
    }
}
