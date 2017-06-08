using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Unity;
using System.Windows;
using Microsoft.Practices.Unity;
using MvvmDialogs;
using Prism.Logging;
using Prism.Modularity;
using Prism.Mvvm;
using Smart365Operation.Modules.Dashboard;
using Smart365Operation.Modules.Log4NetLogger;
using Smart365Operation.Modules.VideoMonitoring;
using Smart365Operation.Modules.VideoMonitoring.Services;
using Smart365Operation.Modules.VideoMonitoring.ViewModels;
using Smart365Operations.Client.Services;
using Smart365Operations.Client.ViewModels;
using Smart365Operations.Client.Views;
using Smart365Operations.Common.Infrastructure.Interfaces;
using System.Reflection;
using Prism.Regions;
using Smart365Operations.Common.Infrastructure;
using Smart365Operations.Common.Infrastructure.Prism;

namespace Smart365Operations.Client
{
    public class Smart365OperationsBootstrapper : UnityBootstrapper
    {

        public Smart365OperationsBootstrapper()
        {
            //ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            //{
            //    Type type;
            //    string viewModelName;
            //    var viewName = viewType.FullName;
            //    var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            //    viewModelName = $"{viewName}ViewModel,{viewAssemblyName}";
            //    type = Type.GetType(viewModelName);
            //    if (type == null)
            //    {
            //        viewModelName = $"{viewName}Model,{viewAssemblyName}";
            //        type = Type.GetType(viewModelName);
            //    }
            //    return type;
            //});
        }

        private readonly Log4NetLogger _logger = new Log4NetLogger();
        private readonly IDialogService dialogService = new DialogService();
        protected override ILoggerFacade CreateLogger()
        {
            return _logger;
        }

        protected override DependencyObject CreateShell()
        {
            Shell view = this.Container.TryResolve<Shell>();
            return view;
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new AggregateModuleCatalog();
        }

        protected override void ConfigureModuleCatalog()
        {
            Type DashboardModuleType = typeof(DashboardModule);
            ModuleCatalog.AddModule(new ModuleInfo(DashboardModuleType.Name, DashboardModuleType.AssemblyQualifiedName));
           

            ConfigurationModuleCatalog configurationCatalog = new ConfigurationModuleCatalog();
            ((AggregateModuleCatalog)ModuleCatalog).AddCatalog(configurationCatalog);

           // ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
           // moduleCatalog.AddModule(typeof(MonitoringModule));
           //// moduleCatalog.AddModule(typeof(VideoMonitoringModule));
           // moduleCatalog.AddModule(typeof(DataAnaysisModule));
           //// moduleCatalog.AddModule(typeof(DashboardModule));

        }

        public void Show()
        {
            var regionManager = RegionManager.GetRegionManager(Shell);                                      
            RegionManagerAware.SetRegionManagerAware(Shell, regionManager);
            regionManager.RequestNavigate(KnownRegionNames.MainRegion, "OverviewMapView");
            App.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            //Container.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());
            Container.RegisterInstance<IDialogService>(this.dialogService);
            Container.RegisterType<IAuthenticationService, AuthenticationService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ICustomerService, CustomerService>(new ContainerControlledLifetimeManager());
            //Container.RegisterType<ICameraService, CameraService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ICustomerEquipmentService, CustomerEquipmentService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IShellService, ShellService>(new ContainerControlledLifetimeManager());
           
        }

        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            IRegionBehaviorFactory behaviors = base.ConfigureDefaultRegionBehaviors();
            behaviors.AddIfMissing(RegionManagerAwareBehavior.BehaviorKey, typeof(RegionManagerAwareBehavior));
            return behaviors;
        }
    }
}
