using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Com.Shengzuo.RuntimeCore;
using Com.Shengzuo.RuntimeCore.Common;
using Smart365Operation.Modules.Monitoring.ViewModels;
using Smart365Operations.Common.Infrastructure.Interfaces;

namespace Smart365Operation.Modules.Monitoring.Views
{
    /// <summary>
    /// MonitoringView.xaml 的交互逻辑
    /// </summary>
    public partial class MonitoringView : UserControl
    {
        //private readonly IMonitoringDataService _monitoringDataService;
        //private UIManager _uiManager;
        //private XamlUI _xamlUi;
        public MonitoringView(MonitoringViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        //private void MonitoringView_Loaded(object sender, RoutedEventArgs e)
        //{
        //    _monitoringDataService.DataUpdated += _monitoringDataService_DataUpdated;
        //    _uiManager = UIManager.Instance;
        //    _uiManager.Dispatcher = Application.Current.Dispatcher;
        //    _uiManager.EnableSafeMode = true;
        //    _xamlUi = _uiManager.Load(@"C:\Users\Hardborn\Desktop\005.xml");
        //    box.Child = _xamlUi.UI;
        //}

        //private void _monitoringDataService_DataUpdated(object sender, MonitoringDataEventArgs e)
        //{
        //    _uiManager.UpdateData(e.Key, e.Value.ToString());
        //}
    }
}
