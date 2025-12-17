using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TTManagement.Data;
using TTManagement.DTOs;
using TTManagement.Entities;

namespace TTManagement.Features.Teams;

public record GetTeamsQuery(
    string? SearchTerm,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PagedResult<TeamDto>>;

public class GetTeamsHandler : IRequestHandler<GetTeamsQuery, PagedResult<TeamDto>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetTeamsHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResult<TeamDto>> Handle(GetTeamsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Teams.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(t => t.Name.Contains(request.SearchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .OrderBy(t => t.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<TeamDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PagedResult<TeamDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}

