using BdsCP.Util;
using LLNET.Form;
using MC;

namespace BdsCP.View;

public class EditTitleForm
{
    private static readonly Configuration Configuration = Configuration.Config;

    private readonly CustomForm _form;

    private readonly Player _player;

    private void AddControls()
    {
        if (!Data.CheckIsMarried(_player))
            return;
        _form.Append(new Label("tips",
                $"付费修改CP称号。彩色文本需要额外花费。\n纯文本：${Configuration.ChangeTitleCost}\n彩色：{Configuration.ColorTitleCost}"))
            .Append(new Input("nick", "请输入", "输入称号", Data.GetCoupleByPlayer(_player)?.Name));
        _form.Callback = OnSubmit;
    }

    private void OnSubmit(Player player, Dictionary<string, CustomFormElement> data)
    {
        var couple = Data.GetCoupleByPlayer(player);
        if(couple==null)
            return;
        var newTitle = data["nick"].Value ?? couple.Name;
        var cost = newTitle.Contains('§') ? Configuration.ColorTitleCost : Configuration.ChangeTitleCost;
        if (PluginMain.EconomySystem.GetMoney(player.Xuid) < cost)
        {
            player.SendModalForm(
                "§c错误", 
                "您的方余额不足，无法完成修改。如有任何异议请向管理员反馈", 
                "确定", "反馈", _ => { });
            return;
        }
        PluginMain.EconomySystem.ReduceMoney(player.Xuid, cost);
        couple.Name = newTitle;
    }

    public EditTitleForm(Player player)
    {
        _form = new CustomForm("编辑CP称号");
        _player = player;
        AddControls();
    }

    public void Show()
    {
        _form.SendTo(_player);
    }
}