using Domain.DTOs;
using Domain.Interfaces.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface ITodoService : IBaseService
    {
        Task<IEnumerable<TodoDto>> GetAllTodos();
        Task<TodoDto> GetTodoById(int id);
        Task<TodoDto> CreateTodo(TodoDto todoDto);
        Task<TodoDto> UpdateTodo(TodoDto todoDto);
        Task<bool> DeleTeTodoById(int id);
        Task<bool?> IsDoneTodo(int id);
    }
}
