using System.Collections.Immutable;
using Seido.Utilities.SeedGenerator;

namespace Models.OpenWeather;

public record Forecast (
    string City, 
    ImmutableList<ForecastItem> Items,
    bool Seeded = false
) : ISeed<Forecast>
{
    public Forecast() : this(default, default) {}

    #region randomly seed this instance
    public virtual Forecast Seed(SeedGenerator seedGenerator)
    {
        var ret = new Forecast(
            seedGenerator.City(),
            seedGenerator.ItemsToList<ForecastItem>(seedGenerator.Next(5, 15)).OrderBy(f => f.DateTime).ToImmutableList()
        );
        return ret;
    }
    #endregion
}