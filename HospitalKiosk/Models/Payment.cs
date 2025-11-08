using System;

namespace HospitalKiosk.Models
{
    public enum PaymentStatus
    {
        Pending,        // 미납
        Paid,           // 납부완료
        PartiallyPaid,  // 부분납부
        Refunded        // 환불
    }

    public enum PaymentMethod
    {
        Cash,           // 현금
        Card,           // 카드
        Transfer        // 계좌이체
    }

    public class Payment
    {
        public int PaymentId { get; set; }
        public string PaymentNumber { get; set; }
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? PaymentDateTime { get; set; }
        public string ReceiptNumber { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Appointment Appointment { get; set; }
        public Patient Patient { get; set; }

        public Payment()
        {
            PaidAmount = 0;
            PaymentStatus = "Pending";
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public decimal GetRemainingAmount()
        {
            return TotalAmount - PaidAmount;
        }

        public bool IsFullyPaid()
        {
            return PaidAmount >= TotalAmount;
        }

        public void UpdatePaymentStatus()
        {
            if (PaidAmount >= TotalAmount)
            {
                PaymentStatus = "Paid";
            }
            else if (PaidAmount > 0)
            {
                PaymentStatus = "PartiallyPaid";
            }
            else
            {
                PaymentStatus = "Pending";
            }
        }
    }
}
