using System;
using System.Collections.Generic;
using System.Text;

namespace Isu.Entities
{
    public class Student
    {
        public Student(string name, int id)
        {
            Name = name;
            Id = id;
        }
        public int Id { get; }
        public string Name { get; }

        public CourseNumber Course { get; }
       
    }
}
