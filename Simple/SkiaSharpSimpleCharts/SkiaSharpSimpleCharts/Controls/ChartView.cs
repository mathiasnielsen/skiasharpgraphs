using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using SkiaSharpSimpleCharts.Client.Core;
using Xamarin.Forms;

namespace SkiaSharpSimpleCharts.Controls
{
    public class ChartView : SKCanvasView
    {
        private const float AnimationInterval = 0.01f;

        private static float animationProgress = 1.0f;
        private static float easingAnimationProgress = 1.0f;
        private static bool reactedOnNewData;
        private static int shownBars;
        private int oldShownBars;

        private SKPaint barPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Blue
        };

        private SKPaint strokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 2,
            Color = SKColors.Black
        };

        private SKPaint textPaint = new SKPaint
        {
            TextSize = 30.0f,
            Color = SKColors.Black,
            TextAlign = SKTextAlign.Center
        };

        public ChartView()
        {
            PaintSurface += OnPaintCanvas;
        }

        public event EventHandler GotData;

        public event EventHandler ShowsABar;

        public static readonly BindableProperty BarDataProperty = BindableProperty.Create(nameof(BarData), typeof(List<BarData>), typeof(List<BarData>), null, propertyChanged: OnChartChanged);

        public List<BarData> BarData
        {
            get { return (List<BarData>)GetValue(BarDataProperty); }
            set { SetValue(BarDataProperty, value); }
        }

        private static void OnChartChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chartView = ((ChartView)bindable);
            StartAnimation(chartView);
        }

        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var canvas = surface.Canvas;

            canvas.Clear(SKColors.Orange);

            if (BarData != null)
            {
                //AnimateBars(canvas, e.Info);
                AnimateLightningBars(canvas, e.Info);
            }
        }

        private static void StartAnimation(ChartView charView)
        {
            easingAnimationProgress = 0.0f;
            animationProgress = 0.0f;
            reactedOnNewData = false;
            shownBars = 0;

            // Runs 60 times pr. second.
            Device.StartTimer(TimeSpan.FromSeconds(1.0f / 60), () =>
            {
                easingAnimationProgress = (float)Easing.CubicIn.Ease(animationProgress);

                var currentShownIndex = (double)charView.BarData.Count * animationProgress;
                shownBars = (int)currentShownIndex;

                if (animationProgress >= 1.0f)
                {
                    animationProgress = 1.0f;
                    easingAnimationProgress = 1.0f;
                    charView.InvalidateSurface();
                    return false;
                }

                // Redraw the chart
                charView.InvalidateSurface();
                animationProgress += AnimationInterval;

                return true;
            });
        }

        private void AnimateLightningBars(SKCanvas canvas, SKImageInfo info)
        {
            if (reactedOnNewData == false)
            {
                ShowsABar?.Invoke(this, null);
                oldShownBars = shownBars;
                reactedOnNewData = true;
            }
            else
            {
                if (oldShownBars != shownBars && shownBars < BarData?.Count)
                {
                    ShowsABar?.Invoke(this, null);
                    oldShownBars = shownBars;
                }   
            }

            // Applying decorations )
            var margin = 40;
            var textHeight = 60;

            var chart = new Chart() { Entries = BarData, ChartColor = SKColors.Blue };

            var heighestBarValue = chart.Entries.Max(bar => bar.Value);

            // Remove the left margin
            var canvasWidth = info.Width - margin;
            var canvasHeight = info.Height - margin * 2;
            var barContentHeight = canvasHeight - textHeight;

            // Takes spacing into account for bar width
            var barWidth = (canvasWidth / chart.Entries.Count) - margin;

            for (int index = 0; index < chart.Entries.Count; index++)
            {
                if (index > shownBars)
                {
                    return;
                }

                var currentBar = chart.Entries[index];
                var procentageHeight = (float)currentBar.Value / heighestBarValue;

                // Takes margin into height account
                var barHeight = (int)(procentageHeight * barContentHeight); // * easingAnimationProgress;

                // Find start X
                var startX = margin + (margin * index) + index * barWidth;
                var startY = info.Height - margin - textHeight;

                canvas.DrawRect(SKRect.Create(startX, startY, barWidth, -barHeight), barPaint);
                canvas.DrawRect(SKRect.Create(startX, startY, barWidth, -barHeight), strokePaint);

                var startTextX = startX + (barWidth / 2);
                var startTextY = startY + textHeight;

                textPaint.Color = SKColors.Black.WithAlpha((byte)(byte.MaxValue * easingAnimationProgress));

                canvas.DrawText(currentBar.Title, startTextX, startTextY, textPaint);
            }
        }

        private void AnimateBars(SKCanvas canvas, SKImageInfo info)
        {
            if (reactedOnNewData == false)
            {
                GotData?.Invoke(null, null);
                reactedOnNewData = true;
            }

            // Applying decorations 
            var margin = 40;
            var textHeight = 60;

            var chart = new Chart() { Entries = BarData, ChartColor = SKColors.Blue };

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
                var barHeight = (int)(procentageHeight * barContentHeight) * easingAnimationProgress;

                // Find start X
                var startX = margin + (margin * index) + index * barWidth;
                var startY = info.Height - margin - textHeight;

                canvas.DrawRect(SKRect.Create(startX, startY, barWidth, -barHeight), barPaint);
                canvas.DrawRect(SKRect.Create(startX, startY, barWidth, -barHeight), strokePaint);

                var startTextX = startX + (barWidth / 2);
                var startTextY = startY + textHeight;

                textPaint.Color = SKColors.Black.WithAlpha((byte)(byte.MaxValue * easingAnimationProgress));

                canvas.DrawText(currentBar.Title, startTextX, startTextY, textPaint);
            }
        }
    }
}
