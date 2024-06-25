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
    public class CRUDClassesController : ControllerBase
    {
        private readonly CRUDbContext _context;

        public CRUDClassesController(CRUDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetClasses")]
        public IActionResult GetClasses()
        {

            try
            {
                var classes = _context.Classes.ToList();

                if (classes == null || classes.Count == 0)
                {
                    return NotFound("There is not classes to show.");
                }

                return Ok(new { message = "ok", classes });
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Error triying to get Classes: {ex.Message}");
            }
            
        }
        [HttpPut("UpdateClass/{id}")]
        public IActionResult UpdateClass(int id, Class updatedClass)
        {
            try
            {
                var classToUpdate = _context.Classes.Find(id);

                if (classToUpdate == null)
                {
                    return NotFound($"There is not classes with the id {id}.");
                }

                classToUpdate.ClassName = updatedClass.ClassName;
                classToUpdate.Year = updatedClass.Year;
                classToUpdate.Semester = updatedClass.Semester;
                classToUpdate.ProfessorId = updatedClass.ProfessorId;

                _context.SaveChanges();

                return Ok(new { actualizado = true, clase = classToUpdate });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la clase: {ex.Message}");
            }
        }
        [HttpDelete("DeleteClass/{id}")]
        public IActionResult DeleteClass(int id)
        {
            try
            {
                    var classToDelete = _context.Classes.Find(id);

                    if (classToDelete == null)
                    {
                        return NotFound($"No se encontró ninguna clase con el ID {id}.");
                    }

                    //uso tambien esta forma porque no se deberian de elimimar los datos, debido a la integridad de la base de datos, en otras partes si elimino por completo el registro
                    classToDelete.IsDeleted = true;

                    _context.Entry(classToDelete).State = EntityState.Modified;
                    _context.SaveChanges();

                    return Ok(new { eliminado = true, clase = classToDelete });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la clase: {ex.Message}");
            }
        }


        [HttpPost("CreateClass")]
        public IActionResult CreateClass(Class newClass)
        {
            if (newClass == null)
            {
                return BadRequest("Los datos de la clase no pueden ser nulos.");
            }
            try
            {
                    _context.Classes.Add(newClass);
                    _context.SaveChanges();

                    return Ok(new { creado = true, clase = newClass });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear la clase: {ex.Message}");
            }
        }

    }
}

