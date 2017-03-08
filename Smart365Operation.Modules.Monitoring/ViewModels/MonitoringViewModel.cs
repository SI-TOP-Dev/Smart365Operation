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
            InitializeDataTaskAsync();
        }


        private Task InitializeDataTaskAsync() => Task.Run(() => InitializeData());
        private void InitializeData()
        {
            var principal = Thread.CurrentPrincipal as SystemPrincipal;
            var agentId = principal.Identity.Id;
            var customers = _customerService.GetCustomersBy(agentId);
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                CustomerList.AddRange(customers);
                if (SelectedCustomer != null)
                {
                    SelectedCustomer = CustomerList.FirstOrDefault(c => c.Id == SelectedCustomer.Id);
                }
                else
                {
                    SelectedCustomer = CustomerList.Count == 0 ? null : CustomerList[0];
                }
            }));
        }


        private bool CanInitialize()
        {
            return true;
        }


        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var parameter = navigationContext.Parameters.FirstOrDefault(p => p.Key == "CustomerId");
            if (parameter.Value != null)
            {
                SelectedCustomer = new Customer() { Id = int.Parse(parameter.Value.ToString()) };
            }
            else
            {
                parameter = navigationContext.Parameters.FirstOrDefault(p => p.Key == "Customer");
                if (parameter.Value != null)
                {
                    SelectedCustomer = parameter.Value as Customer;
                }
            }

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
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
