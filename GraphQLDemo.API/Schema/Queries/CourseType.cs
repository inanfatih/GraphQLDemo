using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Models;
using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphQLDemo.API.Schema.Queries
{
    public class CourseType
    {
        public CourseType(CourseDTO courseDTO)
        {
            Id = courseDTO.Id;
            Name = courseDTO.Name;
            Subject = courseDTO.Subject;
            Instructor = new InstructorType(courseDTO.Instructor);
            Subject = courseDTO.Subject;
            Students = courseDTO.Students?.Select(x => new StudentType(x));
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Subject Subject { get; set; }
        [GraphQLNonNullType]
        public InstructorType Instructor { get; set; }
        public IEnumerable<StudentType> Students { get; set; }

        /*
         {
            courses {
            name
            description
            }
         }
        */
        public string Description()
        {
            return $"description: {Name}";
        }

    }
}
