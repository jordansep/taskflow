using System.Text.Json;
using TaskFlow.Models;

namespace TaskFlow.Services;

public class TaskItemService
{
    private readonly List<TaskItem> _tasks = new ();
    public TaskItemService()
    {
        _tasks = new List<TaskItem>();
    }
    public void CreateTask(string title, string description, string responsible) //Método para crear una tarea con título, descripción y responsable
    {
        var newTask = new TaskItem
        {
            Id = _tasks.Count + 1, //Cuando agreguemos filemanager, tasks será la inyección de la base de datos, por ahora es la lista en memoria
            Title = title,
            Description = description,
            Responsible = responsible,
            Status = TaskStatus.ToDo,
            CreatedAt = DateTime.UtcNow
        };
        _tasks.Add(newTask); //Agregamos la nueva tarea a la lista de tareas
    }

    public void CreateTask(string title, string responsible) //Sobrecarga del método CreateTask para permitir crear tareas sin descripción
    {
        var newTask = new TaskItem
        {
            Id = _tasks.Count + 1,
            Title = title,
            Description = null,
            Responsible = responsible,
            Status = TaskStatus.ToDo,
            CreatedAt = DateTime.UtcNow
        };
        _tasks.Add(newTask);
    }

    public  List<TaskItem> ListTasks() //Método para listar todas las tareas
    {
        return _tasks;
    }

    public void UpdateTaskStatus(int id, TaskStatus newStatus)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id) ?? throw new ArgumentException("Tarea no encontrada.");
        
        task.Status = newStatus;
        task.UpdatedAt = DateTime.UtcNow;

        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText("tasks.json", JsonSerializer.Serialize(_tasks, options));
    }
}
