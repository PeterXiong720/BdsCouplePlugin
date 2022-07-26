using LLNET;
using LLNET.Form;
using MC;
using PTSoft.FantasyCouple.Model;
using PTSoft.FantasyCouple.Util;

namespace PTSoft.FantasyCouple.View;

public class MainForm
{
    //private static readonly Configuration Configuration = Configuration.Config;
    
    private readonly Player _player;

    private readonly SimpleForm _form;

    private readonly Couple? _couple;

    private void AddButtons()
    {
        if (_player.IsOP)
        {
            _form.AddButton("管理员菜单", string.Empty, pl =>
            {
                pl.SendText("§c此功能未包括在定制协议中，暂不予以实现");
            });
        }
        
        if (_couple == null)
        {
            _form.AddButton("登记结婚", string.Empty, pl =>
            {
                if (Data.CheckIsMarried(pl))
                {
                    pl.SendModalForm(
                        "§c错误",
                        "您当前处于已婚状态，无法于他人结婚！如有任何异议请向管理员反馈。",
                        "确定", "反馈",
                        _ => { });
                    return;
                }

                var marryForm = new MarryForm(pl);
                marryForm.Show();
            });
        }
        else
        {
            void Tell(Player pl)
            {
                pl.SendText(pl.IsOP
                    ? "§c此功能未包括在定制协议中，暂不予以实现（主要是界面太单调了，放几个按钮占位置，手动滑稽，只有OP才会看到这条提示）"
                    : "§e此功能未开放");
            }
            
            _form.AddButton("Oh baby 情话多说一点（私信）", string.Empty, Tell)
                .AddButton("想我就多看一眼（交换坐标）", string.Empty, Tell)
                .AddButton("在大头贴画满心号，贴在手机上对你微笑（编辑称号）", string.Empty, pl =>
                {
                    var form = new EditTitleForm(pl);
                    form.Show();
                })
                .AddButton("爱过也哭过笑过痛过之后只剩再见（离婚）", string.Empty, pl =>
                {
                    #region 离婚

                    var form = new ModalForm(
                        "协议离婚",
                        "你确定要解除CP关系吗？（点击确定后需要对方同意）",
                        "The day you went away",
                        "我点错了");
                    form.Callback = (player, confirm) =>
                    {
                        if (!confirm)
                            return;
                        var lover = _couple.Husband == player.Xuid ? _couple.Wife : _couple.Husband;
                        var loverPlayer = GlobalService.Level.GetPlayer(lover);
                        if (loverPlayer == null)
                        {
                            player.SendModalForm(
                                "§c错误",
                                "发送失败或对方不在线。如有任何异议请向管理员反馈",
                                "确定", "反馈", _ => { });
                            return;
                        }

                        loverPlayer.SendModalForm(
                            "协议离婚",
                            "对方想与您解除CP关系",
                            "同意", "拒绝", result =>
                            {
                                if (!result) return;
                                Data.DeleteCouple(_couple.Id);
                                Data.SaveAsync().Wait();
                            });
                    };
                    form.SendTo(pl);

                    #endregion
                });
        }
    }

    public MainForm(Player player)
    {
        _player = player;
        _couple = Data.GetCoupleByPlayer(player);
        string content;
        if (_couple == null)
        {
            content = "当前婚姻状态：未婚";
        }
        else
        {
            var lover = _couple.Husband == player.Xuid ? _couple.Wife : _couple.Husband;
            var loverPlayer = GlobalService.Level.GetPlayer(lover);
            content = $"CP编号：{_couple.Name}\nCP Id：{_couple.Id}\n当前CP：{loverPlayer?.Name ?? "已离线"}";
        }

        _form = new SimpleForm("CP管理", content);
        AddButtons();
    }

    public void ShowTo(Player player)
    {
        _form.SendTo(player);
    }
}