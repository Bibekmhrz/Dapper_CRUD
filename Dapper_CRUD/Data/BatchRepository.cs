using Dapper_CRUD.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;

namespace Dapper_CRUD.Data
{
    public class BatchRepository
    {
        private readonly NpgsqlConnection connection;
        private readonly string connectionString;

        public BatchRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("conn");
            this.connection = new NpgsqlConnection(connectionString);
        }

        public IEnumerable<Batch> GetAllBatches()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var batches = connection.Query<Batch>("SELECT * FROM Batches").ToList();

                foreach (var batch in batches)
                {
                    batch.Students = GetStudentsByBatchId(batch.BatchId).ToList();
                }

                return batches;
            }
        }

        public Batch GetBatchById(int id)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var batch = connection.QueryFirstOrDefault<Batch>("SELECT * FROM Batches WHERE BatchId = @Id", new { Id = id });

                if (batch != null)
                {
                    batch.Students = GetStudentsByBatchId(batch.BatchId).ToList();
                }

                return batch;
            }
        }

        public int AddBatch(Batch batch)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                return connection.Execute("INSERT INTO Batches DEFAULT VALUES");
            }
        }

        public int UpdateBatch(Batch batch)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                return connection.Execute("UPDATE Batches SET BatchId = @BatchId  WHERE BatchId = @BatchId",
                    new { BatchId = batch.BatchId });
            }
        }

        public int DeleteBatch(int id)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                connection.Execute("DELETE FROM Students WHERE BatchId = @Id", new { Id = id });

                // Delete the batch
                return connection.Execute("DELETE FROM Batches WHERE BatchId = @Id", new { Id = id });
            }
        }

        private IEnumerable<Student> GetStudentsByBatchId(int batchId)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Student>("SELECT * FROM Students WHERE BatchId = @BatchId", new { BatchId = batchId }).ToList();
            }
        }
    }
}
