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
using Smart365Operations.Common.Infrastructure.Models;

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

            App.ShellTable = new Dictionary<ShellInfo, Shell>(new ShellInfoEqualityComparer());
        }
        public void ShowShell(string uri, NavigationParameters parameters)
        {
            var shellInfo = new ShellInfo(uri, parameters["Customer"] as Customer);
            if (App.ShellTable.ContainsKey(shellInfo))
            {
                var shell = App.ShellTable[shellInfo];
                shell.Activate();
            }
            else
            {
                var shell = _container.Resolve<Shell>();

                (shell.DataContext as ShellViewModel).IsMainShell = false;

                var scopedRegion = _regionManager.CreateRegionManager();
                RegionManager.SetRegionManager(shell, scopedRegion);


                RegionManagerAware.SetRegionManagerAware(shell, scopedRegion);

                scopedRegion.RequestNavigate(KnownRegionNames.MainRegion, uri, parameters);
                shell.Info = shellInfo;
                App.ShellTable.Add(shellInfo, shell);

                shell.Show();
            }

        }

    }

    public class ShellInfo
    {
        public ShellInfo(string uri, Customer customer)
        {
            Uri = uri;
            Customer = customer;
        }
        public string Uri { get; set; }
        public Customer Customer { get; set; }
    }

    class ShellInfoEqualityComparer : IEqualityComparer<ShellInfo>
    {
        public bool Equals(ShellInfo info1, ShellInfo info2)
        {
            if (info2 == null && info1 == null)
                return true;
            else if (info1 == null | info2 == null)
                return false;
            else if (info1.Uri == info2.Uri && info1.Customer.Id == info2.Customer.Id)
                return true;
            else
                return false;
        }

        public int GetHashCode(ShellInfo info)
        {
            string hCode = info.Uri + info.Customer.Id;
            return hCode.GetHashCode();
        }
    }
}
