using ABC.BLL.DataCenterService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.BLL.Helpers
{
    public class DataCenterServiceHelper
    {
        public static int CityCode { get; set; }

        static DataCenterServiceHelper()
        {
            CityCode = 440100;
        }

        public static List<CityDayData> GetCityDayDataList(DateTime startTime, DateTime endTime)
        {
            using (MeteorologyDataClient client = new MeteorologyDataClient())
            {
                return client.GetCityDayDataHistory(startTime, endTime).Where(o => o.CityCode == CityCode).ToList();
            }
        }

        public static List<City_WeatherForecastInfo> GetCity_WeatherForecastInfoList(DateTime startTime, DateTime endTime, int days)
        {
            using (MeteorologyDataClient client = new MeteorologyDataClient())
            {
                return client.GetDayForecastData(new string[] { "101280101" }, startTime, endTime).Where(o => o.ForTime == o.TimePoint.Value.AddDays(days)).ToList();
            }
        }
    }
}
