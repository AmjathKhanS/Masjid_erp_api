using Microsoft.AspNetCore.Mvc;
using PersonalDetailsAPI.Models.DTOs;
using PersonalDetailsAPI.Services;

namespace PersonalDetailsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonalDetailsController : ControllerBase
{
    private readonly IPersonalDetailService _service;
    private readonly ILogger<PersonalDetailsController> _logger;

    public PersonalDetailsController(IPersonalDetailService service, ILogger<PersonalDetailsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all personal details with pagination and optional search
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="searchTerm">Search term for filtering by name, phone, email, or Aadhar</param>
    /// <returns>Paginated list of personal details</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<PersonalDetailDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
    {
        try
        {
            var result = await _service.GetAllAsync(pageNumber, pageSize, searchTerm);
            return Ok(ApiResponse<PaginatedResponse<PersonalDetailDto>>.SuccessResponse(
                result,
                "Personal details retrieved successfully"
            ));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving personal details");
            return StatusCode(500, ApiResponse<PaginatedResponse<PersonalDetailDto>>.ErrorResponse(
                "An error occurred while retrieving personal details",
                new List<string> { ex.Message }
            ));
        }
    }

    /// <summary>
    /// Get a personal detail by ID
    /// </summary>
    /// <param name="id">Personal detail ID</param>
    /// <returns>Personal detail</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<PersonalDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PersonalDetailDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound(ApiResponse<PersonalDetailDto>.ErrorResponse(
                    "Personal detail not found",
                    new List<string> { $"No record found with ID: {id}" }
                ));
            }

            return Ok(ApiResponse<PersonalDetailDto>.SuccessResponse(
                result,
                "Personal detail retrieved successfully"
            ));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving personal detail with ID: {Id}", id);
            return StatusCode(500, ApiResponse<PersonalDetailDto>.ErrorResponse(
                "An error occurred while retrieving the personal detail",
                new List<string> { ex.Message }
            ));
        }
    }

    /// <summary>
    /// Create a new personal detail
    /// </summary>
    /// <param name="createDto">Personal detail data</param>
    /// <returns>Created personal detail</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PersonalDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<PersonalDetailDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePersonalDetailDto createDto)
    {
        try
        {
            var result = await _service.CreateAsync(createDto);
            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                ApiResponse<PersonalDetailDto>.SuccessResponse(
                    result,
                    "Personal detail created successfully"
                )
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating personal detail");
            return StatusCode(500, ApiResponse<PersonalDetailDto>.ErrorResponse(
                "An error occurred while creating the personal detail",
                new List<string> { ex.Message }
            ));
        }
    }

    /// <summary>
    /// Update an existing personal detail
    /// </summary>
    /// <param name="id">Personal detail ID</param>
    /// <param name="updateDto">Updated personal detail data</param>
    /// <returns>Updated personal detail</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<PersonalDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PersonalDetailDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<PersonalDetailDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePersonalDetailDto updateDto)
    {
        try
        {
            var result = await _service.UpdateAsync(id, updateDto);

            if (result == null)
            {
                return NotFound(ApiResponse<PersonalDetailDto>.ErrorResponse(
                    "Personal detail not found",
                    new List<string> { $"No record found with ID: {id}" }
                ));
            }

            return Ok(ApiResponse<PersonalDetailDto>.SuccessResponse(
                result,
                "Personal detail updated successfully"
            ));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating personal detail with ID: {Id}", id);
            return StatusCode(500, ApiResponse<PersonalDetailDto>.ErrorResponse(
                "An error occurred while updating the personal detail",
                new List<string> { ex.Message }
            ));
        }
    }

    /// <summary>
    /// Delete a personal detail (soft delete)
    /// </summary>
    /// <param name="id">Personal detail ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(
                    "Personal detail not found",
                    new List<string> { $"No record found with ID: {id}" }
                ));
            }

            return Ok(ApiResponse<object?>.SuccessResponse(
                null,
                "Personal detail deleted successfully"
            ));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting personal detail with ID: {Id}", id);
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "An error occurred while deleting the personal detail",
                new List<string> { ex.Message }
            ));
        }
    }
}
