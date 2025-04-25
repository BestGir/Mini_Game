using Footballer;
using System.Xml.Linq;

namespace DataWork;
/// <summary>
/// Класс отвечающий за event c обработкой команды игроков.
/// </summary>
public class ComandCheck
{
    /// <summary>
    /// Функция отвечающая за подписку.
    /// </summary>
    public static void SubscribeTo()
    {
        foreach (var footballer in DataSortFil.Data)
        {
            footballer.StateUpdated += Check;
        }
    }

    /// <summary>
    /// Функция проверяет команду на количество карточек.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="check"></param>
    public static void Check(object sender, CheckEventArgs check) 
    {
        List<Footballer.Footballer> tempdata = new List<Footballer.Footballer>(DataSortFil.Data);
        // Оставляем игроков из нужной команды.
        tempdata.RemoveAll((Footballer.Footballer x) => (x.Team != check.Team));
        string red = "Red Cards", yellow = "Yellow Cards";
        int countCards = 0;
        // Подсчет количества карточек.
        foreach (var item in tempdata)
        {
            foreach (var stat in item.Stats) 
            {
                if (red == stat.StatType || yellow == stat.StatType) 
                {
                    countCards++;
                }
            }
        }
        // Дисквалификация команды.
        if (countCards > 7) 
        {
            Console.WriteLine($"The team {check.Team} was disqualified");
            DataSortFil.Data.RemoveAll((Footballer.Footballer x) => (x.Team == check.Team));
        }
    }
}
