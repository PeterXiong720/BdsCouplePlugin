using Newtonsoft.Json;

namespace BdsCP.Util;

public class Configuration
{
    [JsonProperty("data")]
    public string Data { get; set; } = "plugins/BdsCp/data.json";

    [JsonProperty("simpleTitle")]
    public bool SimpleTitle { get; set; } = true;

    [JsonProperty("cost")]
    public decimal Cost { get; set; } = decimal.Zero;
    
    [JsonProperty("titleTemplate")]
    public string TitleTemplate { get; set; } = "[${platform}][${time}][${cp}]<${name}>${msg}";
    
    [JsonIgnore]
    public static Configuration Config { get; private set; } = null!;

    public static async Task InitAsync()
    {
        Directory.CreateDirectory("plugins/BdsCp");
        Configuration? config;
        if (!File.Exists("plugins/BdsCp/configuration.json"))
        {
            config = new Configuration();
            await SaveAsync();
        }
        else
        {
            using var sr = new StreamReader("plugins/BdsCp/configuration.json");
            var json = await sr.ReadToEndAsync();
            config = JsonConvert.DeserializeObject<Configuration>(json);
        }

        Config = config ?? new Configuration();
    }

    public static async Task SaveAsync()
    {
        await using var sw = new StreamWriter("plugins/BdsCp/configuration.json");
        await sw.WriteAsync(JsonConvert.SerializeObject(Config));
    }
}