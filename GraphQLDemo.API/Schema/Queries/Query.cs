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

        public async Task<IEnumerable<CourseType>> GetCoursesAsync()
        {
            IEnumerable<CourseDTO> courseDTOs = await _coursesRepository.GetAll();
            return courseDTOs.Select(x => new CourseType(x));
        }

        [UseDbContext(typeof(SchoolDbContext))]
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 5)]
        public IQueryable<CourseType> GetCoursesPaging([ScopedService] SchoolDbContext context)
        {
            return context.Courses.Select(c => new CourseType()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId
            });
        }

        // Cursor based pagination is recommended but offset based pagination is also available
        [UseDbContext(typeof(SchoolDbContext))]
        [UseOffsetPaging(IncludeTotalCount = true, DefaultPageSize = 5)]
        public IQueryable<CourseType> GetCoursesOffsetPagination([ScopedService] SchoolDbContext context)
        {
            return context.Courses.Select(c => new CourseType()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId
            });
        }

        [UseDbContext(typeof(SchoolDbContext))]
        [UseFiltering]
        public IQueryable<CourseType> GetCoursesFiltering([ScopedService] SchoolDbContext context)
        {
            return context.Courses.Select(c => new CourseType()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId
            });
        }

        [UseSorting]
        public async Task<IEnumerable<CourseType>> GetCoursesSortingAsync()
        {
            IEnumerable<CourseDTO> courseDTOs = await _coursesRepository.GetAll();
            return courseDTOs.Select(x => new CourseType(x));
        }

        [UseDbContext(typeof(SchoolDbContext))]
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 5)]
        [UseProjection]
        [UseFiltering] // Order of Filtering and paging matters. Paging must come before filtering
        [UseSorting]
        public IQueryable<CourseType> GetCoursesPagingFilteringSorting([ScopedService] SchoolDbContext context)
        {
            return context.Courses.Select(c => new CourseType()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId
            });
        }

        public async Task<CourseType> GetCourseByIdAsync(Guid id)
        {
            return new CourseType(await _coursesRepository.GetById(id));
        }

        [UseDbContext(typeof(SchoolDbContext))]
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 5)]
        [UseProjection]
        [UseFiltering] // Order of Filtering and paging matters. Paging must come before filtering
        [UseSorting]
        public IQueryable<StudentType> GetStudents([ScopedService] SchoolDbContext context)
        {
            return context.Students.Select(x => new StudentType(x));
        }

        [UseDbContext(typeof(SchoolDbContext))]
        public IQueryable<InstructorType> GetInstructors([ScopedService] SchoolDbContext context)
        {
            return context.Instructors.Select(x => new InstructorType(x));
        }

        // Cursor based pagination is the recommended way of pagination.
        // THIS WAY IS NOT EFFICIENT IN THE DB: PAGINATION WILL NOT BE USED WHEN QUERYING FROM THE DB
        // SO ALL RESULTS WILL BE RETRIEVED FROM THE DB FIRST AND THEN THE RESULTS WILL BE PAGINATED
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 5)]
        [UseFiltering]
        public async Task<IEnumerable<CourseType>> GetCoursesCurserPaginationAsync()
        {
            IEnumerable<CourseDTO> courseDTOs = await _coursesRepository.GetAll();
            return courseDTOs.Select(x => new CourseType(x));
        }

        [UseDbContext(typeof(SchoolDbContext))]
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 5)]
        [UseProjection]
        [UseFiltering]
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

        [UseFiltering]
        public async Task<IEnumerable<CourseType>> GetCoursesInefficientlyAsync()
        {
            IEnumerable<CourseDTO> courseDTOs = await _coursesRepository.GetAll();
            return courseDTOs.Select(x => new CourseType(x));
        }

        [UseDbContext(typeof(SchoolDbContext))]
        [UseFiltering]
        public IQueryable<CourseType> GetCoursesEfficiently([ScopedService] SchoolDbContext context)
        {
            return context.Courses.Select(c => new CourseType()
            {
                Id = c.Id,
                Name = c.Name,
                Subject = c.Subject,
                InstructorId = c.InstructorId
            });
        }
    }
}
