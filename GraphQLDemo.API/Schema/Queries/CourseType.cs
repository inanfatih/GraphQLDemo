using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Models;
using GraphQLDemo.API.Services.Instructors;
using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Schema.Queries
{
    public class CourseType
    {
        public CourseType(CourseDTO courseDTO)
        {
            if (courseDTO == null)
            {
                return;
            }
            Id = courseDTO.Id;
            Name = courseDTO.Name;
            Subject = courseDTO.Subject;
            // Instructor = new InstructorType(courseDTO.Instructor);
            Subject = courseDTO.Subject;
            // Students = courseDTO.Students?.Select(x => new StudentType(x));
            InstructorId = courseDTO.InstructorId;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Subject Subject { get; set; }
        public Guid InstructorId { get; set; }

        [GraphQLNonNullType]
        public async Task<InstructorType> Instructor([Service] InstructorsRepository instructorsRepository) 
        {
            return new InstructorType(await instructorsRepository.GetById(InstructorId));
        }

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
