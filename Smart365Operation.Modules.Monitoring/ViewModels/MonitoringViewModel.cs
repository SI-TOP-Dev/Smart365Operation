using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Shengzuo.RuntimeCore;
using Prism.Commands;
using Prism.Mvvm;
using Smart365Operations.Common.Infrastructure.Interfaces;
using System.Windows;
using RestSharp;
using Smart365Operations.Common.Infrastructure.Models.TO;

namespace Smart365Operation.Modules.Monitoring.ViewModels
{
    public class MonitoringViewModel : BindableBase
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



        private FrameworkElement _wiringDiagramUI;
        public FrameworkElement WiringDiagramUI
        {
            get { return _wiringDiagramUI; }
            set { SetProperty(ref _wiringDiagramUI, value); }
        }

        public DelegateCommand InitializeCommand => new DelegateCommand(Initialize, CanInitialize);

        private void Initialize()
        {
            _uiManager = UIManager.Instance;
            _uiManager.Dispatcher = Application.Current.Dispatcher;
            _uiManager.EnableSafeMode = true;
            var wiringDiagramConfig = _wiringDiagramService.GetWiringDiagramConfig("11");
            var mainDiagram = wiringDiagramConfig.FirstOrDefault(d => d.isMain == 1);
            Uri uri = new Uri(mainDiagram.filePath);
            var fileName = uri.Segments[uri.Segments.Length - 1];
            var dataBuffer = GetWiringDiagram(uri);
            var xamlUI = _uiManager.Load(dataBuffer, fileName);
            WiringDiagramUI = xamlUI.UI;
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
    }
}
