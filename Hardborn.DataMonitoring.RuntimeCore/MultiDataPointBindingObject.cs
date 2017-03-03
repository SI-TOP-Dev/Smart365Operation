using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Hardborn.DataMonitoring.RuntimeCore
{
    public class MultiDataPointBindingObject : INotifyPropertyChanged, IDisposable
    {
        // Fields
        private List<DataPointBindingObject> bindingSources;
        private ISetValue targetElement;
        private object value;

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        // Methods
        public MultiDataPointBindingObject()
        {
        }

        public MultiDataPointBindingObject(ISetValue target)
        {
            this.targetElement = target;
        }

        public void AddBindingObject(DataPointBindingObject obj)
        {
            if (obj != null)
            {
                if (this.bindingSources == null)
                {
                    this.bindingSources = new List<DataPointBindingObject>();
                }
                if (!this.bindingSources.Contains(obj))
                {
                    this.bindingSources.Add(obj);
                    obj.PropertyChanged += new PropertyChangedEventHandler(this.obj_PropertyChanged);
                }
            }
        }

        public void Dispose()
        {
            if (this.bindingSources != null)
            {
                foreach (DataPointBindingObject obj2 in this.bindingSources.ToArray())
                {
                    this.RemoveBindingObject(obj2);
                }
            }
            this.bindingSources = null;
            this.targetElement = null;
        }

        private void obj_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                DataPointBindingObject obj2 = sender as DataPointBindingObject;
                DataPointValue value2 = new DataPointValue
                {
                    ID = obj2.DataPointIdentity,
                    Value = obj2.Value
                };
                if (this.targetElement != null)
                {
                    this.targetElement.SetValue(value2);
                }
                else
                {
                    this.Value = value2;
                }
            }
        }

        public void RemoveBindingObject(DataPointBindingObject obj)
        {
            if (obj != null)
            {
                if (this.bindingSources == null)
                {
                    this.bindingSources = new List<DataPointBindingObject>();
                }
                if (this.bindingSources.Contains(obj))
                {
                    this.bindingSources.Remove(obj);
                    obj.PropertyChanged -= new PropertyChangedEventHandler(this.obj_PropertyChanged);
                }
            }
        }

        // Properties
        public object Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Value"));
                }
            }
        }
    }

    public class DataPointBindingObject : INotifyPropertyChanged
    {
        // Fields
        private string dataPointIdentity;
        private object value;

        // Events
        public event PropertyChangedEventHandler PropertyChanged;

        // Methods
        public DataPointBindingObject(string dataIdentity)
        {
            this.dataPointIdentity = dataIdentity;
        }

        private void FirePropertyChangedEvent(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Properties
        public string DataPointIdentity
        {
            get
            {
                return this.dataPointIdentity;
            }
        }

        public object Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
                this.FirePropertyChangedEvent("Value");
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DataPointValue
    {
        public string ID;
        public object Value;
        public override string ToString()
        {
            if (this.Value != null)
            {
                return this.Value.ToString();
            }
            return base.ToString();
        }
    }



}
