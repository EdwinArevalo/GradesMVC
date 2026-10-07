using System.Data;
using Dapper;
using Grades.Web.Models;
using Microsoft.Data.SqlClient;

namespace Grades.Web.Repositories;

public class GradeRepository : IGradeRepository
{
    private readonly string _connectionString;

    // IConfiguration lee la cadena de conexión desde appsettings.json
    public GradeRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("GradesDB")!;
    }

    // studentId es opcional: si llega null, el stored procedure devuelve todas las notas.
    public async Task<IEnumerable<Grade>> GetAllAsync(int? studentId)
    {
        using var connection = new SqlConnection(_connectionString);

        return await connection.QueryAsync<Grade>(
            "usp_Grades_List",
            new { StudentId = studentId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Grade?> GetByIdAsync(int gradeId)
    {
        using var connection = new SqlConnection(_connectionString);

        return await connection.QueryFirstOrDefaultAsync<Grade>(
            "usp_Grades_GetById",
            new { GradeId = gradeId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task InsertAsync(Grade grade)
    {
        using var connection = new SqlConnection(_connectionString);

        await connection.ExecuteAsync(
            "usp_Grades_Insert",
            new { grade.StudentId, grade.Course, grade.Score1, grade.Score2, grade.Score3 },
            commandType: CommandType.StoredProcedure);
    }

    public async Task UpdateAsync(Grade grade)
    {
        using var connection = new SqlConnection(_connectionString);

        await connection.ExecuteAsync(
            "usp_Grades_Update",
            new { grade.GradeId, grade.StudentId, grade.Course, grade.Score1, grade.Score2, grade.Score3 },
            commandType: CommandType.StoredProcedure);
    }

    public async Task DeleteAsync(int gradeId)
    {
        using var connection = new SqlConnection(_connectionString);

        await connection.ExecuteAsync(
            "usp_Grades_Delete",
            new { GradeId = gradeId },
            commandType: CommandType.StoredProcedure);
    }
}
