using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Hardborn.DataMonitoring.RuntimeCore
{
    public class XamlUI
    {
        // Fields
        private BindingTag[] bindingInfo;
        private string[] identities;
        private FrameworkElement ui;

        // Methods
        internal XamlUI(FrameworkElement ele, string[] id)
        {
            this.ui = ele;
            this.identities = id;
        }

        internal void ClearBinding()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (BindingTag tag in this.bindingInfo)
            {
                if (!dictionary.ContainsKey(tag.ElementName))
                {
                    FrameworkElement element = this.ui.FindName(tag.ElementName) as FrameworkElement;
                    if (element != null)
                    {
                        if (element is ISetValue)
                        {
                            string name = "ValueProperty";
                            FieldInfo field = element.GetType().GetField(name, BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static);
                            if ((field != null) && (field.FieldType == typeof(DependencyProperty)))
                            {
                                DependencyProperty dp = field.GetValue(element) as DependencyProperty;
                                Binding binding = BindingOperations.GetBinding(element, dp);
                                if (binding != null)
                                {
                                    MultiDataPointBindingObject source = binding.Source as MultiDataPointBindingObject;
                                    if (source != null)
                                    {
                                        source.Dispose();
                                    }
                                }
                            }
                        }
                        BindingOperations.ClearAllBindings(element);
                        if (element is IDisposable)
                        {
                            ((IDisposable)element).Dispose();
                        }
                        dictionary.Add(tag.ElementName, tag.ElementName);
                    }
                }
            }
        }

        public void ClearUIParent()
        {
            if (this.ui.Parent != null)
            {
                if (this.ui.Parent is ContentControl)
                {
                    ((ContentControl)this.ui.Parent).Content = null;
                }
                else if (this.ui.Parent is Page)
                {
                    ((Page)this.ui.Parent).Content = null;
                }
                else if (this.ui.Parent is Panel)
                {
                    ((Panel)this.ui.Parent).Children.Remove(this.ui);
                }
            }
        }

        // Properties
        public BindingTag[] BindingInfo
        {
            get
            {
                return this.bindingInfo;
            }
            internal set
            {
                this.bindingInfo = value;
            }
        }

        public string[] Identities
        {
            get
            {
                return this.identities;
            }
        }

        public FrameworkElement OriginalUI
        {
            get
            {
                return this.ui;
            }
        }

        public FrameworkElement UI
        {
            get
            {
                if (this.ui.Parent != null)
                {
                    if (this.ui.Parent is Window)
                    {
                        ((Window)this.ui.Parent).Content = null;
                    }
                    if (this.ui.Parent is Page)
                    {
                        ((Page)this.ui.Parent).Content = null;
                    }
                }
                return this.ui;
            }
        }
    }


}
