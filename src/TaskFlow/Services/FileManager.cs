using System.IO;

namespace TaskFlow.Services;

public class FileManager
{
    private readonly string _filePath = "tasks.json";

    public bool JsonExist()
    {
        return File.Exists(_filePath);
    }

    public void CreateJson()
    {
        if (!JsonExist())
        {
            // Inicializar con un arreglo JSON vacío
            File.WriteAllText(_filePath, "[]");
        }
    }
}
