using System;
using System.Collections.Generic;
using System.Linq;
using HospitalKiosk.Data;
using HospitalKiosk.Models;

namespace HospitalKiosk.Services
{
    public class DoctorScheduleService
    {
        private readonly DoctorScheduleRepository _scheduleRepo;
        private readonly DoctorRepository _doctorRepo;
        private readonly DepartmentRepository _departmentRepo;

        public DoctorScheduleService()
        {
            _scheduleRepo = new DoctorScheduleRepository();
            _doctorRepo = new DoctorRepository();
            _departmentRepo = new DepartmentRepository();
        }

        /// <summary>
        /// 의사 일정 등록
        /// </summary>
        public ScheduleResult CreateSchedule(int doctorId, string scheduleType, DateTime startDateTime,
            DateTime endDateTime, string title, string description)
        {
            try
            {
                // 의사 확인
                var doctor = _doctorRepo.GetById(doctorId);
                if (doctor == null)
                {
                    return new ScheduleResult { Success = false, Message = "의사를 찾을 수 없습니다." };
                }

                // 시간 유효성 검증
                if (startDateTime >= endDateTime)
                {
                    return new ScheduleResult { Success = false, Message = "종료 시간은 시작 시간보다 늦어야 합니다." };
                }

                var schedule = new DoctorSchedule
                {
                    DoctorId = doctorId,
                    ScheduleType = scheduleType,
                    StartDateTime = startDateTime,
                    EndDateTime = endDateTime,
                    Title = title,
                    Description = description
                };

                var scheduleId = _scheduleRepo.Insert(schedule);
                schedule.ScheduleId = scheduleId;

                return new ScheduleResult
                {
                    Success = true,
                    Message = "일정이 등록되었습니다.",
                    Schedule = schedule
                };
            }
            catch (Exception ex)
            {
                return new ScheduleResult { Success = false, Message = "일정 등록 중 오류가 발생했습니다: " + ex.Message };
            }
        }

        /// <summary>
        /// 의사 일정 수정
        /// </summary>
        public ScheduleResult UpdateSchedule(int scheduleId, string scheduleType, DateTime startDateTime,
            DateTime endDateTime, string title, string description)
        {
            try
            {
                var schedule = _scheduleRepo.GetById(scheduleId);
                if (schedule == null)
                {
                    return new ScheduleResult { Success = false, Message = "일정을 찾을 수 없습니다." };
                }

                // 시간 유효성 검증
                if (startDateTime >= endDateTime)
                {
                    return new ScheduleResult { Success = false, Message = "종료 시간은 시작 시간보다 늦어야 합니다." };
                }

                schedule.ScheduleType = scheduleType;
                schedule.StartDateTime = startDateTime;
                schedule.EndDateTime = endDateTime;
                schedule.Title = title;
                schedule.Description = description;

                _scheduleRepo.Update(schedule);

                return new ScheduleResult
                {
                    Success = true,
                    Message = "일정이 수정되었습니다.",
                    Schedule = schedule
                };
            }
            catch (Exception ex)
            {
                return new ScheduleResult { Success = false, Message = "일정 수정 중 오류가 발생했습니다: " + ex.Message };
            }
        }

        /// <summary>
        /// 의사 일정 삭제
        /// </summary>
        public ScheduleResult DeleteSchedule(int scheduleId)
        {
            try
            {
                var schedule = _scheduleRepo.GetById(scheduleId);
                if (schedule == null)
                {
                    return new ScheduleResult { Success = false, Message = "일정을 찾을 수 없습니다." };
                }

                _scheduleRepo.Delete(scheduleId);

                return new ScheduleResult { Success = true, Message = "일정이 삭제되었습니다." };
            }
            catch (Exception ex)
            {
                return new ScheduleResult { Success = false, Message = "일정 삭제 중 오류가 발생했습니다: " + ex.Message };
            }
        }

        /// <summary>
        /// 의사의 일정 조회
        /// </summary>
        public List<ScheduleDetail> GetDoctorSchedules(int doctorId, DateTime startDate, DateTime endDate)
        {
            var schedules = _scheduleRepo.GetByDoctorId(doctorId, startDate, endDate);
            var result = new List<ScheduleDetail>();

            var doctor = _doctorRepo.GetById(doctorId);

            foreach (var schedule in schedules)
            {
                result.Add(new ScheduleDetail
                {
                    Schedule = schedule,
                    DoctorName = doctor?.DoctorName,
                    DepartmentName = doctor?.Department?.DepartmentName
                });
            }

            return result;
        }

        /// <summary>
        /// 진료과별 근무 중인 의사 목록 조회
        /// </summary>
        public List<DoctorAvailability> GetAvailableDoctors(int departmentId, DateTime date)
        {
            var doctors = _doctorRepo.GetByDepartmentId(departmentId);
            var result = new List<DoctorAvailability>();

            foreach (var doctor in doctors)
            {
                var availableSchedules = _scheduleRepo.GetAvailableSchedules(doctor.DoctorId, date);
                var hasAvailability = availableSchedules.Any();

                result.Add(new DoctorAvailability
                {
                    Doctor = doctor,
                    Date = date,
                    IsAvailable = hasAvailability,
                    AvailableSlots = availableSchedules.Count()
                });
            }

            return result.Where(x => x.IsAvailable).ToList();
        }

        /// <summary>
        /// 모든 진료과 목록 조회
        /// </summary>
        public List<Department> GetAllDepartments()
        {
            return new List<Department>(_departmentRepo.GetAll());
        }

        /// <summary>
        /// 진료과별 의사 목록 조회
        /// </summary>
        public List<Doctor> GetDoctorsByDepartment(int departmentId)
        {
            return new List<Doctor>(_doctorRepo.GetByDepartmentId(departmentId));
        }
    }

    // 헬퍼 클래스들
    public class ScheduleResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public DoctorSchedule Schedule { get; set; }
    }

    public class ScheduleDetail
    {
        public DoctorSchedule Schedule { get; set; }
        public string DoctorName { get; set; }
        public string DepartmentName { get; set; }
    }

    public class DoctorAvailability
    {
        public Doctor Doctor { get; set; }
        public DateTime Date { get; set; }
        public bool IsAvailable { get; set; }
        public int AvailableSlots { get; set; }
    }
}
