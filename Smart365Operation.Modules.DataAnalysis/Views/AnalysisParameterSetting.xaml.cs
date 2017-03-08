using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Smart365Operation.Modules.DataAnalysis.Views
{
    /// <summary>
    /// AnalysisParameterSetting.xaml 的交互逻辑
    /// </summary>
    public partial class AnalysisParameterSetting : UserControl
    {
        public AnalysisParameterSetting()
        {
            InitializeComponent();
        }

        private void DatePicker_CalendarOpened(object sender, RoutedEventArgs e)
        {
            DatePicker datepicker = (DatePicker)sender;
            Popup popup = (Popup)datepicker.Template.FindName("PART_Popup", datepicker);
            Calendar cal = (Calendar)popup.Child;
            cal.DisplayModeChanged += Cal_DisplayModeChanged;
            cal.DisplayMode = System.Windows.Controls.CalendarMode.Decade;
            
        }

        private void Cal_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            Calendar cal = (Calendar)sender;
            if (cal.DisplayMode == CalendarMode.Month)
            {
                cal.DisplayMode = CalendarMode.Year;
            }
        }
    }
}
