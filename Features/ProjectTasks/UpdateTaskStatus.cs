using System.Security.Claims;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TTManagement.Data;
using TTManagement.Entities;
using FluentValidation;

namespace TTManagement.Features.ProjectTasks;

public record UpdateTaskStatusCommand(int Id, ProjectTaskStatus Status) : IRequest<bool>;

public class UpdateTaskStatusValidator : AbstractValidator<UpdateTaskStatusCommand>
{
    public UpdateTaskStatusValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
    }
}

public class UpdateTaskStatusHandler : IRequestHandler<UpdateTaskStatusCommand, bool>
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateTaskStatusHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        var roleClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role);

        if (userIdClaim == null || roleClaim == null)
            throw new UnauthorizedAccessException("User not authenticated properly.");

        var userId = int.Parse(userIdClaim.Value);
        var role = Enum.Parse<UserRole>(roleClaim.Value);

        var task = await _context.Tasks.FindAsync(new object[] { request.Id }, cancellationToken);
        if (task == null) return false;

        // Validation logic
        if (role == UserRole.Employee)
        {
            if (task.AssignedToUserId != userId)
            {
                throw new UnauthorizedAccessException("You can only update status of tasks assigned to you.");
            }
        }
        // Managers and Admins can update status of any task (though Manager usually does full update, this is fine too)

        task.Status = request.Status;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

