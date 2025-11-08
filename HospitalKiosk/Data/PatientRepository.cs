using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using HospitalKiosk.Models;

namespace HospitalKiosk.Data
{
    public class PatientRepository : IRepository<Patient>
    {
        public Patient GetById(int id)
        {
            Patient patient = null;

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand("SELECT * FROM Patients WHERE PatientId = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        patient = MapToPatient(reader);
                    }
                }
            }

            return patient;
        }

        public Patient GetByPatientNumber(string patientNumber)
        {
            Patient patient = null;

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand("SELECT * FROM Patients WHERE PatientNumber = @PatientNumber", connection))
            {
                command.Parameters.AddWithValue("@PatientNumber", patientNumber);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        patient = MapToPatient(reader);
                    }
                }
            }

            return patient;
        }

        public IEnumerable<Patient> GetAll()
        {
            var patients = new List<Patient>();

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand("SELECT * FROM Patients WHERE IsActive = 1 ORDER BY PatientName", connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        patients.Add(MapToPatient(reader));
                    }
                }
            }

            return patients;
        }

        public IEnumerable<Patient> SearchPatients(string searchTerm)
        {
            var patients = new List<Patient>();

            var query = @"SELECT * FROM Patients
                         WHERE IsActive = 1 AND (
                             PatientName LIKE @SearchTerm OR
                             PatientNumber LIKE @SearchTerm OR
                             PhoneNumber LIKE @SearchTerm
                         )
                         ORDER BY PatientName";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        patients.Add(MapToPatient(reader));
                    }
                }
            }

            return patients;
        }

        public int Insert(Patient entity)
        {
            var query = @"INSERT INTO Patients (PatientNumber, PatientName, BirthDate, Gender, PhoneNumber, Address, EmergencyContact, IsActive, CreatedAt, UpdatedAt)
                         VALUES (@PatientNumber, @PatientName, @BirthDate, @Gender, @PhoneNumber, @Address, @EmergencyContact, @IsActive, @CreatedAt, @UpdatedAt);
                         SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                AddParameters(command, entity);
                connection.Open();

                return (int)command.ExecuteScalar();
            }
        }

        public void Update(Patient entity)
        {
            var query = @"UPDATE Patients SET
                         PatientName = @PatientName,
                         BirthDate = @BirthDate,
                         Gender = @Gender,
                         PhoneNumber = @PhoneNumber,
                         Address = @Address,
                         EmergencyContact = @EmergencyContact,
                         IsActive = @IsActive,
                         UpdatedAt = @UpdatedAt
                         WHERE PatientId = @Id";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                AddParameters(command, entity);
                command.Parameters.AddWithValue("@Id", entity.PatientId);
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            var query = "UPDATE Patients SET IsActive = 0, UpdatedAt = GETDATE() WHERE PatientId = @Id";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public string GeneratePatientNumber()
        {
            var datePart = DateTime.Now.ToString("yyyyMMdd");
            var query = @"SELECT ISNULL(MAX(CAST(RIGHT(PatientNumber, 4) AS INT)), 0) + 1
                         FROM Patients
                         WHERE PatientNumber LIKE @Prefix";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Prefix", "PT" + datePart + "%");
                connection.Open();

                var seqNum = (int)command.ExecuteScalar();
                return "PT" + datePart + seqNum.ToString("D4");
            }
        }

        private Patient MapToPatient(SqlDataReader reader)
        {
            return new Patient
            {
                PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                PatientNumber = reader.GetString(reader.GetOrdinal("PatientNumber")),
                PatientName = reader.GetString(reader.GetOrdinal("PatientName")),
                BirthDate = reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                Gender = reader.GetString(reader.GetOrdinal("Gender")),
                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                EmergencyContact = reader.IsDBNull(reader.GetOrdinal("EmergencyContact")) ? null : reader.GetString(reader.GetOrdinal("EmergencyContact")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
            };
        }

        private void AddParameters(SqlCommand command, Patient entity)
        {
            command.Parameters.AddWithValue("@PatientNumber", entity.PatientNumber);
            command.Parameters.AddWithValue("@PatientName", entity.PatientName);
            command.Parameters.AddWithValue("@BirthDate", entity.BirthDate);
            command.Parameters.AddWithValue("@Gender", entity.Gender);
            command.Parameters.AddWithValue("@PhoneNumber", entity.PhoneNumber);
            command.Parameters.AddWithValue("@Address", (object)entity.Address ?? DBNull.Value);
            command.Parameters.AddWithValue("@EmergencyContact", (object)entity.EmergencyContact ?? DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", entity.IsActive);
            command.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);
            command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
        }
    }
}
