using Dapper_CRUD.Data;
using Dapper_CRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Dapper_CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentRepository studentRepository;
        private readonly BatchRepository batchRepository;

        public StudentsController(StudentRepository studentRepository, BatchRepository batchRepository)
        {
            this.studentRepository = studentRepository;
            this.batchRepository = batchRepository;
        }

        [HttpGet]
        public IActionResult GetAllStudents(int batchId)
        {
            var students = studentRepository.GetAllStudents(batchId);
            return Ok(students);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id, int batchId)
        {
            var student = studentRepository.GetStudentById(id, batchId);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        public IActionResult AddStudent([FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the latest batch from the database
            var latestBatch = batchRepository.GetAllBatches().OrderByDescending(b => b.BatchId).FirstOrDefault();

            if (latestBatch == null)
            {
                // If no batch exists, create a new one
                latestBatch = new Batch();
                batchRepository.AddBatch(latestBatch);
            }

            student.BatchId = latestBatch.BatchId;

            var result = studentRepository.AddStudent(student);
            if (result > 0)
            {
                return Ok();
            }
            return StatusCode(500);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, int batchId, [FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingStudent = studentRepository.GetStudentById(id, batchId);
            if (existingStudent == null)
            {
                return NotFound();
            }

            student.Id = id;
            var result = studentRepository.UpdateStudent(student);
            if (result > 0)
            {
                return Ok();
            }
            return StatusCode(500);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id, int batchId)
        {
            var existingStudent = studentRepository.GetStudentById(id, batchId);
            if (existingStudent == null)
            {
                return NotFound();
            }

            var result = studentRepository.DeleteStudent(id, batchId);
            if (result > 0)
            {
                return Ok();
            }
            return StatusCode(500);
        }
    }
}