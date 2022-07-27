using Newtonsoft.Json;

namespace PTSoft.FantasyCouple.Util;

public class Configuration
{
    /// <summary>
    /// 数据文件位置
    /// </summary>
    [JsonProperty("data")]
    public string Data { get; set; } = "plugins/FantasyCouple/data.json";

    /// <summary>
    /// 是否开启内置称号系统
    /// </summary>
    [JsonProperty("simpleTitle")]
    public bool SimpleTitle { get; set; } = true;

    /// <summary>
    /// 登记结婚花费
    /// </summary>
    [JsonProperty("cost")]
    public long Cost { get; set; }
    
    /// <summary>
    /// 内置称号系统称号模板
    /// </summary>
    [JsonProperty("titleTemplate")]
    public string TitleTemplate { get; set; } = "§2[${platform}]§e[${time}]§c[${cp}]§3<${name}>§7~$§r ${msg}";

    /// <summary>
    /// 修改CP称号花费
    /// </summary>
    [JsonProperty("changeTitleCost")]
    public long ChangeTitleCost { get; set; }

    /// <summary>
    /// 修改带颜色代码的CP称号花费
    /// </summary>
    [JsonProperty("colorTitleCost")]
    public long ColorTitleCost { get; set; }

    /// <summary>
    /// 维度名称
    /// </summary>
    [JsonProperty("dimensionName")]
    public List<string> DimensionName { get; set; } = new() { "主世界", "下界", "末地", };
    
    [JsonIgnore]
    public static Configuration Config { get; private set; } = null!;

    public static async Task InitAsync()
    {
        Directory.CreateDirectory("plugins/FantasyCouple");
        if (!File.Exists("plugins/FantasyCouple/configuration.json"))
        {
            Config = new Configuration();
            await SaveAsync();
        }
        else
        {
            using var sr = new StreamReader("plugins/FantasyCouple/configuration.json");
            var json = await sr.ReadToEndAsync();
            var config = JsonConvert.DeserializeObject<Configuration>(json);
            Config = config ?? new Configuration();
        }
    }

    public static async Task SaveAsync()
    {
        await using var sw = new StreamWriter("plugins/FantasyCouple/configuration.json");
        await sw.WriteAsync(JsonConvert.SerializeObject(Config, Formatting.Indented));
    }
}