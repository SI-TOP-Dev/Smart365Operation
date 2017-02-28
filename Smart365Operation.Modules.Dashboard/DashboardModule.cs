using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Smart365Operation.Modules.Dashboard.Interfaces;
using Smart365Operation.Modules.Dashboard.Services;
using Smart365Operation.Modules.Dashboard.Views;

namespace Smart365Operation.Modules.Dashboard
{
    public class DashboardModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;

        public DashboardModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }
        public void Initialize()
        {
            this._container.RegisterType<IDataStatisticsService, DataStatisticsService>();
            this._regionManager.RegisterViewWithRegion("MainRegion", () => this._container.Resolve<OverviewMapView>());

        }
    }
}
