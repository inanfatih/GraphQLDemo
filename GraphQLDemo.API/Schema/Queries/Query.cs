using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Schema.Filters;
using GraphQLDemo.API.Services;
using GraphQLDemo.API.Services.Courses;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
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
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 5)]
        [UseFiltering]
        [UseSorting]
        public async Task<IEnumerable<CourseType>> GetCoursesAsync()
        {
            IEnumerable<CourseDTO> courseDTOs = await _coursesRepository.GetAll();
            return courseDTOs.Select(x => new CourseType(x));
        }

        /*
         query {
          coursesCurserPagination (first: 3, after: "Mg==") {
            edges {
              node {
                id
                name
                subject
                instructorId
              }
              cursor
            }
            pageInfo {
              hasNextPage
              hasPreviousPage
              startCursor
              endCursor
            }
            totalCount
          }
        }
        */
        // Cursor based pagination is the recommended way of pagination.
        // THIS WAY IS NOT EFFICIENT IN THE DB: PAGINATION WILL NOT BE USED WHEN QUERYING FROM THE DB
        // SO ALL RESULTS WILL BE RETRIEVED FROM THE DB FIRST AND THEN THE RESULTS WILL BE PAGINATED
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 5)]
        [UseFiltering] // Order of Filtering and paging matters. Paging must come before filtering
        public async Task<IEnumerable<CourseType>> GetCoursesCurserPaginationAsync()
        {
            IEnumerable<CourseDTO> courseDTOs = await _coursesRepository.GetAll();
            return courseDTOs.Select(x => new CourseType(x));
        }

        /*
        {
          paginatedFilteredSortedCoursesAsync(first: 3, 
            where: {
              or: [
                {
                  name: {
                    contains: "hay"
                  }
                },
                {
                  subject: {
                    eq: MATHEMATICS
                  }
                }
              ]
            },
            order: {
              name:DESC
              subject: ASC
            })
            {
              edges {
                node {
                  id
                  name
                  subject
                }
                cursor
            }
            pageInfo {
              endCursor
            }
            totalCount
          }
        }
        */
        // HOT CHOCOLATE WILL APPLY THE PAGING AND FILTERING IN THE QUERY SO THIS WAY IS MORE EFFICIENT AND MORE PERFORMANT IN THE DB
        [UseDbContext(typeof(SchoolDbContext))]
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 5)]
        [UseProjection] // BUNUNLA ALANLAR DIREKT DATABASE'E GONDERILIYOR VE SADECE ONLAR QUERY EDILIYOR
        [UseFiltering/*(typeof(CourseFilterType))*/] // Order of Filtering and paging matters. Paging must come before filtering
        [UseSorting]
        public IQueryable<CourseType> GetPaginatedFilteredSortedCoursesAsync([ScopedService] SchoolDbContext context)
        {
            return context.Courses.Select(c => new CourseType()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId
            });
        }

        /*
        query {
            coursesOffsetPagination (skip: 2, take: 1) {
            items {
                id 
                name
                subject
                instructorId 
                __typename
            }
            pageInfo {
                __typename
                hasNextPage
                hasPreviousPage
            }
            totalCount
            }
        }
        */
        // Cursor based pagination is recommended but offset based pagination is also available
        [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 5)]
        public async Task<IEnumerable<CourseType>> GetCoursesOffsetPaginationAsync()
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
