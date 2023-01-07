using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQLDemo.API.Schema.Queries;
using GraphQLDemo.API.Schema.Subscriptions;
using HotChocolate;
using HotChocolate.Subscriptions;

namespace GraphQLDemo.API.Schema.Mutations
{
    public class Mutation
    {
        private readonly List<CourseResult> _courses;
        public Mutation()
        {
            _courses = new List<CourseResult>();
        }

        /**
          mutation {
          createCourse (courseInput: {
            instructorId: "666ab08e-1438-47b2-a8f5-21ed717c1da9", name: "haydar", subject: MATHEMATICS
          })
          {
            id
            name
          }
        }
         */
        public async Task<CourseResult> CreateCourse(CourseInputType courseInput, [Service] ITopicEventSender topicEventSender)
        {
            CourseResult course = new CourseResult()
            {
                Id = Guid.NewGuid(),
                Name = courseInput.Name,
                Subject = courseInput.Subject,
                InstructorId = courseInput.InstructorId
            };
            _courses.Add(course);
            await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), course);
            return course;
        }

        /*
        mutation
        {
            updateCourse (id: "911dfc8d-1b86-428d-897d-eeb416bf1359", 
                courseInput : { name: "serafetting", instructorId: "10aa9637-2d13-436b-b4f6-d2d3a23afd49", subject: MATHEMATICS }) 
            {
                id
                name
                subject
            }
        }
         */
        public async Task<CourseResult> UpdateCourse(Guid id, CourseInputType courseInput, [Service] ITopicEventSender topicEventSender)
        {
            CourseResult course = _courses.FirstOrDefault(x => x.Id == id);
            if (course == null)
            {
                throw new GraphQLException(new Error("Course not found", "COURSE_NOT_FOUND"));
            }
            course.Subject = courseInput.Subject;
            course.InstructorId = courseInput.InstructorId;
            course.Name = courseInput.Name;

            string updateCourseTopic = $"{course.Id}_{nameof(Subscription.CourseUpdated)}";
            await topicEventSender.SendAsync(updateCourseTopic, course);

            return course;
        }

        /*
        mutation {
          deleteCourse(id: "4ebcd94d-3a1d-40c5-ad40-451aa2bda685")
        }
        */
        public bool DeleteCourse(Guid id)
        {
            return _courses.RemoveAll(c => c.Id == id) > 0;
        }
    }
}
