using TaskFlow.Models;
using TaskFlow.Services;

public class ConsoleHelper
{
    private readonly TaskItemService _service;

    public ConsoleHelper(TaskItemService service)
    {
        _service = service;
    }

    public static void StartApp()
    {
        
    }
    public static void EndApp(){}
    public static TaskItem CreateTaskFromConsole()=> new TaskItem();
    public static void SelectTask()
    {
        Console.WriteLine("Por favor, introduzca el ID de la tarea a seleccionar:");
        string input = Console.ReadLine();
        if (int.TryParse(input, out int taskId))
        {
            Console.WriteLine($"Se ha seleccionado la tarea con ID: {taskId}");
        }
        else
        {
            Console.WriteLine("Entrada no válida. Por favor ingrese un número entero.");
        }
    }
    public static void ShowListOfTasks(){}
    public static void UpdateTaskStatusFromConsole(){}

}