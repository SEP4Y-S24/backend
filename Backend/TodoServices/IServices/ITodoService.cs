﻿using TodoServices.Model;

namespace TodoServices.IServices;

public interface ITodoService
{
    Task<Todo> CreateAsync(Todo todoToCreate);
    Task<Todo?> GetByIdAsync(Guid todoId);
    Task UpdateAsync(Todo todo);
    Task DeleteAsync(Guid todoId);
    Task<IEnumerable<Todo>> GetAllAsync();
}