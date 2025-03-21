using Application.Features.Schools;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Schools
{
    public class SchoolService(ApplicationDbContext context) : ISchoolService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<int> CreateSchoolAsync(School school)
        {
            await _context.Schools.AddAsync(school);
            await _context.SaveChangesAsync();
            return school.Id;
        }

        public async Task<int> DeleteSchoolAsync(School school)
        {
            _context.Schools.Remove(school);
            await _context.SaveChangesAsync();
            return school.Id;
        }

        public async Task<School> GetSchoolByIdAsync(int schoolId)
        {
            var schoolInDb = await _context.Schools
                .Where(s => s.Id == schoolId)
                .FirstOrDefaultAsync();
            return schoolInDb;
        }

        public async Task<School> GetSchoolByNameAsync(string schoolName)
        {
            var schoolInDb = await _context.Schools
                .Where(s => s.Name.ToUpper() == schoolName.ToUpper())
                .FirstOrDefaultAsync();
            return schoolInDb;
        }

        public async Task<List<School>> GetSchoolsAsync()
        {
            var schoolsInDb = await _context.Schools
                .ToListAsync();
            return schoolsInDb;
        }

        public async Task<int> UpdateSchoolAsync(School school)
        {
            _context.Schools.Update(school);
            await _context.SaveChangesAsync();
            return school.Id;
        }
    }
}
