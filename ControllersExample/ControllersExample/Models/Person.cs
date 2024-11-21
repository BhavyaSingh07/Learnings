using System.Globalization;

namespace ControllersExample.Models
{
    public class Person
    {
        public Guid Id { get; set; }
        public string? firstName { get; set; }
        public string? lastName {  get; set; }
        public int Age { get; set; }
    }
}
