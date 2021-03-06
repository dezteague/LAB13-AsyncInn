﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AsyncInn.Data;
using AsyncInn.Models;
using AsyncInn.Models.Interfaces;

namespace AsyncInn.Controllers
{
    public class AmenitiesController : Controller
    {
        private readonly IAmenityManager _context;
        private readonly AsyncInnDbContext _amenities;

        public AmenitiesController(IAmenityManager context, AsyncInnDbContext amenities)
        {
            _context = context;
            _amenities = amenities;
        }

        // GET: Amenities
        /// <summary>
        /// Gets all amenities (allows search filter) and displays them on the index page
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>index view</returns>
        public async Task<IActionResult> Index(string searchString)
        {
            var amenities = from h in _amenities.Amenities
                         select h;
            if (!String.IsNullOrEmpty(searchString))
            {
                amenities = amenities.Where(amen => amen.Name.Contains(searchString));
            }
            return View(await amenities.ToListAsync());
        }

        // GET: Amenities/Details/5
        /// <summary>
        /// Show the details of an amentiy
        /// </summary>
        /// <param name="id"></param>
        /// <returns>details view</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var amenities = await _context.GetAmenity(id);
                
            if (amenities == null)
            {
                return NotFound();
            }

            return View(amenities);
        }

        // GET: Amenities/Create
        /// <summary>
        /// Displays the create an amenity page
        /// </summary>
        /// <returns>create view</returns>
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        /// <summary>
        /// Posts search results
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>index view</returns>
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // POST: Amenities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Show the details of new amentiy
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns>index view</returns>
        public async Task<IActionResult> Create([Bind("ID,Name")] Amenities amenities)
        {
            if (ModelState.IsValid)
            {
               
                await _context.CreateAmenity(amenities);
                return RedirectToAction(nameof(Index));
            }
            return View(amenities);
        }

        // GET: Amenities/Edit/5
        /// <summary>
        /// Display details of an amenity for editing
        /// </summary>
        /// <param name="id"></param>
        /// <returns>edit view</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var amenities = await _context.GetAmenity(id);
            if (amenities == null)
            {
                return NotFound();
            }
            return View(amenities);
        }

        // POST: Amenities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Display details of an amenity that were edited
        /// </summary>
        /// <param name="id"></param>
        /// <returns>amenity view</returns>
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] Amenities amenities)
        {
            if (id != amenities.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.UpdateAmenity(amenities);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AmenityExists(amenities.ID))
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
            return View(amenities);
        }

        // GET: Amenities/Delete/5
        /// <summary>
        /// Shows amenities to be deleted
        /// </summary>
        /// <param name="id"></param>
        /// <returns>delete view page</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var amenities = await _context.GetAmenity(id);
                
            if (amenities == null)
            {
                return NotFound();
            }

            return View(amenities);
        }

        // POST: Amenities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Asks user to confirm deletion
        /// </summary>
        /// <param name="id"></param>
        /// <returns>amenity view page</returns>
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            await _context.DeleteAmenity(id);
            return RedirectToAction(nameof(Index));
        }

        private bool AmenityExists(int id)
        {
            return _context.AmenityExists( id);
        }
    }
}
