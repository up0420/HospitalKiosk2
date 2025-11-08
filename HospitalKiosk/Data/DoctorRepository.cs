using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using HospitalKiosk.Models;

namespace HospitalKiosk.Data
{
    public class DoctorRepository : IRepository<Doctor>
    {
        public Doctor GetById(int id)
        {
            Doctor doctor = null;

            var query = @"SELECT d.*, dept.DepartmentName, dept.DepartmentCode, dept.Description as DeptDescription
                         FROM Doctors d
                         INNER JOIN Departments dept ON d.DepartmentId = dept.DepartmentId
                         WHERE d.DoctorId = @Id";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        doctor = MapToDoctor(reader);
                    }
                }
            }

            return doctor;
        }

        public IEnumerable<Doctor> GetAll()
        {
            var doctors = new List<Doctor>();

            var query = @"SELECT d.*, dept.DepartmentName, dept.DepartmentCode, dept.Description as DeptDescription
                         FROM Doctors d
                         INNER JOIN Departments dept ON d.DepartmentId = dept.DepartmentId
                         WHERE d.IsActive = 1
                         ORDER BY d.DoctorName";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        doctors.Add(MapToDoctor(reader));
                    }
                }
            }

            return doctors;
        }

        public IEnumerable<Doctor> GetByDepartmentId(int departmentId)
        {
            var doctors = new List<Doctor>();

            var query = @"SELECT d.*, dept.DepartmentName, dept.DepartmentCode, dept.Description as DeptDescription
                         FROM Doctors d
                         INNER JOIN Departments dept ON d.DepartmentId = dept.DepartmentId
                         WHERE d.DepartmentId = @DepartmentId AND d.IsActive = 1
                         ORDER BY d.DoctorName";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DepartmentId", departmentId);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        doctors.Add(MapToDoctor(reader));
                    }
                }
            }

            return doctors;
        }

        public int Insert(Doctor entity)
        {
            var query = @"INSERT INTO Doctors (DoctorCode, DoctorName, DepartmentId, LicenseNumber, PhoneNumber, Email, IsActive, CreatedAt, UpdatedAt)
                         VALUES (@DoctorCode, @DoctorName, @DepartmentId, @LicenseNumber, @PhoneNumber, @Email, @IsActive, @CreatedAt, @UpdatedAt);
                         SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                AddParameters(command, entity);
                connection.Open();

                return (int)command.ExecuteScalar();
            }
        }

        public void Update(Doctor entity)
        {
            var query = @"UPDATE Doctors SET
                         DoctorName = @DoctorName,
                         DepartmentId = @DepartmentId,
                         LicenseNumber = @LicenseNumber,
                         PhoneNumber = @PhoneNumber,
                         Email = @Email,
                         IsActive = @IsActive,
                         UpdatedAt = @UpdatedAt
                         WHERE DoctorId = @Id";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                AddParameters(command, entity);
                command.Parameters.AddWithValue("@Id", entity.DoctorId);
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            var query = "UPDATE Doctors SET IsActive = 0, UpdatedAt = GETDATE() WHERE DoctorId = @Id";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public Doctor GetByUsername(string username)
        {
            Doctor doctor = null;

            var query = @"SELECT d.*, dept.DepartmentName, dept.DepartmentCode, dept.Description as DeptDescription
                         FROM Doctors d
                         INNER JOIN Departments dept ON d.DepartmentId = dept.DepartmentId
                         WHERE d.Username = @Username AND d.IsActive = 1";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        doctor = MapToDoctor(reader);
                    }
                }
            }

            return doctor;
        }

        private Doctor MapToDoctor(SqlDataReader reader)
        {
            var doctor = new Doctor
            {
                DoctorId = reader.GetInt32(reader.GetOrdinal("DoctorId")),
                DoctorCode = reader.GetString(reader.GetOrdinal("DoctorCode")),
                DoctorName = reader.GetString(reader.GetOrdinal("DoctorName")),
                DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                LicenseNumber = reader.GetString(reader.GetOrdinal("LicenseNumber")),
                PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                Username = reader.IsDBNull(reader.GetOrdinal("Username")) ? null : reader.GetString(reader.GetOrdinal("Username")),
                PasswordHash = reader.IsDBNull(reader.GetOrdinal("PasswordHash")) ? null : reader.GetString(reader.GetOrdinal("PasswordHash")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
            };

            // Department 정보가 있으면 매핑
            var deptNameOrdinal = reader.GetOrdinal("DepartmentName");
            if (!reader.IsDBNull(deptNameOrdinal))
            {
                doctor.Department = new Department
                {
                    DepartmentId = doctor.DepartmentId,
                    DepartmentName = reader.GetString(deptNameOrdinal),
                    DepartmentCode = reader.GetString(reader.GetOrdinal("DepartmentCode")),
                    Description = reader.IsDBNull(reader.GetOrdinal("DeptDescription")) ? null : reader.GetString(reader.GetOrdinal("DeptDescription"))
                };
            }

            return doctor;
        }

        private void AddParameters(SqlCommand command, Doctor entity)
        {
            command.Parameters.AddWithValue("@DoctorCode", entity.DoctorCode);
            command.Parameters.AddWithValue("@DoctorName", entity.DoctorName);
            command.Parameters.AddWithValue("@DepartmentId", entity.DepartmentId);
            command.Parameters.AddWithValue("@LicenseNumber", entity.LicenseNumber);
            command.Parameters.AddWithValue("@PhoneNumber", (object)entity.PhoneNumber ?? DBNull.Value);
            command.Parameters.AddWithValue("@Email", (object)entity.Email ?? DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", entity.IsActive);
            command.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);
            command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
        }
    }
}
