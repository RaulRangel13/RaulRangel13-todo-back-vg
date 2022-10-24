using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{

    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;
        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        private string? GetUserId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            return identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoDto>>> GetAll()
        {
            var userId = GetUserId();

            if (userId is null)
                return BadRequest();

            return Ok(await _todoService.GetAllTodos(userId));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoDto>> GetTodoById(int id)
        {
            var todo = await _todoService.GetTodoById(id);

            if (todo is null)
                return BadRequest();

            return Ok(todo);
        }

        [HttpPost]
        public async Task<ActionResult<TodoDto>> Post([FromBody] TodoDto todoDto)
        {
            var userId = GetUserId();

            if (userId is null)
                return BadRequest();

            var todoResult = await _todoService.CreateTodo(todoDto, userId);

            if (todoResult is null)
                return BadRequest();

            return Ok(todoResult);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> PostTodos([FromBody] IEnumerable<TodoDto> todosDto)
        {
            var userId = GetUserId();

            if (userId is null)
                return BadRequest();

            var todoResult = await _todoService.CreateTodos(todosDto, userId);

            if (!todoResult)
                return BadRequest();

            return Ok(todoResult);
        }

        [HttpPut]
        public async Task<ActionResult<TodoDto>> Put([FromBody] TodoDto todoDto)
        {
            var userId = GetUserId();

            if (userId is null)
                return BadRequest();

            var todoResult = await _todoService.UpdateTodo(todoDto, userId);

            if (todoResult is null)
                return BadRequest();

            return Ok(todoResult);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id) =>
            Ok(await _todoService.DeleTeTodoById(id));
    }
}
