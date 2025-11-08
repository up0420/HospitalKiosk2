using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using HospitalKiosk.Models;

namespace HospitalKiosk.Data
{
    public class DepartmentRepository : IRepository<Department>
    {
        public Department GetById(int id)
        {
            Department department = null;

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand("SELECT * FROM Departments WHERE DepartmentId = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        department = MapToDepartment(reader);
                    }
                }
            }

            return department;
        }

        public IEnumerable<Department> GetAll()
        {
            var departments = new List<Department>();

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand("SELECT * FROM Departments WHERE IsActive = 1 ORDER BY DepartmentName", connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        departments.Add(MapToDepartment(reader));
                    }
                }
            }

            return departments;
        }

        public int Insert(Department entity)
        {
            var query = @"INSERT INTO Departments (DepartmentName, DepartmentCode, Description, IsActive, CreatedAt, UpdatedAt)
                         VALUES (@Name, @Code, @Description, @IsActive, @CreatedAt, @UpdatedAt);
                         SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                AddParameters(command, entity);
                connection.Open();

                return (int)command.ExecuteScalar();
            }
        }

        public void Update(Department entity)
        {
            var query = @"UPDATE Departments SET
                         DepartmentName = @Name,
                         DepartmentCode = @Code,
                         Description = @Description,
                         IsActive = @IsActive,
                         UpdatedAt = @UpdatedAt
                         WHERE DepartmentId = @Id";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                AddParameters(command, entity);
                command.Parameters.AddWithValue("@Id", entity.DepartmentId);
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            var query = "UPDATE Departments SET IsActive = 0, UpdatedAt = GETDATE() WHERE DepartmentId = @Id";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        private Department MapToDepartment(SqlDataReader reader)
        {
            return new Department
            {
                DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName")),
                DepartmentCode = reader.GetString(reader.GetOrdinal("DepartmentCode")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
            };
        }

        private void AddParameters(SqlCommand command, Department entity)
        {
            command.Parameters.AddWithValue("@Name", entity.DepartmentName);
            command.Parameters.AddWithValue("@Code", entity.DepartmentCode);
            command.Parameters.AddWithValue("@Description", (object)entity.Description ?? DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", entity.IsActive);
            command.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);
            command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
        }
    }
}
