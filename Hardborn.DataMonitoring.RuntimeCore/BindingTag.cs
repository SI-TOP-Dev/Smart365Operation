using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hardborn.DataMonitoring.RuntimeCore
{
    public class BindingTag
    {
        // Fields
        private string attributeName;
        private string bindingPath;
        private string elementName;
        private string expression;

        // Methods
        public BindingTag() : this(string.Empty, string.Empty, string.Empty)
        {
        }

        public BindingTag(string eleName, string attribName, string bindPath)
        {
            this.elementName = "";
            this.attributeName = "";
            this.bindingPath = "";
            this.elementName = eleName;
            this.attributeName = attribName;
            this.bindingPath = bindPath;
        }

        // Properties
        [XmlAttribute]
        public string AttributeName
        {
            get
            {
                return this.attributeName;
            }
            set
            {
                this.attributeName = value;
            }
        }

        [XmlAttribute]
        public string BindingPath
        {
            get
            {
                return this.bindingPath;
            }
            set
            {
                this.bindingPath = value;
            }
        }

        [XmlAttribute]
        public string ElementName
        {
            get
            {
                return this.elementName;
            }
            set
            {
                this.elementName = value;
            }
        }

        [XmlAttribute]
        public string Expression
        {
            get
            {
                return this.expression;
            }
            set
            {
                this.expression = value;
            }
        }
    }


}
