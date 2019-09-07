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

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject cObject)
        {
            _context.Add(cObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = cObject.Id }, cObject);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, CelestialObject cObject)
        {
            var originalObject = _context.CelestialObjects.Find(id);

            if (originalObject == null)
            {
                return NotFound();
            }

            originalObject.Name = cObject.Name;
            originalObject.OrbitalPeriod = cObject.OrbitalPeriod;
            originalObject.OrbitedObjectId = cObject.OrbitedObjectId;

            _context.Update(originalObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var originalObject = _context.CelestialObjects.Find(id);

            if (originalObject == null)
            {
                return NotFound();
            }

            originalObject.Name = name;
            _context.CelestialObjects.Update(originalObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObjects = _context.CelestialObjects.Where(c => c.Id == id || c.OrbitedObjectId == id);

            if (!celestialObjects.Any())
            {
                return NotFound();
            }

            _context.CelestialObjects.RemoveRange(celestialObjects);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
