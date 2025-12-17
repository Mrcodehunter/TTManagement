using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TTManagement.Data;
using TTManagement.DTOs;
using TTManagement.Entities;

namespace TTManagement.Features.Users;

public record GetUsersQuery(
    string? SearchTerm,
    UserRole? Role,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PagedResult<UserDto>>;

public class GetUsersHandler : IRequestHandler<GetUsersQuery, PagedResult<UserDto>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetUsersHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResult<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(u => u.FullName.Contains(request.SearchTerm) || u.Email.Contains(request.SearchTerm));
        }

        if (request.Role.HasValue)
        {
            query = query.Where(u => u.Role == request.Role.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .OrderBy(u => u.FullName)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PagedResult<UserDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}

