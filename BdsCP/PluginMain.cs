using BdsCP.Command;
using BdsCP.Util;
using LLNET.Core;
using LLNET.DynamicCommand;

[assembly: LibPath("plugins\\BdsCP\\libs")]

namespace BdsCP;

[PluginMain("BdsCP")]
public class PluginMain : IPluginInitializer
{
    public void OnInitialize()
    {
#pragma warning disable CS4014
        Configuration.InitAsync().Wait();
        Data.InitAsync();
#pragma warning restore CS4014
        DynamicCommand.RegisterCommand<CoupleCommand>();
    }

    public Dictionary<string, string> MetaData => new()
    {
        { "license", "PTSoft copyright (c) 2022" }
    };

    public string Introduction => "CP插件";

    public Version Version => new (0, 1, 1);

    public static readonly EconomySystem EconomySystem = new();
}