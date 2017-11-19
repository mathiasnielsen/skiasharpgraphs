using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace SkiaSharpSimpleCharts.Client.Core
{
    public class SkiaSharpSimpleChartsViewModel : ViewModelBase
    {
        private const string Api = "http://webapplication220171118090730.azurewebsites.net/getlightning";

        private readonly HttpRequestExecutor httpRequestExecutor;

        private List<BarData> chartData;

        public SkiaSharpSimpleChartsViewModel()
        {
            httpRequestExecutor = new HttpRequestExecutor();

            RefreshCommand = new RelayCommand(Refresh);

            ChartData = GetDummyData();
        }

        public RelayCommand RefreshCommand { get; }

        public List<BarData> ChartData
        {
            get { return chartData; }
            set { Set(ref chartData, value); }
        }

        private async void Refresh()
        {
            await GetDataFromApiAsync();
        }

        private async Task GetDataFromApiAsync()
        {
            var data = await httpRequestExecutor.Get<List<Lightning>>(Api);

            var barData = ConvertDataToBarData(data);

            ChartData = barData;
        }

        private List<BarData> ConvertDataToBarData(List<Lightning> data)
        {
            var firstLightningDate = data.Min(lightning => lightning.TimeStamp);
            var lastLightningDate = data.Max(lightning => lightning.TimeStamp);
            var deltaLightingDate = lastLightningDate - firstLightningDate;

            var barData = new List<BarData>();
            var hours = deltaLightingDate.Hours;
            for (int intervalIndex = 0; intervalIndex < hours; intervalIndex++)
            {
                var interval = 1;
                var startLight = firstLightningDate.AddHours(intervalIndex * interval);
                var endLight = firstLightningDate.AddHours((intervalIndex + 1) * interval);
                var lights = data.Where(x => x.TimeStamp > startLight && x.TimeStamp < endLight).ToList();
                barData.Add(new BarData() { Title = startLight.Hour + ":" + endLight.Hour, Value = lights.Count() });
            }

            return barData;
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
