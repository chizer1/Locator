using Newtonsoft.Json;

namespace Locator.Models;

public class UserLog
{
    [JsonProperty("date")]
    public DateTime Date { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("ip")]
    public string IP { get; set; }

    [JsonProperty("user_agent")]
    public string UserAgent { get; set; }

    [JsonProperty("isMobile")]
    public bool IsMobile { get; set; }

    [JsonProperty("location_info")]
    public UserLogLocationInfo UserLogLocationInfo { get; set; }
}

public class UserLogLocationInfo
{
    [JsonProperty("country_code")]
    public string CountryCode { get; set; }

    [JsonProperty("country_name")]
    public string CountryName { get; set; }

    [JsonProperty("city_name")]
    public string CityName { get; set; }

    [JsonProperty("latitude")]
    public double Latitude { get; set; }

    [JsonProperty("longitude")]
    public double Longitude { get; set; }

    [JsonProperty("time_zone")]
    public string TimeZone { get; set; }
}
