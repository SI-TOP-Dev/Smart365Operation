using Abt.Controls.SciChart.ChartModifiers;
using Abt.Controls.SciChart.Visuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart365Operation.Modules.DataAnalysis.ChartExtensions
{
    public class CustomZoomExtentsModifier : ChartModifierBase
    {
        public override void OnModifierDoubleClick(ModifierMouseArgs e)
        {
            base.OnModifierDoubleClick(e);

            ParentSurface.ZoomExtentsToVisibleRangeLimit();
        }
    }

    public static class SciChartSurfaceExtensions
    {
        public static void ZoomExtentsToVisibleRangeLimit(this ISciChartSurface scs)
        {
            using (scs.SuspendUpdates())
            {
                foreach (var axis in scs.YAxes)
                {
                    axis.VisibleRange = axis.VisibleRangeLimit ?? axis.GetMaximumRange();
                    axis.VisibleRange.GrowBy(0.4, 0.4);
                }

                foreach (var axis in scs.XAxes)
                {
                    var range = axis.VisibleRangeLimit ?? axis.GetMaximumRange();
                    axis.AnimateVisibleRangeTo(range, TimeSpan.FromMilliseconds(500));
                }
            }
        }
    }
}
