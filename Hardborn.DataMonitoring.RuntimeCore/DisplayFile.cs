using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hardborn.DataMonitoring.RuntimeCore
{
    [Serializable]
    public class DisplayFile
    {
        // Fields
        private BindingInformation bindingInfo;
        private string content;
        private double height;
        private string version = "1.0";
        private double width;

        // Properties
        public BindingInformation BindingInfo
        {
            get
            {
                return this.bindingInfo;
            }
            set
            {
                this.bindingInfo = value;
            }
        }

        public string Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
            }
        }

        [XmlAttribute]
        public double Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
            }
        }

        [XmlAttribute]
        public string Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }

        [XmlAttribute]
        public double Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }
    }


}
