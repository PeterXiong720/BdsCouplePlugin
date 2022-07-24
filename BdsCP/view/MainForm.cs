using BdsCP.Model;
using BdsCP.Util;
using LLNET.Form;
using MC;

namespace BdsCP.View;

public class MainForm
{
    private static readonly Configuration Configuration = Configuration.Config;
    
    private readonly SimpleForm _form;

    private readonly Couple? _couple;

    public MainForm(Player player)
    {
        _couple = Data.GetCoupleByPlayer(player);
        string content;
        if (_couple == null)
        {
            content = "当前婚姻状态：未婚";
        }
        else
        {
            var lover = _couple.Husband == player.Xuid ? _couple.Wife : _couple.Husband;
            var loverPlayer = Level.GetAllPlayers().FirstOrDefault(pl => pl.Xuid == lover);
            content = $"CP编号：{_couple.Name}\nCP Id：{_couple.Id}\n当前CP：{loverPlayer?.Name}";
        }
        _form = new SimpleForm("CP管理", content);
    }
}