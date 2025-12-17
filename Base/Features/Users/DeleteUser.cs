using MediatR;
using TTManagement.Data;

namespace TTManagement.Features.Users;

public record DeleteUserCommand(int Id) : IRequest<bool>;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly AppDbContext _context;

    public DeleteUserHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(new object[] { request.Id }, cancellationToken);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

