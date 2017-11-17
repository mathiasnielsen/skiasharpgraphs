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

        SKPaint barPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Blue
        };

        SKPaint strokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 2,
            Color = SKColors.Black
        };

        SKPaint textPaint = new SKPaint
        {
            TextSize = 30.0f,
            Color = SKColors.Black,
            TextAlign = SKTextAlign.Center
        };

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

            // Step 3 - Decorate bar chart
            canvas.Clear(SKColors.Orange);
            DrawDecoratedBars(canvas, args.Info);
        }

        private void DrawDecoratedBars(SKCanvas canvas, SKImageInfo info)
        {
            // Applying decorations 
            var margin = 40;
            var textHeight = 60;

            var data = viewModel.GetDummyData();
            var chart = new Chart() { Entries = data, ChartColor = SKColors.Blue };

            var heighestBarValue = chart.Entries.Max(bar => bar.Value);

            // Remove the left margin
            var canvasWidth = info.Width - margin;
            var canvasHeight = info.Height - margin * 2;
            var barContentHeight = canvasHeight - textHeight;

            // Takes spacing into account for bar width
            var barWidth = (canvasWidth / chart.Entries.Count) - margin;

            for (int index = 0; index < chart.Entries.Count; index++)
            {
                var currentBar = chart.Entries[index];
                var procentageHeight = (float)currentBar.Value / heighestBarValue;

                // Takes margin into height account
                var barHeight = (int)(procentageHeight * barContentHeight);

                // Find start X
                var startX = margin + (margin * index) + index * barWidth;
                var startY = info.Height - margin - textHeight;

                canvas.DrawRect(SKRect.Create(startX, startY, barWidth, -barHeight), barPaint);
                canvas.DrawRect(SKRect.Create(startX, startY, barWidth, -barHeight), strokePaint);

                var startTextX = startX + (barWidth / 2);
                var startTextY = startY + textHeight;
                canvas.DrawText(currentBar.Title, startTextX, startTextY, textPaint);
            }
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
                var procentageHeight = (float)currentBar.Value / heighestBarValue;
                var barHeight = (int)(procentageHeight * info.Height);
                var startX = barWidth * index;

                canvas.DrawRect(SKRect.Create(startX, info.Height, barWidth, -barHeight), barPaint);
                canvas.DrawRect(SKRect.Create(startX, info.Height, barWidth, -barHeight), strokePaint);
            }
        }

        private void OnRefreshClicked(object sender, EventArgs args)
        {
            // Redraw the chart
            barChart.InvalidateSurface();
        }
    }
}