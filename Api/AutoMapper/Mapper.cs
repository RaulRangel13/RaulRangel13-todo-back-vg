using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AutoMapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Todo, TodoDto>().ReverseMap();
        }
    }
}
