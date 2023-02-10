using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FirebaseAdminAuthentication.DependencyInjection.Models;
using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Schema.Queries;
using GraphQLDemo.API.Schema.Subscriptions;
using GraphQLDemo.API.Services.Courses;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Subscriptions;

namespace GraphQLDemo.API.Schema.Mutations
{
    public class Mutation
    {
        private readonly CoursesRepository _coursesRepository;

        public Mutation(CoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
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

        // [Authorize]
        public async Task<CourseResult> CreateCourse(
            CourseTypeInput courseInput, 
            [Service] ITopicEventSender topicEventSender,
            ClaimsPrincipal claimsPrincipal)
        {
            string userId = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.ID);
            string email = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.EMAIL);
            string userName = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.USERNAME);
            string emailVerified = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.EMAIL_VERIFIED);

            CourseDTO courseDTO = new CourseDTO(courseInput);
            courseDTO = await _coursesRepository.Create(courseDTO);

            CourseResult course = new(courseDTO);

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

        [Authorize]
        public async Task<CourseResult> UpdateCourse(Guid id, CourseTypeInput courseInput, [Service] ITopicEventSender topicEventSender)
        {
            CourseDTO courseDTO = new(id, courseInput);

            //CourseResult course = _courses.FirstOrDefault(x => x.Id == id);
            //if (course == null)
            //{
            //    throw new GraphQLException(new Error("Course not found", "COURSE_NOT_FOUND"));
            //}
            
            await _coursesRepository.Update(courseDTO);

            CourseResult course = new(courseDTO);

            string updateCourseTopic = $"{course.Id}_{nameof(Subscription.CourseUpdated)}";
            await topicEventSender.SendAsync(updateCourseTopic, course);

            return course;
        }

        /*
        mutation {
          deleteCourse(id: "4ebcd94d-3a1d-40c5-ad40-451aa2bda685")
        }
        */
        [Authorize]
        public async Task<bool> DeleteCourse(Guid id)
        {
            try
            {
                return await _coursesRepository.Delete(id);
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
