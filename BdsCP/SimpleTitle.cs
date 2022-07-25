using BdsCP.Util;
using LLNET.Event;
using LLNET.Hook;
using LLNET.Logger;
using MC;

namespace BdsCP;

public static class SimpleTitle
{
    private static readonly Configuration Configuration = Configuration.Config;

    public static void Init()
    {
        var logger = new Logger("Chat");
        PlayerChatEvent.Event += ev =>
        {
            var text = ev.Message;
            if (text == null)
                return true;

            Level.BroadcastText(Format(text, ev.Player), TextType.RAW);
            logger.info.WriteLine($"{ev.Player.RealName} -> {text}");
            return false;
        };
    }

    private static int GetDim(Player player)
    {
        return HookAPI.SymCall<int, IntPtr>(
            "?getDimensionId@Actor@@UEBA?AV?$AutomaticID@VDimension@@H@@XZ", player.Intptr);
    }

    private static string Format(string text, Player player)
    {
        return Configuration.TitleTemplate
            .ReplaceVariable("cp", player, pl =>
            {
                var couple = Data.GetCoupleByPlayer(pl);
                return couple == null ? "单身" : couple.Name;
            })
            .ReplaceVariable("name", player, pl => pl.Name)
            .ReplaceVariable("msg", player, _ => text)
            .ReplaceVariable("platform", player, pl => pl.DeviceTypeName)
            .ReplaceVariable("time", player, _ => DateTime.Now.ToString("MM/dd - HH:mm"))
            .ReplaceVariable("ping", player, pl => pl.AvgPing.ToString())
            .ReplaceVariable("dim", player, pl => Configuration.DimensionName[GetDim(pl)]);
    }
}

static class Text
{
    public delegate string Replacer(Player player);

    public static string ReplaceVariable(this string ori, string varName, Player player, Replacer replacer)
    {
        var var = "${" + varName + "}";
        return ori.Contains(var) ? ori.Replace(var, replacer(player)) : ori;
    }
}