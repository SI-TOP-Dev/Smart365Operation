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
using Smart365Operation.Modules.Dashboard.ViewModels;

namespace Smart365Operation.Modules.Dashboard.Views
{
    /// <summary>
    /// CustomerMarker.xaml 的交互逻辑
    /// </summary>
    public partial class CustomerMarker : UserControl
    {
        public CustomerMarker(CustomerMonitoringViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        private void elevator_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (!string.IsNullOrEmpty(Elevators.First().ID))
            //{
            //    //MyEventArgs me = new MyEventArgs();
            //    //me.myEventArgsData = Elevators.First().ID;
            //    //ComEventDelegate.FireEvent(me);
            //}
            //else
            //{
            //    //if (popupView.IsOpen)
            //    //    popupView.IsOpen = false;
            //    //else
            //    popupView.IsOpen = true;
            //}
        }

        private void elevator_MouseEnter(object sender, MouseEventArgs e)
        {
            popupView.IsOpen = true;
        }

        private void elevator_MouseLeave(object sender, MouseEventArgs e)
        {
            popupView.IsOpen = false;
        }
    }
}
