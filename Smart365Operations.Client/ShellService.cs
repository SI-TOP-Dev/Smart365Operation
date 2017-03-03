using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Regions;
using Smart365Operations.Common.Infrastructure;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Prism;

namespace Smart365Operations.Client
{
    public class ShellService : IShellService
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public ShellService(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }
        public void ShowShell(string uri)
        {
            var shell = _container.Resolve<Shell>();

            var scopedRegion = _regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(shell, scopedRegion);

            RegionManagerAware.SetRegionManagerAware(shell, scopedRegion);

            scopedRegion.RequestNavigate(KnownRegionNames.MainRegion, uri);

            shell.Show();
        }
    }
}
