using System.Text.Json;
using System.Text.Json.Serialization;
namespace Footballer;

/// <summary>
/// Класс отвечающий за отдельные stat.
/// </summary>
public struct Stat
{
    public string StatType { get; init; }
    public string StatId { get; init; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="id"></param>
    public Stat(string type, string id) 
    {
        StatType = type;
        StatId = id;
    }
    /// <summary>
    /// Вспомогательная функция для перевода класса в строковый тип.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{{stat_type: {StatType}, stat_id: {StatId}}}";
    }
}

/// <summary>
/// Класс хранящий все данные про игроков.
/// </summary>
public class Footballer
{
    /// <summary>
    /// Id игрока.
    /// </summary>
    [JsonInclude]
    public string PlayerId { get; private set; }
    /// <summary>
    /// Имя игрока.
    /// </summary>
    [JsonInclude]
    public string PlayerName { get; private set; }
    /// <summary>
    /// Позиция игрока на поле.
    /// </summary>
    [JsonInclude]
    public string Position { get; private set; }
    /// <summary>
    /// Номер игрока.
    /// </summary>
    [JsonInclude]
    public int JerseyNumber { get; private set; }
    /// <summary>
    /// Название команды игрока.
    /// </summary>
    [JsonInclude]
    public string Team { get; private set; }
    /// <summary>
    /// Вся статистика.
    /// </summary>
    [JsonInclude]
    public List<Stat> Stats { get; private set; }
    /// <summary>
    /// Вспомогательный тип заголовков.
    /// </summary>
    public static string[] Handle { get; } = { "\"playerId\"", "\"playerName\"",
        "\"position\"", "\"jerseyNumber\"", "\"team\"", "\"stats\""};
    public event EventHandler<TimeEventArgs> Updated;
    public event EventHandler<CheckEventArgs> StateUpdated;

    /// <summary>
    /// Констркутор.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="pos"></param>
    /// <param name="num"></param>
    /// <param name="team"></param>
    /// <param name="stats"></param>
    public Footballer(string id, string name, string pos,
        int num, string team, List<Stat> stats)
    {
        PlayerId = id;
        PlayerName = name;
        Position = pos;
        JerseyNumber = num;
        Team = team;
        Stats = new List<Stat>(stats);
    }

    /// <summary>
    /// Дефолтный констркутор.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="pos"></param>
    /// <param name="num"></param>
    /// <param name="team"></param>
    /// <param name="stats"></param>
    public Footballer() 
    {
        PlayerId = "";
        PlayerName = "";
        Position = "";
        JerseyNumber = 0;
        Team = "";
        Stats = new List<Stat>();
    }

    /// <summary>
    /// Изменение имени игрока.
    /// </summary>
    /// <param name="s"></param>
    public void NameUpdate(string s)
    {
        PlayerName = s;
        Updated?.Invoke(this, new TimeEventArgs(DateTime.Now));
    }

    /// <summary>
    /// Изменение позиции игрока.
    /// </summary>
    /// <param name="s"></param>
    public void PosUpdate(string s)
    {
        Position = s;
        Updated?.Invoke(this, new TimeEventArgs(DateTime.Now));
    }

    /// <summary>
    /// Изменение номера игрока.
    /// </summary>
    /// <param name="s"></param>
    public void NumberUpdate(int num)
    {
        JerseyNumber = num;
        Updated?.Invoke(this, new TimeEventArgs(DateTime.Now));
    }

    /// <summary>
    /// Изменение команды игрока.
    /// </summary>
    /// <param name="s"></param>
    public void TeamUpdate(string s)
    {
        Team = s;
        Updated?.Invoke(this, new TimeEventArgs(DateTime.Now));
    }

    /// <summary>
    /// Изменение статистики игрока.
    /// </summary>
    /// <param name="s"></param>
    public void StateAdd(string name, string id)
    {
        Stats.Add(new Stat(name, id));
        Updated?.Invoke(this, new TimeEventArgs(DateTime.Now));
        StateUpdated?.Invoke(this, new CheckEventArgs(Team));
    }

    /// <summary>
    /// Перевод в строку JSON формата.
    /// </summary>
    /// <returns></returns>
    public string ToJSON() 
    {
        string jsonString = JsonSerializer.Serialize(this);
        return jsonString;
    }
    /// <summary>
    /// Перевод в строку для удобного чтения.
    /// </summary>
    /// <returns></returns>
    public string ToConsoleString()
    {
        string sts = "[";
        foreach (var sensor in Stats)
        {
            sts += "\n\t\t\t"+sensor.ToString();
        }
        sts += "\n\t\t]";
        return $"\t{{\n\t\t{Handle[0]}: {PlayerId}\n\t\t{Handle[1]}: {PlayerName}\n\t\t{Handle[2]}: {Position}\n" +
            $"\t\t{Handle[3]}: {JerseyNumber}\n\t\t{Handle[4]}: {Team}\n\t\t{Handle[5]}: {sts}\n\t}}";
    }
}

/// <summary>
/// Класс требуемый для хранения времени по задания для event.
/// </summary>
public class TimeEventArgs : EventArgs
{
    public DateTime UpdateTime { get; set; }

    public TimeEventArgs(DateTime time)
    {
        UpdateTime = time;
    }
}

/// <summary>
/// Класс требуемый для хранения названия команды для event.
/// </summary>
public class CheckEventArgs : EventArgs 
{
    public string Team { get; set; }
    public CheckEventArgs(string team) 
    {
        Team = team;
    }
}
