using SkiaSharp;
using SkiaSharpSimpleCharts.Client.Core;
using Xamarin.Forms;

namespace SkiaSharpSimpleCharts.Features
{
    public partial class SkiaSharpSimpleChartsPage : ContentPage
    {
        private SkiaSharpSimpleChartsViewModel viewModel;

        public SkiaSharpSimpleChartsPage()
        {
            InitializeComponent();

            viewModel = new SkiaSharpSimpleChartsViewModel();
            BindingContext = viewModel;

            // STEP 1
            chart1.SupplyData(viewModel.BarDatas);

            // STEP 2
            //chart2.ShowsABar += (sender, e) => InitiateQuickFlash();
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