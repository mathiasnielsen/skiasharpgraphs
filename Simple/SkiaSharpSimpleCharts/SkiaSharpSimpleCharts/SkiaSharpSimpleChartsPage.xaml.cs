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
            BindingContext = viewModel;

            bindableBarChart.ShowsABar += (sender, e) => InitiateQuickFlash();
        }

        private async void InitiateQuickFlash()
        {
            FlashView.IsVisible = true;
            FlashView.IsEnabled = false;
            FlashView.Opacity = 1;

            await FlashView.FadeTo(0, 400, null);

            Device.BeginInvokeOnMainThread(() => FlashView.IsVisible = false);
        }
    }
}