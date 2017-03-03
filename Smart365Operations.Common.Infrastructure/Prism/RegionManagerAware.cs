using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Regions;

namespace Smart365Operations.Common.Infrastructure.Prism
{
    public static class RegionManagerAware
    {
        public static void SetRegionManagerAware(object item, IRegionManager regionManager)
        {
            var rmAware = item as IRegionManagerAware;
            if (null != rmAware)
            {
                rmAware.RegionManager = regionManager;
            }
            var rmAwareFrameworkElement = item as FrameworkElement;
            if (rmAwareFrameworkElement != null)
            {
                var rmAwareDataContext = rmAwareFrameworkElement.DataContext as IRegionManagerAware;
                if (rmAwareDataContext != null)
                {
                    rmAwareDataContext.RegionManager = regionManager;
                }
            }
        }
    }
}
