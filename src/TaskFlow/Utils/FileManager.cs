using System.Text.Json;
using TaskFlow.Models;

namespace TaskFlow.Utils;

public static class FileManager
{
    private const string FilePath = "data/tasks.json";
    private const string DirectoryPath = "data";

    // Verifica y en caso de que no existan, crea el directorio y el archivo necesarios
    private static void EnsureDirectoryAndFileExist()
    {
        if (!Directory.Exists(DirectoryPath))
        {
            Directory.CreateDirectory(DirectoryPath);
        }

        if (!File.Exists(FilePath))
        {
            File.WriteAllText(FilePath, "[]");
        }
    }

    // Guarda la lista de tareas en un archivo JSON de forma segura
    public static void SaveTasks(List<TaskItem> tasks)
    {
        try
        {
            EnsureDirectoryAndFileExist();

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
            EnsureDirectoryAndFileExist();

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
