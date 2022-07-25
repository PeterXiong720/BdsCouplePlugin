using BdsCP.Util;
using LLNET.Form;
using MC;

namespace BdsCP.View;

public class MarryForm
{
    private static readonly Configuration Configuration = Configuration.Config;

    private readonly Player _player;

    private readonly CustomForm _form;

    private void AddControls()
    {
        var players =
            from pl in Level.GetAllPlayers()
            where pl.Xuid != _player.Xuid
            select pl.RealName;
        _form.Append(new Label("tips", "提交申请后需等待对方同意"))
            .Append(new Dropdown("lover", "请选择您的对象", new List<string>(players)))
            .Append(new Input("message", "说两句呗", "Honey 你把爱放进我的心里"));
        _form.Callback = OnSubmit;
    }

    private void OnSubmit(Player player, Dictionary<string, CustomFormElement> data)
    {
        var lover = data["lover"].Value;
        var message = data["message"].Value ?? string.Empty;
        if (lover == null)
            return;
        var loverPlayer = (from pl in Level.GetAllPlayers()
            where pl.Xuid != player.Xuid
            select pl).FirstOrDefault(pl => pl.Xuid == lover);
        loverPlayer?.SendModalForm(
            "求婚申请",
            $"{message} -- {player.Name}",
            "同意",
            "拒绝",
            result =>
            {
                if (!result)
                    return;
                if (PluginMain.EconomySystem.GetMoney(player.Xuid) < Configuration.Cost ||
                    PluginMain.EconomySystem.GetMoney(lover) < Configuration.Cost)
                {
                    player.SendModalForm(
                        "§c错误", 
                        "您或对方余额不足，无法完成登记。如有任何异议请向管理员反馈", 
                        "确定", "反馈", _ => { });
                    loverPlayer.SendModalForm(
                        "§c错误", 
                        "您或对方余额不足，无法完成登记。如有任何异议请向管理员反馈", 
                        "确定", "反馈", _ => { });
                    return;
                }

                PluginMain.EconomySystem.ReduceMoney(player.Xuid, Configuration.Cost);
                PluginMain.EconomySystem.ReduceMoney(lover, Configuration.Cost);
                Data.AddCouple(player, loverPlayer);
            });
    }

    public MarryForm(Player player)
    {
        _player = player;
        _form = new CustomForm("登记结婚");
        AddControls();
    }

    public void Show()
    {
        _form.SendTo(_player);
    }
}