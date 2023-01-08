using GraphQLDemo.API.DTOs;
using System;

namespace GraphQLDemo.API.Schema.Queries
{
    public class InstructorType
    {
        public InstructorType(InstructorDTO instructorDTO)
        {
            Id = instructorDTO.Id;
            FirstName = instructorDTO.FirstName;
            LastName = instructorDTO.LastName;
            Salary = instructorDTO.Salary;
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Salary { get; set; }
    }
}
