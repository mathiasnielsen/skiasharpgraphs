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
        private const float WidthMargin = 20;
        private const float HeightMargin = 60;
        private const float BarTitleTextHeight = 40;

        private static float animationInterval = 0.01f;
        private static float animationProgress = 1.0f;
        private static float easingAnimationProgress = 1.0f;
        private static bool reactedOnNewData;
        private static int currentShownBarIndex;

        private AnimationTypes animationType;
        private SKColor backgroundColor;
        private int oldShownBars;

        private readonly SKPaint BarPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.Yellow
        };

        private readonly SKPaint BarStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 4,
            Color = SKColors.Black
        };

        private readonly SKPaint TitleTextPaint = new SKPaint
        {
            TextSize = 20.0f,
            Color = SKColors.Black,
            TextAlign = SKTextAlign.Center
        };

        private readonly SKPaint ValueTextPaint = new SKPaint
        {
            TextSize = 16.0f,
            Color = SKColors.Black,
            TextAlign = SKTextAlign.Center
        };

        public ChartView()
        {
            backgroundColor = SKColors.LightGray;
            animationType = AnimationTypes.Lightning;

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

        private static void StartAnimation(ChartView charView)
        {
            easingAnimationProgress = 0.0f;
            animationProgress = 0.0f;
            reactedOnNewData = false;
            currentShownBarIndex = 0;

            // Runs 60 times pr. second.
            Device.StartTimer(TimeSpan.FromSeconds(1.0f / 60), () =>
            {
                easingAnimationProgress = (float)Easing.CubicIn.Ease(animationProgress);
                currentShownBarIndex = (int)(charView.BarData.Count * animationProgress);

                if (animationProgress >= 1.0f)
                {
                    animationProgress = 1.0f;
                    easingAnimationProgress = 1.0f;
                    charView.InvalidateSurface();

                    return false;
                }

                charView.InvalidateSurface();
                animationProgress += animationInterval;

                return true;
            });
        }

        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var canvas = surface.Canvas;

            canvas.Clear(backgroundColor);

            if (BarData != null)
            {
                AnimateLightningBars(canvas, e.Info);
            }
        }

        private void AnimateLightningBars(SKCanvas canvas, SKImageInfo info)
        {
            InvokeEventsIfNeeded();

            if (BarData == null)
            {
                return;
            }

            if (BarData.Any() == false)
            {
                return;
            }

            var heighestBarValue = BarData.Max(bar => bar.Value);

            var canvasWidth = info.Width - WidthMargin;
            var barWidth = (canvasWidth / BarData.Count) - WidthMargin;

            var canvasHeight = info.Height - HeightMargin * 2;
            var barContentHeight = canvasHeight - BarTitleTextHeight - BarTitleTextHeight;

            for (int index = 0; index < BarData.Count; index++)
            {
                if (animationType == AnimationTypes.Lightning)
                {
                    // Only show bars that need to be shown.
                    if (index > currentShownBarIndex)
                    {
                        return;
                    }
                }

                var currentBar = BarData[index];
                var procentageHeight = (float)currentBar.Value / heighestBarValue;

                int barHeight = 0;
                switch (animationType)
                {
                    case AnimationTypes.Lightning:
                        barHeight = (int)(procentageHeight * barContentHeight);
                        break;

                    case AnimationTypes.SlideUp:
                        barHeight = (int)(procentageHeight * barContentHeight * easingAnimationProgress);
                        break;
                }

                // Find start X
                var startX = WidthMargin + (WidthMargin * index) + index * barWidth;
                var startY = info.Height - HeightMargin - BarTitleTextHeight;

                ApplyGradientShader(startY, barHeight);

                canvas.DrawRect(SKRect.Create(startX, startY, barWidth, -barHeight), BarPaint);
                canvas.DrawRect(SKRect.Create(startX, startY, barWidth, -barHeight), BarStrokePaint);

                var startTextX = startX + (barWidth / 2);
                var startTextY = startY + BarTitleTextHeight;

                TitleTextPaint.Color = SKColors.Black.WithAlpha((byte)(byte.MaxValue * easingAnimationProgress));

                canvas.DrawText(currentBar.Value.ToString(), startTextX, startY - barHeight - 20, ValueTextPaint);
                canvas.DrawText(currentBar.Title, startTextX, startTextY, TitleTextPaint);
            }
        }

        private void InvokeEventsIfNeeded()
        {
            switch (animationType)
            {
                case AnimationTypes.Lightning:
                    if (reactedOnNewData == false)
                    {
                        ShowsABar?.Invoke(this, null);
                        oldShownBars = currentShownBarIndex;
                        reactedOnNewData = true;
                    }
                    else
                    {
                        var amountOfBarChanged = oldShownBars != currentShownBarIndex;
                        var shouldInvokeShowsABar = amountOfBarChanged && currentShownBarIndex < BarData?.Count;
                        if (shouldInvokeShowsABar)
                        {
                            ShowsABar?.Invoke(this, null);
                            oldShownBars = currentShownBarIndex;
                        }
                    }
                    break;

                case AnimationTypes.SlideUp:
                    if (reactedOnNewData == false)
                    {
                        GotData?.Invoke(this, null);
                        oldShownBars = currentShownBarIndex;
                        reactedOnNewData = true;
                    }
                    break;
            }
        }

        private void ApplyGradientShader(float barY, float barHeight)
        {
            using (var shader = SKShader.CreateLinearGradient(
                new SKPoint(0, barY),
                new SKPoint(0, -barHeight),
                new[] { BarPaint.Color.WithAlpha(255 / 5), BarPaint.Color.WithAlpha(255) },
                null,
                SKShaderTileMode.Clamp))
            {
                BarPaint.Shader = shader;
            }
        }
    }
}