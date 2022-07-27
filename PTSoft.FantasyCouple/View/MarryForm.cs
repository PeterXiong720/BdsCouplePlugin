using LLMoney;
using LLNET;
using LLNET.Form;
using MC;
using PTSoft.FantasyCouple.Util;

namespace PTSoft.FantasyCouple.View;

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
        {
            player.SendModalForm(
                "§c错误",
                "求婚申请发送失败，或您已点击取消。如有任何异议请向管理员反馈",
                "确定", "反馈", _ => { });
            return;
        }

        var loverPlayer = GlobalService.Level.GetPlayer(lover);
        if (Data.CheckIsMarried(loverPlayer))
        {
            player.SendModalForm(
                "§c错误",
                "对方已经名花有主力（悲）。如有任何异议请向管理员反馈",
                "确定", "反馈", _ => { });
            return;
        }

        var success = loverPlayer!.SendModalForm(
            "求婚申请",
            $"{message} -- {player.Name}",
            "同意",
            "拒绝",
            result =>
            {
                if (!result)
                    return;
                if (EconomySystem.GetMoney(player.Xuid) < Configuration.Cost ||
                    EconomySystem.GetMoney(lover) < Configuration.Cost)
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

                var couple = Data.AddCouple(player, loverPlayer);
                if (couple == null)
                    return;
                EconomySystem.ReduceMoney(player.Xuid, Configuration.Cost);
                EconomySystem.ReduceMoney(lover, Configuration.Cost);

                Level.BroadcastText($"§3{player.RealName}§c和§3{loverPlayer.RealName}§c刚刚登记结婚力，恭喜这对新人喜结良缘！",
                    TextType.CHAT);
                
                var item1 = ItemStack.Create("couple:marriage_certificate", 1);
                item1.SetLore(new[]
                {
                    $"§3Husband: {player.RealName}",
                    $"§3Wife: {loverPlayer.RealName}",
                    $"§gCP编号: {couple.Name}",
                    $"§7ID: {couple.Id}"
                });
                var item2 = ItemStack.Create("couple:marriage_certificate", 1);
                item2.SetLore(new[]
                {
                    $"§3Husband: {player.RealName}",
                    $"§3Wife: {loverPlayer.RealName}",
                    $"§gCP编号: {couple.Name}",
                    $"§7ID: {couple.Id}"
                });
                player.GiveItem(item1);
                loverPlayer.GiveItem(item2);
                
                Level.RuncmdEx($"playsound music.aini {player.Name}");
                Level.RuncmdEx($"playsound music.aini {loverPlayer.Name}");
                
                Data.SaveAsync().Wait();
            });
        if (!success)
        {
            player.SendModalForm(
                "§c错误",
                "求婚申请发送失败。如有任何异议请向管理员反馈",
                "确定", "反馈", _ => { });
        }
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