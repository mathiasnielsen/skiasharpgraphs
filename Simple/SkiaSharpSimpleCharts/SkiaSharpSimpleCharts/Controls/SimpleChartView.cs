using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using SkiaSharpSimpleCharts.Client.Core;

namespace SkiaSharpSimpleCharts.Controls
{
    public class SimpleChartView : SKCanvasView
    {
        private List<BarData> data;

        public SimpleChartView()
        {
            PaintSurface += OnDraw;
        }

        public void SupplyData(List<BarData> data)
        {
            this.data = data;
            InvalidateSurface();
        }

        private void OnDraw(object sender, SKPaintSurfaceEventArgs e)
        {
            // 1. Simple canvas
            var surface = e.Surface;
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.Orange);

            // 2. Simple Bars
            for (int index = 0; index < data.Count; index++)
            {
                // MetaData
                var height = e.Info.Height;
                var width = e.Info.Width;

                // Find bar height
                var currentBar = data[index];
                var heighestBarValue = data.Max(bar => bar.Value);
                var procentageHeight = (float)currentBar.Value / heighestBarValue;
                var barHeight = (int)(procentageHeight * height);

                // Find bar width
                var barWidth = width / data.Count;

                // Find startX
                var startX = index * barWidth;

                // Find startY
                var startY = height - 40;

                // Draw the bar
                using (var strokePaint = new SKPaint
                {
                    Color = SKColors.Black,
                    StrokeWidth = 2,
                    Style = SKPaintStyle.Stroke
                })
                using (var paint = new SKPaint
                {
                    Color = SKColors.Blue,
                    Style = SKPaintStyle.Fill
                })
                {
                    canvas.DrawRect(SKRect.Create(startX, startY, barWidth, -barHeight + 40), paint);
                    canvas.DrawRect(SKRect.Create(startX, startY, barWidth, -barHeight + 40), strokePaint);
                }

                // Draw text
                using (var textPaint = new SKPaint
                {
                    Color = SKColors.Black,
                    TextSize = 12,
                    TextAlign = SKTextAlign.Center
                })
                {
                    canvas.DrawText($"{currentBar.Title}: {currentBar.Value}", startX + barWidth / 2, height, textPaint);
                }
            }
        }
    }
}
