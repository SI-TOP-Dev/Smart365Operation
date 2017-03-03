using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;

namespace Hardborn.DataMonitoring.RuntimeCore
{
    public class UIManager : IDisposable
    {
        // Fields
        private Dictionary<string, DataPointBindingObject> BindingDictionary = new Dictionary<string, DataPointBindingObject>();
        private Dispatcher dispatcher;
        private bool enableSafeMode;
        private bool expired;
        private static UIManager instance;
       // private Timer timer;
        private Dictionary<string, XamlUI> UIDictionary = new Dictionary<string, XamlUI>();

        // Methods
        private UIManager()
        {
        }

        internal void AddBindingObjcet(string identity, DataPointBindingObject obj)
        {
            if (!this.BindingDictionary.ContainsKey(identity))
            {
                this.BindingDictionary.Add(identity, obj);
            }
        }

        public bool ClearBindingObject(string readkey)
        {
            return this.BindingDictionary.Remove(readkey);
        }

        public void ClearBindingObjects()
        {
            this.BindingDictionary.Clear();
        }

        public void ClearUI()
        {
            foreach (XamlUI lui in this.UIDictionary.Values)
            {
                lui.ClearBinding();
                lui.ClearUIParent();
            }
            this.UIDictionary.Clear();
        }

        public void ClearUI(string filepath)
        {
            if (this.UIDictionary.ContainsKey(filepath))
            {
                this.UIDictionary[filepath].ClearBinding();
                this.UIDictionary[filepath].ClearUIParent();
                this.UIDictionary.Remove(filepath);
            }
        }

        public void ClearUI(string uri, string suffix)
        {
            string filepath = uri + string.Format("({0})", suffix);
            this.ClearUI(filepath);
        }

        public void Dispose()
        {
            this.ClearUI();
            this.BindingDictionary.Clear();
        }

        internal DataPointBindingObject GetBindingObject(string identity)
        {
            if (this.BindingDictionary.ContainsKey(identity))
            {
                return this.BindingDictionary[identity];
            }
            return null;
        }

        public XamlUI Load(string xamlURI)
        {
            try
            {
                if (this.expired)
                {
                    throw new Exception("Invalid License Information!");
                }
                if (string.IsNullOrEmpty(xamlURI))
                {
                    throw new ArgumentNullException("xamlURI");
                }
                if (!this.UIDictionary.ContainsKey(xamlURI))
                {
                    if (xamlURI.EndsWith(".xaml", true, null))
                    {
                        XamlUI lui = XamlHelper.ConvertXamlToUI(xamlURI);
                        this.UIDictionary.Add(xamlURI, lui);
                    }
                    else if (xamlURI.EndsWith(".dll", true, null))
                    {
                        XamlUI lui2 = XamlHelper.ConvertDLLToUI(xamlURI);
                        this.UIDictionary.Add(xamlURI, lui2);
                    }
                    else
                    {
                        XamlUI lui3 = XamlHelper.ConvertXmlToUI(xamlURI);
                        this.UIDictionary.Add(xamlURI, lui3);
                    }
                }
                return this.UIDictionary[xamlURI];
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return null;
            }
        }

        public XamlUI Load(byte[] fileBuffer, string name)
        {
            try
            {
                if (this.expired)
                {
                    throw new LicenseException(typeof(UIManager), instance, "Invalid License Information!");
                }
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException("name");
                }
                if (!this.UIDictionary.ContainsKey(name))
                {
                    if (!name.EndsWith(".xaml", true, CultureInfo.CurrentCulture))
                    {
                        if (!name.EndsWith(".xml", true, CultureInfo.CurrentCulture))
                        {
                            throw new Exception("不支持的文件类型！");
                        }
                        XamlUI lui2 = XamlHelper.ConvertXmlToUI(fileBuffer);
                        this.UIDictionary.Add(name, lui2);
                    }
                    else
                    {
                        XamlUI lui = XamlHelper.ConvertXamlToUI(fileBuffer);
                        this.UIDictionary.Add(name, lui);
                    }
                }
                return this.UIDictionary[name];
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return null;
            }
        }

        public XamlUI Load(string xamlURI, string suffix)
        {
            if (string.IsNullOrEmpty(suffix))
            {
                return this.Load(xamlURI);
            }
            try
            {
                if (string.IsNullOrEmpty(xamlURI))
                {
                    throw new ArgumentNullException("xamlURI");
                }
                string key = xamlURI + string.Format("({0})", suffix);
                if (!this.UIDictionary.ContainsKey(key))
                {
                    if (xamlURI.EndsWith(".xaml", true, null))
                    {
                        XamlUI lui = XamlHelper.ConvertXamlToUI(xamlURI, suffix);
                        this.UIDictionary.Add(key, lui);
                    }
                    else if (xamlURI.EndsWith(".dll", true, null))
                    {
                        XamlUI lui2 = XamlHelper.ConvertDLLToUI(xamlURI, suffix);
                        this.UIDictionary.Add(key, lui2);
                    }
                    else
                    {
                        XamlUI lui3 = XamlHelper.ConvertXmlToUI(xamlURI, suffix);
                        this.UIDictionary.Add(key, lui3);
                    }
                }
                return this.UIDictionary[key];
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return null;
            }
        }

