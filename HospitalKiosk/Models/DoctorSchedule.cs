using System;
using System.Collections.Generic;
using System.Linq;

namespace HospitalKiosk.Models
{
    public enum ScheduleType
    {
        Available,      // 근무
        Vacation,       // 휴가
        Meeting,        // 회의
        ClosedDay       // 휴진
    }

    public class DoctorSchedule
    {
        public int ScheduleId { get; set; }
        public int DoctorId { get; set; }
        public string ScheduleType { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsRecurring { get; set; }
        public string RecurrencePattern { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property
        public Doctor Doctor { get; set; }

        public DoctorSchedule()
        {
            IsRecurring = false;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public bool IsAvailableSchedule()
        {
            return ScheduleType == "Available";
        }

        public bool ConflictsWith(DateTime startTime, DateTime endTime)
        {
            return StartDateTime < endTime && EndDateTime > startTime;
        }

        /// <summary>
        /// RecurrencePattern에서 반복되는 요일 목록을 가져옵니다.
        /// 패턴 형식: "WEEKLY:0,6" (매주 일요일,토요일)
        /// 0=일요일, 1=월요일, ..., 6=토요일
        /// </summary>
        public List<DayOfWeek> GetRecurringDaysOfWeek()
        {
            var result = new List<DayOfWeek>();

            if (!IsRecurring || string.IsNullOrWhiteSpace(RecurrencePattern))
                return result;

            try
            {
                var parts = RecurrencePattern.Split(':');
                if (parts.Length == 2 && parts[0] == "WEEKLY")
                {
                    var dayNumbers = parts[1].Split(',')
                        .Select(d => int.Parse(d.Trim()))
                        .Where(d => d >= 0 && d <= 6);

                    foreach (var dayNum in dayNumbers)
                    {
                        result.Add((DayOfWeek)dayNum);
                    }
                }
            }
            catch
            {
                // 파싱 실패 시 빈 리스트 반환
            }

            return result;
        }

        /// <summary>
        /// 특정 날짜가 이 반복 일정에 해당하는지 확인합니다.
        /// </summary>
        public bool IsRecurringOnDate(DateTime date)
        {
            if (!IsRecurring)
                return false;

            var recurringDays = GetRecurringDaysOfWeek();
            return recurringDays.Contains(date.DayOfWeek);
        }

        /// <summary>
        /// 요일 목록으로 RecurrencePattern을 생성합니다.
        /// </summary>
        public static string CreateWeeklyPattern(List<DayOfWeek> daysOfWeek)
        {
            if (daysOfWeek == null || !daysOfWeek.Any())
                return null;

            var dayNumbers = daysOfWeek.Select(d => (int)d).OrderBy(d => d);
            return "WEEKLY:" + string.Join(",", dayNumbers);
        }
    }
}
