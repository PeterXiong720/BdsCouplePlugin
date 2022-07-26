using LLNET.Core;
using LLNET.DynamicCommand;
using LLNET.Event;
using PTSoft.FantasyCouple.Command;
using PTSoft.FantasyCouple.Util;

[assembly: LibPath("plugins\\BdsCP\\libs")]

namespace PTSoft.FantasyCouple;

[PluginMain("BdsCP")]
public class PluginMain : IPluginInitializer
{
    public void OnInitialize()
    {
#pragma warning disable CS4014
        Configuration.InitAsync().Wait();
        Data.InitAsync();
#pragma warning restore CS4014
        if (Configuration.Config.SimpleTitle)
        {
            SimpleTitle.Init();
        }

        DynamicCommand.RegisterCommand<CoupleCommand>();
        ServerStartedEvent.Event += ev =>
        {
            //EconomySystem = new EconomySystem();
            
            Console.WriteLine("========================================");
            Console.WriteLine("[定制插件][BdsCp] 已加载");
            Console.WriteLine("作者：PeterXiong720");
            Console.WriteLine("PTSoft copyright (c) 2022.");
            Console.WriteLine("========================================");
            return true;
        };
    }

    public Dictionary<string, string> MetaData => new()
    {
        { "license", "PTSoft copyright (c) 2022" }
    };

    public string Introduction => "CP插件";

    public Version Version => new(0, 1, 1);

    //public static EconomySystem EconomySystem { get; private set; } = null!;
}