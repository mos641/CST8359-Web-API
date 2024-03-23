using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab6.Data;
using Lab6.Models;

namespace Lab6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentDbContext _context;

        public StudentsController(StudentDbContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)] // returned when we return list of Students successfully
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // returned when there is an internal error
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return Ok(await _context.Students.ToListAsync());
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // returned when we return list of Students successfully
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // returned when parameters are incorrect
        [ProducesResponseType(StatusCodes.Status404NotFound)] // returned when we cannot find the student with specified id
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // returned when there is an internal error
        public async Task<ActionResult<Student>> GetStudent(Guid id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // returned when we return list of Students successfully
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // returned when parameters are incorrect or the two parameters mismatch
        [ProducesResponseType(StatusCodes.Status404NotFound)] // returned when we cannot find the student with specified id
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // returned when there is an internal error
        public async Task<ActionResult<Student>> PutStudent(Guid id, Student student)
        {
            if (!student.ID.Equals(Guid.Empty) && (id != student.ID))
            {
                return BadRequest("Student id in payload does not match parameter id - keep empty or the same");
            } else
            {
                student.ID = id;
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(student);
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)] // returned when a student was successfully created
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // returned when parameters are incorrect
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // returned when there is an internal error
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            if (!student.ID.Equals(Guid.Empty))
            {
                return BadRequest("Can not specify student id in payload");
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.ID }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // returned when we successfully delete a student
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // returned when parameters are incorrect
        [ProducesResponseType(StatusCodes.Status404NotFound)] // returned when we cannot find the student with specified id
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // returned when there is an internal error
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(Guid id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
