﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFCoreMVC.Data;
using EFCoreMVC.Models;

namespace EFCoreMVC.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly SchoolContext _context;

        public DepartmentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.Departments.Include(d => d.Administrator);
            return View(await schoolContext.ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string query = "SELECT * FROM Deparment WHERE DeparmentID = {0}";
            //var department = await _context.Departments

            var department = await _context.Departments.FromSqlRaw(query, id)
                                            .Include(d => d.Administrator)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync();
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentID,Name,Budget,StartDate,InstructorID,RowVersion")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                                                .Include(i => i.Administrator)
                                                .AsNoTracking()
                                                .FirstOrDefaultAsync(m => m.DepartmentID == id);

            if (department == null)
            {
                return NotFound();
            }
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, byte[] rowVersion)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentToUpdate = await _context.Departments.Include(i => i.Administrator).FirstOrDefaultAsync(m => m.DepartmentID == id);

            // Otro usuario eliminó el departamento
            if (departmentToUpdate == null)
            {
                Department deletedDepartment = new Department();

                await TryUpdateModelAsync(deletedDepartment);

                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The department was deleted by another user.");

                ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName", deletedDepartment.InstructorID);

                return View(deletedDepartment);
            }

            _context.Entry(departmentToUpdate).Property("RowVersion").OriginalValue = rowVersion;

            if (await TryUpdateModelAsync<Department>(departmentToUpdate, "", s => s.StartDate, s => s.Budget, s => s.InstructorID))
            {
                try
                {
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {

                    // El código del bloque catch de esa excepción obtiene la entidad Department afectada que tiene los valores actualizados de la propiedad Entries del objeto de excepción.
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Department)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();

                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Department)databaseEntry.ToObject();

                        if (databaseValues.Name != clientValues.Name)
                        {
                            ModelState.AddModelError("Name", $"Current Value: {databaseValues.Name}");
                        }

                        if (databaseValues.Budget != clientValues.Budget)
                        {
                            ModelState.AddModelError("Budget", $"Current value: {databaseValues.Budget}");
                        }

                        if (databaseValues.StartDate != clientValues.StartDate)
                        {
                            ModelState.AddModelError("Start Date", $"Current value: {databaseValues.StartDate}");
                        }

                        if (databaseValues.InstructorID != clientValues.InstructorID)
                        {
                            Instructor databaseInstructor = await _context.Instructors.FirstOrDefaultAsync(i => i.ID == databaseValues.InstructorID);
                            ModelState.AddModelError("InstructorID", $"Current value: {databaseInstructor?.FullName}");
                        }

                            ModelState.AddModelError(string.Empty, "The record you attempted to edit " +
                                "was modified by another user after you got the original value. The " +
                                "edit operation was canceled and the current value in the database " +
                                "have been displayed. If you still want to edit this record, click " +
                                "the Save button again. Otherwise click the Back to List hyperlink");

                        // Por último, el código establece el valor RowVersion de departmentToUpdate para el nuevo valor recuperado de la base de datos. Este nuevo valor RowVersion se almacenará en el campo oculto cuando se vuelva a mostrar la página Edit y, la próxima vez que el usuario haga clic en Save, solo se detectarán los errores de simultaneidad que se produzcan desde que se vuelva a mostrar la página Edit.
                        departmentToUpdate.RowVersion = (byte[])databaseValues.RowVersion;

                        ModelState.Remove("RowVersion");
                    }
                }
            }

            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName", departmentToUpdate.InstructorID);

            return View(departmentToUpdate);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                                                .Include(d => d.Administrator)
                                                .AsNoTracking()
                                                .FirstOrDefaultAsync(m => m.DepartmentID == id);
            if (department == null)
            {
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction(nameof(Index));
                }

                return NotFound();
            }

            if (concurrencyError.GetValueOrDefault())
            {
                ViewData["ConcurrencyErrorMessage"] = "The record you attempted to delete " +
                    "was modifiedd by another user after you got the original values. " +
                    "The delete operation was canceled and the current values in the " +
                    "database have been displayed. If you still want to delete this " +
                    "record, click the Delete button again. Otherwise " +
                    "click the Back to List hyperlink.";
            }

            return View(department);
        }


        // POST: Departments/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Department department)
        // Ha cambiado este parámetro por una instancia de la entidad Department creada por el enlazador de modelos. Esto proporciona a EF acceso al valor de la propiedad RowVersion, además de la clave de registro.
        {
            try
            {
                // Si ya se ha eliminado el departamento, el método AnyAsync devuelve false y la aplicación simplemente vuelve al método de índice.
                if (await _context.Departments.AnyAsync(m => m.DepartmentID == department.DepartmentID))
                {
                    _context.Departments.Remove(department);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            // Si se detecta un error de simultaneidad, el código vuelve a mostrar la página de confirmación de Delete y proporciona una marca que indica que se debería mostrar un mensaje de error de simultaneidad.
            catch (DbUpdateConcurrencyException ex)
            {
                // Log the error
                return RedirectToAction(nameof(Delete), new { concurrencyError = true, id = department.DepartmentID });
            }

        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.DepartmentID == id);
        }
    }
}
