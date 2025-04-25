using Footballer;
namespace DataWork;
/// <summary>
/// Класс отвечающий за автосохранение данных.
/// </summary>
public static class AutoSaver
{
    /// <summary>
    /// Последнее время изменения.
    /// </summary>
    private static DateTime changeTime = DateTime.Now;

    /// <summary>
    /// Функция запускает таймера.
    /// </summary>
    public static void Start() 
    {
        changeTime = DateTime.Now;
    }

    /// <summary>
    /// Функция отвечает за подписку.
    /// </summary>
    public static void SubscribeTo()
    {
        foreach (var footballer in DataSortFil.Data)
        {
            footballer.Updated += HandleUpdated;
        }
    }

    /// <summary>
    /// Сохранение данных.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="timeEvent"></param>
    public static void HandleUpdated(object sender, TimeEventArgs timeEvent)
    {
        if ((timeEvent.UpdateTime - changeTime).TotalSeconds <= 15)
        {
            JsonParse.path = JsonParse.path.Replace(".json", "_tmp.json");
            JsonParse.Save(DataSortFil.ToFileString());
            JsonParse.path = JsonParse.path.Replace("_tmp.json", ".json");
        }
        changeTime = DateTime.Now;
    }
}
