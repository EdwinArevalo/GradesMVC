using System.Data;
using Dapper;
using Grades.Web.Models;
using Microsoft.Data.SqlClient;

namespace Grades.Web.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly string _connectionString;

    // IConfiguration lee la cadena de conexión desde appsettings.json
    public StudentRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("GradesDB")!;
    }

    public async Task<IEnumerable<Student>> GetAllAsync()
    {
        using var connection = new SqlConnection(_connectionString);

        return await connection.QueryAsync<Student>(
            "usp_Students_List",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Student?> GetByIdAsync(int studentId)
    {
        using var connection = new SqlConnection(_connectionString);

        return await connection.QueryFirstOrDefaultAsync<Student>(
            "usp_Students_GetById",
            new { StudentId = studentId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task InsertAsync(Student student)
    {
        using var connection = new SqlConnection(_connectionString);

        await connection.ExecuteAsync(
            "usp_Students_Insert",
            new { student.Code, student.FirstName, student.LastName, student.Email },
            commandType: CommandType.StoredProcedure);
    }

    public async Task UpdateAsync(Student student)
    {
        using var connection = new SqlConnection(_connectionString);

        await connection.ExecuteAsync(
            "usp_Students_Update",
            new { student.StudentId, student.Code, student.FirstName, student.LastName, student.Email },
            commandType: CommandType.StoredProcedure);
    }

    public async Task DeleteAsync(int studentId)
    {
        using var connection = new SqlConnection(_connectionString);

        await connection.ExecuteAsync(
            "usp_Students_Delete",
            new { StudentId = studentId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> ExistsCodeAsync(string code, int studentId)
    {
        using var connection = new SqlConnection(_connectionString);

        int count = await connection.ExecuteScalarAsync<int>(
            "usp_Students_ExistsCode",
            new { Code = code, StudentId = studentId },
            commandType: CommandType.StoredProcedure);

        return count > 0;
    }
}
