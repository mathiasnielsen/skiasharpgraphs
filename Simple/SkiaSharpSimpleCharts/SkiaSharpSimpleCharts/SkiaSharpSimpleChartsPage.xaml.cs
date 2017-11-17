using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace SkiaSharpSimpleCharts
{
    public partial class SkiaSharpSimpleChartsPage : ContentPage
    {
        public SkiaSharpSimpleChartsPage()
        {
            InitializeComponent();
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

            // Step 2 - Draw a coorda
        }
    }
}
