using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TTManagement.Data;
using TTManagement.DTOs;
using TTManagement.Entities;

namespace TTManagement.Features.ProjectTasks;

public record GetTasksQuery(
    ProjectTaskStatus? Status, 
    int? AssignedToUserId, 
    int? TeamId, 
    DateTime? DueDate,
    string? SortBy,
    bool IsDescending = false,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PagedResult<ProjectTaskDto>>;

public class GetTasksHandler : IRequestHandler<GetTasksQuery, PagedResult<ProjectTaskDto>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetTasksHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResult<ProjectTaskDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Tasks
            .AsNoTracking()
            .AsQueryable();

        if (request.Status.HasValue)
            query = query.Where(t => t.Status == request.Status.Value);
        
        if (request.AssignedToUserId.HasValue)
            query = query.Where(t => t.AssignedToUserId == request.AssignedToUserId.Value);

        if (request.TeamId.HasValue)
            query = query.Where(t => t.TeamId == request.TeamId.Value);

        if (request.DueDate.HasValue)
            query = query.Where(t => t.DueDate.Date == request.DueDate.Value.Date);

        // Sorting
        query = request.SortBy?.ToLower() switch
        {
            "duedate" => request.IsDescending ? query.OrderByDescending(t => t.DueDate) : query.OrderBy(t => t.DueDate),
            "status" => request.IsDescending ? query.OrderByDescending(t => t.Status) : query.OrderBy(t => t.Status),
            "title" => request.IsDescending ? query.OrderByDescending(t => t.Title) : query.OrderBy(t => t.Title),
            _ => query.OrderByDescending(t => t.Id) // Default sort
        };

        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<ProjectTaskDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PagedResult<ProjectTaskDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}

