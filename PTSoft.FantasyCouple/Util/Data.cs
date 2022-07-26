using MC;
using Newtonsoft.Json;
using PTSoft.FantasyCouple.Model;

namespace PTSoft.FantasyCouple.Util;

public class Data
{
    private static readonly Configuration Configuration = Configuration.Config;

    public static List<Couple> Couples { get; private set; } = null!;

    public static bool CheckIsMarried(Player player) =>
        Couples.Where(couple => !couple.IsDeleted)
            .Any(couple => couple.Husband == player.Xuid || couple.Wife == player.Xuid);

    public static Couple? GetCoupleByPlayer(Player player) =>
        Couples.Where(couple => !couple.IsDeleted)
            .FirstOrDefault(couple => couple.Husband == player.Xuid || couple.Wife == player.Xuid);

    public static Couple? GetCoupleById(Guid id) =>
        Couples.Where(couple => !couple.IsDeleted).FirstOrDefault(couple => couple.Id == id);

    public static bool AddCouple(Player husband, Player wife, string? nick = null)
    {
        if (CheckIsMarried(husband) || CheckIsMarried(wife)) { return false; }

        var cp = new Couple(husband.Xuid, wife.Xuid, nick)
        {
            Id = Guid.NewGuid(),
            Index = Couples.Count,
        };
        Couples.Add(cp);
        return true;
    }

    public static bool DeleteCouple(Player player)
    {
        var couple = GetCoupleByPlayer(player);
        if (couple == null)
        {
            return false;
        }

        couple.IsDeleted = true;
        return true;
    }
    
    public static bool DeleteCouple(Guid id)
    {
        var couple = GetCoupleById(id);
        if (couple == null)
        {
            return false;
        }

        couple.IsDeleted = true;
        return true;
    }

    public static async Task InitAsync()
    {
        if (File.Exists(Configuration.Data))
        {
            using var sr = new StreamReader(Configuration.Data);
            var cps = JsonConvert.DeserializeObject<List<Couple>>(await sr.ReadToEndAsync());
            Couples = cps ?? new List<Couple>();
        }
        else
        {
            Couples = new List<Couple>();
            await SaveAsync();
        }
    }

    public static async Task SaveAsync()
    {
        await using var sw = new StreamWriter(Configuration.Data);
        await sw.WriteAsync(JsonConvert.SerializeObject(Couples));
    }
}