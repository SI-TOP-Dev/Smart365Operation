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
        private readonly IShellService _shellService;

        public ShellViewModel(IShellService shellService)
        {
            _shellService = shellService;
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public DelegateCommand<string> OpenShellCommand => new DelegateCommand<string>(OpenShell);
        public DelegateCommand<string> NavigateCommand => new DelegateCommand<string>(Navigate);
        public DelegateCommand<object> InitializeCommand => new DelegateCommand<object>(Initialize);
        public DelegateCommand CloseShellCommand => new DelegateCommand(CloseShell, CanCloseShell);

        private bool CanCloseShell()
        {
            return true;
        }

        private void CloseShell()
        {
            if (IsMainShell)
            {
                Environment.Exit(1);
            }
        }

        private bool _isMainShell = true;
        public bool IsMainShell
        {
            get { return _isMainShell; }
            set { SetProperty(ref _isMainShell, value); }
        }
        private void Initialize(object obj)
        {
            //Navigate("OverviewMapView");
        }

        private void OpenShell(string viewName)
        {
            _shellService.ShowShell(viewName,null);
        }

        private void Navigate(string viewName)
        {
            RegionManager.RequestNavigate(KnownRegionNames.MainRegion, viewName);
        }

        public IRegionManager RegionManager { get; set; }
    }
}
