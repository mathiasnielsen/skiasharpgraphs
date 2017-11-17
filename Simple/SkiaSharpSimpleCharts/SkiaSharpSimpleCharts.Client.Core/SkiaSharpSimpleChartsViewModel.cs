using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace SkiaSharpSimpleCharts.Client.Core
{
    public class SkiaSharpSimpleChartsViewModel : ViewModelBase
    {
        private List<BarData> _chartData;

        public SkiaSharpSimpleChartsViewModel()
        {
            RefreshCommand = new RelayCommand(Refresh);

            ChartData = GetDummyData();
        }

        public RelayCommand RefreshCommand { get; }

        public List<BarData> ChartData
        {
            get { return _chartData; }
            set { Set(ref _chartData, value); }
        }

        private async void Refresh()
        {
            await GetDataFromApiAsync();
        }

        private Task GetDataFromApiAsync()
        {
            ChartData = GetSimulationData();

            return Task.FromResult(true);
        }

        private List<BarData> GetSimulationData()
        {
            return new List<BarData>()
            {
                new BarData() { Title = "SF", Value = 300 },
                new BarData() { Title = "A", Value = 800 },
                new BarData() { Title = "V", Value = 600 },
                new BarData() { Title = "R", Value = 1400 }
            };
        }

        private List<BarData> GetDummyData()
        {
            return new List<BarData>()
            {
                new BarData() { Title = "SF", Value = 100 },
                new BarData() { Title = "A", Value = 500 },
                new BarData() { Title = "V", Value = 400 }
            };
        }
    }
}
