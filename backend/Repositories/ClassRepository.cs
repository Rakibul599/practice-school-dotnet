using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class ClassRepository : IClassRepository
{
    private readonly AppDbContext _context;

    public ClassRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SchoolClass> CreateClassAsync(SchoolClass schoolClass)
    {
        await _context.SchoolClasses.AddAsync(schoolClass);
        await _context.SaveChangesAsync();
        return schoolClass;
    }

    public async Task<SchoolClass?> GetClassByIdAsync(string id)
    {
        return await _context.SchoolClasses.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> ClassExistsByNameAsync(string name)
    {
        return await _context.SchoolClasses.AnyAsync(c => c.Name.ToLower() == name.Trim().ToLower());
    }

    public async Task<(List<SchoolClass>Items,int TotalCount)> GetAllClassesWithHoldsAsync(string? name, bool OnlyAvailable,int page, int pageSize)
    {
        IQueryable<SchoolClass> query = _context.SchoolClasses.AsNoTracking().Include(c => c.Holds);

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(c => EF.Functions.ILike(c.Name, $"%{name}%"));
        }

        if (OnlyAvailable)
        {
            query = query.Where(c => c.TotalSeats - c.Holds.Count(h => h.Status == "CONFIRMED" || (h.Status == "HELD" && h.ExpiresAt > DateTime.UtcNow)) > 0);
        }
        var totalCount=await query.CountAsync();
        var items=await query.Skip((page-1)*pageSize).Take(pageSize).ToListAsync();

        return (items,totalCount);
        // return await query.ToListAsync();
        // Previous code
        // return await _context.SchoolClasses
        //     .Include(c => c.Holds)
        //     .OrderBy(c => c.CreatedAt)
        //     .ToListAsync();
    }

    public async Task<SchoolClass?> GetClassWithHoldsByIdAsync(string id)
    {
        return await _context.SchoolClasses
            .Include(c => c.Holds)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}