using API.Data;
using API.DDBBModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class CRUDProfessorController : ControllerBase
    {
        private readonly CRUDbContext context;

        public CRUDProfessorController(CRUDbContext _context)
        {
            context = _context;
        }

        [HttpGet("GetProfessor")]
        public IActionResult GetProfessor()
        {
            try
            {
                var query = context.Professors.ToList();
                if (query == null)
                    return BadRequest("there is not professors to show");
                return Ok(new { message = "ok", students = query });
            }
            catch (Exception)
            {
                return BadRequest("there is not professors to show");
            }
        }

        [HttpGet("GetProfessor/{id}")]
        public IActionResult GetProfessor(int id)
        {
            try
            {
                var query = context.Professors.Where(e => e.ProfessorId == id).FirstOrDefault();
                if (query == null)
                    return BadRequest("there is not professors to show");
                return Ok(new { message = "ok", students = query });
            }
            catch (Exception)
            {
                return BadRequest("there is not professors to show");
            }
        }

        [HttpPut("UpdateProfessor")]
        public IActionResult UpdateProfessor(Professor professor)
        {
            try
            {
                var query = context.Professors.Where(e => e.ProfessorId == professor.ProfessorId).FirstOrDefault();
                if (query == null)
                    return BadRequest("There is not professor to update");
                query.FirstName = professor.FirstName;
                query.LastName = professor.LastName;
                query.Address = professor.Address;
                query.Phone = professor.Phone;
                context.SaveChanges();
                return Ok(new { messge = "Professor updated!" });
            }
            catch (Exception)
            {
                return BadRequest("there is not professors to update");
            }
        }

        [HttpDelete("DeleteProfessor/{id}")]
        public IActionResult DeleteProfessor(int idProfessor)
        {
            try
            {
                if (idProfessor < 0)
                    return BadRequest("The id cannot be less than 0");
                var query = context.Professors.Where(e => e.ProfessorId == idProfessor).FirstOrDefault();
                if (query != null)
                    query.IsActive = false;
                context.SaveChanges();
                return Ok(new { message = "the professor was deleted succesfully" });
            }
            catch (Exception ex)
            {
                throw new RankException("El usuario no pudo ser eliminado", ex);
            }
        }

        [HttpPost("CreateProfessor")]
        public IActionResult CreateProfessor([FromBody] Professor professor)
        {
            professor.IsActive = true;
            if (professor == null)
            {
                return BadRequest("Profesor no puede ser nulo");
            }
            try
            {
                if (professor.IsActive == null)
                {
                    professor.IsActive = true;
                }
                context.Professors.Add(professor);
                context.SaveChanges();

                return Ok(new { creado = true, professor });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el profesor: {ex.Message}");
            }
        }
    }
}
