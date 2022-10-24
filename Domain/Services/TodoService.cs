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

        public async Task<IEnumerable<TodoDto>> GetAllTodos(string userId)
        {
            var todoList = _repository.GetAllAsync().Result.Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedAt);
            return _mapper.Map<IEnumerable<TodoDto>>(todoList.Where(x => x.UserId == userId));
        }

        public async Task<TodoDto> GetTodoById(int id)
        {
            var todo = await _repository.GetByIdyAsync(id);
            return _mapper.Map<TodoDto>(todo);
        }
        public async Task<TodoDto> CreateTodo(TodoDto todoDto, string userId)
        {
            if (todoDto is null || userId is null)
                return null;

            var todo = _mapper.Map<Todo>(todoDto);
            todo.UserId = userId;
            todo.CreatedAt = DateTime.Now;
            todo = await _repository.CreateAsync(todo);
            return _mapper.Map<TodoDto>(todo);
        }

        public async Task<bool> CreateTodos(IEnumerable<TodoDto> todosDto, string userId)
        {
            if (todosDto is null || userId is null)
                return false;

            try
            {
                var todos = _mapper.Map<IEnumerable<Todo>>(todosDto);
                foreach (var item in todos)
                {
                    item.UserId = userId;
                    if (await _repository.GetByIdyAsync(item.Id) == null)
                    {
                        item.Id = 0;
                        item.CreatedAt = DateTime.Now;
                        await _repository.CreateAsync(item);
                    }
                    else
                    {
                        item.AlteratedAt = DateTime.Now;
                        await _repository.UpdateAsync(item);
                    }
                }
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
            
        }

        public async Task<TodoDto> UpdateTodo(TodoDto todoDto, string userId)
        {
            var todo = _mapper.Map<Todo>(todoDto);
            var oldTodo = await _repository.GetByIdyAsync(todo.Id);
            todo.CreatedAt = oldTodo.CreatedAt;
            todo.UserId = userId;
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
