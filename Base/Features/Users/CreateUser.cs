using MediatR;
using TTManagement.Data;
using TTManagement.Entities;
using FluentValidation;
using BCrypt.Net;

namespace TTManagement.Features.Users;

public record CreateUserCommand(
    string FullName,
    string Email,
    string Password,
    UserRole Role
) : IRequest<int>;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.FullName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.Role).IsInEnum();
    }
}

public class CreateUserHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly AppDbContext _context;

    public CreateUserHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            Role = request.Role,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
}

