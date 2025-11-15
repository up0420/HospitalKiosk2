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
        /// 의사의 일정 조회 (반복 일정 포함)
        /// </summary>
        public List<ScheduleDetail> GetDoctorSchedules(int doctorId, DateTime startDate, DateTime endDate)
        {
            var schedules = _scheduleRepo.GetByDoctorId(doctorId, startDate, endDate);
            var result = new List<ScheduleDetail>();

            var doctor = _doctorRepo.GetById(doctorId);

            // 일반 일정 추가
            foreach (var schedule in schedules)
            {
                if (!schedule.IsRecurring)
                {
                    result.Add(new ScheduleDetail
                    {
                        Schedule = schedule,
                        DoctorName = doctor?.DoctorName,
                        DepartmentName = doctor?.Department?.DepartmentName
                    });
                }
            }

            // 반복 일정 확장
            var recurringSchedules = schedules.Where(s => s.IsRecurring).ToList();
            var expandedSchedules = ExpandRecurringSchedules(recurringSchedules, startDate, endDate);

            foreach (var schedule in expandedSchedules)
            {
                result.Add(new ScheduleDetail
                {
                    Schedule = schedule,
                    DoctorName = doctor?.DoctorName,
                    DepartmentName = doctor?.Department?.DepartmentName
                });
            }

            return result.OrderBy(s => s.Schedule.StartDateTime).ToList();
        }

        /// <summary>
        /// 반복 일정을 기간 내의 실제 일정으로 확장합니다.
        /// </summary>
        private List<DoctorSchedule> ExpandRecurringSchedules(List<DoctorSchedule> recurringSchedules, DateTime startDate, DateTime endDate)
        {
            var result = new List<DoctorSchedule>();

            foreach (var template in recurringSchedules)
            {
                if (!template.IsRecurring)
                    continue;

                var recurringDays = template.GetRecurringDaysOfWeek();
                if (!recurringDays.Any())
                    continue;

                // 시작 시간과 종료 시간 (시간 부분만)
                var startTime = template.StartDateTime.TimeOfDay;
                var endTime = template.EndDateTime.TimeOfDay;

                // 기간 내의 모든 날짜에 대해 반복
                for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                {
                    if (recurringDays.Contains(date.DayOfWeek))
                    {
                        result.Add(new DoctorSchedule
                        {
                            ScheduleId = template.ScheduleId, // 원본 ID 유지 (읽기 전용)
                            DoctorId = template.DoctorId,
                            ScheduleType = template.ScheduleType,
                            StartDateTime = date.Add(startTime),
                            EndDateTime = date.Add(endTime),
                            Title = template.Title,
                            Description = template.Description,
                            IsRecurring = true,
                            RecurrencePattern = template.RecurrencePattern
                        });
                    }
                }
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

        /// <summary>
        /// 의사의 기본 평일 근무 스케줄을 생성합니다 (월~금 9:00-18:00)
        /// 1년치 실제 일정을 DB에 생성합니다.
        /// </summary>
        public ScheduleResult CreateDefaultWeekdaySchedule(int doctorId)
        {
            try
            {
                var doctor = _doctorRepo.GetById(doctorId);
                if (doctor == null)
                {
                    return new ScheduleResult { Success = false, Message = "의사를 찾을 수 없습니다." };
                }

                // 오늘부터 1년 후까지
                var startDate = DateTime.Today;
                var endDate = DateTime.Today.AddYears(1);

                // 평일(월~금)
                var weekdays = new List<DayOfWeek>
                {
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday
                };

                int createdCount = 0;

                // 1년치 평일마다 일정 생성
                for (var date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    // 평일인지 확인
                    if (weekdays.Contains(date.DayOfWeek))
                    {
                        var schedule = new DoctorSchedule
                        {
                            DoctorId = doctorId,
                            ScheduleType = "Available",
                            StartDateTime = date.AddHours(9),  // 9:00
                            EndDateTime = date.AddHours(18),   // 18:00
                            Title = "정규 진료 시간",
                            Description = "평일 기본 근무 시간",
                            IsRecurring = false,
                            RecurrencePattern = null
                        };

                        _scheduleRepo.Insert(schedule);
                        createdCount++;
                    }
                }

                return new ScheduleResult
                {
                    Success = true,
                    Message = $"기본 평일 근무 스케줄 {createdCount}개가 생성되었습니다. (1년치)",
                    Schedule = null
                };
            }
            catch (Exception ex)
            {
                return new ScheduleResult { Success = false, Message = "기본 스케줄 생성 중 오류: " + ex.Message };
            }
        }

        /// <summary>
        /// 특정 요일을 반복 휴진일로 설정합니다
        /// </summary>
        public ScheduleResult CreateRecurringClosedDay(int doctorId, List<DayOfWeek> daysOfWeek, string description = "정기 휴진")
        {
            try
            {
                var doctor = _doctorRepo.GetById(doctorId);
                if (doctor == null)
                {
                    return new ScheduleResult { Success = false, Message = "의사를 찾을 수 없습니다." };
                }

                if (daysOfWeek == null || !daysOfWeek.Any())
                {
                    return new ScheduleResult { Success = false, Message = "휴진 요일을 선택해주세요." };
                }

                var schedule = new DoctorSchedule
                {
                    DoctorId = doctorId,
                    ScheduleType = "ClosedDay",
                    StartDateTime = DateTime.Today.AddHours(9),
                    EndDateTime = DateTime.Today.AddHours(18),
                    Title = "정기 휴진",
                    Description = description,
                    IsRecurring = true,
                    RecurrencePattern = DoctorSchedule.CreateWeeklyPattern(daysOfWeek)
                };

                var scheduleId = _scheduleRepo.Insert(schedule);
                schedule.ScheduleId = scheduleId;

                return new ScheduleResult
                {
                    Success = true,
                    Message = "정기 휴진일이 설정되었습니다.",
                    Schedule = schedule
                };
            }
            catch (Exception ex)
            {
                return new ScheduleResult { Success = false, Message = "휴진일 설정 중 오류: " + ex.Message };
            }
        }

        /// <summary>
        /// 특정 월의 모든 일정을 삭제합니다
        /// </summary>
        public ScheduleResult DeleteSchedulesByMonth(int doctorId, int year, int month)
        {
            try
            {
                var doctor = _doctorRepo.GetById(doctorId);
                if (doctor == null)
                {
                    return new ScheduleResult { Success = false, Message = "의사를 찾을 수 없습니다." };
                }

                var deletedCount = _scheduleRepo.DeleteByDoctorAndMonth(doctorId, year, month);

                return new ScheduleResult
                {
                    Success = true,
                    Message = $"{year}년 {month}월의 일정 {deletedCount}건이 삭제되었습니다."
                };
            }
            catch (Exception ex)
            {
                return new ScheduleResult { Success = false, Message = "일정 삭제 중 오류: " + ex.Message };
            }
        }

        /// <summary>
        /// 모든 일정을 삭제합니다
        /// </summary>
        public ScheduleResult DeleteAllSchedules(int doctorId)
        {
            try
            {
                var doctor = _doctorRepo.GetById(doctorId);
                if (doctor == null)
                {
                    return new ScheduleResult { Success = false, Message = "의사를 찾을 수 없습니다." };
                }

                var deletedCount = _scheduleRepo.DeleteAllByDoctor(doctorId);

                return new ScheduleResult
                {
                    Success = true,
                    Message = $"전체 일정 {deletedCount}건이 삭제되었습니다."
                };
            }
            catch (Exception ex)
            {
                return new ScheduleResult { Success = false, Message = "일정 삭제 중 오류: " + ex.Message };
            }
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
