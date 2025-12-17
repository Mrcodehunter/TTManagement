using MediatR;
using Microsoft.EntityFrameworkCore;
using TTManagement.Data;
using TTManagement.Entities;
using FluentValidation;

namespace TTManagement.Features.ProjectTasks;

public record UpdateTaskCommand(
    int Id,
    string Title,
    string Description,
    ProjectTaskStatus Status,
    DateTime DueDate,
    int? AssignedToUserId,
    int? TeamId
) : IRequest<bool>;

public class UpdateTaskValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DueDate).GreaterThan(DateTime.Now);
        RuleFor(x => x.Status).IsInEnum();
    }
}

public class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand, bool>
{
    private readonly AppDbContext _context;

    public UpdateTaskHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.FindAsync(new object[] { request.Id }, cancellationToken);
        if (task == null) return false;

        task.Title = request.Title;
        task.Description = request.Description;
        task.Status = request.Status;
        task.DueDate = request.DueDate;
        task.AssignedToUserId = request.AssignedToUserId;
        task.TeamId = request.TeamId;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

