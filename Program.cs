using System;
using System.Text.Json;
using Footballer;
using DataWork;

public class Program 
{
    static void Main()
    {
        AutoSaver.Start();
        Menu.Menu.ProgramFunctionality();
    }
}
