using System;
using System.Collections.Generic;

#nullable disable

namespace FastCore.Repositories.Entities
{
    public class Book
    {
        public int BookId { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Isbn { get; set; }
        public DateTime ReleaseDate { get; set; }
        public virtual Author Author { get; set; }
        public virtual Category Category { get; set; }
    }
}
