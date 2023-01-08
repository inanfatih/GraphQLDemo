using GraphQLDemo.API.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace GraphQLDemo.API.Services.Instructors
{
    public class InstructorsRepository
    {
        private readonly IDbContextFactory<SchoolDbContext> _contextFactory;

        public InstructorsRepository(IDbContextFactory<SchoolDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<InstructorDTO> GetById(Guid id)
        {
            using (SchoolDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Instructors
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task<IEnumerable<InstructorDTO>> GetManyIds(IReadOnlyList<Guid> instructorIds)
        {
            using (SchoolDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Instructors
                    .Where(x => instructorIds.Contains(x.Id))
                    .ToListAsync();
            }
        }
    }
}
