﻿using Microsoft.Practices.Unity;
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
using Prism.Logging;
using Smart365Operations.Client.ViewModels;

namespace Smart365Operations.Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Shell : Window
    {
        public Shell()
        {
            InitializeComponent();
            this.Closed += Shell_Closed;
           // DataContext = viewModel;
        }

        private void Shell_Closed(object sender, EventArgs e)
        {
            if (Info != null)
            {
                App.ShellTable.Remove(Info);
            }
        }

        public ShellInfo Info { get; set; }
    }
}
