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

            // 일반 일정과 모든 반복 일정을 가져옴
            var query = @"SELECT * FROM DoctorSchedules
                         WHERE DoctorId = @DoctorId
                         AND (
                             IsRecurring = 1
                             OR (StartDateTime >= @StartDate AND EndDateTime <= @EndDate)
                         )
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

            // 일반 일정과 반복 일정 모두 가져오기
            var query = @"SELECT * FROM DoctorSchedules
                         WHERE DoctorId = @DoctorId
                         AND ScheduleType = 'Available'
                         AND (
                             (IsRecurring = 0 AND CAST(StartDateTime AS DATE) = @Date)
                             OR IsRecurring = 1
                         )
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

            // 반복 일정 확장
            var result = new List<DoctorSchedule>();

            foreach (var schedule in schedules)
            {
                if (schedule.IsRecurring)
                {
                    // 반복 일정인 경우, 요일 확인
                    var recurringDays = schedule.GetRecurringDaysOfWeek();
                    if (recurringDays.Contains(date.DayOfWeek))
                    {
                        // 해당 날짜에 맞게 확장
                        result.Add(new DoctorSchedule
                        {
                            ScheduleId = schedule.ScheduleId,
                            DoctorId = schedule.DoctorId,
                            ScheduleType = schedule.ScheduleType,
                            StartDateTime = date.Date.Add(schedule.StartDateTime.TimeOfDay),
                            EndDateTime = date.Date.Add(schedule.EndDateTime.TimeOfDay),
                            Title = schedule.Title,
                            Description = schedule.Description,
                            IsRecurring = true,
                            RecurrencePattern = schedule.RecurrencePattern,
                            CreatedAt = schedule.CreatedAt,
                            UpdatedAt = schedule.UpdatedAt
                        });
                    }
                }
                else
                {
                    // 일반 일정은 그대로 추가
                    result.Add(schedule);
                }
            }

            return result;
        }

        public bool IsDoctorAvailable(int doctorId, DateTime startTime, DateTime endTime)
        {
            var targetDate = startTime.Date;
            var targetDayOfWeek = targetDate.DayOfWeek;

            // 근무 시간 확인 (일반 일정 + 반복 일정)
            var workQuery = @"SELECT * FROM DoctorSchedules
                             WHERE DoctorId = @DoctorId
                             AND ScheduleType = 'Available'
                             AND (
                                 (IsRecurring = 0 AND CAST(StartDateTime AS DATE) = @Date)
                                 OR IsRecurring = 1
                             )";

            // 휴가/회의/휴진 확인 (일반 일정 + 반복 일정)
            var blockQuery = @"SELECT * FROM DoctorSchedules
                              WHERE DoctorId = @DoctorId
                              AND ScheduleType IN ('Vacation', 'Meeting', 'ClosedDay')
                              AND (
                                  (IsRecurring = 0 AND CAST(StartDateTime AS DATE) = @Date)
                                  OR IsRecurring = 1
                              )";

            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();

                // 근무 시간에 포함되는지 확인
                bool hasWorkSchedule = false;
                using (var command = new SqlCommand(workQuery, connection))
                {
                    command.Parameters.AddWithValue("@DoctorId", doctorId);
                    command.Parameters.AddWithValue("@Date", targetDate);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var schedule = MapToSchedule(reader);

                            if (schedule.IsRecurring)
                            {
                                // 반복 일정: 요일 확인 후 시간 확장
                                var recurringDays = schedule.GetRecurringDaysOfWeek();
                                if (recurringDays.Contains(targetDayOfWeek))
                                {
                                    var scheduleStart = targetDate.Add(schedule.StartDateTime.TimeOfDay);
                                    var scheduleEnd = targetDate.Add(schedule.EndDateTime.TimeOfDay);

                                    if (scheduleStart <= startTime && scheduleEnd >= endTime)
                                    {
                                        hasWorkSchedule = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                // 일반 일정
                                if (schedule.StartDateTime <= startTime && schedule.EndDateTime >= endTime)
                                {
                                    hasWorkSchedule = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (!hasWorkSchedule) return false;

                // 블록된 시간이 있는지 확인
                using (var command = new SqlCommand(blockQuery, connection))
                {
                    command.Parameters.AddWithValue("@DoctorId", doctorId);
                    command.Parameters.AddWithValue("@Date", targetDate);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var schedule = MapToSchedule(reader);

                            if (schedule.IsRecurring)
                            {
                                // 반복 일정: 요일 확인 후 시간 확장
                                var recurringDays = schedule.GetRecurringDaysOfWeek();
                                if (recurringDays.Contains(targetDayOfWeek))
                                {
                                    var scheduleStart = targetDate.Add(schedule.StartDateTime.TimeOfDay);
                                    var scheduleEnd = targetDate.Add(schedule.EndDateTime.TimeOfDay);

                                    // 시간이 겹치는지 확인
                                    if (scheduleStart < endTime && scheduleEnd > startTime)
                                    {
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                // 일반 일정
                                if (schedule.StartDateTime < endTime && schedule.EndDateTime > startTime)
                                {
                                    return false;
                                }
                            }
                        }
                    }
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

        /// <summary>
        /// 특정 의사의 특정 월의 모든 일정을 삭제합니다.
        /// </summary>
        public int DeleteByDoctorAndMonth(int doctorId, int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1).Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            var query = @"DELETE FROM DoctorSchedules
                         WHERE DoctorId = @DoctorId
                         AND (
                             (IsRecurring = 0 AND StartDateTime >= @StartDate AND StartDateTime <= @EndDate)
                             OR IsRecurring = 1
                         )";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DoctorId", doctorId);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);
                connection.Open();

                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 특정 의사의 모든 일정을 삭제합니다.
        /// </summary>
        public int DeleteAllByDoctor(int doctorId)
        {
            var query = "DELETE FROM DoctorSchedules WHERE DoctorId = @DoctorId";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DoctorId", doctorId);
                connection.Open();

                return command.ExecuteNonQuery();
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
