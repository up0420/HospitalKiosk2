using System;

namespace HospitalKiosk.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string DoctorCode { get; set; }
        public string DoctorName { get; set; }
        public int DepartmentId { get; set; }
        public string LicenseNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation property
        public Department Department { get; set; }

        public Doctor()
        {
            IsActive = true;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}
