using System;
using System.Collections.Generic;
using System.Linq;
using HospitalKiosk.Data;
using HospitalKiosk.Models;

namespace HospitalKiosk.Services
{
    public class AppointmentService
    {
        private readonly AppointmentRepository _appointmentRepo;
        private readonly DoctorScheduleRepository _scheduleRepo;
        private readonly PatientRepository _patientRepo;
        private readonly DoctorRepository _doctorRepo;

        public AppointmentService()
        {
            _appointmentRepo = new AppointmentRepository();
            _scheduleRepo = new DoctorScheduleRepository();
            _patientRepo = new PatientRepository();
            _doctorRepo = new DoctorRepository();
        }

        /// <summary>
        /// 특정 날짜의 의사 가용 시간 슬롯 조회
        /// </summary>
        public List<TimeSlot> GetAvailableTimeSlots(int doctorId, DateTime date, int slotDurationMinutes = 30)
        {
            var availableSlots = new List<TimeSlot>();
            var targetDate = date.Date;

            // 의사의 근무 시간 조회
            var workSchedules = _scheduleRepo.GetAvailableSchedules(doctorId, targetDate);
            if (!workSchedules.Any())
            {
                return availableSlots; // 근무 시간이 없음
            }

            foreach (var workSchedule in workSchedules)
            {
                var currentTime = workSchedule.StartDateTime;
                var endTime = workSchedule.EndDateTime;

                // 시간 슬롯 생성
                while (currentTime.AddMinutes(slotDurationMinutes) <= endTime)
                {
                    var slotEndTime = currentTime.AddMinutes(slotDurationMinutes);

                    // 예약 가능 여부 확인
                    bool isAvailable = _appointmentRepo.IsTimeSlotAvailable(doctorId, currentTime, slotEndTime);

                    // 의사가 가용한지 확인 (휴가, 회의, 휴진이 아닌지)
                    if (isAvailable)
                    {
                        isAvailable = _scheduleRepo.IsDoctorAvailable(doctorId, currentTime, slotEndTime);
                    }

                    availableSlots.Add(new TimeSlot
                    {
                        StartTime = currentTime,
                        EndTime = slotEndTime,
                        IsAvailable = isAvailable
                    });

                    currentTime = slotEndTime;
                }
            }

            return availableSlots;
        }

        /// <summary>
        /// 예약 생성
        /// </summary>
        public AppointmentResult CreateAppointment(int patientId, int doctorId, DateTime appointmentDateTime,
            int durationMinutes, string reason, string createdBy = "Patient")
        {
            try
            {
                // 1. 환자 확인
                var patient = _patientRepo.GetById(patientId);
                if (patient == null)
                {
                    return new AppointmentResult { Success = false, Message = "환자 정보를 찾을 수 없습니다." };
                }

                // 2. 의사 확인
                var doctor = _doctorRepo.GetById(doctorId);
                if (doctor == null)
                {
                    return new AppointmentResult { Success = false, Message = "의사 정보를 찾을 수 없습니다." };
                }

                // 3. 시간 슬롯 가용성 확인
                var endTime = appointmentDateTime.AddMinutes(durationMinutes);
                if (!_appointmentRepo.IsTimeSlotAvailable(doctorId, appointmentDateTime, endTime))
                {
                    return new AppointmentResult { Success = false, Message = "선택한 시간에 이미 다른 예약이 있습니다." };
                }

                // 4. 의사 일정 확인
                if (!_scheduleRepo.IsDoctorAvailable(doctorId, appointmentDateTime, endTime))
                {
                    return new AppointmentResult { Success = false, Message = "선택한 시간에 의사가 진료할 수 없습니다." };
                }

                // 5. 예약 생성
                var appointment = new Appointment
                {
                    AppointmentNumber = _appointmentRepo.GenerateAppointmentNumber(appointmentDateTime),
                    PatientId = patientId,
                    DoctorId = doctorId,
                    AppointmentDateTime = appointmentDateTime,
                    DurationMinutes = durationMinutes,
                    Reason = reason,
                    Status = "Scheduled",
                    CreatedBy = createdBy
                };

                var appointmentId = _appointmentRepo.Insert(appointment);
                appointment.AppointmentId = appointmentId;

                return new AppointmentResult
                {
                    Success = true,
                    Message = "예약이 완료되었습니다.",
                    Appointment = appointment
                };
            }
            catch (Exception ex)
            {
                return new AppointmentResult { Success = false, Message = "예약 중 오류가 발생했습니다: " + ex.Message };
            }
        }

