using System.Collections.Generic;

namespace StudentDraw.Models
{
    public class SchoolClass
    {
        public string ClassName { get; set; }
        public List<Student> Students { get; set; } = new List<Student>();
    }
}