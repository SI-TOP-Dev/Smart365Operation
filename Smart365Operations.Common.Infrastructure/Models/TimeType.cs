using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smart365Operations.Common.Infrastructure.Converters;

namespace Smart365Operations.Common.Infrastructure.Models
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum TimeType
    {
        [Description("日")]
        Day = 0,
        [Description("月")]
        Month,
        [Description("年")]
        Year
    }
}
