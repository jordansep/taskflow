using System.Collections.Generic;
using TaskFlow.Models;

namespace TaskFlow.Services;

public class TaskItemService
{
    private List<TaskItem> _tasks = new List<TaskItem>();
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
        List<TaskItem> _tasks = new List<TaskItem>(); //En esta parte se simula la base de datos con una lista en memoria, luego se reemplazará por la inyección de la base de datos
        foreach (var task in _tasks)
        {
            Console.WriteLine($"{task.Id} - {task.Title} - {task.Description} - {task.Responsible} - {task.Status} - {task.CreatedAt}");
        }
        return _tasks;
    }

}
