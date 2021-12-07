using System;
using System.Collections.Generic;

#nullable disable

namespace FastCore.Repositories.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public virtual List<Book> Books { get; set; }
    }
}
