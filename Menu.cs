using Footballer;
using DataWork;
using static Menu.Menu;
using System.IO;

namespace Menu;

/// <summary>
/// Меню выборов.
/// </summary>
public static class Menu
{
    /// <summary>
    /// Считывание целого числа.
    /// </summary>
    /// <param name="n"></param>
    /// <param name="minVal"> Нижняя граница.</param>
    /// <param name="maxVal"> Верхняя граница.</param>
    static void InPut(out int n, int minVal, int maxVal)
    {
        Console.WriteLine($"Enter an integer in the range [{minVal}; {maxVal}).");
        while (!int.TryParse(Console.ReadLine(), out n) || n < minVal || n >= maxVal)
        {
            Console.WriteLine($"The data is incorrect, repeat entering the number.");
        }
    }

    /// <summary>
    /// Выводит Список возможных действий.
    /// </summary>
    public static void Help()
    {
        Console.WriteLine("1. Enter the data.");
        Console.WriteLine("2. Sort data.");
        for (int i = 0; i < Footballer.Footballer.Handle.Length - 1; i++)
        {
            Console.WriteLine($"\t2.{i + 1} Sort data using" +
                $" a parameter {Footballer.Footballer.Handle[i]}.");
        }
        Console.WriteLine("3. Select data.");
        for (int i = 0; i < Footballer.Footballer.Handle.Length - 1; i++)
        {
            Console.WriteLine($"\t3.{i + 1} Select data using" +
                $" a parameter {Footballer.Footballer.Handle[i]}.");
        }
        Console.WriteLine("4. Update data.");
        for (int i = 1; i < Footballer.Footballer.Handle.Length; i++)
        {
            Console.WriteLine($"\t4.{i} Update data using" +
                $" a parameter {Footballer.Footballer.Handle[i]}.");
        }
        Console.WriteLine("5. Save data.");
        Console.WriteLine("6. Write data to console.");
        Console.WriteLine("7. Exit the program.");
        Console.WriteLine("Enter an integer from 1 to 7:");
    }

    /// <summary>
    /// Тип возможных сообщений.
    /// </summary>
    public enum HelpType
    {
        Sort,
        Select,
        Update
    }

    /// <summary>
    /// Вспомогательные сообщения для пользователя.
    /// </summary>
    /// <param name="helpType"></param>
    public static void Help(HelpType helpType)
    {
        string s = "";
        switch (helpType)
        {
            case HelpType.Sort:
                s = "Sort";
                break;
            case HelpType.Select:
                s = "Select";
                break;
            case HelpType.Update:
                s = "Update";
                break;
            default: break;
        }
        if (helpType == HelpType.Update)
        {
            for (int i = 1; i < Footballer.Footballer.Handle.Length; i++)
            {
                Console.WriteLine($"{i}. Update data using" +
                    $" a parameter {Footballer.Footballer.Handle[i]}.");
            }
            return;
        }
        for (int i = 0; i < Footballer.Footballer.Handle.Length - 1; i++)
        {
            Console.WriteLine($"{i + 1} {s} data using a parameter {Footballer.Footballer.Handle[i]}.");
        }
    }

    /// <summary>
    /// Вызов действий меню.
    /// </summary>
    public static void ProgramFunctionality()
    {
        string ans;
        DataSortFil.Data = new List<Footballer.Footballer>();
        int x;
        while (true)
        {
            Help();
            ans = Console.ReadLine().Trim();
            // 1. Считать данные из файла.
            if (ans == "1")
            {
                JsonParse.Reader();
                continue;
            }
            // Проверка на наличие данных. 
            if (DataSortFil.Data.Count != 0)
            {
                // 2. Произвести сортировки.
                if (ans == "2")
                {
                    Help(HelpType.Sort);
                    InPut(out x, 1, 6);
                    DataSortFil.Sort((DataSortFil.SelectionSortType)(x - 1));
                    continue;
                }
                // 3. Произвести выборку.
                if (ans == "3")
                {
                    Help(HelpType.Select);
                    InPut(out x, 1, 6);
                    Console.WriteLine("Enter the selection parameter:");
                    // Выборка по номеру игрока.
                    if (4 == x) 
                    {
                        InPut(out x, 0, 100);
                        DataSortFil.Select((DataSortFil.SelectionSortType)(3), x);
                        continue;
                    }
                    // Выборка по строкам.
                    DataSortFil.Select((DataSortFil.SelectionSortType)(x - 1), Console.ReadLine());
                    continue;
                }
                // 4. Изменение данных.
                if (ans == "4")
                {
                    Help(HelpType.Update);
                    InPut(out x, 1, 6);
                    Console.WriteLine("Enter the playerId:");
                    string id = Console.ReadLine().Trim('"');
                    // Отлов ошибки на отсутствие игрока.
                    try
                    {
                        // Изменение номера игрока.
                        if ((DataSortFil.SelectionSortType)x == DataSortFil.SelectionSortType.JerseyNumber)
                        {
                            InPut(out x, 0, 100);
                            DataSortFil.Update(id, (DataSortFil.SelectionSortType)(x - 1), x);
                            continue;
                        }
                        // Добавление статуса игроку.
                        if (x == 5)
                        {
                            Console.WriteLine("Enter the statType:");
                            string t = Console.ReadLine();
                            Console.WriteLine("Enter the statId:");
                            string i = Console.ReadLine();
                            DataSortFil.Add(id, t, i);
                            continue;
                        }
                        // Все остальные изменения.
                        Console.WriteLine("Enter the new parametr:");
                        DataSortFil.Update(id, (DataSortFil.SelectionSortType)x, Console.ReadLine());
                    }
                    catch (Exception) 
                    {
                        Console.WriteLine("The player wasn't found");
                    }
                    continue;
                }
                // 5. Записать данные.
                if (ans == "5")
                {
                    Console.WriteLine("Enter the new full path to the file:");
                    
                    JsonParse.path = Console.ReadLine().Trim('"');
                    JsonParse.Save(DataSortFil.ToFileString());
                    continue;
                }
                // 6. Запись данных в консоль.
                if (ans == "6")
                {
                    JsonParse.ConsoleWriter(DataSortFil.ToConsoleString());
                    continue;
                }
            }
            else
            {
                Console.WriteLine("No data available.");
            }
            // 7. Выйти из программы.
            if (ans == "7")
            {
                Console.WriteLine("Удачи, путник!");
                return;
            }
            // Неправильный выбор
            Console.WriteLine("The value is incorrect. Indulge the input:");
        }
    }
}
