using Newtonsoft.Json;

namespace BdsCP.Util;

// ReSharper disable once InconsistentNaming
public class UITexts
{
    [JsonProperty("mainFormTitle")]
    public string MainFormTitle { get; set; } = "CP管理";

    [JsonProperty("mainFormContent")]
    public string MainFormContent { get; set; } = "";
}