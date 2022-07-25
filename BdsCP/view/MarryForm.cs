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