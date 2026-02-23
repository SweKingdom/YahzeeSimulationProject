using Seido.Utilities.SeedGenerator;

namespace Models.OpenWeather;

public record ForecastItem(
    DateTime DateTime,
    double Temperature,
    double WindSpeed,
    string Description,
    string Icon,
    bool Seeded = false
) : ISeed<ForecastItem>
{
    public ForecastItem() : this(default, default, default, default, default) { }

    #region randomly seed this instance
    public virtual ForecastItem Seed(SeedGenerator seedGenerator)
    {
        var iconCode = seedGenerator.FromString("01d, 02d, 03d, 04d, 09d, 10d, 11d, 13d, 50d");
        var ret = new ForecastItem(
            seedGenerator.DateAndTime(),
            seedGenerator.Next(-15, 35) + seedGenerator.NextDouble(),
            seedGenerator.NextDouble() * 20,
            seedGenerator.FromString("clear sky, few clouds, scattered clouds, broken clouds, light rain, moderate rain, snow, mist"),
            $"http://openweathermap.org/img/w/{iconCode}.png"
        );
        return ret;
    }
    #endregion
}

