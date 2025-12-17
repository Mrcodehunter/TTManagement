using FluentValidation;
using MediatR;
using System.Security.Claims;
using TTManagement.Data;
using TTManagement.Entities;

namespace TTManagement.Features.ProjectTasks;

public record CreateTaskCommand(string Title, string Description, DateTime DueDate, int? AssignedToUserId, int? TeamId) : IRequest<int>;

public class CreateTaskValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.DueDate).GreaterThan(DateTime.Now);
    }
}

public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, int>
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateTaskHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<int> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token");
        }

        var task = new ProjectTask
        {
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            Status = ProjectTaskStatus.Todo,
            CreatedByUserId = userId,
            AssignedToUserId = request.AssignedToUserId,
            TeamId = request.TeamId
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync(cancellationToken);
        return task.Id;
    }
}

