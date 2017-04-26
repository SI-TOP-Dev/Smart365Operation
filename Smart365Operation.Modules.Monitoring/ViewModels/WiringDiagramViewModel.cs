using Com.Shengzuo.RuntimeCore;
using Com.Shengzuo.RuntimeCore.Common;
using Prism.Mvvm;
using Prism.Regions;
using RestSharp;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Smart365Operation.Modules.Monitoring.ViewModels
{
    public  class WiringDiagramViewModel:BindableBase, INavigationAware
    {
        private readonly IWiringDiagramService _wiringDiagramService;
        private readonly IMonitoringDataService _monitoringDataService;
        private UIManager _uiManager;

        public WiringDiagramViewModel(IWiringDiagramService wiringDiagramService, IMonitoringDataService monitoringDataService)
        {
            _wiringDiagramService = wiringDiagramService;
            _monitoringDataService = monitoringDataService;

            _uiManager = UIManager.Instance;
            _uiManager.Dispatcher = Application.Current.Dispatcher;
            _uiManager.EnableSafeMode = true;

            _monitoringDataService.MonitoringDataUpdated += _monitoringDataService_DataUpdated;
        }

        private FrameworkElement _wiringDiagramUI;
        public FrameworkElement WiringDiagramUI
        {
            get { return _wiringDiagramUI; }
            set { SetProperty(ref _wiringDiagramUI, value); }
        }

        private void _monitoringDataService_DataUpdated(object sender, MonitoringDataEventArgs e)
        {

            var action = new Action(() =>
            {
                _uiManager.UpdateData(e.Key, e.Value);
            });
            action.BeginInvoke(null, null);
        }

        public Task SetWiringDiagramUITaskAsync(string s) => Task.Run(() =>
        {
            var ui = GetWiringDiagramUI(s);
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                WiringDiagramUI = ui;
            }));
        });

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
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    xamlUI = _uiManager.Load(dataBuffer, fileName);
                    if (xamlUI != null)
                    {
                        var viewBox = new Viewbox();
                        viewBox.Stretch = System.Windows.Media.Stretch.Fill;
                        if (xamlUI.UI.Parent != null)
                        {
                            (xamlUI.UI.Parent as Viewbox).Child = null;
                        }
                        viewBox.Child = xamlUI.UI;
                        wiringDiagramUI = viewBox;
                    }

                }));

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

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var customer = navigationContext.Parameters["Customer"] as Customer;
            if (customer != null)
            {
               SetWiringDiagramUITaskAsync(customer.Id.ToString());
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }
    }
}
