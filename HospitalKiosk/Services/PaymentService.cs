using System;
using System.Collections.Generic;
using HospitalKiosk.Data;
using HospitalKiosk.Models;

namespace HospitalKiosk.Services
{
    public class PaymentService
    {
        private readonly PaymentRepository _paymentRepo;
        private readonly AppointmentRepository _appointmentRepo;
        private readonly PatientRepository _patientRepo;

        public PaymentService()
        {
            _paymentRepo = new PaymentRepository();
            _appointmentRepo = new AppointmentRepository();
            _patientRepo = new PatientRepository();
        }

        /// <summary>
        /// 수납 생성
        /// </summary>
        public PaymentResult CreatePayment(int appointmentId, decimal totalAmount, string paymentMethod, decimal paidAmount)
        {
            try
            {
                // 1. 예약 확인
                var appointment = _appointmentRepo.GetById(appointmentId);
                if (appointment == null)
                {
                    return new PaymentResult { Success = false, Message = "예약을 찾을 수 없습니다." };
                }

                // 2. 기존 수납 확인 (중복 방지)
                var existingPayment = _paymentRepo.GetByAppointmentId(appointmentId);
                if (existingPayment != null)
                {
                    return new PaymentResult { Success = false, Message = "이미 수납 내역이 존재합니다." };
                }

                // 3. 금액 유효성 검증
                if (totalAmount <= 0)
                {
                    return new PaymentResult { Success = false, Message = "총 금액은 0보다 커야 합니다." };
                }

                if (paidAmount < 0 || paidAmount > totalAmount)
                {
                    return new PaymentResult { Success = false, Message = "납부 금액이 올바르지 않습니다." };
                }

                // 4. 수납 생성
                var payment = new Payment
                {
                    PaymentNumber = _paymentRepo.GeneratePaymentNumber(),
                    AppointmentId = appointmentId,
                    PatientId = appointment.PatientId,
                    TotalAmount = totalAmount,
                    PaidAmount = paidAmount,
                    PaymentMethod = paymentMethod,
                    PaymentDateTime = DateTime.Now
                };

                payment.UpdatePaymentStatus();

                // 영수증 번호 생성 (납부 완료 시에만)
                if (payment.IsFullyPaid())
                {
                    payment.ReceiptNumber = _paymentRepo.GenerateReceiptNumber();
                }

                var paymentId = _paymentRepo.Insert(payment);
                payment.PaymentId = paymentId;

                return new PaymentResult
                {
                    Success = true,
                    Message = "수납이 완료되었습니다.",
                    Payment = payment
                };
            }
            catch (Exception ex)
            {
                return new PaymentResult { Success = false, Message = "수납 처리 중 오류가 발생했습니다: " + ex.Message };
            }
        }

        /// <summary>
        /// 추가 납부 처리
        /// </summary>
        public PaymentResult ProcessAdditionalPayment(int paymentId, decimal amount, string paymentMethod)
        {
            try
            {
                var payment = _paymentRepo.GetById(paymentId);
                if (payment == null)
                {
                    return new PaymentResult { Success = false, Message = "수납 내역을 찾을 수 없습니다." };
                }

                if (payment.IsFullyPaid())
                {
                    return new PaymentResult { Success = false, Message = "이미 완납된 수납입니다." };
                }

                var remainingAmount = payment.GetRemainingAmount();
                if (amount > remainingAmount)
                {
                    return new PaymentResult { Success = false, Message = "납부 금액이 잔액보다 큽니다." };
                }

                _paymentRepo.ProcessPayment(paymentId, amount, paymentMethod);

                // 완납 시 영수증 번호 생성
                payment = _paymentRepo.GetById(paymentId);
                if (payment.IsFullyPaid() && string.IsNullOrEmpty(payment.ReceiptNumber))
                {
                    payment.ReceiptNumber = _paymentRepo.GenerateReceiptNumber();
                    _paymentRepo.Update(payment);
                }

                return new PaymentResult
                {
                    Success = true,
                    Message = "납부가 완료되었습니다.",
                    Payment = payment
                };
            }
            catch (Exception ex)
            {
                return new PaymentResult { Success = false, Message = "납부 처리 중 오류가 발생했습니다: " + ex.Message };
            }
        }

        /// <summary>
        /// 예약별 수납 정보 조회
        /// </summary>
        public Payment GetPaymentByAppointmentId(int appointmentId)
        {
            return _paymentRepo.GetByAppointmentId(appointmentId);
        }

        /// <summary>
        /// 환자의 수납 내역 조회
        /// </summary>
        public List<PaymentDetail> GetPatientPayments(int patientId)
        {
            var payments = _paymentRepo.GetByPatientId(patientId);
            var result = new List<PaymentDetail>();

            foreach (var payment in payments)
            {
                var appointment = _appointmentRepo.GetById(payment.AppointmentId);
                var patient = _patientRepo.GetById(payment.PatientId);

                result.Add(new PaymentDetail
                {
                    Payment = payment,
                    PatientName = patient?.PatientName,
                    AppointmentDateTime = appointment?.AppointmentDateTime
                });
            }

            return result;
        }

        /// <summary>
        /// 미납 수납 목록 조회
        /// </summary>
        public List<PaymentDetail> GetPendingPayments()
        {
            var payments = _paymentRepo.GetPendingPayments();
            var result = new List<PaymentDetail>();

            foreach (var payment in payments)
            {
                var appointment = _appointmentRepo.GetById(payment.AppointmentId);
                var patient = _patientRepo.GetById(payment.PatientId);

                result.Add(new PaymentDetail
                {
                    Payment = payment,
                    PatientName = patient?.PatientName,
                    AppointmentDateTime = appointment?.AppointmentDateTime
                });
            }

            return result;
        }

        /// <summary>
        /// 영수증 정보 조회
        /// </summary>
        public ReceiptInfo GetReceiptInfo(int paymentId)
        {
            var payment = _paymentRepo.GetById(paymentId);
            if (payment == null) return null;

            var appointment = _appointmentRepo.GetById(payment.AppointmentId);
            var patient = _patientRepo.GetById(payment.PatientId);

            return new ReceiptInfo
            {
                Payment = payment,
                PatientName = patient?.PatientName,
                PatientNumber = patient?.PatientNumber,
                AppointmentDateTime = appointment?.AppointmentDateTime
            };
        }

        /// <summary>
        /// 수납 가능한 예약 확인
        /// </summary>
        public bool CanCreatePayment(int appointmentId)
        {
            var appointment = _appointmentRepo.GetById(appointmentId);
            if (appointment == null) return false;

            var existingPayment = _paymentRepo.GetByAppointmentId(appointmentId);
            return existingPayment == null;
        }
    }

    // 헬퍼 클래스들
    public class PaymentResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Payment Payment { get; set; }
    }

    public class PaymentDetail
    {
        public Payment Payment { get; set; }
        public string PatientName { get; set; }
        public DateTime? AppointmentDateTime { get; set; }
    }

    public class ReceiptInfo
    {
        public Payment Payment { get; set; }
        public string PatientName { get; set; }
        public string PatientNumber { get; set; }
        public DateTime? AppointmentDateTime { get; set; }
    }
}
