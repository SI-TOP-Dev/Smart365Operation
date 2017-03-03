using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardborn.DataMonitoring.RuntimeCore
{
    public interface ISetValue
    {
        // Methods
        void SetValue(object value);

        // Properties
        object Value { get; set; }
    }



}
