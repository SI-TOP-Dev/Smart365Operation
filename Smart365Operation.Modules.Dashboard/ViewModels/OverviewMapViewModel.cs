using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Feeling.GIS.Map;
using Feeling.GIS.Map.Core;
using Prism.Mvvm;
using Smart365Operation.Modules.Dashboard.Interfaces;
using Prism.Commands;
using Prism.Regions;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Models;
using Smart365Operations.Common.Infrastructure.Prism;
using System.Windows.Data;

namespace Smart365Operation.Modules.Dashboard
{
    public class OverviewMapViewModel : BindableBase, IRegionManagerAware
    {
        private readonly IShellService _shellService;
        private readonly IDataStatisticsService _dataStatisticsService;
        private readonly ICustomerService _customerService;
        private readonly IMonitoringDataService _monitoringDataService;

        public OverviewMapViewModel(IShellService shellService, IDataStatisticsService dataStatisticsService, ICustomerService customerService, IMonitoringDataService monitoringDataService)
        {
            _shellService = shellService;
            _dataStatisticsService = dataStatisticsService;
            _customerService = customerService;
            _monitoringDataService = monitoringDataService;
        }

        private DataStatisticsViewModel _statisticsViewModel;// = new DataStatisticsViewModel();
        public DataStatisticsViewModel StatisticsViewModel
        {
            get { return _statisticsViewModel; }
            set { SetProperty(ref _statisticsViewModel, value); }
        }

        private ObservableCollection<CustomerMonitoringViewModel> _customerMonitoringList = new ObservableCollection<CustomerMonitoringViewModel>();
        public ObservableCollection<CustomerMonitoringViewModel> CustomerMonitoringList
        {
            get { return _customerMonitoringList; }
            set { SetProperty(ref _customerMonitoringList, value); }
        }

        private CustomerMonitoringViewModel _selectedCustomer;
        public CustomerMonitoringViewModel SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                //if (value != null)
                //{
                //    value.CustomerSelectedCommand.Execute();
                //}
                SetProperty(ref _selectedCustomer, value);
            }
        }


        private bool _isInitialShow = false;
        public bool IsInitialShow
        {
            get { return _isInitialShow; }
            set
            {
                SetProperty(ref _isInitialShow, value);
            }
        }

        public DelegateCommand<object> InitializeCommand => new DelegateCommand<object>(Initialize);
        public DelegateCommand ExpandedCommand => new DelegateCommand(OnExpand);
        public DelegateCommand MouseDownCommand => new DelegateCommand(OnMouseDown);
        public DelegateCommand RegionGroupingCommand => new DelegateCommand(RegionGrouping);
        public DelegateCommand AgencyGroupingCommand => new DelegateCommand(AgencyGrouping);

        private void AgencyGrouping()
        {
            CustomersView.GroupDescriptions.Clear();
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Agency");
            if (!CustomersView.GroupDescriptions.Contains(groupDescription))
            {
                CustomersView.GroupDescriptions.Add(groupDescription);
            }
        }

        private void RegionGrouping()
        {
            CustomersView.GroupDescriptions.Clear();
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("City");
            if (!CustomersView.GroupDescriptions.Contains(groupDescription))
            {
                CustomersView.GroupDescriptions.Add(groupDescription);
            }
        }

        private void OnMouseDown()
        {
            IsInitialShow = true;
        }

        private void OnExpand()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (StatisticsViewModel == null)
                {
                    StatisticsViewModel = new DataStatisticsViewModel(_dataStatisticsService);
                }
            }));
        }

        private void Initialize(object obj)
        {

            InitializeDataTaskAsync(obj);
            RegionManager.RequestNavigate("AlarmRegion", "AlarmTips");
        }

        private Task InitializeDataTaskAsync(object obj) => Task.Run(() => InitializeData(obj));

        private void InitializeData(object obj)
        {
            var map = obj as MapControl;
            if (map != null)
            {
                var principal = Thread.CurrentPrincipal as SystemPrincipal;
                var agentId = principal.Identity.Id;
                var customerList = _customerService.GetCustomersBy(agentId);
                var customerMonitoringList = new List<CustomerMonitoringViewModel>();
                var customerMapMarkerList = new List<MapMarker>();
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
               {
                   foreach (var customer in customerList)
                   {
                       var customerViewModel = new CustomerMonitoringViewModel(_shellService, RegionManager, _monitoringDataService, customer);
                       var mapMarker = new MapMarker(new PointLatLng(customerViewModel.Latitude, customerViewModel.Longitude));
                       mapMarker.Shape = new CustomerMarker(customerViewModel);
                       customerMonitoringList.Add(customerViewModel);
                       customerMapMarkerList.Add(mapMarker);
                   }


                   CustomerMonitoringList.AddRange(customerMonitoringList);
                   map.Markers.AddRange(customerMapMarkerList);

                   CustomersView = GetCustomerCollectionView(CustomerMonitoringList);
                   CustomersView.Filter = OnFilterCustomer;
               }));

            }

            var action = new Action(() =>
            {
                Thread.Sleep(1000);
                IsInitialShow = true;
            });
            action.BeginInvoke(null, null);
            //Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    IsInitialShow = false;
            //}));

        }

        private bool OnFilterCustomer(object item)
        {
            var customer = (CustomerMonitoringViewModel)item;
            if (string.IsNullOrEmpty(CustomerKeyword))
            {
                return true;
            }
            else
            {
                return customer.CustomerName.Contains(CustomerKeyword);
            }
        }

        public CollectionView GetCustomerCollectionView(ObservableCollection<CustomerMonitoringViewModel> customerList)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(customerList);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Agency");
            view.GroupDescriptions.Add(groupDescription);
            return view;
        }


        private CollectionView _customersView;
        public CollectionView CustomersView
        {
            get { return _customersView; }
            set
            {
                SetProperty(ref _customersView, value);
                _customersView.Refresh();
            }
        }


        private string _customerKeyword = string.Empty;
        public string CustomerKeyword
        {
            get { return _customerKeyword; }
            set
            {
                SetProperty(ref _customerKeyword, value);
                _customersView.Refresh();
            }
        }

        public IRegionManager RegionManager { get; set; }
    }
}
