using System.ComponentModel.DataAnnotations;

namespace Grades.Web.Models;

public class Grade
{
    public int GradeId { get; set; }

    [Display(Name = "Estudiante")]
    public int StudentId { get; set; }

    [Display(Name = "Curso")]
    [Required(ErrorMessage = "El curso es obligatorio.")]
    [StringLength(100, ErrorMessage = "El curso no puede tener más de 100 caracteres.")]
    public string Course { get; set; } = string.Empty;

    [Display(Name = "Nota 1")]
    [Range(0, 20, ErrorMessage = "La nota debe estar entre 0 y 20.")]
    public decimal Score1 { get; set; }

    [Display(Name = "Nota 2")]
    [Range(0, 20, ErrorMessage = "La nota debe estar entre 0 y 20.")]
    public decimal Score2 { get; set; }

    [Display(Name = "Nota 3")]
    [Range(0, 20, ErrorMessage = "La nota debe estar entre 0 y 20.")]
    public decimal Score3 { get; set; }

    // Viene del JOIN con Students en usp_Grades_List (solo para mostrar).
    [Display(Name = "Estudiante")]
    public string? StudentName { get; set; }

    // Propiedades calculadas: no se guardan en la base de datos.
    [Display(Name = "Promedio")]
    public decimal Average => Math.Round((Score1 + Score2 + Score3) / 3, 2);

    [Display(Name = "Condición")]
    public string Status => Average >= 10.5m ? "Aprobado" : "Desaprobado";
}
