using System.Runtime.InteropServices;
using LLNET.LL;

namespace BdsCP.Util;

public class EconomySystem
{
    public delegate long GetMoneyHandler(string xuid);

    public delegate bool SetMoneyHandler(string xuid, long money);
    
    public delegate bool AddMoneyHandler(string xuid, long money);
    
    public delegate bool ReduceMoneyHandler(string xuid, long money);
    
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern int GetProcAddress(IntPtr hModule, string lpProcName);

    public readonly GetMoneyHandler GetMoney;

    public readonly SetMoneyHandler SetMoney;

    public readonly AddMoneyHandler AddMoney;

    public readonly ReduceMoneyHandler ReduceMoney;

    public EconomySystem()
    {
        var llmoney = LLAPI.GetPlugin("LLMoney");
        if (llmoney == null)
        {
            throw new Exception("LLMoney not found!");
        }

        var handler = llmoney.Handler;
        var getMoney = GetProcAddress(handler, "LLMoneyGet");
        if (getMoney != 0)
        {
            throw new Exception("LLMoney not found!");
        }
        GetMoney = Marshal.GetDelegateForFunctionPointer<GetMoneyHandler>(new IntPtr(getMoney));
        var setMoney = GetProcAddress(handler, "LLMoneySet");
        if (setMoney != 0)
        {
            throw new Exception("LLMoney not found!");
        }
        SetMoney = Marshal.GetDelegateForFunctionPointer<SetMoneyHandler>(new IntPtr(setMoney));
        var addMoney = GetProcAddress(handler, "LLMoneyAdd");
        if (addMoney != 0)
        {
            throw new Exception("LLMoney not found!");
        }
        AddMoney = Marshal.GetDelegateForFunctionPointer<AddMoneyHandler>(new IntPtr(addMoney));
        var reduceMoney = GetProcAddress(handler, "LLMoneyReduce");
        if (reduceMoney != 0)
        {
            throw new Exception("LLMoney not found!");
        }
        ReduceMoney = Marshal.GetDelegateForFunctionPointer<ReduceMoneyHandler>(new IntPtr(reduceMoney));
    }
}