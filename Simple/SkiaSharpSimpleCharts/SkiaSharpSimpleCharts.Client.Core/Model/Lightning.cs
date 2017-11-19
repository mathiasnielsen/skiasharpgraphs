using System;
using Newtonsoft.Json;

namespace SkiaSharpSimpleCharts.Client.Core
{
    public class Lightning
    {
        //        <Lightning>
        //<Latitude>56.75</Latitude>
        //<Longitude>9.83</Longitude>
        //<Power>0.8</Power>
        //<Timestamp>2017-11-19T02:31:13.467</Timestamp>
        //</Lightning>
        //<Lightning>

        [JsonProperty("Timestamp")]
        public DateTime TimeStamp { get; set; }
    }
}
