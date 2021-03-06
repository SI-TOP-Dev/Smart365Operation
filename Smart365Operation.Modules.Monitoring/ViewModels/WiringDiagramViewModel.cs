﻿using Com.Shengzuo.RuntimeCore;
using Com.Shengzuo.RuntimeCore.Common;
using Microsoft.Practices.Unity;
using Polly;
using Prism.Logging;
using Prism.Mvvm;
using Prism.Regions;
using RestSharp;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Smart365Operation.Modules.Monitoring.ViewModels
{
    public class WiringDiagramViewModel : BindableBase, INavigationAware
    {
        private readonly string WIRING_DIAGRAM_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Smart365", "WiringDiagram");
        private readonly IWiringDiagramService _wiringDiagramService;
        private readonly IMonitoringDataService _monitoringDataService;
        private UIManager _uiManager;
        private List<string> _keys = new List<string>();

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

            if (!_keys.Contains(e.Key))
            {
                return;
            }
            try
            {
                _uiManager.UpdateData(e.Key, e.Value);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task SetWiringDiagramUITaskAsync(string s) //=> Task.Run(() =>
        {
            var ui = await GetWiringDiagramUI(s);
            SubscriberToCurrentData();
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                WiringDiagramUI = ui;
            }));
        }

        private void SubscriberToCurrentData()
        {
            var mainKeys = _keys.Select(k => k.Split(new Char[] { '_' })[0]).Distinct();
            var subscriberKeys = mainKeys.Select(k => $"{k}.*.*.*").ToArray();
            _monitoringDataService.SubscriberToRealData(subscriberKeys);
        }

        private async Task<FrameworkElement> GetWiringDiagramUI(string customerId)
        {
            FrameworkElement wiringDiagramUI = null;

            var wiringDiagramConfig = _wiringDiagramService.GetWiringDiagramConfig(customerId);
            var displayDiagramList = wiringDiagramConfig.Where(d => d.isDisplay == 1);
            var mainDiagram = wiringDiagramConfig.FirstOrDefault(d => d.isMain == 1);
            if (!Directory.Exists(WIRING_DIAGRAM_PATH))
            {
                Directory.CreateDirectory(WIRING_DIAGRAM_PATH);
            }

            IEnumerable<Task<bool>> downloadFileTasksQuery =
               from diagram in displayDiagramList
               select DownloadMediaFileAsync(diagram.filePath.Replace('\\', '/'), Path.Combine(WIRING_DIAGRAM_PATH, GetFileName(diagram.filePath)));

            Task<bool>[] downloadTasks = downloadFileTasksQuery.ToArray();
            bool[] downloadFileResults = await Task.WhenAll(downloadTasks);
            string targetMainFileName = Path.Combine(WIRING_DIAGRAM_PATH, GetFileName(mainDiagram.filePath));
            //bool downloadResult = await DownloadMediaFileAsync(mainDiagram.filePath, targetFileName);

            if (downloadFileResults.Any())
            {
                //Uri uri = new Uri(mainDiagram.filePath);
                //var fileName = uri.Segments[uri.Segments.Length - 1];
                //var dataBuffer = GetWiringDiagram(uri);
                XamlUI xamlUI = null;
                xamlUI = _uiManager.Load(targetMainFileName);
                if (xamlUI != null)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {

                        var viewBox = new Viewbox();
                        viewBox.Stretch = System.Windows.Media.Stretch.Fill;
                        if (xamlUI.UI.Parent != null)
                        {
                            (xamlUI.UI.Parent as Viewbox).Child = null;
                        }
                        viewBox.Child = xamlUI.UI;
                        xamlUI.UI.MouseLeftButtonUp += UI_MouseLeftButtonUp;
                        xamlUI.UI.MouseLeftButtonDown += UI_MouseLeftButtonDown;
                        xamlUI.UI.MouseWheel += UI_MouseWheel;
                        wiringDiagramUI = viewBox;
                    }));
                    foreach (var item in xamlUI.Identities)
                    {
                        if (!_keys.Contains(item))
                        {
                            _keys.Add(item);
                        }
                    }
                }

            }
            return wiringDiagramUI;
        }

        private void UI_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var image = sender as FrameworkElement;
                if (image == null)
                {
                    return;
                }

                var st = image.RenderTransform as ScaleTransform;
                if (st == null)
                {
                    return;
                }

                st.ScaleX = 1.0;
                st.ScaleY = 1.0;
            }
        }

        private void UI_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var image = sender as FrameworkElement;
            if (image == null)
            {
                return;
            }


            double zoom = e.Delta > 0 ? .05 : -.05;
            var mousePosition = Mouse.GetPosition(image);

            var st = image.RenderTransform as ScaleTransform;
            if (st == null)
            {
                ScaleTransform scaleTransform1 = new ScaleTransform(zoom + 1, zoom + 1, image.ActualWidth / 2, image.ActualHeight / 2);
                image.RenderTransform = scaleTransform1;
            }
            else
            {
                if (st.ScaleX + zoom < 0.2)
                {
                    return;
                }
                st.ScaleX += zoom;
                st.ScaleY += zoom;
                System.Diagnostics.Debug.WriteLine(st.ScaleX);
            }
        }

        private void UI_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var targetElement = e.OriginalSource as TextBlock;
            if (targetElement != null && !string.IsNullOrEmpty(targetElement.Text))
            {
                string targetFileName = Path.Combine(WIRING_DIAGRAM_PATH, targetElement.Text.Trim() + ".xml");
                XamlUI xamlUI = null;
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    xamlUI = _uiManager.Load(targetFileName);
                    if (xamlUI != null)
                    {
                        var viewBox = new Viewbox();
                        viewBox.Stretch = System.Windows.Media.Stretch.Fill;
                        if (xamlUI.UI.Parent != null)
                        {
                            (xamlUI.UI.Parent as Viewbox).Child = null;
                        }
                        viewBox.Child = xamlUI.UI;
                        xamlUI.UI.MouseLeftButtonUp += UI_MouseLeftButtonUp;
                        xamlUI.UI.MouseLeftButtonDown += UI_MouseLeftButtonDown;
                        xamlUI.UI.MouseWheel += UI_MouseWheel;
                        foreach (var item in xamlUI.Identities)
                        {
                            if (!_keys.Contains(item))
                            {
                                _keys.Add(item);
                            }
                        }
                        SubscriberToCurrentData();
                        WiringDiagramUI = viewBox;
                    }

                }));
            }
            //throw new NotImplementedException();
        }


        [Dependency]
        public ILoggerFacade Logger { get; set; }


        private byte[] GetWiringDiagram(Uri diagramUri)
        {
            var httpClient = new RestClient(diagramUri);
            var request = new RestRequest();
            var response = httpClient.Execute(request);
            if (response.ErrorMessage != null)
            {
                Logger.Log($"下载一次接线图图时，发生错误响应{response.ErrorMessage}", Category.Exception, Priority.High);
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

        private string GetFileNameByUri(Uri uri)
        {
            string filename = string.Empty;

            //if (uri.IsFile)
            if (!string.IsNullOrEmpty(uri.AbsolutePath))
            {
                filename = Path.GetFileName(uri.AbsolutePath);
            }
            return filename;
        }

        private string GetFileName(string path)
        {
            string str = string.Empty;
            int pos1 = path.LastIndexOf('/');
            int pos2 = path.LastIndexOf('\\');
            int pos = Math.Max(pos1, pos2);
            if (pos < 0)
                str = path;
            else
                str = path.Substring(pos + 1);

            return str;
        }
        private async Task<bool> DownloadMediaFileAsync(string downloadUrl, string targetFilePath)
        {
            //if (File.Exists(targetFilePath))
            //{
            //    if (!string.IsNullOrEmpty(mediaMD5) && FileUtils.ComputeFileMd5(mediaFilePath) == mediaMD5)
            //    {
            //        return DownloadResult.Existed;
            //    }
            //    else
            //    {
            //        File.Delete(mediaFilePath);
            //    }
            //}

            var policy = Policy.Handle<Exception>().WaitAndRetryAsync(
                retryCount: 5, // Retry 3 times
                sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(2000), // Wait 2000ms between each try.
                onRetry: (exception, calculatedWaitDuration) => // Capture some info for logging!
                {
                    Logger.Log($"下载[{downloadUrl}时，发生异常，正在进行重试下载]", Category.Warn, Priority.High);
                    //Log4NetLogger.LogDebug(string.Format("{0}下载异常，开始下载重试！\r\n{1}", mediaFilePath, exception.Message));
                });


            await policy.ExecuteAsync(async () =>
            {
                using (FileStream fileStream = File.Create(targetFilePath))
                {
                    HttpWebRequest webReq = (HttpWebRequest)HttpWebRequest.Create(downloadUrl);

                    Task<WebResponse> responseTask = webReq.GetResponseAsync();

                    using (WebResponse response = await responseTask)
                    {

                        using (Stream responseStream = response.GetResponseStream())
                        {
                            await responseStream.CopyToAsync(fileStream);
                        }
                    }
                }
            });

            return true;
        }
    }
}
