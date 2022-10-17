using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class TodoRepository : BaseRepository<Todo>, ITodoRepository
    {
        private readonly DbContext _dbContext;
        public TodoRepository(TodoContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool?> IsDoneStatus(int id)
        {
            var todo = await _dbContext.Set<Todo>().FindAsync(id);

            if (todo is null)
                return null;

            return todo.IsDone;
        }
    }
}
