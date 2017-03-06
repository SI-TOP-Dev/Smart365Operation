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
    public class MonitoringViewModel : BindableBase, INavigationAware,IRegionManagerAware
    {
        private readonly ICustomerService _customerService;
        private readonly IWiringDiagramService _wiringDiagramService;
        private readonly IMonitoringDataService _monitoringDataService;
        private UIManager _uiManager;

        public MonitoringViewModel(ICustomerService customerService, IWiringDiagramService wiringDiagramService, IMonitoringDataService monitoringDataService)
        {
            _customerService = customerService;
            _wiringDiagramService = wiringDiagramService;
            _monitoringDataService = monitoringDataService;
            _monitoringDataService.DataUpdated += _monitoringDataService_DataUpdated;

           
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
                if (value != _selectedCustomer)
                {
                    var parameters = new NavigationParameters();
                    parameters.Add("Customer", value);
                    RegionManager.RequestNavigate("CustomerDetailRegion", "CustomerDetail", parameters);
                }
                SetProperty(ref _selectedCustomer, value);
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
            //var wiringDiagramConfig = _wiringDiagramService.GetWiringDiagramConfig("11");
            //var mainDiagram = wiringDiagramConfig.FirstOrDefault(d => d.isMain == 1);
            //Uri uri = new Uri(mainDiagram.filePath);
            //var fileName = uri.Segments[uri.Segments.Length - 1];
            //var dataBuffer = GetWiringDiagram(uri);
            //var xamlUI = _uiManager.Load(dataBuffer, fileName);
            //WiringDiagramUI = xamlUI.UI;
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

        private bool CanInitialize()
        {
            return true;
        }

        private void _monitoringDataService_DataUpdated(object sender, MonitoringDataEventArgs e)
        {
            _uiManager.UpdateData(e.Key, e.Value);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var customer = navigationContext.Parameters["Customer"] as Customer;

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
    }
}
