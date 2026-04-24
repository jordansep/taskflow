using System.Text.Json;
using TaskFlow.Models;

namespace TaskFlow.Utils;

public static class FileManager
{
    private const string FilePath = "data/tasks.json";
    private const string DirectoryPath = "data";

    // Guarda la lista de tareas en un archivo JSON de forma segura
    public static void SaveTasks(List<TaskItem> tasks)
    {
        try
        {
            // Verifica y crea el directorio si no existe
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }

            // Opciones para guardar el JSON con sangría (limpio y legible)
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(tasks, options);
            
            File.WriteAllText(FilePath, jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar las tareas: {ex.Message}");
        }
    }

    // Lee el archivo JSON y devuelve la lista de tareas
    public static List<TaskItem> LoadTasks()
    {
        try
        {
            // Si el archivo no existe, retornamos una lista vacía
            if (!File.Exists(FilePath))
            {
                return new List<TaskItem>();
            }

            string jsonString = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<TaskItem>>(jsonString) ?? new List<TaskItem>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al leer las tareas: {ex.Message}");
            return new List<TaskItem>();
        }
    }
}
