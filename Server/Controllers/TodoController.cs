using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schiavello.Shared;
using Server.Core;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly DataContext _context;

    public TodoController(DataContext context)
    {
        _context = context;
    }

    [HttpGet("get-all")]
    public async Task<IEnumerable<TodoItem>> Get()
    {
        var todos = await _context.Todos.AsNoTracking().ToListAsync();

        return todos.Select(t => new TodoItem()
        {
            Id = t.Id,
            Status = t.Status,
            TaskName = t.TaskName
        }).ToList();
    }

    [HttpPost("save-todo")]
    public async Task<int> SaveTodo([FromBody] SaveTodoRequest todo)
    {
        if (todo.Id == 0)
        {
            var todoToAdd = new Todo();

            todoToAdd.Status = Statuses.New;
            todoToAdd.TaskName = todo.TaskName;

            await _context.AddAsync(todoToAdd);
            await _context.SaveChangesAsync();

            return todoToAdd.Id;
        }

        var todoToUpdate = await _context.Todos.FirstOrDefaultAsync(t => t.Id == todo.Id);

        todoToUpdate.Status = todo.Status;
        todoToUpdate.TaskName = todo.TaskName;

        await _context.SaveChangesAsync();

        return todoToUpdate.Id;
    }

    [HttpDelete("delete-todo/{id}")]
    public async Task Delete(int id)
    {
        var todoToDelete = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id);

        _context.Todos.Remove(todoToDelete);

        await _context.SaveChangesAsync();
    }

}
