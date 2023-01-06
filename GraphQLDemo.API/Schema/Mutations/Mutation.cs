using System;
using System.Collections.Generic;
using System.Linq;
using GraphQLDemo.API.Schema.Queries;
using HotChocolate;

namespace GraphQLDemo.API.Schema.Mutations
{
    public class Mutation
    {
        private readonly List<CourseResult> _courses;
        public Mutation()
        {
            _courses = new List<CourseResult>();
        }

        public CourseResult CreateCourse(string name, CourseInputType courseInput)
        {
            CourseResult courseType = new CourseResult()
            {
                Id = Guid.NewGuid(),
                Name = courseInput.Name,
                Subject = courseInput.Subject,
                InstructorId = courseInput.InstructorId
            };
            _courses.Add(courseType);
            return courseType;
        }

        public CourseResult UpdateCourse(Guid id, CourseInputType courseInput)
        {
            CourseResult course = _courses.FirstOrDefault(x => x.Id == id);
            if (course == null)
            {
                throw new GraphQLException(new Error("Course not found", "COURSE_NOT_FOUND"));
            }
            course.Subject = courseInput.Subject;
            course.InstructorId = courseInput.InstructorId;
            course.Name = courseInput.Name;

            return course;
        }

        public bool DeleteCourse(Guid id)
        {
            return _courses.RemoveAll(c => c.Id == id) > 0;
        }
    }
}
