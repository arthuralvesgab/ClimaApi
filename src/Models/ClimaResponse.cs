using ClimaApi.Models;


namespace ClimaApi.Models
{
    public class ClimaResponse // classe da resposta da API Clima
    {
        public CurrentWeather? Currentweather { get; set;}
    }
    public class CurrentWeather // classe que representa o clima atual
    {
        public double Temperature { get; set;}
        public double Windspeed { get; set;}
    }
}