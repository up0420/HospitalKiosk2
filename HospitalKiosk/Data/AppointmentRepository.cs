using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using HospitalKiosk.Models;

namespace HospitalKiosk.Data
{
    public class AppointmentRepository : IRepository<Appointment>
    {
        public Appointment GetById(int id)
        {
            Appointment appointment = null;

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand("SELECT * FROM Appointments WHERE AppointmentId = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        appointment = MapToAppointment(reader);
                    }
                }
            }

            return appointment;
        }

        public IEnumerable<Appointment> GetAll()
        {
            var appointments = new List<Appointment>();

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand("SELECT * FROM Appointments ORDER BY AppointmentDateTime DESC", connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        appointments.Add(MapToAppointment(reader));
                    }
                }
            }

            return appointments;
        }

        public IEnumerable<Appointment> GetByPatientId(int patientId)
        {
            var appointments = new List<Appointment>();

            var query = @"SELECT * FROM Appointments
                         WHERE PatientId = @PatientId
                         ORDER BY AppointmentDateTime DESC";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PatientId", patientId);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        appointments.Add(MapToAppointment(reader));
                    }
                }
            }

            return appointments;
        }

        public IEnumerable<Appointment> GetByDoctorId(int doctorId, DateTime date)
        {
            var appointments = new List<Appointment>();

            var query = @"SELECT * FROM Appointments
                         WHERE DoctorId = @DoctorId
                         AND CAST(AppointmentDateTime AS DATE) = @Date
                         AND Status = 'Scheduled'
                         ORDER BY AppointmentDateTime";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DoctorId", doctorId);
                command.Parameters.AddWithValue("@Date", date.Date);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        appointments.Add(MapToAppointment(reader));
                    }
                }
            }

            return appointments;
        }

        public bool IsTimeSlotAvailable(int doctorId, DateTime startTime, DateTime endTime, int? excludeAppointmentId = null)
        {
            var query = @"SELECT COUNT(*) FROM Appointments
                         WHERE DoctorId = @DoctorId
                         AND Status = 'Scheduled'
                         AND AppointmentDateTime < @EndTime
                         AND DATEADD(MINUTE, DurationMinutes, AppointmentDateTime) > @StartTime";

            if (excludeAppointmentId.HasValue)
            {
                query += " AND AppointmentId <> @ExcludeId";
            }

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DoctorId", doctorId);
                command.Parameters.AddWithValue("@StartTime", startTime);
                command.Parameters.AddWithValue("@EndTime", endTime);

                if (excludeAppointmentId.HasValue)
                {
                    command.Parameters.AddWithValue("@ExcludeId", excludeAppointmentId.Value);
                }

                connection.Open();
                var count = (int)command.ExecuteScalar();

                return count == 0;
            }
        }

        public int Insert(Appointment entity)
        {
            var query = @"INSERT INTO Appointments (
                             AppointmentNumber, PatientId, DoctorId, AppointmentDateTime,
                             DurationMinutes, Status, Reason, Notes, CreatedBy, CreatedAt, UpdatedAt
                         )
                         VALUES (
                             @AppointmentNumber, @PatientId, @DoctorId, @AppointmentDateTime,
                             @DurationMinutes, @Status, @Reason, @Notes, @CreatedBy, @CreatedAt, @UpdatedAt
                         );
                         SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                AddParameters(command, entity);
                connection.Open();

                return (int)command.ExecuteScalar();
            }
        }

        public void Update(Appointment entity)
        {
            var query = @"UPDATE Appointments SET
                         PatientId = @PatientId,
                         DoctorId = @DoctorId,
                         AppointmentDateTime = @AppointmentDateTime,
                         DurationMinutes = @DurationMinutes,
                         Status = @Status,
                         Reason = @Reason,
                         Notes = @Notes,
                         UpdatedAt = @UpdatedAt
                         WHERE AppointmentId = @Id";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                AddParameters(command, entity);
                command.Parameters.AddWithValue("@Id", entity.AppointmentId);
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public void Cancel(int appointmentId, string cancelReason, string cancelledBy)
        {
            var query = @"UPDATE Appointments SET
                         Status = 'Cancelled',
                         CancelledAt = @CancelledAt,
                         CancelledBy = @CancelledBy,
                         CancelReason = @CancelReason,
                         UpdatedAt = @UpdatedAt
                         WHERE AppointmentId = @Id";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", appointmentId);
                command.Parameters.AddWithValue("@CancelledAt", DateTime.Now);
                command.Parameters.AddWithValue("@CancelledBy", cancelledBy);
                command.Parameters.AddWithValue("@CancelReason", (object)cancelReason ?? DBNull.Value);
                command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            Cancel(id, "Deleted", "System");
        }

        public string GenerateAppointmentNumber(DateTime appointmentDate)
        {
            var datePart = appointmentDate.ToString("yyyyMMdd");
            var query = @"SELECT ISNULL(MAX(CAST(RIGHT(AppointmentNumber, 4) AS INT)), 0) + 1
                         FROM Appointments
                         WHERE AppointmentNumber LIKE @Prefix";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Prefix", "APT" + datePart + "%");
                connection.Open();

                var seqNum = (int)command.ExecuteScalar();
                return "APT" + datePart + seqNum.ToString("D4");
            }
        }

        private Appointment MapToAppointment(SqlDataReader reader)
        {
            return new Appointment
            {
                AppointmentId = reader.GetInt32(reader.GetOrdinal("AppointmentId")),
                AppointmentNumber = reader.GetString(reader.GetOrdinal("AppointmentNumber")),
                PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                DoctorId = reader.GetInt32(reader.GetOrdinal("DoctorId")),
                AppointmentDateTime = reader.GetDateTime(reader.GetOrdinal("AppointmentDateTime")),
                DurationMinutes = reader.GetInt32(reader.GetOrdinal("DurationMinutes")),
                Status = reader.GetString(reader.GetOrdinal("Status")),
                Reason = reader.IsDBNull(reader.GetOrdinal("Reason")) ? null : reader.GetString(reader.GetOrdinal("Reason")),
                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                CreatedBy = reader.GetString(reader.GetOrdinal("CreatedBy")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                CancelledAt = reader.IsDBNull(reader.GetOrdinal("CancelledAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CancelledAt")),
                CancelledBy = reader.IsDBNull(reader.GetOrdinal("CancelledBy")) ? null : reader.GetString(reader.GetOrdinal("CancelledBy")),
                CancelReason = reader.IsDBNull(reader.GetOrdinal("CancelReason")) ? null : reader.GetString(reader.GetOrdinal("CancelReason"))
            };
        }

        private void AddParameters(SqlCommand command, Appointment entity)
        {
            command.Parameters.AddWithValue("@AppointmentNumber", entity.AppointmentNumber);
            command.Parameters.AddWithValue("@PatientId", entity.PatientId);
            command.Parameters.AddWithValue("@DoctorId", entity.DoctorId);
            command.Parameters.AddWithValue("@AppointmentDateTime", entity.AppointmentDateTime);
            command.Parameters.AddWithValue("@DurationMinutes", entity.DurationMinutes);
            command.Parameters.AddWithValue("@Status", entity.Status);
            command.Parameters.AddWithValue("@Reason", (object)entity.Reason ?? DBNull.Value);
            command.Parameters.AddWithValue("@Notes", (object)entity.Notes ?? DBNull.Value);
            command.Parameters.AddWithValue("@CreatedBy", entity.CreatedBy);
            command.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);
            command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
        }
    }
}
