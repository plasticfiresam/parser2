﻿using System.Collections.Generic;

namespace course.Entities
{
    public class Seller
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public List<Product> Products { get; set; } 
    }
}
