using Grades.Web.Models;
using Grades.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Grades.Web.Controllers;

public class StudentsController : Controller
{
    private readonly IStudentRepository _studentRepository;

    // El repository llega por inyección de dependencias (se registró en Program.cs)
    public StudentsController(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    // GET: /Students
    public async Task<IActionResult> Index()
    {
        var students = await _studentRepository.GetAllAsync();
        return View(students);
    }

    // GET: /Students/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Students/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Student student)
    {
        // Validación propia: el código no se puede repetir.
        if (await _studentRepository.ExistsCodeAsync(student.Code, 0))
        {
            ModelState.AddModelError("Code", "Ya existe un estudiante con ese código.");
        }

        // Si alguna validación falló, se vuelve a mostrar el formulario con los errores.
        if (!ModelState.IsValid)
        {
            return View(student);
        }

        await _studentRepository.InsertAsync(student);

        TempData["Message"] = "Estudiante registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Students/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var student = await _studentRepository.GetByIdAsync(id);

        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    // POST: /Students/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Student student)
    {
        if (await _studentRepository.ExistsCodeAsync(student.Code, student.StudentId))
        {
            ModelState.AddModelError("Code", "Ya existe un estudiante con ese código.");
        }

        if (!ModelState.IsValid)
        {
            return View(student);
        }

        await _studentRepository.UpdateAsync(student);

        TempData["Message"] = "Estudiante actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    // POST: /Students/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _studentRepository.DeleteAsync(id);

        TempData["Message"] = "Estudiante eliminado correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
