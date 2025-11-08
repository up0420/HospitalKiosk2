using System;

namespace HospitalKiosk.Models
{
    public enum AppointmentStatus
    {
        Scheduled,      // 예약됨
        Completed,      // 완료
        Cancelled,      // 취소
        NoShow          // 부재
    }

    public class Appointment
    {
        public int AppointmentId { get; set; }
        public string AppointmentNumber { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public int DurationMinutes { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string CancelledBy { get; set; }
        public string CancelReason { get; set; }

        // Navigation properties
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }

        public Appointment()
        {
            DurationMinutes = 30;
            Status = "Scheduled";
            CreatedBy = "Patient";
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public DateTime GetEndDateTime()
        {
            return AppointmentDateTime.AddMinutes(DurationMinutes);
        }

        public bool IsActive()
        {
            return Status == "Scheduled";
        }

        public bool CanBeCancelled()
        {
            return Status == "Scheduled" && AppointmentDateTime > DateTime.Now;
        }
    }
}
