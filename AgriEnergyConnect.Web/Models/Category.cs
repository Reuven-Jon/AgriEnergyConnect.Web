﻿// /Models/Category.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.Web.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        // Navigation
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}