        public XamlUI Load(byte[] fileBuffer, string name, string suffix)
        {
            if (string.IsNullOrEmpty(suffix))
            {
                return this.Load(fileBuffer, name);
            }
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException("name");
                }
                string key = name + string.Format("({0})", suffix);
                if (!this.UIDictionary.ContainsKey(key))
                {
                    if (!name.EndsWith(".xaml", true, CultureInfo.CurrentCulture))
                    {
                        if (!name.EndsWith(".xml", true, CultureInfo.CurrentCulture))
                        {
                            throw new Exception("不支持的文件类型！");
                        }
                        XamlUI lui2 = XamlHelper.ConvertXmlToUI(fileBuffer, suffix);
                        this.UIDictionary.Add(key, lui2);
                    }
                    else
                    {
                        XamlUI lui = XamlHelper.ConvertXamlToUI(fileBuffer, suffix);
                        this.UIDictionary.Add(key, lui);
                    }
                }
                return this.UIDictionary[key];
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return null;
            }
        }

        public void Purge(string readkey)
        {
            if (this.BindingDictionary.ContainsKey(readkey))
            {
                this.BindingDictionary[readkey].Value = null;
            }
        }

        public void PurgeAll()
        {
            foreach (DataPointBindingObject obj2 in this.BindingDictionary.Values)
            {
                obj2.Value = null;
            }
        }

        public void SetBinding(DependencyObject target, DependencyProperty targetProperty, string identity)
        {
            if (string.IsNullOrEmpty(identity))
            {
                BindingOperations.ClearBinding(target, targetProperty);
            }
            else
            {
                DataPointBindingObject bindingObject = this.GetBindingObject(identity);
                if (bindingObject == null)
                {
                    bindingObject = new DataPointBindingObject(identity);
                    this.BindingDictionary.Add(identity, bindingObject);
                }
                Binding binding = new Binding("Value")
                {
                    Source = bindingObject
                };
                if (target is RotateTransform)
                {
                    binding.Converter = new RotateValueConverter();
                }
                BindingOperations.SetBinding(target, targetProperty, binding);
            }
        }

        public void SetBinding(DependencyObject target, DependencyProperty targetProperty, string[] identities)
        {
            if (identities != null)
            {
                Binding binding = BindingOperations.GetBinding(target, targetProperty);
                if (binding != null)
                {
                    MultiDataPointBindingObject source = binding.Source as MultiDataPointBindingObject;
                    if (source != null)
                    {
                        source.Dispose();
                    }
                }
                if (identities.Length == 1)
                {
                    this.SetBinding(target, targetProperty, identities[0]);
                }
                else if (target is ISetValue)
                {
                    BindingOperations.ClearBinding(target, targetProperty);
                    MultiDataPointBindingObject obj3 = new MultiDataPointBindingObject(target as ISetValue);
                    foreach (string str in identities)
                    {
                        DataPointBindingObject bindingObject = this.GetBindingObject(str);
                        if (bindingObject == null)
                        {
                            bindingObject = new DataPointBindingObject(str);
                            this.AddBindingObjcet(str, bindingObject);
                        }
                        obj3.AddBindingObject(bindingObject);
                    }
                    Binding binding2 = new Binding("Value")
                    {
                        Mode = BindingMode.OneWay,
                        Source = obj3
                    };
                    BindingOperations.SetBinding(target, targetProperty, binding2);
                }
            }
        }

        public void UpdateData(string identity, object value)
        {
            Action action2 = null;
            if (!this.BindingDictionary.ContainsKey(identity))
            {
                this.BindingDictionary.Add(identity, new DataPointBindingObject(identity));
            }
            DataPointBindingObject obj = this.BindingDictionary[identity];
            if (this.enableSafeMode && (this.dispatcher != null))
            {
                if (action2 == null)
                {
                    action2 = () => obj.Value = value;
                }
                Action method = action2;
                this.dispatcher.Invoke(method, new object[0]);
            }
            else if (obj.Value != value)
            {
                obj.Value = value;
            }
        }

        // Properties
        public Dispatcher Dispatcher
        {
            get
            {
                return this.dispatcher;
            }
            set
            {
                this.dispatcher = value;
            }
        }

        public bool EnableSafeMode
        {
            get
            {
                return this.enableSafeMode;
            }
            set
            {
                this.enableSafeMode = value;
            }
        }

        public static UIManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UIManager();
                }
                return instance;
            }
        }

        internal object this[string identity]
        {
            get
            {
                DataPointBindingObject bindingObject = this.GetBindingObject(identity);
                if (bindingObject != null)
                {
                    return bindingObject.Value;
                }
                return null;
            }
        }
    }


}
