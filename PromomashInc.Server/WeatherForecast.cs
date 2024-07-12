namespace PromomashInc.Server
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }

    public class CountryDto
    {

        public string Code { get; set; }

        public string DisplayText { get; set; }
    }

    public class ProvinceDto
    {

        public string Code { get; set; }
        public string ParentCode { get; set; }

        public string DisplayText { get; set; }
    }

}
