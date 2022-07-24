using LLNET.DynamicCommand;
using MC;

namespace BdsCP.Command;

[Command("couple", Description = "打开CP管理页面")]
public class CoupleCommand: ICommand
{
    public void Execute(CommandOrigin origin, CommandOutput output)
    {
        
    }
}