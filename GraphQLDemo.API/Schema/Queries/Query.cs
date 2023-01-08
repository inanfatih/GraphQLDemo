using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Services.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.API.Schema.Queries
{
    public class Query
    {
        private readonly CoursesRepository _coursesRepository;

        public Query(CoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
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
        public async Task<IEnumerable<CourseType>> GetCoursesAsync()
        {
            IEnumerable<CourseDTO> courseDTOs = await _coursesRepository.GetAll();
            return courseDTOs.Select(x => new CourseType(x));
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
        public async Task<CourseType> GetCourseByIdAsync(Guid id)
        {
            return new CourseType(await _coursesRepository.GetById(id));
        }
    }
}
