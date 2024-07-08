﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Medical_Laboratory_Management.Data;
using Medical_Laboratory_Management.Models;

namespace Medical_Laboratory_Management.Controllers
{
    public class LabAssistantsController : Controller
    {
        private readonly LabDbContext _context;

        public LabAssistantsController(LabDbContext context)
        {
            _context = context;
        }

        // GET: LabAssistants
        public async Task<IActionResult> Index()
        {
            var labDbContext = _context.LabAssistants.Include(l => l.Department);
            return View(await labDbContext.ToListAsync());
        }
        //Dashboard
        //public async Task<IActionResult> Dashboard()
        //{
        //    var labAssistantId = HttpContext.Session.GetInt32("LabAssistantId");
        //    if (labAssistantId == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    var appointments = await _context.Appointments
        //                                     .Include(a => a.User)
        //                                     .Include(a => a.Department)
        //                                     .Where(a => a.LabAssistantId == labAssistantId)
        //                                     .ToListAsync();

        //    return View(appointments);
        //}

        //public async Task<IActionResult> Dashboard()
        //{
        //    var labAssistantId = User.Identity.GetUserId(); // Retrieve the logged-in lab assistant's ID
        //    var appointments = await _context.Appointments
        //        .Include(a => a.User)
        //        .Include(a => a.Department)
        //        .Where(a => a.LabAssistantId == labAssistantId)
        //        .ToListAsync();

        //    return View(appointments);
        //}

        // GET: LabAssistants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labAssistant = await _context.LabAssistants
                .Include(l => l.Department)
                .FirstOrDefaultAsync(m => m.LabAssistantId == id);
            if (labAssistant == null)
            {
                return NotFound();
            }

            return View(labAssistant);
        }

        // GET: LabAssistants/Create
        public IActionResult Create()
        {
            ViewBag.Departments = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,DepartmentId,AppointmentDate,AppointmentTime,Symptoms")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                var labAssistant = await _context.LabAssistants
                    .FirstOrDefaultAsync(la => la.DepartmentId == appointment.DepartmentId);

                if (labAssistant != null)
                {
                    appointment.LabAssistantId = labAssistant.LabAssistantId;
                    _context.Add(appointment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "No lab assistant available for the selected department.");
            }

            ViewBag.Departments = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", appointment.DepartmentId);
            return View(appointment);
        }


        // GET: LabAssistants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labAssistant = await _context.LabAssistants.FindAsync(id);
            if (labAssistant == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", labAssistant.DepartmentId);
            return View(labAssistant);
        }

        // POST: LabAssistants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LabAssistantId,Name,Age,Gender,PhoneNumber,Email,Address,AadharCard,DepartmentId")] LabAssistant labAssistant)
        {
            if (id != labAssistant.LabAssistantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(labAssistant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabAssistantExists(labAssistant.LabAssistantId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentId", labAssistant.DepartmentId);
            return View(labAssistant);
        }

        // GET: LabAssistants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labAssistant = await _context.LabAssistants
                .Include(l => l.Department)
                .FirstOrDefaultAsync(m => m.LabAssistantId == id);
            if (labAssistant == null)
            {
                return NotFound();
            }

            return View(labAssistant);
        }

        // POST: LabAssistants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var labAssistant = await _context.LabAssistants.FindAsync(id);
            if (labAssistant != null)
            {
                _context.LabAssistants.Remove(labAssistant);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabAssistantExists(int id)
        {
            return _context.LabAssistants.Any(e => e.LabAssistantId == id);
        }
    }
}
