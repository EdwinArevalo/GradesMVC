using System.ComponentModel.DataAnnotations;

namespace Grades.Web.Models;

public class Student
{
    public int StudentId { get; set; }

    [Display(Name = "Código")]
    [Required(ErrorMessage = "El código es obligatorio.")]
    [StringLength(10, ErrorMessage = "El código no puede tener más de 10 caracteres.")]
    public string Code { get; set; } = string.Empty;

    [Display(Name = "Nombres")]
    [Required(ErrorMessage = "Los nombres son obligatorios.")]
    [StringLength(50, ErrorMessage = "Los nombres no pueden tener más de 50 caracteres.")]
    public string FirstName { get; set; } = string.Empty;

    [Display(Name = "Apellidos")]
    [Required(ErrorMessage = "Los apellidos son obligatorios.")]
    [StringLength(50, ErrorMessage = "Los apellidos no pueden tener más de 50 caracteres.")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Correo")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [StringLength(100, ErrorMessage = "El correo no puede tener más de 100 caracteres.")]
    public string? Email { get; set; }

    // Propiedad calculada: no existe en la base de datos.
    public string FullName => FirstName + " " + LastName;
}
