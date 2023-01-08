﻿using Bogus;
using GraphQLDemo.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Schema.Queries
{
    public class Query
    {
        private readonly Faker<CourseType> _courseFaker;
        private readonly Faker<StudentType> _studentFaker;
        private readonly Faker<InstructorType> _instructorFaker;

        public Query()
        {
            _instructorFaker = new Faker<InstructorType>()
                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => f.Name.LastName());

            _studentFaker = new Faker<StudentType>()
                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => f.Name.LastName());

            _courseFaker = new Faker<CourseType>()
                .RuleFor(x => x.Id, f => Guid.NewGuid())
                .RuleFor(x => x.Subject, f => f.PickRandom<Subject>())
                .RuleFor(x => x.Instructor, f => _instructorFaker.Generate())
                .RuleFor(x => x.Students, f => _studentFaker.Generate(5))
                .RuleFor(x => x.Name, f => f.Name.JobTitle());
        }

        /*
         * query {
            courses {
              id
              name
              subject
              instructor {
                id
                firstName
                lastName
                salary
              }
              students {
                id
                firstName
                lastName
                gpa
                id
              }
            }
        }
         */
        public IEnumerable<CourseType> GetCourses()
        {
            List<CourseType> courses = _courseFaker.Generate(1);
            return courses;
        }

        /*
        {
          courseById (id: "666ab08e-1438-47b2-a8f5-21ed717c1da9") {
            name
            instructor {
              id
              firstName
            }
            students {
              firstName
            }
          }
        }
        */
        public async Task<CourseType> GetCourseById(Guid id)
        {
            await Task.Delay(1000);
            var course = _courseFaker.Generate();
            course.Id = id;
            return course;
        }
    }
}
