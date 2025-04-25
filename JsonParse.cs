using System.Security;
using System.Text.Json;
using Footballer;

namespace DataWork;

/// <summary>
/// Класс для работы с файлами.
/// </summary>
public static class JsonParse
{
    /// <summary>
    /// Путь к файлу.
    /// </summary>
    public static string path { get; set; } = "";

    /// <summary>
    /// Считывание данных из файла.
    /// </summary>
    public static void Reader()
    {
        while (true)
        {
            Console.WriteLine("Enter the full path to the file:");
            path = Console.ReadLine().Trim('"');
            try
            {
                // Настройки регистра файлов. 
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                string jsonString = File.ReadAllText(path);
                DataSortFil.Data = new List<Footballer.Footballer>(
                    JsonSerializer.Deserialize<List<Footballer.Footballer>>(jsonString, options));
                // Подписка данных.
                AutoSaver.SubscribeTo();
                ComandCheck.SubscribeTo();
                Console.WriteLine(jsonString);
                break;
            }
            // Отлов ошибок.
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("Stream is not exist!");
            }
            catch (SecurityException ex)
            {
                Console.WriteLine("The caller does not have the necessary permission!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error has occurred!");
            }
        } 

    }

    /// <summary>
    /// Функция сохранения данных в файл.
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static void Save(string data)
    {
        while(true)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.Write(data);
                    break;
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("Stream is not exist!");
            }
            catch (SecurityException ex)
            {
                Console.WriteLine("The caller does not have the necessary permission!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error has occurred!");
            }
            Console.WriteLine("Enter the new full path to the file:");
            path = Console.ReadLine().Trim('"');
        } 
    }
    /// <summary>
    /// Функция вывода данных в консоль.
    /// </summary>
    /// <param name="data"></param>
    public static void ConsoleWriter(string data)
    {
        Console.WriteLine(data);
    }
}