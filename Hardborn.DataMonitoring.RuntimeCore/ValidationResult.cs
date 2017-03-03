using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardborn.DataMonitoring.RuntimeCore
{
    public class ValidationResult
    {
        // Fields
        private BindingInformation bindingInfo;
        private Stream outputXmlReader;

        // Methods
        public ValidationResult(BindingInformation info, Stream reader)
        {
            this.bindingInfo = info;
            this.outputXmlReader = reader;
        }

        // Properties
        public BindingInformation BindingInfo
        {
            get
            {
                return this.bindingInfo;
            }
        }

        public Stream OutputXmlReader
        {
            get
            {
                return this.outputXmlReader;
            }
        }
    }


}
