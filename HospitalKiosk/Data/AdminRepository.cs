using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using HospitalKiosk.Models;

namespace HospitalKiosk.Data
{
    public class AdminRepository : IRepository<Admin>
    {
        public Admin GetById(int id)
        {
            Admin admin = null;

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand("SELECT * FROM Admins WHERE AdminId = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        admin = MapToAdmin(reader);
                    }
                }
            }

            return admin;
        }

        public IEnumerable<Admin> GetAll()
        {
            var admins = new List<Admin>();

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand("SELECT * FROM Admins ORDER BY AdminName", connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        admins.Add(MapToAdmin(reader));
                    }
                }
            }

            return admins;
        }

        public int Insert(Admin entity)
        {
            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(@"
                INSERT INTO Admins (AdminCode, AdminName, Username, PasswordHash, Email, PhoneNumber, IsActive, CreatedAt, UpdatedAt)
                VALUES (@AdminCode, @AdminName, @Username, @PasswordHash, @Email, @PhoneNumber, @IsActive, @CreatedAt, @UpdatedAt);
                SELECT CAST(SCOPE_IDENTITY() as int);", connection))
            {
                command.Parameters.AddWithValue("@AdminCode", entity.AdminCode);
                command.Parameters.AddWithValue("@AdminName", entity.AdminName);
                command.Parameters.AddWithValue("@Username", entity.Username);
                command.Parameters.AddWithValue("@PasswordHash", entity.PasswordHash);
                command.Parameters.AddWithValue("@Email", entity.Email ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PhoneNumber", entity.PhoneNumber ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@IsActive", entity.IsActive);
                command.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);
                command.Parameters.AddWithValue("@UpdatedAt", entity.UpdatedAt);

                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        public void Update(Admin entity)
        {
            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(@"
                UPDATE Admins
                SET AdminCode = @AdminCode,
                    AdminName = @AdminName,
                    Username = @Username,
                    Email = @Email,
                    PhoneNumber = @PhoneNumber,
                    IsActive = @IsActive,
                    UpdatedAt = @UpdatedAt
                WHERE AdminId = @AdminId", connection))
            {
                command.Parameters.AddWithValue("@AdminId", entity.AdminId);
                command.Parameters.AddWithValue("@AdminCode", entity.AdminCode);
                command.Parameters.AddWithValue("@AdminName", entity.AdminName);
                command.Parameters.AddWithValue("@Username", entity.Username);
                command.Parameters.AddWithValue("@Email", entity.Email ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PhoneNumber", entity.PhoneNumber ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@IsActive", entity.IsActive);
                command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand("DELETE FROM Admins WHERE AdminId = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public Admin GetByUsername(string username)
        {
            Admin admin = null;

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand("SELECT * FROM Admins WHERE Username = @Username AND IsActive = 1", connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        admin = MapToAdmin(reader);
                    }
                }
            }

            return admin;
        }

        private Admin MapToAdmin(SqlDataReader reader)
        {
            return new Admin
            {
                AdminId = (int)reader["AdminId"],
                AdminCode = reader["AdminCode"].ToString(),
                AdminName = reader["AdminName"].ToString(),
                Username = reader["Username"].ToString(),
                PasswordHash = reader["PasswordHash"].ToString(),
                Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
                PhoneNumber = reader["PhoneNumber"] != DBNull.Value ? reader["PhoneNumber"].ToString() : null,
                IsActive = (bool)reader["IsActive"],
                CreatedAt = (DateTime)reader["CreatedAt"],
                UpdatedAt = (DateTime)reader["UpdatedAt"]
            };
        }
    }
}
