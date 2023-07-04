using Dapper;
using Dapper_CRUD.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Dapper_CRUD.Data
{
    public class StudentRepository
    {
        private readonly NpgsqlConnection _connection;
        private readonly string _connectionString;

        public StudentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("conn");
            
        }
        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        public IEnumerable<Student> GetAllStudents(int batchId)
        {
            using (var connection = CreateConnection())
            {
                return connection.Query<Student>("SELECT * FROM Students WHERE BatchId = @BatchId", new { BatchId = batchId });
            }
        }

        public Student GetStudentById(int id, int batchId)
        {
            using (var connection = CreateConnection())
            {
                return connection.QuerySingleOrDefault<Student>("SELECT * FROM Students WHERE Id = @Id AND BatchId = @BatchId", new { Id = id, BatchId = batchId });
            }
        }

        public int AddStudent(Student student)
        {
            /*var query = @"
                INSERT INTO Students (FullName, Gender, Email, Hobbies, BatchId)
                VALUES (@FullName, @Gender, @Email, @Hobbies, @BatchId)";

            return _connection.Execute(query, student);*/
            
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    var query = @"
                    INSERT INTO Students (FullName, Gender, Email, Hobbies, BatchId)
                    VALUES (@FullName, @Gender, @Email, @Hobbies, @BatchId)";

                    return connection.Execute(query, student);
                }
            }
        

        public int UpdateStudent(Student student)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                return connection.Execute("UPDATE Students SET FullName = @FullName, Gender = @Gender, Email = @Email, Hobbies = @Hobbies WHERE Id = @Id AND BatchId = @BatchId",
                    new { student.FullName, student.Gender, student.Email, Hobbies = student.Hobbies ?? new string[] { }, student.Id, student.BatchId });
            }
        }

        public int DeleteStudent(int id, int batchId)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                return connection.Execute("DELETE FROM Students WHERE Id = @Id AND BatchId = @BatchId", new { Id = id, BatchId = batchId });
            }
        }
    }
}