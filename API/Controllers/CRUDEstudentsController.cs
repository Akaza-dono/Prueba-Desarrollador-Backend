using API.Data;
using API.DDBBModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CRUDEstudentsController : ControllerBase
    {

        private readonly CRUDbContext context;

        public CRUDEstudentsController(CRUDbContext context_)
        {
            context = context_;
        }

        [HttpGet("GetStudents")]
        public IActionResult GetStudents()
        {
            try
            {
                var query = context.Students.ToList();
                if (query == null)
                    return BadRequest("there is not students to show");
                return Ok(new { message = "ok", students = query });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error triying to create student: {ex.Message}");
            }
        }

        [HttpGet("GetStudents/{id}")]
        public IActionResult GetStudents(int id)
        {
            try
            {
                var query = context.Students.Where(e => e.StudentId == id).FirstOrDefault();
                if (query == null)
                    return BadRequest("there is not students to show");
                return Ok(new { message = "ok", students = query });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error triying to create student: {ex.Message}");
            }
        }

        [HttpPut("UpdateStudent")]
        public IActionResult UpdateStudent(Student student)
        {
            try
            {
                var query = context.Students.Where(e => e.StudentId == student.StudentId).FirstOrDefault();
                if (query == null)
                    return BadRequest("there is not students to update");
                query.FirstName = student.FirstName ?? query.FirstName;
                query.LastName = student.LastName ?? query.LastName;
                query.Address = student.Address ?? query.Address;
                query.Phone = student.Phone ?? query.Phone;
                query.Email = student.Email ?? query.Email;
                context.SaveChanges();
                return Ok(new { messge = "Student updated!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error triying to create student: {ex.Message}");
            }
        }

        [HttpDelete("DeleteStudent/{id}")]
        public IActionResult DeleteStudent(int id)
        {
            try
            {
                if (id < 0)
                    return BadRequest("Id cannot be null");
                var query = context.Students.Where(e => e.StudentId == id).FirstOrDefault();
                if (query != null)
                    query.IsActive = !query.IsActive;
                context.SaveChanges();
                return Ok(new { message = "student deleted sucessfully" });
            }
            catch (Exception ex)
            {
                throw new RankException("the student cannot be eliminated", ex);
            }
        }

        [HttpPost("CreateStudent")]
        public IActionResult CreateStudent(Student student)
        {
            try
            {
                if (student == null)
                {
                    return BadRequest("The student canot be null");
                }
                context.Students.Add(student);
                context.SaveChanges();
                return Ok(new { creado = true });
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Error triying to create student: {ex.Message}");
            }
        }
    }
}
