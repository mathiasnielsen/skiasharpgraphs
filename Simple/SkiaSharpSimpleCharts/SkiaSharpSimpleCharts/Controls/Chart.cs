using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using SkiaSharpSimpleCharts.Client.Core;

namespace SkiaSharpSimpleCharts.Controls
{
    public class Chart : SKCanvasView
    {
        private List<BarData> data;

        public Chart()
        {
            data = new List<BarData>();
            PaintSurface += OnPaint;
        }

        public void InsertData(List<BarData> data)
        {
            this.data = data;
            InvalidateSurface();
        }

        private void OnPaint(object sender, SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var canvas = surface.Canvas;

            canvas.Clear(SKColors.Orange);

            for (int index = 0; index < data.Count; index++)
            {
                var currentData = data[index];

                var highestValue = data.Max(x => x.Value);
                var procentageHeight = (float)currentData.Value / (float)highestValue;
                var barHeight = e.Info.Height * procentageHeight;

                var barWidth = e.Info.Width / data.Count;

                var startXPos = barWidth * index;
                var startYPos = e.Info.Height;

                using (var barPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = SKColors.AliceBlue
                })
                using (var barstrokePaint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = SKColors.Black,
                    StrokeWidth = 2
                })
                {
                    canvas.DrawRect(SKRect.Create(startXPos, startYPos, barWidth, -barHeight), barPaint);
                    canvas.DrawRect(SKRect.Create(startXPos, startYPos, barWidth, -barHeight), barstrokePaint);   
                }
            }
        }
    }
}
