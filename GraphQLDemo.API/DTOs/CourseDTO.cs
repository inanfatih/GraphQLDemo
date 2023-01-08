using System.Collections.Generic;
using System;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Schema.Mutations;

namespace GraphQLDemo.API.DTOs
{
    public class CourseDTO
    {
        public CourseDTO()
        {

        }

        public CourseDTO(CourseTypeInput courseInput)
        {
            Name = courseInput.Name;
            Subject = courseInput.Subject;
            InstructorId = courseInput.InstructorId;
        }

        public CourseDTO(Guid id, CourseTypeInput courseInput) : this(courseInput)
        {
            Id = id;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Subject Subject { get; set; }
        public Guid InstructorId { get; set; }
        public InstructorDTO Instructor { get; set; }
        public IEnumerable<StudentDTO> Students { get; set; }
    }
}
