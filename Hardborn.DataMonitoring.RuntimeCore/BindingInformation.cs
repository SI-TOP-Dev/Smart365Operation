using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardborn.DataMonitoring.RuntimeCore
{
    public class BindingInformation : List<BindingTag>
    {
        // Methods
        public void RemoveElement(string elementName)
        {
            List<BindingTag> list = new List<BindingTag>();
            foreach (BindingTag tag in this)
            {
                if (tag.ElementName == elementName)
                {
                    list.Add(tag);
                }
            }
            foreach (BindingTag tag2 in list)
            {
                base.Remove(tag2);
            }
        }
    }



}
