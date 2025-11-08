using System;

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
    }
}
