using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class TodoService : BaseService, ITodoService
    {
        private readonly ITodoRepository _repository;
        private readonly IMapper _mapper;

        public TodoService(ITodoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TodoDto>> GetAllTodos()
        {
            var todoList = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TodoDto>>(todoList);
        }

        public async Task<TodoDto> GetTodoById(int id)
        {
            var todo = await _repository.GetByIdyAsync(id);
            return _mapper.Map<TodoDto>(todo);
        }
        public async Task<TodoDto> CreateTodo(TodoDto todoDto)
        {
            if (todoDto is null)
                return null;

            var todo = _mapper.Map<Todo>(todoDto);
            todo.CreatedAt = DateTime.Now;
            todo = await _repository.CreateAsync(todo);
            return _mapper.Map<TodoDto>(todo);
        }

        public async Task<TodoDto> UpdateTodo(TodoDto todoDto)
        {
            var todo = _mapper.Map<Todo>(todoDto);
            todo.AlteratedAt = DateTime.Now;
            todo = await _repository.UpdateAsync(todo);
            return _mapper.Map<TodoDto>(todo);
        }

        public async Task<bool> DeleTeTodoById(int id) =>
            await _repository.DeleteAsync(id);

        public async Task<bool?> IsDoneTodo(int id) =>
             await _repository.IsDoneStatus(id);


    }
}
