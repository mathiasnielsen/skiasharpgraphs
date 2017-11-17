using System;
using System.Linq;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using SkiaSharpSimpleCharts.Client.Core;
using Xamarin.Forms;

namespace SkiaSharpSimpleCharts
{
    public partial class SkiaSharpSimpleChartsPage : ContentPage
    {
        private SkiaSharpSimpleChartsViewModel viewModel;

        public SkiaSharpSimpleChartsPage()
        {
            InitializeComponent();

            viewModel = new SkiaSharpSimpleChartsViewModel();
        }

        /// <summary>
        /// Called when SKCanvas is drawn/invalidated
        /// </summary>
        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            // Step 1 - Simple background
            var surface = args.Surface;
            var canvas = surface.Canvas;

            canvas.Clear(SKColors.Orange);

            // Step 2 - Draw a coordates
            DrawBars(canvas, args.Info);
        }

        private void OnRefreshClicked(object sender, EventArgs args)
        {
            // Redraw the chart
            barChart.InvalidateSurface();
        }

        private void DrawBars(SKCanvas canvas, SKImageInfo info)
        {
            var data = viewModel.GetDummyData();
            var chart = new Chart() { Entries = data, ChartColor = SKColors.Blue };

            var heighestBarValue = chart.Entries.Max(bar => bar.Value);
            var barWidth = info.Width / chart.Entries.Count;

            for (int index = 0; index < chart.Entries.Count; index++)
            {
                var currentBar = chart.Entries[index];

                using (var barPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = chart.ChartColor
                })
                using (var strokePaint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = 2,
                    Color = SKColors.Black
                })
                {
                    var procentageHeight = (float)currentBar.Value / heighestBarValue;
                    var barHeight = (int)(procentageHeight * info.Height);
                    var startX = barWidth * index;

                    canvas.DrawRect(SKRect.Create(startX, info.Height, barWidth, -barHeight), barPaint);
                    canvas.DrawRect(SKRect.Create(startX, info.Height, barWidth, -barHeight), strokePaint);
                }
            }
        }
    }
}
