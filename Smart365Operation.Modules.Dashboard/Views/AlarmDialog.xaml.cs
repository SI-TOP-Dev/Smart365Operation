using MvvmDialogs;
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
using System.Windows.Shapes;

namespace Smart365Operation.Modules.Dashboard.Views
{
    /// <summary>
    /// AlarmDialogView.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmDialog : Window //,IWindow
    {
        public AlarmDialog()
        {
            InitializeComponent();
        }

        //object IWindow.DataContext
        //{
        //    get { return this.DataContext; }
        //    set { this.DataContext = value; }
        //}

        //bool? IWindow.DialogResult
        //{
        //    get { return this.DialogResult; }
        //    set { this.DialogResult = value; }
        //}

        //ContentControl IWindow.Owner
        //{
        //    get { return this.Owner; }
        //    set { this.Owner = (Window)value; }
        //}

        //bool? IWindow.ShowDialog()
        //{
        //    return this.ShowDialog();
        //}

        //void IWindow.Show()
        //{
        //    this.Show();
        //}
    }
}
