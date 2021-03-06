﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using Smart365Operation.Modules.Monitoring.Services;
using Smart365Operation.Modules.Monitoring.Views;
using Smart365Operations.Common.Infrastructure.Interfaces;

namespace Smart365Operation.Modules.Monitoring
{
    public class MonitoringModule:IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;

        public MonitoringModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }
        public void Initialize()
        {
            _container.RegisterType<IWiringDiagramService, WiringDiagramService>();
            _container.RegisterType<IMonitoringSummaryService, MonitoringSummaryService>();
            _container.RegisterType<IMonitoringDataService, MonitoringDataService>(new ContainerControlledLifetimeManager());
            _container.RegisterType(typeof(object), typeof(CustomerDetail), "CustomerDetail");
            _container.RegisterType(typeof(object), typeof(WiringDiagramView), "WiringDiagramView");
            _container.RegisterType(typeof(object), typeof(Monitoring), "Monitoring");
            foreach (var item in _container.Registrations)
            {
                var str = item.Name;
            }
            // _container.RegisterTypeForNavigation<Monitoring>();
            // this._regionManager.RegisterViewWithRegion("MainRegion", () => this._container.Resolve<MonitoringView>());

        }
    }
}
