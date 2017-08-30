﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetcliWebApi.Dtos
{
    public class FoodCreateDto
    {
        [Required]
        public string Name { get; set; }
        public int Calories { get; set; }
        public DateTime Created { get; set; }
    }
}
