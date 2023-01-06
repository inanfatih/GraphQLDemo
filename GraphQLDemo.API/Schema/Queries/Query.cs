using Bogus;
using HotChocolate;
using System;
using System.Collections;
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

        public IEnumerable<CourseType> GetCourses()
        {
            List<CourseType> courses = _courseFaker.Generate(1);
            return courses;
        }

        public async Task<CourseType> GetCourseById(Guid id)
        {
            await Task.Delay(1000);
            var course = _courseFaker.Generate();
            course.Id = id;
            return course;
        }
    }
}
