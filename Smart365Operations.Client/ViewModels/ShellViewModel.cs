using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Smart365Operations.Common.Infrastructure;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Prism;

namespace Smart365Operations.Client
{
    public class ShellViewModel : BindableBase,IRegionManagerAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IShellService _shellService;

        public ShellViewModel(IRegionManager regionManager, IShellService shellService)
        {
            _regionManager = regionManager;
            _shellService = shellService;
        }

        public DelegateCommand<string> OpenShellCommand => new DelegateCommand<string>(OpenShell);
        public DelegateCommand<string> NavigateCommand => new DelegateCommand<string>(Navigate);

      

        private void OpenShell(string viewName)
        {
            _shellService.ShowShell(viewName);
        }

        private void Navigate(string viewName)
        {
            _regionManager.RequestNavigate(KnownRegionNames.MainRegion, viewName);
        }

        public IRegionManager RegionManager { get; set; }
    }
}
