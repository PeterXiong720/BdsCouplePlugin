using Newtonsoft.Json;

namespace BdsCP.Util;

public class Configuration
{
    /// <summary>
    /// 数据文件位置
    /// </summary>
    [JsonProperty("data")]
    public string Data { get; set; } = "plugins/BdsCp/data.json";

    /// <summary>
    /// 是否开启内置称号系统
    /// </summary>
    [JsonProperty("simpleTitle")]
    public bool SimpleTitle { get; set; } = true;

    /// <summary>
    /// 登记结婚花费
    /// </summary>
    [JsonProperty("cost")]
    public decimal Cost { get; set; } = decimal.Zero;
    
    /// <summary>
    /// 内置称号系统称号模板
    /// </summary>
    [JsonProperty("titleTemplate")]
    public string TitleTemplate { get; set; } = "[${platform}][${time}][${cp}]<${name}> ~$ ${msg}";

    /// <summary>
    /// 维度名称
    /// </summary>
    [JsonProperty("dimensionName")]
    public List<string> DimensionName { get; set; } = new() { "主世界", "下界", "末地", };
    
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