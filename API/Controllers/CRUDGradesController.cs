using API.Data;
using API.DDBBModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CRUDGradesController : ControllerBase
    {
        private readonly CRUDbContext context;

        public CRUDGradesController(CRUDbContext context_)
        {
            context = context_;
        }

        [HttpGet("GetGrades")]
        public IActionResult GetGrades()
        {
            try
            {
                var grades = context.Grades.ToList();

                if (grades == null || grades.Count == 0)
                {
                    return NotFound();
                }
                return Ok(new { message = "ok", grades });
            }
            catch (Exception ex)
            {
                return BadRequest($"Cannot get grades. {ex.Message}");
            }
        }

        [HttpGet("GetGrades/{id}")]
        public IActionResult GetGrades(int id)
        {
            try
            {
                var grades = context.Grades.Where(e => e.GradeId == id).FirstOrDefault();

                if (grades == null)
                {
                    return NotFound("There is not grades to show");
                }
                return Ok(new { message = "ok", grades });
            }
            catch (Exception ex)
            {
                return BadRequest($"Cannot get grade. {ex.Message}");
            }
        }

        [HttpPut("UpdateGrade/{id}")]
        public IActionResult UpdateGrade(int id, Grade updatedGrade)
        {
            if (id != updatedGrade.GradeId)
            {
                return BadRequest("Rating ID does not match the submitted object.");
            }

            try
            {
                var gradeToUpdate = context.Grades.Find(id);

                if (gradeToUpdate == null)
                {
                    return NotFound($"No qualification found with ID {id}.");
                }

                // Actualiza los campos de la calificación
                gradeToUpdate.StudentId = updatedGrade.StudentId ?? gradeToUpdate.StudentId;
                gradeToUpdate.ClassId = updatedGrade.ClassId ?? gradeToUpdate.ClassId;
                gradeToUpdate.Grade1 = updatedGrade.Grade1 ?? gradeToUpdate.Grade1;

                context.SaveChanges();

                return Ok(new { actualizado = true, grade = gradeToUpdate });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating the rating: {ex.Message}");
            }

        }

        [HttpDelete("DeleteGrade/{id}")]
        public IActionResult DeleteGrade(int id)
        {
            using (CRUDbContext context = new())
            {
                try
                {
                    var gradeToDelete = context.Grades.Find(id);

                    if (gradeToDelete == null)
                    {
                        return NotFound($"cannot found the grade {id}.");
                    }
                    //Esto no son buenas practicas a menos que se almacenen los datos en otro lado ya que no cumple con mantener la integridad de los datos, en otros casos simplemente desabilito el registro
                    context.Grades.Remove(gradeToDelete);
                    context.SaveChanges();

                    return Ok(new { deleted = true, grade = gradeToDelete });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error triying to delete the grade: {ex.Message}");
                }
            }
        }

        [HttpPost("CreateGrade")]
        public IActionResult CreateGrade(Grade newGrade)
        {
            if (newGrade == null)
            {
                return BadRequest("The data cannot be null");
            }
                try
                {
                    context.Grades.Add(newGrade);
                    context.SaveChanges();

                    return Ok(new { creado = true, grade = newGrade });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error triying to create the grade: {ex.Message}");
                }
        }

    }
}

