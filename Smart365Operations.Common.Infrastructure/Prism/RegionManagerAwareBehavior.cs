using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Regions;

namespace Smart365Operations.Common.Infrastructure.Prism
{
    public class RegionManagerAwareBehavior:RegionBehavior
    {
        public const string BehaviorKey = "RegionManagerAwareBehavior";
        protected override void OnAttach()
        {
            Region.ActiveViews.CollectionChanged += ActiveViews_CollectionChanged;
        }

        private void ActiveViews_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    IRegionManager regionManager = Region.RegionManager;
                    FrameworkElement element = item as FrameworkElement;
                    if (element != null)
                    {
                        IRegionManager scopeRegionManger =
                            element.GetValue(RegionManager.RegionManagerProperty) as IRegionManager;
                        if (scopeRegionManger != null)
                        {
                            regionManager = scopeRegionManger;
                        }
                        InvokeOnRegionManagerAwareElement(item, x => x.RegionManager = regionManager);
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    InvokeOnRegionManagerAwareElement(item, x => x.RegionManager = null);
                }
            }
        }

        static void InvokeOnRegionManagerAwareElement(object item, Action<IRegionManagerAware> invocation)
        {
            var rmAwareItem = item as IRegionManagerAware;
            if (rmAwareItem != null)
            {
                invocation(rmAwareItem);
            }
            var frameworkElement = item as FrameworkElement;
            if (frameworkElement != null)
            {
                IRegionManagerAware rmAwareDataContext = frameworkElement.DataContext as IRegionManagerAware;
                if (rmAwareDataContext != null)
                {
                    var frameworkElementParent = frameworkElement.Parent as FrameworkElement;
                    if (frameworkElement != null)
                    {
                        var rmAwareDataContextParent = frameworkElementParent.DataContext as IRegionManagerAware;
                        if (rmAwareDataContextParent != null)
                        {
                            if (rmAwareDataContext == rmAwareDataContextParent)
                            {
                                return;
                            }
                        }
                    }
                    invocation(rmAwareDataContext);
                }
            }
        }
    }
}
