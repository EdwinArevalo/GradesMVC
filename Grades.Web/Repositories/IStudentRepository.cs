using Grades.Web.Models;

namespace Grades.Web.Repositories;

public interface IStudentRepository
{
    Task<IEnumerable<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(int studentId);
    Task InsertAsync(Student student);
    Task UpdateAsync(Student student);
    Task DeleteAsync(int studentId);
    Task<bool> ExistsCodeAsync(string code, int studentId);
}
