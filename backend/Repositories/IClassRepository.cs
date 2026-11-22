using backend.Models;

namespace backend.Repositories;

public interface IClassRepository
{
    Task<SchoolClass> CreateClassAsync(SchoolClass schoolClass);
    Task<SchoolClass?> GetClassByIdAsync(string id);
    Task<bool> ClassExistsByNameAsync(string name);
    Task<(List<SchoolClass>Items,int TotalCount)> GetAllClassesWithHoldsAsync(string? name, bool OnlyAvailable,int page, int pageSize);
    Task<SchoolClass?> GetClassWithHoldsByIdAsync(string id);
}