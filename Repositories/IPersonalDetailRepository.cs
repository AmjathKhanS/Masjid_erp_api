using PersonalDetailsAPI.Models.Entities;

namespace PersonalDetailsAPI.Repositories;

public interface IPersonalDetailRepository
{
    Task<IEnumerable<PersonalDetail>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm = null);
    Task<int> GetTotalCountAsync(string? searchTerm = null);
    Task<PersonalDetail?> GetByIdAsync(int id);
    Task<PersonalDetail> CreateAsync(PersonalDetail personalDetail);
    Task<PersonalDetail> UpdateAsync(PersonalDetail personalDetail);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
