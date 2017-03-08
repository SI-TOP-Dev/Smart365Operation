using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Smart365Operation.Modules.DataAnalysis.Services;
using Smart365Operation.Modules.DataAnalysis.Views;
using Smart365Operations.Common.Infrastructure.Interfaces;

namespace Smart365Operation.Modules.DataAnalysis
{
    public class DataAnaysisModule:IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public DataAnaysisModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }
        public void Initialize()
        {
            _container.RegisterType<IDeviceParameterInfoService, DeviceParameterInfoService>();
            _container.RegisterType<IHistoryDataService, HistoryDataService>();
            _container.RegisterType(typeof(object), typeof(DataAnaysisView), "DataAnaysisView");
            _container.RegisterType(typeof(object), typeof(DataCurveChart), "DataCurveChart");
            _container.RegisterType(typeof(object), typeof(AnalysisParameterSetting), "AnalysisParameterSetting");

        }
    }
}
