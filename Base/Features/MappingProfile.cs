using AutoMapper;
using TTManagement.DTOs;
using TTManagement.Entities;

namespace TTManagement.Features;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProjectTask, ProjectTaskDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.AssignedToUserName, o => o.MapFrom(s => s.AssignedToUser != null ? s.AssignedToUser.FullName : null))
            .ForMember(d => d.CreatedByUserName, o => o.MapFrom(s => s.CreatedByUser.FullName))
            .ForMember(d => d.TeamName, o => o.MapFrom(s => s.Team != null ? s.Team.Name : null));
            
        CreateMap<User, UserDto>(); // Assuming we might need this later
        CreateMap<Team, TeamDto>();
    }
}

public class UserDto {
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

public class TeamDto {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

