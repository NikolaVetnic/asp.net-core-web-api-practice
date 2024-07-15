using System.ComponentModel.DataAnnotations;

namespace TodoApi;
public class TodoItem
{
    public long Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = "Untitled";

    public bool IsCompleted { get; set; } = false;
}

