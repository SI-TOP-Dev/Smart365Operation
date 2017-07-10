using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Smart365Operations.Client.ViewModels;
using Smart365Operations.Client.Views;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Models;
using Microsoft.Practices.Unity;
using MvvmDialogs;
using Prism.Logging;

namespace Smart365Operations.Client
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private static System.Threading.Mutex mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            bool can_execute = true;
            try
            {
                mutex = new System.Threading.Mutex(false, "SMART365CLIENT", out can_execute);
            }
            catch (Exception ex)
            {

            }
            if (can_execute)
            {
                SystemPrincipal customPrincipal = new SystemPrincipal();
                AppDomain.CurrentDomain.SetThreadPrincipal(customPrincipal);

                base.OnStartup(e);

                Smart365OperationsBootstrapper bootStrapper = new Smart365OperationsBootstrapper();
                bootStrapper.Run();

                ShutdownMode = ShutdownMode.OnExplicitShutdown;

                LoginScreen loginWindow = bootStrapper.Container.Resolve<LoginScreen>();
                //AuthenticationViewModel viewModel =
                //    new AuthenticationViewModel(bootStrapper.Container.Resolve<IAuthenticationService>());
                //loginWindow.DataContext = viewModel;
                bool? logonResult = loginWindow.ShowDialog();
                var viewModel = loginWindow.ViewModel as AuthenticationViewModel;
                if (logonResult.HasValue && viewModel != null && viewModel.IsAuthenticated)
                {
                    bootStrapper.Show(viewModel.DisplayName);
                    Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                }
                else
                {
                    Application.Current.Shutdown(1);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("SMART365CLIENT already running!");
                Application.Current.Shutdown(1);
            }

        }

        public static Dictionary<ShellInfo, Shell> ShellTable;
    }
}
