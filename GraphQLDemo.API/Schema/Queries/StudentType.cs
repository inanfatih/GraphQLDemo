using GraphQLDemo.API.DTOs;
using HotChocolate;
using System;

namespace GraphQLDemo.API.Schema.Queries
{
    public class StudentType
    {
        public StudentType(StudentDTO studentDTO)
        {
            if (studentDTO == null)
            {
                return;
            }
            Id = studentDTO.Id;
            FirstName = studentDTO.FirstName;
            LastName = studentDTO.LastName;
            Gpa = studentDTO.Gpa;
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [GraphQLName("gpa")]
        public string Gpa { get; set; }
    }
}
