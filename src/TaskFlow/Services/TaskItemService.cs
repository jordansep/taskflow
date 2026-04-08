using System.Collections.Generic;
using TaskFlow.Models;

namespace TaskFlow.Services;

public class TaskItemService
{
    public void CreateTask(string title, string description, string responsible) //Método para crear una tarea con título, descripción y responsable
    {
        int task = ListTask().Count(); //Obtenemos el número de tareas actuales para asignar un nuevo ID incremental
        var newTask = new TaskItem
        {
            Id = tasks + 1, //Cuando agreguemos filemanager, tasks será la inyección de la base de datos, por ahora es la lista en memoria
            Title = title,
            Description = description,
            Responsible = responsible,
            Status = TaskStatus.ToDo,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void CreateTask(string title, string responsible) //Sobrecarga del método CreateTask para permitir crear tareas sin descripción
    {
        var newTask = new TaskItem
        {
            Id = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1,
            Title = title,
            Description = null,
            Responsible = responsible,
            Status = TaskStatus.ToDo,
            CreatedAt = DateTime.UtcNow
        };
    }

    private readonly List<TaskItem> _tasks = new List<TaskItem>
    {
        new TaskItem { Id = 1, Title = "Aprender GitFlow", Description = "Comprender cómo usar ramas de feature, develop y main", IsCompleted = false },
        new TaskItem { Id = 2, Title = "Crear la feature de listar", Description = "Implementar ListTask en TaskItemService", IsCompleted = true }
    };

    public IEnumerable<TaskItem> ListTask()
    {
        return _tasks;
    }
}
