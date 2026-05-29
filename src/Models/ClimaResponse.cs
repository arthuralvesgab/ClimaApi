using ClimaApi.Models;


namespace ClimaApi.Models
{
    public class ClimaResponse
    {
        public CurretWeather? current_weather { get; set;}
    }
    public class CurretWeather
    {
        public double temperature { get; set;}
        public double windspeed { get; set;}
    }
}