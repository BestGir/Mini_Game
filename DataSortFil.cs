using Footballer;
using System;
using System.Text;

namespace DataWork;

/// <summary>
/// Класс хронящий и обрабатывающий данные.
/// </summary>
public static class DataSortFil
{
    /// <summary>
    /// Данные о всех футболистах.
    /// </summary>
    public static List<Footballer.Footballer> Data { get; set; }
    
    /// <summary>
    /// Тип выборки, сортировки или изменений.
    /// </summary>
    public enum SelectionSortType
    {
        PlayerId,
        PlayerName,
        Position,
        JerseyNumber,
        Team
    }

    /// <summary>
    /// Сортировка по указанному параметру.
    /// </summary>
    /// <param name="sortType"></param>
    public static void Sort(SelectionSortType sortType)
    {
        if (sortType == SelectionSortType.PlayerId)
        {
            Data.Sort((Footballer.Footballer x, Footballer.Footballer y) => x.PlayerId.CompareTo(y.PlayerId));
        }
        if (sortType == SelectionSortType.PlayerName)
        {
            Data.Sort((Footballer.Footballer x, Footballer.Footballer y) => x.PlayerName.CompareTo(y.PlayerName));
        }
        if (sortType == SelectionSortType.Position)
        {
            Data.Sort((Footballer.Footballer x, Footballer.Footballer y) => x.Position.CompareTo(y.Position));
        }
        if (sortType == SelectionSortType.JerseyNumber)
        {
            Data.Sort((Footballer.Footballer x, Footballer.Footballer y) => x.JerseyNumber.CompareTo(y.JerseyNumber));
        }
        if (sortType == SelectionSortType.Team)
        {
            Data.Sort((Footballer.Footballer x, Footballer.Footballer y) => x.Team.CompareTo(y.Team));
        }
    }

    /// <summary>
    /// Выборка по указанному параметру целого типа.
    /// </summary>
    /// <param name="selectionType"></param>
    /// <param name="num"></param>
    public static void Select(SelectionSortType selectionType, int num)
    {
        if (selectionType == SelectionSortType.JerseyNumber)
        {
            Data.RemoveAll((Footballer.Footballer x) => (x.JerseyNumber != num));
        }
    }

    /// <summary>
    /// Выборка по указанному параметру строчного типа.
    /// </summary>
    /// <param name="selectionType"></param>
    /// <param name="num"></param>
    public static void Select(SelectionSortType selectionType, string name)
    {;
        if (selectionType == SelectionSortType.PlayerId)
        {
            Data.RemoveAll((Footballer.Footballer x) => (x.PlayerId != name));
        }
        if (selectionType == SelectionSortType.PlayerName)
        {
            Data.RemoveAll((Footballer.Footballer x) => (x.PlayerName != name));
        }
        if (selectionType == SelectionSortType.Position)
        {
            Data.RemoveAll((Footballer.Footballer x) => (x.Position != name));
        }
        if (selectionType == SelectionSortType.Team)
        {
            Data.RemoveAll((Footballer.Footballer x) => (x.Team != name));
        }
    }

    /// <summary>
    /// Перевод данных в строчный тип для вывода в файл.
    /// </summary>
    /// <returns></returns>
    public static string ToFileString()
    {
        StringBuilder jsonFile = new StringBuilder();
        jsonFile.AppendLine("[");
        for (int i = 0; i < Data.Count - 1; i++)
        {
            jsonFile.Append(Data[i].ToJSON() + ",\n");
            
        }
        jsonFile.Append(Data[Data.Count - 1].ToJSON() + "\n");
        jsonFile.Append("]");
        return jsonFile.ToString();
    }
    /// <summary>
    /// Перевод данных в строчный тип для вывода в консоль.
    /// </summary>
    /// <returns></returns>
    public static string ToConsoleString()
    {
        StringBuilder jsonFile = new StringBuilder();
        jsonFile.AppendLine("[\n");
        for (int i = 0; i < Data.Count - 1; i++)
        {
            jsonFile.Append("\t" + Data[i].ToConsoleString() + ",\n");

        }
        jsonFile.Append("\t" + Data[Data.Count - 1].ToConsoleString() + "\n");
        jsonFile.Append("]\n");
        return jsonFile.ToString();
    }
    /// <summary>
    /// Вспомогательная функция для поиска игрока по id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private static int Findind(string id) 
    {
        for (int i = 0; i < Data.Count; i++)
        {
            if (Data[i].PlayerId == id.Trim('"'))
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Функция изменения параметра игрока по id строчного типа.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="type"></param>
    /// <param name="str"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void Update(string id, SelectionSortType type, string str) 
    {
        int index = Findind(id);
        if (index == -1)
        {
            throw new ArgumentOutOfRangeException("Player not found.");
        }
        if (type == SelectionSortType.PlayerName)
        {
            Data[index].NameUpdate(str);
        }
        if (type == SelectionSortType.Position)
        {
            Data[index].PosUpdate(str);
        }
        if (type == SelectionSortType.Team)
        {
            Data[index].TeamUpdate(str);
        }
    }
    /// <summary>
    /// Функция изменения параметра игрока по id целого типа.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="type"></param>
    /// <param name="str"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void Update(string id, SelectionSortType type, int num)
    {
        int index = Findind(id);
        if (index == -1)
        {
            throw new ArgumentOutOfRangeException("Player not found.");
        }
        if (type == SelectionSortType.JerseyNumber)
        {
            Data[index].NumberUpdate(num);
        }
    }
    /// <summary>
    /// Функция добавления игроку статистики по id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="type"></param>
    /// <param name="str"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void Add(string id, string stat, string idStat) 
    {
        int index = Findind(id);
        if (index == -1) 
        {
            throw new ArgumentOutOfRangeException("Player not found.");
        }
        Data[index].StateAdd(stat, idStat);
    }
}
