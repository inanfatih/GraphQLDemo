using GraphQLDemo.API.Schema.Queries;
using HotChocolate.Data.Sorting;

namespace GraphQLDemo.API.Schema.Sorters
{
    public class CourseSortType : SortInputType<CourseType>
    {
        protected override void Configure(ISortInputTypeDescriptor<CourseType> descriptor)
        {
            descriptor
                .Ignore(c => c.Id)
                .Ignore(c => c.InstructorId)
                .Field(c => c.Name).Name("CourseName"); // Bu sekilde sort icin kullanilan field ismini degistirebiliyoruz
            base.Configure(descriptor);
        }
    }
}
