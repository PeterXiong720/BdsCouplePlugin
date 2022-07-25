using BdsCP.View;
using LLNET.DynamicCommand;
using MC;

namespace BdsCP.Command;

[Command("couple", Description = "打开CP管理页面", Alia = "cp", Permission = CommandPermissionLevel.Any)]
[CommandEmptyOverload]
public class CoupleCommand: ICommand
{
    public void Execute(CommandOrigin origin, CommandOutput output)
    {
        var player = origin.Player;
        if (player == null) return;
        var form = new MainForm(player);
        form.ShowTo(player);
    }
}