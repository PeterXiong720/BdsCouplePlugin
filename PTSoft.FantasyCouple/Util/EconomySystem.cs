// using System.Runtime.InteropServices;
// using LLNET.LL;
//
// namespace BdsCP.Util;
//
// public class EconomySystem
// {
//     public delegate long GetMoneyHandler(ulong xuid);
//
//     public delegate bool SetMoneyHandler(ulong xuid, long money);
//
//     public delegate bool AddMoneyHandler(ulong xuid, long money);
//
//     public delegate bool ReduceMoneyHandler(ulong xuid, long money);
//
//     [DllImport("kernel32.dll", SetLastError = true)]
//     public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
//
//     public readonly GetMoneyHandler GetMoney;
//
//     public readonly SetMoneyHandler SetMoney;
//
//     public readonly AddMoneyHandler AddMoney;
//
//     public readonly ReduceMoneyHandler ReduceMoney;
//
//     public unsafe EconomySystem()
//     {
//         var llmoney = LLAPI.GetPlugin("LLMoney");
//         if (llmoney == null)
//         {
//             throw new Exception("LLMoney not found!");
//         }
//
//         GetMoney = llmoney.GetFunction<GetMoneyHandler>("LLMoneyGet");
//         SetMoney = llmoney.GetFunction<SetMoneyHandler>("LLMoneySet");
//         AddMoney = llmoney.GetFunction<AddMoneyHandler>("LLMoneyAdd");
//         ReduceMoney = llmoney.GetFunction<ReduceMoneyHandler>("LLMoneyReduce");
//         
//     }
// }