        /// <summary>
        /// 예약 취소
        /// </summary>
        public AppointmentResult CancelAppointment(int appointmentId, string cancelReason, string cancelledBy = "Patient")
        {
            try
            {
                var appointment = _appointmentRepo.GetById(appointmentId);
                if (appointment == null)
                {
                    return new AppointmentResult { Success = false, Message = "예약을 찾을 수 없습니다." };
                }

                if (!appointment.CanBeCancelled())
                {
                    return new AppointmentResult { Success = false, Message = "취소할 수 없는 예약입니다." };
                }

                _appointmentRepo.Cancel(appointmentId, cancelReason, cancelledBy);

                return new AppointmentResult { Success = true, Message = "예약이 취소되었습니다." };
            }
            catch (Exception ex)
            {
                return new AppointmentResult { Success = false, Message = "예약 취소 중 오류가 발생했습니다: " + ex.Message };
            }
        }

        /// <summary>
        /// 예약 상태 변경
        /// </summary>
        public AppointmentResult UpdateAppointmentStatus(int appointmentId, string newStatus)
        {
            try
            {
                var appointment = _appointmentRepo.GetById(appointmentId);
                if (appointment == null)
                {
                    return new AppointmentResult { Success = false, Message = "예약을 찾을 수 없습니다." };
                }

                // 유효한 상태 값인지 확인
                var validStatuses = new[] { "Scheduled", "Completed", "Cancelled", "NoShow" };
                if (!validStatuses.Contains(newStatus))
                {
                    return new AppointmentResult { Success = false, Message = "유효하지 않은 상태 값입니다." };
                }

                appointment.Status = newStatus;
                _appointmentRepo.Update(appointment);

                return new AppointmentResult { Success = true, Message = "예약 상태가 변경되었습니다." };
            }
            catch (Exception ex)
            {
                return new AppointmentResult { Success = false, Message = "상태 변경 중 오류가 발생했습니다: " + ex.Message };
            }
        }

        /// <summary>
        /// 환자의 예약 목록 조회
        /// </summary>
        public List<AppointmentDetail> GetPatientAppointments(int patientId)
        {
            var appointments = _appointmentRepo.GetByPatientId(patientId);
            var result = new List<AppointmentDetail>();

            foreach (var appointment in appointments)
            {
                var doctor = _doctorRepo.GetById(appointment.DoctorId);
                var patient = _patientRepo.GetById(appointment.PatientId);

                result.Add(new AppointmentDetail
                {
                    Appointment = appointment,
                    DoctorName = doctor?.DoctorName,
                    DepartmentName = doctor?.Department?.DepartmentName,
                    PatientName = patient?.PatientName
                });
            }

            return result;
        }

        /// <summary>
        /// 특정 날짜의 의사별 예약 목록 조회
        /// </summary>
        public List<AppointmentDetail> GetDoctorAppointments(int doctorId, DateTime date)
        {
            var appointments = _appointmentRepo.GetByDoctorId(doctorId, date);
            var result = new List<AppointmentDetail>();

            var doctor = _doctorRepo.GetById(doctorId);

            foreach (var appointment in appointments)
            {
                var patient = _patientRepo.GetById(appointment.PatientId);

                result.Add(new AppointmentDetail
                {
                    Appointment = appointment,
                    DoctorName = doctor?.DoctorName,
                    DepartmentName = doctor?.Department?.DepartmentName,
                    PatientName = patient?.PatientName
                });
            }

            return result;
        }

        /// <summary>
        /// 예약 상세 조회
        /// </summary>
        public AppointmentDetail GetAppointmentDetail(int appointmentId)
        {
            var appointment = _appointmentRepo.GetById(appointmentId);
            if (appointment == null) return null;

            var doctor = _doctorRepo.GetById(appointment.DoctorId);
            var patient = _patientRepo.GetById(appointment.PatientId);

            return new AppointmentDetail
            {
                Appointment = appointment,
                DoctorName = doctor?.DoctorName,
                DepartmentName = doctor?.Department?.DepartmentName,
                PatientName = patient?.PatientName
            };
        }
    }

    // 헬퍼 클래스들
    public class TimeSlot
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsAvailable { get; set; }

        public string DisplayTime
        {
            get { return StartTime.ToString("HH:mm") + " - " + EndTime.ToString("HH:mm"); }
        }
    }

    public class AppointmentResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Appointment Appointment { get; set; }
    }

    public class AppointmentDetail
    {
        public Appointment Appointment { get; set; }
        public string DoctorName { get; set; }
        public string DepartmentName { get; set; }
        public string PatientName { get; set; }
    }
}
