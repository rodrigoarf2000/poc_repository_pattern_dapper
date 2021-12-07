using System;
using System.Collections.Generic;

#nullable disable

namespace FastCore.Repositories.Entities
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public virtual List<Book> Books { get; set; }
    }
}
