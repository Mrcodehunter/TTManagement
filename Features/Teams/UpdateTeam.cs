using MediatR;
using TTManagement.Data;
using TTManagement.Entities;
using FluentValidation;

namespace TTManagement.Features.Teams;

public record UpdateTeamCommand(int Id, string Name, string Description) : IRequest<bool>;

public class UpdateTeamValidator : AbstractValidator<UpdateTeamCommand>
{
    public UpdateTeamValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
    }
}

public class UpdateTeamHandler : IRequestHandler<UpdateTeamCommand, bool>
{
    private readonly AppDbContext _context;

    public UpdateTeamHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
    {
        var team = await _context.Teams.FindAsync(new object[] { request.Id }, cancellationToken);
        if (team == null) return false;

        team.Name = request.Name;
        team.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

