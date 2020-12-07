﻿using System.Collections.Generic;

namespace course.Entities
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}
