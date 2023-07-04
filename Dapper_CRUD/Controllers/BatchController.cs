using Dapper_CRUD.Data;
using Dapper_CRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Dapper_CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchController : ControllerBase
    {
        private readonly BatchRepository batchRepository;

        public BatchController(BatchRepository batchRepository)
        {
            this.batchRepository = batchRepository;
        }

        [HttpGet]
        public IActionResult GetAllBatches()
        {
            var batches = batchRepository.GetAllBatches();
            return Ok(batches);
        }

        [HttpGet("{id}")]
        public IActionResult GetBatchById(int id)
        {
            var batch = batchRepository.GetBatchById(id);
            if (batch == null)
            {
                return NotFound();
            }
            return Ok(batch);
        }

        [HttpPost]
        public IActionResult AddBatch(Batch batch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = batchRepository.AddBatch(batch);
            if (result > 0)
            {
                return Ok();
            }
            return StatusCode(500);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBatch(int id, Batch batch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingBatch = batchRepository.GetBatchById(id);
            if (existingBatch == null)
            {
                return NotFound();
            }

            batch.BatchId = id;
            var result = batchRepository.UpdateBatch(batch);
            if (result > 0)
            {
                return Ok();
            }
            return StatusCode(500);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBatch(int id)
        {
            var existingBatch = batchRepository.GetBatchById(id);
            if (existingBatch == null)
            {
                return NotFound();
            }

            var result = batchRepository.DeleteBatch(id);
            if (result > 0)
            {
                return Ok();
            }
            return StatusCode(500);
        }
    }
}