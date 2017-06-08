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
using System.Threading;

namespace Smart365Operations.Client
{
    public class ShellService : IShellService
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly ICustomerService _customerService;

        public ShellService(IUnityContainer container, IRegionManager regionManager, ICustomerService customerService)
        {
            _container = container;
            _regionManager = regionManager;
            _customerService = customerService;
            App.ShellTable = new Dictionary<ShellInfo, Shell>(new ShellInfoEqualityComparer());
        }
        public void ShowShell(string uri, NavigationParameters parameters)
        {
            var principal = Thread.CurrentPrincipal as SystemPrincipal;
            var agentId = principal.Identity.Id;
            var customerList = _customerService.GetCustomersBy(agentId);
           
            ShellInfo shellInfo = null;
            var customer = parameters["Customer"] as Customer;
            if (customer != null)
            {
                if (customerList.Any(c => c.Id == customer.Id))
                {
                    shellInfo = new ShellInfo(uri, customer);
                }
                else
                {
                    return;
                }
                
            }
            else
            {
                string id = parameters["CustomerId"] as string;
                if(!string.IsNullOrEmpty(id))
                {
                    if (customerList.Any(c => c.Id.ToString() == id))
                    {
                        shellInfo = new ShellInfo(uri, id);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            
            if(shellInfo == null)
            {
                return;
            }

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
            Id = Customer.Id.ToString();
        }
        public ShellInfo(string uri, string id)
        {
            Uri = uri;
            Customer = null;
            Id = id;
        }
        public string Uri { get; set; }
        public string Id { get; set; }
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
            else if (info1.Uri == info2.Uri && info1.Id == info2.Id)
                return true;
            else
                return false;
        }

        public int GetHashCode(ShellInfo info)
        {
            string hCode = info.Uri + info.Id;
            return hCode.GetHashCode();
        }
    }
}
