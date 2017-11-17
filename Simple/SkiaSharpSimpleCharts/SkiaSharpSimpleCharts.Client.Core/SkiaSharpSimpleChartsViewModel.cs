using System;
using System.Collections.Generic;

namespace SkiaSharpSimpleCharts.Client.Core
{
    public class SkiaSharpSimpleChartsViewModel
    {
        public SkiaSharpSimpleChartsViewModel()
        {
        }

        public List<BarData> GetDummyData()
        {
            return new List<BarData>()
            {
                new BarData() { Title = "SF", Value = 100 },
                new BarData() { Title = "A", Value = 500 },
                new BarData() { Title = "V", Value = 400 }
            };
        }
    }
}
