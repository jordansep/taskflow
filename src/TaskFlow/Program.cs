using System;
using TaskFlow.Models;
using TaskFlow.Services;

class Program
{
    static void Main(string[] args)
    {
        // Constructores y servicios
        ConsoleHelper consoleService = new ConsoleHelper(new TaskItemService());
        consoleService.StartApp();
    }
}