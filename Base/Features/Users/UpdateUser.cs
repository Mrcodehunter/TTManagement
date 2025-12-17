using MediatR;
using TTManagement.Data;
using TTManagement.Entities;
using FluentValidation;

namespace TTManagement.Features.Users;

public record UpdateUserCommand(
    int Id,
    string FullName,
    string Email,
    UserRole Role,
    string? Password = null
) : IRequest<bool>;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.FullName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Role).IsInEnum();
    }
}

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly AppDbContext _context;

    public UpdateUserHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync(new object[] { request.Id }, cancellationToken);
        if (user == null) return false;

        user.FullName = request.FullName;
        user.Email = request.Email;
        user.Role = request.Role;

        if (!string.IsNullOrEmpty(request.Password))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

