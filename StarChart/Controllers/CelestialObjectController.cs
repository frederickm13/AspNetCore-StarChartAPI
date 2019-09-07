using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var cObject = _context.CelestialObjects.Find(id);

            if (cObject == null)
            {
                return NotFound();
            }

            cObject.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == cObject.Id).ToList();
            return Ok(cObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var cObjects = _context.CelestialObjects.Where(c => c.Name == name);

            if (!cObjects.Any())
            {
                return NotFound();
            }

            foreach (var cObj in cObjects)
            {
                cObj.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == cObj.Id).ToList();
            }
            return Ok(cObjects.ToList());
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var cObjects = _context.CelestialObjects.ToList();

            if (!cObjects.Any())
            {
                return NotFound();
            }

            foreach (var cObj in cObjects)
            {
                cObj.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == cObj.Id).ToList();
            }

            return Ok(cObjects);
        }
    }
}
