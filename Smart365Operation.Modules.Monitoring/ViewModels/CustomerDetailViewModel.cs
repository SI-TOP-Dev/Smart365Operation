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
using Com.Shengzuo.RuntimeCore.Common;
using Smart365Operations.Common.Infrastructure.Models.TO;

namespace Smart365Operation.Modules.Monitoring.ViewModels
{
    public class CustomerDetailViewModel:BindableBase, INavigationAware
    {
        private readonly IWiringDiagramService _wiringDiagramService;
        private readonly IMonitoringDataService _monitoringDataService;
        private readonly IMonitoringSummaryService _monitoringSummaryService;
        private UIManager _uiManager;

        public CustomerDetailViewModel(IWiringDiagramService wiringDiagramService, IMonitoringDataService monitoringDataService,IMonitoringSummaryService monitoringSummaryService)
        {
            _wiringDiagramService = wiringDiagramService;
            _monitoringDataService = monitoringDataService;
            _monitoringSummaryService = monitoringSummaryService;
            _monitoringDataService.DataUpdated += _monitoringDataService_DataUpdated;
            _uiManager = UIManager.Instance;
            _uiManager.Dispatcher = Application.Current.Dispatcher;
            _uiManager.EnableSafeMode = true;
        }

        private void _monitoringDataService_DataUpdated(object sender, MonitoringDataEventArgs e)
        {
            _uiManager.UpdateData(e.Key, e.Value);
        }


        private Customer _currentCustomer;
        public Customer CurrentCustomer
        {
            get { return _currentCustomer; }
            set { SetProperty(ref _currentCustomer, value); }
        }


        private FrameworkElement _wiringDiagramUI;
        public FrameworkElement WiringDiagramUI
        {
            get { return _wiringDiagramUI; }
            set { SetProperty(ref _wiringDiagramUI, value); }
        }

        private AlarmSummaryDTO _alarmSummaryInfo;
        public AlarmSummaryDTO AlarmSummaryInfo
        {
            get { return _alarmSummaryInfo; }
            set { SetProperty(ref _alarmSummaryInfo, value); }
        }

        private PowerSummaryDTO _powerSummaryInfo;
        public PowerSummaryDTO PowerSummaryInfo
        {
            get { return _powerSummaryInfo; }
            set { SetProperty(ref _powerSummaryInfo, value); }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var customer = navigationContext.Parameters["Customer"] as Customer;
            if (customer != null)
            {
                CurrentCustomer = customer;
                var customerId = CurrentCustomer.Id.ToString();
                SetWiringDiagramUITaskAsync(customerId);
                AlarmSummaryInfo = GetAlarmSummaryInfoTaskAsync(customerId).Result;
                PowerSummaryInfo = GetPowerSummaryInfoTaskAsync(customerId).Result;
            }
        }

        public Task<PowerSummaryDTO> GetPowerSummaryInfoTaskAsync(string s) => Task.Run(() => GetPowerSummaryInfo(s));
        private PowerSummaryDTO GetPowerSummaryInfo(string customerId)
        {
            return _monitoringSummaryService.GetPowerSummary(customerId);
        }

        public Task<AlarmSummaryDTO> GetAlarmSummaryInfoTaskAsync(string s) => Task.Run(() => GetAlarmSummaryInfo(s));
        private AlarmSummaryDTO GetAlarmSummaryInfo(string customerId)
        {
            return _monitoringSummaryService.GetAlarmSummary(customerId);
        }
        
        public Task<FrameworkElement> SetWiringDiagramUITaskAsync(string s) => Task.Run(() => WiringDiagramUI = GetWiringDiagramUI(s));
        private FrameworkElement GetWiringDiagramUI(string customerId)
        {
            FrameworkElement wiringDiagramUI = null;
            var wiringDiagramConfig = _wiringDiagramService.GetWiringDiagramConfig(customerId);
            var mainDiagram = wiringDiagramConfig.FirstOrDefault(d => d.isMain == 1);
            if (mainDiagram != null)
            {
                Uri uri = new Uri(mainDiagram.filePath);
                var fileName = uri.Segments[uri.Segments.Length - 1];
                var dataBuffer = GetWiringDiagram(uri);
                XamlUI xamlUI = null;
                _uiManager.Dispatcher.Invoke(new Action(() =>
                {
                    xamlUI = _uiManager.Load(dataBuffer, fileName);
                }));
                wiringDiagramUI = xamlUI.UI;
            }
            return wiringDiagramUI;
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
