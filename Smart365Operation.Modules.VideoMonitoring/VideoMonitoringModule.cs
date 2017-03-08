using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Smart365Operation.Modules.VideoMonitoring.Services;
using Smart365Operation.Modules.VideoMonitoring.ViewModels;
using Smart365Operation.Modules.VideoMonitoring.Views;
using Smart365Operations.Common.Infrastructure.Interfaces;

namespace Smart365Operation.Modules.VideoMonitoring
{
    public class VideoMonitoringModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;

        public VideoMonitoringModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }
        public void Initialize()
        {
            _container.RegisterType(typeof(object), typeof(VideoSurveillanceView), "VideoSurveillanceView");
            _container.RegisterType(typeof(object), typeof(VideoMonitoringView), "VideoMonitoringView");
        }
        
    }
}
