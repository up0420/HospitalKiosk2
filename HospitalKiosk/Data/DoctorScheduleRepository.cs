using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using HospitalKiosk.Models;

namespace HospitalKiosk.Data
{
    public class DoctorScheduleRepository : IRepository<DoctorSchedule>
    {
        public DoctorSchedule GetById(int id)
        {
            DoctorSchedule schedule = null;

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand("SELECT * FROM DoctorSchedules WHERE ScheduleId = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        schedule = MapToSchedule(reader);
                    }
                }
            }

            return schedule;
        }

        public IEnumerable<DoctorSchedule> GetAll()
        {
            var schedules = new List<DoctorSchedule>();

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand("SELECT * FROM DoctorSchedules ORDER BY StartDateTime", connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        schedules.Add(MapToSchedule(reader));
                    }
                }
            }

            return schedules;
        }

        public IEnumerable<DoctorSchedule> GetByDoctorId(int doctorId, DateTime startDate, DateTime endDate)
        {
            var schedules = new List<DoctorSchedule>();

            var query = @"SELECT * FROM DoctorSchedules
                         WHERE DoctorId = @DoctorId
                         AND StartDateTime >= @StartDate
                         AND EndDateTime <= @EndDate
                         ORDER BY StartDateTime";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DoctorId", doctorId);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        schedules.Add(MapToSchedule(reader));
                    }
                }
            }

            return schedules;
        }

        public IEnumerable<DoctorSchedule> GetAvailableSchedules(int doctorId, DateTime date)
        {
            var schedules = new List<DoctorSchedule>();

            var query = @"SELECT * FROM DoctorSchedules
                         WHERE DoctorId = @DoctorId
                         AND ScheduleType = 'Available'
                         AND CAST(StartDateTime AS DATE) = @Date
                         ORDER BY StartDateTime";

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
                        schedules.Add(MapToSchedule(reader));
                    }
                }
            }

            return schedules;
        }

        public bool IsDoctorAvailable(int doctorId, DateTime startTime, DateTime endTime)
        {
            // 근무 시간 확인
            var workQuery = @"SELECT COUNT(*) FROM DoctorSchedules
                             WHERE DoctorId = @DoctorId
                             AND ScheduleType = 'Available'
                             AND StartDateTime <= @StartTime
                             AND EndDateTime >= @EndTime";

            // 휴가/회의/휴진 확인
            var blockQuery = @"SELECT COUNT(*) FROM DoctorSchedules
                              WHERE DoctorId = @DoctorId
                              AND ScheduleType IN ('Vacation', 'Meeting', 'ClosedDay')
                              AND StartDateTime < @EndTime
                              AND EndDateTime > @StartTime";

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                // 근무 시간에 포함되는지 확인
                using (var command = new SqlCommand(workQuery, connection))
                {
                    command.Parameters.AddWithValue("@DoctorId", doctorId);
                    command.Parameters.AddWithValue("@StartTime", startTime);
                    command.Parameters.AddWithValue("@EndTime", endTime);

                    var workCount = (int)command.ExecuteScalar();
                    if (workCount == 0) return false;
                }

                // 블록된 시간이 있는지 확인
                using (var command = new SqlCommand(blockQuery, connection))
                {
                    command.Parameters.AddWithValue("@DoctorId", doctorId);
                    command.Parameters.AddWithValue("@StartTime", startTime);
                    command.Parameters.AddWithValue("@EndTime", endTime);

                    var blockCount = (int)command.ExecuteScalar();
                    if (blockCount > 0) return false;
                }
            }

            return true;
        }

        public int Insert(DoctorSchedule entity)
        {
            var query = @"INSERT INTO DoctorSchedules (
                             DoctorId, ScheduleType, StartDateTime, EndDateTime,
                             Title, Description, IsRecurring, RecurrencePattern, CreatedAt, UpdatedAt
                         )
                         VALUES (
                             @DoctorId, @ScheduleType, @StartDateTime, @EndDateTime,
                             @Title, @Description, @IsRecurring, @RecurrencePattern, @CreatedAt, @UpdatedAt
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

        public void Update(DoctorSchedule entity)
        {
            var query = @"UPDATE DoctorSchedules SET
                         DoctorId = @DoctorId,
                         ScheduleType = @ScheduleType,
                         StartDateTime = @StartDateTime,
                         EndDateTime = @EndDateTime,
                         Title = @Title,
                         Description = @Description,
                         IsRecurring = @IsRecurring,
                         RecurrencePattern = @RecurrencePattern,
                         UpdatedAt = @UpdatedAt
                         WHERE ScheduleId = @Id";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                AddParameters(command, entity);
                command.Parameters.AddWithValue("@Id", entity.ScheduleId);
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            var query = "DELETE FROM DoctorSchedules WHERE ScheduleId = @Id";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        private DoctorSchedule MapToSchedule(SqlDataReader reader)
        {
            return new DoctorSchedule
            {
                ScheduleId = reader.GetInt32(reader.GetOrdinal("ScheduleId")),
                DoctorId = reader.GetInt32(reader.GetOrdinal("DoctorId")),
                ScheduleType = reader.GetString(reader.GetOrdinal("ScheduleType")),
                StartDateTime = reader.GetDateTime(reader.GetOrdinal("StartDateTime")),
                EndDateTime = reader.GetDateTime(reader.GetOrdinal("EndDateTime")),
                Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title")),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                IsRecurring = reader.GetBoolean(reader.GetOrdinal("IsRecurring")),
                RecurrencePattern = reader.IsDBNull(reader.GetOrdinal("RecurrencePattern")) ? null : reader.GetString(reader.GetOrdinal("RecurrencePattern")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
            };
        }

        private void AddParameters(SqlCommand command, DoctorSchedule entity)
        {
            command.Parameters.AddWithValue("@DoctorId", entity.DoctorId);
            command.Parameters.AddWithValue("@ScheduleType", entity.ScheduleType);
            command.Parameters.AddWithValue("@StartDateTime", entity.StartDateTime);
            command.Parameters.AddWithValue("@EndDateTime", entity.EndDateTime);
            command.Parameters.AddWithValue("@Title", (object)entity.Title ?? DBNull.Value);
            command.Parameters.AddWithValue("@Description", (object)entity.Description ?? DBNull.Value);
            command.Parameters.AddWithValue("@IsRecurring", entity.IsRecurring);
            command.Parameters.AddWithValue("@RecurrencePattern", (object)entity.RecurrencePattern ?? DBNull.Value);
            command.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);
            command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
        }
    }
}
