using Microsoft.EntityFrameworkCore;
using PersonalDetailsAPI.Data;
using PersonalDetailsAPI.Models.Entities;

namespace PersonalDetailsAPI.Repositories;

public class PersonalDetailRepository : IPersonalDetailRepository
{
    private readonly ApplicationDbContext _context;

    public PersonalDetailRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PersonalDetail>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm = null)
    {
        var query = _context.PersonalDetails
            .Include(p => p.WifeDetails)
            .Include(p => p.ChildDetails)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p =>
                p.FullName.Contains(searchTerm) ||
                p.PhoneNumber.Contains(searchTerm) ||
                p.EmailId.Contains(searchTerm) ||
                p.AadharNumber.Contains(searchTerm)
            );
        }

        return await query
            .OrderByDescending(p => p.CreatedDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(string? searchTerm = null)
    {
        var query = _context.PersonalDetails.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p =>
                p.FullName.Contains(searchTerm) ||
                p.PhoneNumber.Contains(searchTerm) ||
                p.EmailId.Contains(searchTerm) ||
                p.AadharNumber.Contains(searchTerm)
            );
        }

        return await query.CountAsync();
    }

    public async Task<PersonalDetail?> GetByIdAsync(int id)
    {
        return await _context.PersonalDetails
            .Include(p => p.WifeDetails)
            .Include(p => p.ChildDetails)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<PersonalDetail> CreateAsync(PersonalDetail personalDetail)
    {
        _context.PersonalDetails.Add(personalDetail);
        await _context.SaveChangesAsync();
        return personalDetail;
    }

    public async Task<PersonalDetail> UpdateAsync(PersonalDetail personalDetail)
    {
        _context.Entry(personalDetail).State = EntityState.Modified;

        // Handle wife details
        var existingWives = await _context.WifeDetails
            .Where(w => w.PersonalDetailId == personalDetail.Id)
            .ToListAsync();

        // Remove wives that are not in the updated list
        foreach (var existingWife in existingWives)
        {
            if (!personalDetail.WifeDetails.Any(w => w.Id == existingWife.Id))
            {
                _context.WifeDetails.Remove(existingWife);
            }
        }

        // Handle child details
        var existingChildren = await _context.ChildDetails
            .Where(c => c.PersonalDetailId == personalDetail.Id)
            .ToListAsync();

        // Remove children that are not in the updated list
        foreach (var existingChild in existingChildren)
        {
            if (!personalDetail.ChildDetails.Any(c => c.Id == existingChild.Id))
            {
                _context.ChildDetails.Remove(existingChild);
            }
        }

        await _context.SaveChangesAsync();
        return personalDetail;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var personalDetail = await _context.PersonalDetails.FindAsync(id);
        if (personalDetail == null)
            return false;

        // Soft delete
        personalDetail.IsDeleted = true;
        personalDetail.DeletedDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.PersonalDetails.AnyAsync(p => p.Id == id);
    }
}
