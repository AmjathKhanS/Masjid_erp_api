using PersonalDetailsAPI.Models.DTOs;

namespace PersonalDetailsAPI.Services;

public interface IPersonalDetailService
{
    Task<PaginatedResponse<PersonalDetailDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm = null);
    Task<PersonalDetailDto?> GetByIdAsync(int id);
    Task<PersonalDetailDto> CreateAsync(CreatePersonalDetailDto createDto);
    Task<PersonalDetailDto?> UpdateAsync(int id, UpdatePersonalDetailDto updateDto);
    Task<bool> DeleteAsync(int id);
}
