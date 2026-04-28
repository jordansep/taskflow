using TaskFlow.Models;
using TaskFlow.Services;

public class ConsoleHelper
{
    private readonly TaskItemService _service;

    public ConsoleHelper(TaskItemService service)
    {
        _service = service;
    }
    // Todo: Implementar un metodo para evitar tantos console.writeline asi hacer mas facil su modificacion en futuro, por ejemplo un metodo que reciba un string[] con las opciones y las imprima en formato de menu
    public void StartApp()
    {
        // Inicializamos la aplicacion y mostramos el menú principal
        bool exit = false;
        while (!exit)
        {
        Console.Clear();
        ShowText("=============================");
        ShowText("        || TASKFLOW ||       ");
        ShowText("=============================");
        ShowText("1. Ver lista de tareas");
        ShowText("2. Crear nueva tarea (Completa)");
        ShowText("3. Salir");
        ShowText("=============================");
        Console.Write("Seleccione una opción: ");
    
        string? option = Console.ReadLine();
    
    // Procesamos la opción seleccionada por el usuario
        switch (option)
        {
            case "1":
                ShowListOfTasks();
                break;
            case "2":
                CreateTaskFromConsole();
                break;
            case "3":
                exit = true;
                ShowText("Saliendo de TaskFlow... ¡Hasta luego!");
                break;
            default:
                ShowText("Opción no válida. Presione cualquier tecla para intentar de nuevo...");
                ReadKey();
                break;
            }
        }
    }
    public void ShowText(string text)
    {
        // Método para mostrar un texto en la consola, utilizado para mensajes de error o confirmación
        Console.WriteLine(text);
    }
    public void ShowTextWhitInput(string text)
    {
        // Método para mostrar un texto en la consola y luego solicitar una entrada del usuario, utilizado para formularios de creación o edición de tareas
        Console.Write(text);
    }
    public void ReadKey()
    {
        // Método para esperar a que el usuario presione una tecla, utilizado para pausar la aplicación después de mostrar un mensaje
        Console.ReadKey();
    }
    public void EndApp(){}
    public string ReadLine() => Console.ReadLine() ?? string.Empty;    
    private void CreateTaskFromConsole()
    {
        // Método para crear una tarea a través de la consola, solicitando título, descripción y responsable
          Console.Clear();
        ShowText("=== CREAR NUEVA TAREA ===");
    
            ShowText("Título: ");
            string title = ValidateTaskInput(ReadLine(), "título");

            ShowText("Descripción: ");
            string description = ReadLine() ?? string.Empty;

            ShowText("Responsable: ");
            string responsible = ValidateTaskInput(ReadLine(), "responsable");


        _service.CreateTask(title, description, responsible);
    
        ShowText("\n¡Tarea creada con éxito! Presione cualquier tecla para continuar...");
        ReadKey();
    }
    public string ValidateTaskInput(string input, string fieldName)
    {
        // Método para validar la entrada del usuario al crear una tarea, asegurándose de que el título no esté vacío y que el responsable sea válido
        while (string.IsNullOrWhiteSpace(input))
        {
            ShowText($"El {fieldName} no puede estar vacío. Por favor, ingrese un {fieldName} válido.");
            ShowTextWhitInput($"{fieldName}: ");
            input = ReadLine();
        }
        return input;

    }
    public void SelectTask()
    {
       
    }
    public void ShowListOfTasks(){
        // Método para mostrar la lista de tareas en la consola, incluyendo título, responsable y estado
         Console.Clear();
        ShowText("=== LISTA DE TAREAS ===");
        
        List<TaskItem> tasks = _service.ListTasks();
        
        // Si no hay tareas, mostramos un mensaje indicando que no hay tareas registradas
        if (tasks.Count == 0)
        {
            ShowText("No hay tareas registradas en el sistema.");
        }
        else
        {
            foreach (var task in tasks)
            {
                ShowText($"[{task.Id}] {task.Title} | Resp: {task.Responsible} | Estado: {task.Status}");
                if (!string.IsNullOrEmpty(task.Description))
                {
                    ShowText($"    Descripción: {task.Description}");
                }
                if(task.UpdatedAt.HasValue)
                {
                    ShowText($"    Última actualización: {task.UpdatedAt.Value.ToString("g")}");
                }else
                {
                    ShowText($"    Creada el: {task.CreatedAt.ToString("g")}");
                }
                ShowText("------------------------------------------------");
            }
        }
    
        ShowText("\nPresione cualquier tecla para volver al menú...");
        ReadKey();
    }
    public void UpdateTaskStatusFromConsole()
    {
        Console.Clear();
        ShowText("=== ACTUALIZAR ESTADO DE TAREA ===");

        var tasks = _service.ListTasks();
        if (tasks.Count == 0)
        {
            ShowText("No hay tareas registradas en el sistema.");
            ShowText("\nPresione cualquier tecla para volver al menú...");
            ReadKey();
            return;
        }

        foreach (var task in tasks)
        {
            ShowText($"[{task.Id}] {task.Title} | Estado actual: {task.Status}");
        }

        ShowTextWhitInput("\nIngrese el ID de la tarea a actualizar: ");
        if (!int.TryParse(ReadLine(), out int id))
        {
            ShowText("ID inválido. Presione cualquier tecla para volver...");
            ReadKey();
            return;
        }

        var taskToUpdate = tasks.FirstOrDefault(t => t.Id == id);
        if (taskToUpdate == null)
        {
            ShowText("Tarea no encontrada. Presione cualquier tecla para volver...");
            ReadKey();
            return;
        }

        ShowText("\nSeleccione el nuevo estado:");
        ShowText("1. ToDo");
        ShowText("2. InProgress");
        ShowText("3. Done");
        ShowTextWhitInput("Opción: ");

        string? statusOption = ReadLine();
        TaskStatus newStatus;

        switch (statusOption)
        {
            case "1":
                newStatus = TaskStatus.ToDo;
                break;
            case "2":
                newStatus = TaskStatus.InProgress;
                break;
            case "3":
                newStatus = TaskStatus.Done;
                break;
            default:
                ShowText("Opción de estado inválida. Presione cualquier tecla para volver...");
                ReadKey();
                return;
        }

        try
        {
            _service.UpdateTaskStatus(id, newStatus);
            ShowText("\n¡Estado de la tarea actualizado con éxito!");
        }
        catch (Exception ex)
        {
            ShowText($"\nError al actualizar la tarea: {ex.Message}");
        }

        ShowText("Presione cualquier tecla para continuar...");
        ReadKey();
    }

}