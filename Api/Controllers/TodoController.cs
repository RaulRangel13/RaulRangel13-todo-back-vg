using Domain.DTOs;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;
        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoDto>>> GetAll()
        {
            return Ok(await _todoService.GetAllTodos());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoDto>> GetById(int id)
        {
            var todo = await _todoService.GetTodoById(id);

            if (todo is null)
                return BadRequest();

            return Ok(todo);
        }

        [HttpPost]
        public async Task<ActionResult<TodoDto>> Post([FromBody] TodoDto todoDto)
        {
            var todoResult = await _todoService.CreateTodo(todoDto);

            if (todoResult is null)
                return BadRequest();

            return Ok(todoResult);
        }

        [HttpPut]
        public async Task<ActionResult<TodoDto>> Put([FromBody] TodoDto todoDto)
        {
            var todoResult = await _todoService.UpdateTodo(todoDto);

            if (todoResult is null)
                return BadRequest();

            return Ok(todoResult);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id) =>
            Ok(await _todoService.DeleTeTodoById(id));
    }
}
