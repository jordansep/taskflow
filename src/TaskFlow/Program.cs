using System;
using TaskFlow.Services;

var service = new TaskItemService();
bool exit = false;

while (!exit)
{
    Console.Clear();
    Console.WriteLine("=============================");
    Console.WriteLine("       🚀 TASKFLOW 🚀      ");
    Console.WriteLine("=============================");
    Console.WriteLine("1. Ver lista de tareas");
    Console.WriteLine("2. Crear nueva tarea (Completa)");
    Console.WriteLine("3. Crear nueva tarea (Rápida - sin descripción)");
    Console.WriteLine("4. Salir");
    Console.WriteLine("=============================");
    Console.Write("Seleccione una opción: ");

    string? option = Console.ReadLine();

    switch (option)
    {
        case "1":
            ShowTasks(service);
            break;
        case "2":
            CreateFullTask(service);
            break;
        case "3":
            CreateSimpleTask(service);
            break;
        case "4":
            exit = true;
            Console.WriteLine("Saliendo de TaskFlow... ¡Hasta luego!");
            break;
        default:
            Console.WriteLine("Opción no válida. Presione cualquier tecla para intentar de nuevo...");
            Console.ReadKey();
            break;
    }
}

static void ShowTasks(TaskItemService service)
{
    Console.Clear();
    Console.WriteLine("=== LISTA DE TAREAS ===");
    
    var tasks = service.ListTasks();
    
    if (tasks.Count == 0)
    {
        Console.WriteLine("No hay tareas registradas en el sistema.");
    }
    else
    {
        foreach (var task in tasks)
        {
            Console.WriteLine($"[{task.Id}] {task.Title} | Resp: {task.Responsible} | Estado: {task.Status}")2;
            if (!string.IsNullOrEmpty(task.Description))
            {
                Console.WriteLine($"    Descripción: {task.Description}");
            }
            Console.WriteLine("------------------------------------------------");
        }
    }

    Console.WriteLine("\nPresione cualquier tecla para volver al menú...");
    Console.ReadKey();
}

static void CreateFullTask(TaskItemService service)
{
    Console.Clear();
    Console.WriteLine("=== CREAR NUEVA TAREA ===");
    
    Console.Write("Título: ");
    string title = Console.ReadLine() ?? string.Empty;
    
    Console.Write("Descripción: ");
    string description = Console.ReadLine() ?? string.Empty;
    
    Console.Write("Responsable: ");
    string responsible = Console.ReadLine() ?? string.Empty;

    service.CreateTask(title, description, responsible);
    
    Console.WriteLine("\n¡Tarea creada con éxito! Presione cualquier tecla para continuar...");
    Console.ReadKey();
}

static void CreateSimpleTask(TaskItemService service)
{
    Console.Clear();
    Console.WriteLine("=== CREAR TAREA RÁPIDA ===");
    
    Console.Write("Título: ");
    string title = Console.ReadLine() ?? string.Empty;
    
    Console.Write("Responsable: ");
    string responsible = Console.ReadLine() ?? string.Empty;

    service.CreateTask(title, responsible);
    
    Console.WriteLine("\n¡Tarea creada con éxito! Presione cualquier tecla para continuar...");
    Console.ReadKey();
}