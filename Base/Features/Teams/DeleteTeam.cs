using MediatR;
using TTManagement.Data;

namespace TTManagement.Features.Teams;

public record DeleteTeamCommand(int Id) : IRequest<bool>;

public class DeleteTeamHandler : IRequestHandler<DeleteTeamCommand, bool>
{
    private readonly AppDbContext _context;

    public DeleteTeamHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
    {
        var team = await _context.Teams.FindAsync(new object[] { request.Id }, cancellationToken);
        if (team == null) return false;

        _context.Teams.Remove(team);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

