﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class CategoryDM
    {
        public Guid Id { get; set; }
        public string Name { get; set; } // Required
        public string Description { get; set; }
    }
}