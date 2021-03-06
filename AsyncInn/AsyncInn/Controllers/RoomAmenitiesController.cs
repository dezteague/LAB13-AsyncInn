﻿using AsyncInn.Data;
using AsyncInn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Controllers
{
    public class RoomAmenitiesController : Controller
    {
        private readonly AsyncInnDbContext _context;

        public RoomAmenitiesController(AsyncInnDbContext context)
        {
            _context = context;
        }

        // GET: RoomAmenities
        /// <summary>
        /// Get all room amenities
        /// </summary>
        /// <returns>room amentities view</returns>
        public async Task<IActionResult> Index()
        {
            var asyncInnDbContext = _context.RoomAmenities.Include(r => r.Amenities).Include(r => r.Room);
            return View(await asyncInnDbContext.ToListAsync());
        }

        // GET: RoomAmenities/Details/5
        /// <summary>
        /// Show details of room amenities
        /// </summary>
        /// <param name="roomid"></param>
        /// <param name="amenityid"></param>
        /// <returns>details view</returns>
        public async Task<IActionResult> Details(int? roomid, int? amenityid)
        {
            if (roomid == null || amenityid == null)
            {
                return NotFound();
            }

            var roomAmenities = await _context.RoomAmenities
                .Include(r => r.Amenities)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.RoomID == roomid && m.AmenitiesID == amenityid);
            if (roomAmenities == null)
            {
                return NotFound();
            }

            return View(roomAmenities);
        }

        // GET: RoomAmenities/Create
        /// <summary>
        /// Create a room amenity
        /// </summary>
        /// <returns>create view</returns>
        public IActionResult Create()
        {
            ViewData["AmenitiesID"] = new SelectList(_context.Amenities, "ID", "Name");
            ViewData["RoomID"] = new SelectList(_context.Rooms, "ID", "Name");
            return View();
        }

        // POST: RoomAmenities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Show the details of new room amentiy
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roomAmenities"></param>
        /// <returns>index view</returns>
        public async Task<IActionResult> Create([Bind("AmenitiesID,RoomID")] RoomAmenities roomAmenities)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomAmenities);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AmenitiesID"] = new SelectList(_context.Amenities, "ID", "ID", roomAmenities.AmenitiesID);
            ViewData["RoomID"] = new SelectList(_context.Rooms, "ID", "ID", roomAmenities.RoomID);
            return View(roomAmenities);
        }

        // GET: RoomAmenities/Edit/5
        /// <summary>
        /// display details of a room amenity for editing
        /// </summary>
        /// <param name="roomid"></param>
        /// <param name="amenityid"></param>
        /// <returns>edit view</returns>
        public async Task<IActionResult> Edit(int? roomid, int? amenityid)
        {
            if (roomid == null && amenityid == null)
            {
                return NotFound();
            }

            var roomAmenities = await _context.RoomAmenities
                .Include(r => r.Amenities)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.RoomID == roomid && m.AmenitiesID == amenityid);
            if (roomAmenities == null)
            {
                return NotFound();
            }
            ViewData["AmenitiesID"] = new SelectList(_context.Amenities, "ID", "Name", roomAmenities.AmenitiesID);
            ViewData["RoomID"] = new SelectList(_context.Rooms, "ID", "Name", roomAmenities.RoomID);
            return View(roomAmenities);
        }

        // POST: RoomAmenities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Display details of a room amenity that were edited
        /// </summary>
        /// <param name="roomid"></param>
        /// <param name="amenityid"></param>
        /// <returns>room amenity view</returns>
        public async Task<IActionResult> Edit(int roomid, int amenityid, [Bind("AmenitiesID,RoomID")] RoomAmenities roomAmenities)
        {
            

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomAmenities);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomAmenitiesExists(roomAmenities.AmenitiesID))
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
            ViewData["AmenitiesID"] = new SelectList(_context.Amenities, "ID", "ID", roomAmenities.AmenitiesID);
            ViewData["RoomID"] = new SelectList(_context.Rooms, "ID", "ID", roomAmenities.RoomID);
            return View(roomAmenities);
        }

        // GET: RoomAmenities/Delete/5
        /// <summary>
        /// show room amenity to be deleted
        /// </summary>
        /// <param name="roomid"></param>
        /// <param name="amenityid"></param>
        /// <returns>delete view</returns>
        public async Task<IActionResult> Delete(int roomid, int amenityid)
        {
   

            var roomAmenities = await _context.RoomAmenities
                .Include(r => r.Amenities)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.RoomID == roomid && m.AmenitiesID == amenityid);
            if (roomAmenities == null)
            {
                return NotFound();
            }

            return View(roomAmenities);
        }

        // POST: RoomAmenities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Asks user to confirm deletion
        /// </summary>
        /// <param name="roomid"></param>
        /// <param name="amenityid"></param>
        /// <returns>roomamenity view page</returns>
        public async Task<IActionResult> DeleteConfirmed(int roomid, int amenityid)
        {
            var roomAmenities = await _context.RoomAmenities.Include(r => r.Amenities)
                .Include(r => r.Room).FirstOrDefaultAsync(m => m.RoomID == roomid && m.AmenitiesID == amenityid);

            _context.RoomAmenities.Remove(roomAmenities);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomAmenitiesExists(int id)
        {
            return _context.RoomAmenities.Any(e => e.AmenitiesID == id);
        }
    }
}
