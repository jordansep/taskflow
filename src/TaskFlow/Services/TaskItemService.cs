using System.Collections.Generic;
using System.Linq;
using TaskFlow.Models;

namespace TaskFlow.Services;

public class TaskItemService
{
    private readonly FileManager _fileManager = new FileManager();

    private void ValidateTaskData(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("El título es obligatorio.");
        }
    }

    public void CreateTask(string title, string description, string responsible) //Método para crear una tarea con título, descripción y responsable
    {
        ValidateTaskData(title);
        var tasks = _fileManager.ReadJson();
        var newTask = new TaskItem
        {
            Id = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1,
            Title = title,
            Description = description,
            Responsible = responsible,
            Status = TaskStatus.ToDo,
            CreatedAt = DateTime.UtcNow
        };
        tasks.Add(newTask);
        _fileManager.WriteJson(tasks);
    }

    public void CreateTask(string title, string responsible) //Sobrecarga del método CreateTask para permitir crear tareas sin descripción
    {
        ValidateTaskData(title);
        var tasks = _fileManager.ReadJson();
        var newTask = new TaskItem
        {
            Id = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1,
            Title = title,
            Description = null,
            Responsible = responsible,
            Status = TaskStatus.ToDo,
            CreatedAt = DateTime.UtcNow
        };
        tasks.Add(newTask);
        _fileManager.WriteJson(tasks);
    }

    public List<TaskItem> ListTasks() //Método para listar todas las tareas
    {
        var tasks = _fileManager.ReadJson();
        foreach (var task in tasks)
        {
            Console.WriteLine($"{task.Id} - {task.Title} - {task.Description} - {task.Responsible} - {task.Status} - {task.CreatedAt}");
        }
        return tasks;
    }
}
