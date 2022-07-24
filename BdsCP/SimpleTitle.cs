using BdsCP.Util;
using LLNET.Event;
using MC;

namespace BdsCP;

public class SimpleTitle
{
    private static readonly Configuration Configuration = Configuration.Config;
    
    public SimpleTitle()
    {
        PlayerChatEvent.Event += ev =>
        {
            var text = ev.Message;
            if ( text == null)
                return true;
            
            Level.BroadcastText(Format(text, ev.Player), TextType.RAW);
            return false;
        };
    }

    private static string Format(string text, Player player)
    {
        return Configuration.TitleTemplate
            .ReplaceVariable("cp", player, pl =>
            {
                var couple = Data.GetCoupleByPlayer(pl);
                return couple == null ? string.Empty : couple.Name;
            })
            .ReplaceVariable("name", player, pl=>pl.Name)
            .ReplaceVariable("msg", player, _ => text)
            .ReplaceVariable("platform", player, pl => pl.DeviceTypeName)
            .ReplaceVariable("time", player, _ => DateTime.Now.ToShortTimeString());
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