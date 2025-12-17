using System.Text.Json.Serialization;

namespace TTManagement.Entities;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    [JsonIgnore]
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }

    public ICollection<ProjectTask> AssignedTasks { get; set; } = new List<ProjectTask>();
    public ICollection<ProjectTask> CreatedTasks { get; set; } = new List<ProjectTask>();
}

