namespace TaskFlow.Models;

public class TaskItem
{
    private int _id;
    public int Id 
    { 
        get => _id; 
        set 
        {
            if (value < 0) throw new ArgumentException("El ID no puede ser negativo.");
            _id = value;
        }
    }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Responsible { get; set; }
    public TaskStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
