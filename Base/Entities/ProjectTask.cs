namespace TTManagement.Entities;

public class ProjectTask
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ProjectTaskStatus Status { get; set; }
    public DateTime DueDate { get; set; }

    public int? AssignedToUserId { get; set; }
    public User? AssignedToUser { get; set; }

    public int CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = null!;

    public int? TeamId { get; set; }
    public Team? Team { get; set; }
}

