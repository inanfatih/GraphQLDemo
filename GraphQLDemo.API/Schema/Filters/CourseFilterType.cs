using GraphQLDemo.API.Schema.Queries;
using HotChocolate.Data.Filters;

namespace GraphQLDemo.API.Schema.Filters
{
    public class CourseFilterType : FilterInputType<CourseType>
    {
        protected override void Configure(IFilterInputTypeDescriptor<CourseType> descriptor)
        {
            // Filters in Students are ignored
            descriptor.Ignore(c => c.Students);
            base.Configure(descriptor);
        }
    }
}
