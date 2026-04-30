using System.Text.Json;
using TaskFlow.Models;
using TaskFlow.Utils; // Agregado para usar FileManager

namespace TaskFlow.Services;

public class TaskItemService
{
    private List<TaskItem> _tasks;

    public TaskItemService()
    {
        // Al iniciar el servicio, se cargan las tareas existentes del archivo
        _tasks = FileManager.LoadTasks();
    }
<<<<<<< HEAD
    
=======
    private void ValidateTask(string title, string responsible)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("El título de la tarea no puede estar vacío.");
        if (string.IsNullOrWhiteSpace(responsible)) throw new ArgumentException("El responsable de la tarea no puede estar vacío.");
    }

>>>>>>> develop
    public void CreateTask(string title, string description, string responsible) //Método para crear una tarea con título, descripción y responsable
    {
        ValidateTask(title, responsible);

        var newTask = new TaskItem
        {
            Id = _tasks.Count > 0 ? _tasks.Max(t => t.Id) + 1 : 1, // ID basado en el máximo actual
            Title = title,
            Description = description,
            Responsible = responsible,
            Status = TaskStatus.ToDo,
            CreatedAt = DateTime.UtcNow
        };
        _tasks.Add(newTask); //Agregamos la nueva tarea a la lista de tareas
        
        // Persistencia inmediata
        FileManager.SaveTasks(_tasks);
    }

    public void CreateTask(string title, string responsible) //Sobrecarga del método CreateTask para permitir crear tareas sin descripción
    {
        ValidateTask(title, responsible);

        var newTask = new TaskItem
        {
            Id = _tasks.Count > 0 ? _tasks.Max(t => t.Id) + 1 : 1,
            Title = title,
            Description = null,
            Responsible = responsible,
            Status = TaskStatus.ToDo,
            CreatedAt = DateTime.UtcNow
        };
        _tasks.Add(newTask);
        
        // Persistencia inmediata
        FileManager.SaveTasks(_tasks);
    }

    public List<TaskItem> ListTasks() //Método para listar todas las tareas
    {
        return _tasks;
    }

    public void UpdateTaskStatus(int id, TaskStatus newStatus)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id) ?? throw new ArgumentException("Tarea no encontrada.");
        
        task.Status = newStatus;
        task.UpdatedAt = DateTime.UtcNow;

        // Persistencia inmediata tras actualización
        FileManager.SaveTasks(_tasks);
    }
}
