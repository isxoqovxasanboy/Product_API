﻿using System.ComponentModel.DataAnnotations;

namespace Product_API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }
    }
}
