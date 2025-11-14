using AutoMapper;
using PersonalDetailsAPI.Models.DTOs;
using PersonalDetailsAPI.Models.Entities;
using PersonalDetailsAPI.Repositories;

namespace PersonalDetailsAPI.Services;

public class PersonalDetailService : IPersonalDetailService
{
    private readonly IPersonalDetailRepository _repository;
    private readonly IMapper _mapper;

    public PersonalDetailService(IPersonalDetailRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedResponse<PersonalDetailDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm = null)
    {
        var personalDetails = await _repository.GetAllAsync(pageNumber, pageSize, searchTerm);
        var totalCount = await _repository.GetTotalCountAsync(searchTerm);

        var personalDetailDtos = _mapper.Map<List<PersonalDetailDto>>(personalDetails);

        return new PaginatedResponse<PersonalDetailDto>
        {
            Data = personalDetailDtos,
            TotalRecords = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<PersonalDetailDto?> GetByIdAsync(int id)
    {
        var personalDetail = await _repository.GetByIdAsync(id);
        return personalDetail == null ? null : _mapper.Map<PersonalDetailDto>(personalDetail);
    }

    public async Task<PersonalDetailDto> CreateAsync(CreatePersonalDetailDto createDto)
    {
        var personalDetail = _mapper.Map<PersonalDetail>(createDto);
        var createdPersonalDetail = await _repository.CreateAsync(personalDetail);
        return _mapper.Map<PersonalDetailDto>(createdPersonalDetail);
    }

    public async Task<PersonalDetailDto?> UpdateAsync(int id, UpdatePersonalDetailDto updateDto)
    {
        var existingPersonalDetail = await _repository.GetByIdAsync(id);
        if (existingPersonalDetail == null)
            return null;

        _mapper.Map(updateDto, existingPersonalDetail);
        existingPersonalDetail.Id = id; // Ensure ID is not changed

        // Update wife details
        existingPersonalDetail.WifeDetails.Clear();
        foreach (var wifeDto in updateDto.WifeDetails)
        {
            var wife = _mapper.Map<WifeDetail>(wifeDto);
            wife.PersonalDetailId = id;
            existingPersonalDetail.WifeDetails.Add(wife);
        }

        // Update child details
        existingPersonalDetail.ChildDetails.Clear();
        foreach (var childDto in updateDto.ChildDetails)
        {
            var child = _mapper.Map<ChildDetail>(childDto);
            child.PersonalDetailId = id;
            existingPersonalDetail.ChildDetails.Add(child);
        }

        var updatedPersonalDetail = await _repository.UpdateAsync(existingPersonalDetail);
        return _mapper.Map<PersonalDetailDto>(updatedPersonalDetail);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}
