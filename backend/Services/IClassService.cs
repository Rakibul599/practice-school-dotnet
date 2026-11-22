using backend.DTOs;

namespace backend.Services;

public interface IClassService
{
    Task<ClassResponseDto> CreateClassAsync(CreateClassRequestDto request);
    Task<PagedResponse<ClassDetailResponseDto>> GetAllClassesAsync(string? name, bool OnlyAvailable=false,int page=1, int pageSize=2);
    Task<ClassDetailResponseDto?> GetClassByIdAsync(string classId);
}