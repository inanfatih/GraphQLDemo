using GraphQLDemo.API.DataLoaders;
using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Models;
using HotChocolate;
using System;
using System.Collections.Generic;
using System.Threading;
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
        public async Task<InstructorType> Instructor([Service] InstructorDataLoader instructorDataLoader) 
        {
            //DataLoader sayesinde InstructorId'leri biriktirip hepsi icin birden instructorDataLoader icinde tek query yapiliyor.
            return new InstructorType(await instructorDataLoader.LoadAsync(InstructorId, CancellationToken.None));
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
