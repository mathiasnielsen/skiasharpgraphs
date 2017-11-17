using System;
using System.Collections.Generic;
using SkiaSharp;
using SkiaSharpSimpleCharts.Client.Core;

namespace SkiaSharpSimpleCharts
{
    public class Chart
    {
        public List<BarData> Entries { get; set; }

        public SKColor ChartColor { get; set; }
    }
}
