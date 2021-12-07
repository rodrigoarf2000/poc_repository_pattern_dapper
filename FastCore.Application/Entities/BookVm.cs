using System;

namespace FastCore.Application.Entities
{
    public class BookVm
    {
        public int BookId { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Isbn { get; set; }        
        public DateTime ReleaseDate { get; set; }
        public AuthorVm Author { get; set; }
        public CategoryVm Category { get; set; }
    }
}
