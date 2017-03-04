﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Feeling.GIS.Map;
using Feeling.GIS.Map.Core;
using Prism.Mvvm;
using Smart365Operation.Modules.Dashboard.Interfaces;
using Prism.Commands;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Models;

namespace Smart365Operation.Modules.Dashboard
{
    public class OverviewMapViewModel : BindableBase
    {
        private readonly IDataStatisticsService _dataStatisticsService;
        private readonly ICustomerService _customerService;
        private readonly IMonitoringDataService _monitoringDataService;

        public OverviewMapViewModel(IDataStatisticsService dataStatisticsService, ICustomerService customerService, IMonitoringDataService monitoringDataService)
        {
            _dataStatisticsService = dataStatisticsService;
            _customerService = customerService;
            _monitoringDataService = monitoringDataService;
        }

        private DataStatisticsViewModel _statisticsViewModel;
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

        public DelegateCommand<object> InitializeCommand => new DelegateCommand<object>(Initialize);

        private void Initialize(object obj)
        {
            var map = obj as MapControl;
            if (map != null)
            {
                var principal = Thread.CurrentPrincipal as SystemPrincipal;
                var agentId = principal.Identity.Id;
                var customerList = _customerService.GetCustomersBy(agentId);
                var customerMonitoringList = new List<CustomerMonitoringViewModel>();
                var customerMapMarkerList = new List<MapMarker>();
                foreach (var customer in customerList)
                {
                    var customerViewModel = new CustomerMonitoringViewModel(customer);
                    var mapMarker = new MapMarker(new PointLatLng(customerViewModel.Latitude, customerViewModel.Longitude));
                    mapMarker.Shape = new CustomerMarker(customerViewModel);
                    customerMonitoringList.Add(customerViewModel);
                    customerMapMarkerList.Add(mapMarker);
                    //map.Markers.Add(mapMarker);
                }

                //System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                //{
                CustomerMonitoringList.AddRange(customerMonitoringList);
                map.Markers.AddRange(customerMapMarkerList);
                //}));
            }
            StatisticsViewModel = new DataStatisticsViewModel(_dataStatisticsService);

        }
    }
}
