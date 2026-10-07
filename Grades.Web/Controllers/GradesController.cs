using Grades.Web.Models;
using Grades.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Grades.Web.Controllers;

public class GradesController : Controller
{
    private readonly IGradeRepository _gradeRepository;
    private readonly IStudentRepository _studentRepository;

    public GradesController(IGradeRepository gradeRepository, IStudentRepository studentRepository)
    {
        _gradeRepository = gradeRepository;
        _studentRepository = studentRepository;
    }

    // GET: /Grades              -> todas las notas
    // GET: /Grades?studentId=2  -> solo las notas de ese estudiante
    public async Task<IActionResult> Index(int? studentId)
    {
        var grades = await _gradeRepository.GetAllAsync(studentId);

        await LoadStudentsAsync(studentId);
        return View(grades);
    }

    // GET: /Grades/Create
    public async Task<IActionResult> Create()
    {
        await LoadStudentsAsync();
        return View();
    }

    // POST: /Grades/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Grade grade)
    {
        if (!ModelState.IsValid)
        {
            await LoadStudentsAsync();
            return View(grade);
        }

        await _gradeRepository.InsertAsync(grade);

        TempData["Message"] = "Nota registrada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Grades/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var grade = await _gradeRepository.GetByIdAsync(id);

        if (grade == null)
        {
            return NotFound();
        }

        await LoadStudentsAsync();
        return View(grade);
    }

    // POST: /Grades/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Grade grade)
    {
        if (!ModelState.IsValid)
        {
            await LoadStudentsAsync();
            return View(grade);
        }

        await _gradeRepository.UpdateAsync(grade);

        TempData["Message"] = "Nota actualizada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    // POST: /Grades/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _gradeRepository.DeleteAsync(id);

        TempData["Message"] = "Nota eliminada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    // Envía a la vista la lista de estudiantes para llenar el combo (select).
    // selectedStudentId indica cuál debe aparecer seleccionado (se usa en el filtro).
    private async Task LoadStudentsAsync(int? selectedStudentId = null)
    {
        var students = await _studentRepository.GetAllAsync();
        ViewData["Students"] = new SelectList(students, "StudentId", "FullName", selectedStudentId);
    }
}
