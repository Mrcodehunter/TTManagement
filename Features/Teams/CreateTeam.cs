using MediatR;
using TTManagement.Data;
using TTManagement.Entities;
using FluentValidation;

namespace TTManagement.Features.Teams;

public record CreateTeamCommand(string Name, string Description) : IRequest<int>;

public class CreateTeamValidator : AbstractValidator<CreateTeamCommand>
{
    public CreateTeamValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
    }
}

public class CreateTeamHandler : IRequestHandler<CreateTeamCommand, int>
{
    private readonly AppDbContext _context;

    public CreateTeamHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        var team = new Team
        {
            Name = request.Name,
            Description = request.Description
        };

        _context.Teams.Add(team);
        await _context.SaveChangesAsync(cancellationToken);
        return team.Id;
    }
}

