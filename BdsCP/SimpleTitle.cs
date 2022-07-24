using BdsCP.Util;
using LLNET.Event;

namespace BdsCP;

public class SimpleTitle
{
    private readonly Configuration _configuration = Configuration.Config;

    public SimpleTitle()
    {
        PlayerChatEvent.Event += ev =>
        {
            var text = ev.Message;
            if ( text == null)
                return true;
            
            
            return false;
        };
    }
    
    
}