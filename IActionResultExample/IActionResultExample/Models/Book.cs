using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Models
{
    public class Book
    {
        //[FromQuery]
        public int? bookId { get; set; }
        public string? Author { get; set; }

        public override string ToString()
        {
            return $"Book id is {bookId} and Author is {Author}";
        }
    }
}
