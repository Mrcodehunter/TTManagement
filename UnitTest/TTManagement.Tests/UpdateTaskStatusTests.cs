using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;
using TTManagement.Data;
using TTManagement.Entities;
using TTManagement.Features.ProjectTasks;

namespace TTManagement.Tests;

[TestClass]
public class UpdateTaskStatusTests
{
    private AppDbContext _context;
    private Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private UpdateTaskStatusHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        _context = new AppDbContext(options);

        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _handler = new UpdateTaskStatusHandler(_context, _mockHttpContextAccessor.Object);
    }

    private void SetupUser(int userId, UserRole role)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Role, role.ToString())
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };
        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
    }

    [TestMethod]
    public async Task Employee_CanUpdateStatus_WhenAssigned()
    {
        // Arrange
        var user = new User { Id = 1, Role = UserRole.Employee, Email = "test@test.com", FullName = "Test" };
        var task = new ProjectTask 
        { 
            Id = 1, 
            Title = "Test Task", 
            AssignedToUserId = 1, 
            CreatedByUserId = 2,
            Status = ProjectTaskStatus.Todo 
        };
        
        _context.Users.Add(user);
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        SetupUser(1, UserRole.Employee);
        var command = new UpdateTaskStatusCommand(1, ProjectTaskStatus.InProgress);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsTrue(result);
        var updatedTask = await _context.Tasks.FindAsync(1);
        Assert.AreEqual(ProjectTaskStatus.InProgress, updatedTask!.Status);
    }

    [TestMethod]
    public async Task Manager_CanUpdateStatus_AnyTask()
    {
        // Arrange
        var task = new ProjectTask 
        { 
            Id = 1, 
            Title = "Test Task", 
            AssignedToUserId = 99, 
            CreatedByUserId = 2,
            Status = ProjectTaskStatus.Todo 
        };
        
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        SetupUser(2, UserRole.Manager);
        var command = new UpdateTaskStatusCommand(1, ProjectTaskStatus.Done);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsTrue(result);
        var updatedTask = await _context.Tasks.FindAsync(1);
        Assert.AreEqual(ProjectTaskStatus.Done, updatedTask!.Status);
    }
}

