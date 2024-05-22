﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Models;
using Shared.Context;
using Shared.IDAO;

namespace Shared.DAOImplementation;

public class TodoDao: ITodoDAO
{
    private readonly ClockContext context;
    
    public TodoDao(ClockContext dbContext)
    {
        context = dbContext;
    }
    
    public async Task<Todo> CreateAsync(Todo todo)
    {
        EntityEntry<Todo> added = await context.Todos.AddAsync(todo);
        await context.SaveChangesAsync();
        return added.Entity;
    }

    public async Task UpdateAsync(Todo todo)
    {
        Todo? dbEntity = await GetByIdAsync(todo.Id);

        if (dbEntity == null)
        {
            throw new ArgumentException();
        }

        context.Entry(dbEntity).CurrentValues.SetValues(todo);

        context.Todos.Update(dbEntity);

        await context.SaveChangesAsync();
    }

    public async Task UpdateStatusByIdAsync(Guid todoId, Status status)
    {
        Todo? dbEntity = await GetByIdAsync(todoId);
        if (dbEntity == null)
        {
            throw new ArgumentException();
        }

        dbEntity.Status = status;
        context.Todos.Update(dbEntity);
        await context.SaveChangesAsync();
    }

    public Task<Todo?> GetByIdAsync(Guid todoId)
    {
        if (todoId.Equals(Guid.Empty))
        {
            throw new ArgumentNullException("The given Todo ID is null");
        }
        
        Todo? existing = context.Todos.Include(t=>t.Tags).FirstOrDefault(todo => todo.Id == todoId);
        return Task.FromResult(existing);
    }

    public async Task DeleteAsync(Guid todoId)
    {
        if (todoId.Equals(Guid.Empty))
        {
            throw new ArgumentNullException("Todo ID is null");
        }
        
        Todo? toDelete = await GetByIdAsync(todoId);
        context.Todos.Remove(toDelete);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Todo>> GetAllAsync()
    {
        return await context.Set<Todo>().Include(t=>t.Tags).ToListAsync();
    }

    public async Task<IEnumerable<Todo>> GetAllByAsync(Expression<Func<Todo, bool>> filter)
    {
        return await context.Set<Todo>().Include(t=>t.Tags).Where(filter).ToListAsync();
    }
    
}