using Domain.Entities.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Todo : BaseEntity
    {
        public string Name { get; set; }
        public bool IsDone { get; set; }
        public string UserId { get; set; }
    }
}
