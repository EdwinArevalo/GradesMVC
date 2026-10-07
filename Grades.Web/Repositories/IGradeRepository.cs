using Grades.Web.Models;

namespace Grades.Web.Repositories;

public interface IGradeRepository
{
    Task<IEnumerable<Grade>> GetAllAsync(int? studentId);
    Task<Grade?> GetByIdAsync(int gradeId);
    Task InsertAsync(Grade grade);
    Task UpdateAsync(Grade grade);
    Task DeleteAsync(int gradeId);
}
