-- =============================================
-- Hospital Kiosk - Views for Analytics
-- =============================================

USE HospitalKiosk;
GO

-- =============================================
-- 뷰: 예약 상세 정보
-- =============================================
IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_AppointmentDetails')
    DROP VIEW vw_AppointmentDetails;
GO

CREATE VIEW vw_AppointmentDetails
AS
SELECT
    a.AppointmentId,
    a.AppointmentNumber,
    a.AppointmentDateTime,
    a.DurationMinutes,
    a.Status,
    a.Reason,
    p.PatientId,
    p.PatientNumber,
    p.PatientName,
    p.PhoneNumber AS PatientPhone,
    d.DoctorId,
    d.DoctorCode,
    d.DoctorName,
    dept.DepartmentId,
    dept.DepartmentName,
    dept.DepartmentCode,
    a.CreatedBy,
    a.CreatedAt,
    pay.PaymentStatus,
    pay.TotalAmount,
    pay.PaidAmount
FROM Appointments a
INNER JOIN Patients p ON a.PatientId = p.PatientId
INNER JOIN Doctors d ON a.DoctorId = d.DoctorId
INNER JOIN Departments dept ON d.DepartmentId = dept.DepartmentId
LEFT JOIN Payments pay ON a.AppointmentId = pay.AppointmentId;
GO

-- =============================================
-- 뷰: 시간대별 예약 통계
-- =============================================
IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_HourlyAppointmentStats')
    DROP VIEW vw_HourlyAppointmentStats;
GO

CREATE VIEW vw_HourlyAppointmentStats
AS
SELECT
    CAST(AppointmentDateTime AS DATE) AS AppointmentDate,
    DATEPART(HOUR, AppointmentDateTime) AS HourOfDay,
    DepartmentId,
    DepartmentName,
    COUNT(*) AS AppointmentCount,
    COUNT(CASE WHEN Status = 'Scheduled' THEN 1 END) AS ScheduledCount,
    COUNT(CASE WHEN Status = 'Completed' THEN 1 END) AS CompletedCount,
    COUNT(CASE WHEN Status = 'Cancelled' THEN 1 END) AS CancelledCount,
    COUNT(CASE WHEN Status = 'NoShow' THEN 1 END) AS NoShowCount,
    AVG(DurationMinutes) AS AvgDuration
FROM vw_AppointmentDetails
GROUP BY
    CAST(AppointmentDateTime AS DATE),
    DATEPART(HOUR, AppointmentDateTime),
    DepartmentId,
    DepartmentName;
GO

-- =============================================
-- 뷰: 의사별 예약 통계
-- =============================================
IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_DoctorAppointmentStats')
    DROP VIEW vw_DoctorAppointmentStats;
GO

CREATE VIEW vw_DoctorAppointmentStats
AS
SELECT
    DoctorId,
    DoctorCode,
    DoctorName,
    DepartmentId,
    DepartmentName,
    CAST(AppointmentDateTime AS DATE) AS AppointmentDate,
    COUNT(*) AS TotalAppointments,
    COUNT(CASE WHEN Status = 'Scheduled' THEN 1 END) AS ScheduledCount,
    COUNT(CASE WHEN Status = 'Completed' THEN 1 END) AS CompletedCount,
    COUNT(CASE WHEN Status = 'Cancelled' THEN 1 END) AS CancelledCount,
    COUNT(CASE WHEN Status = 'NoShow' THEN 1 END) AS NoShowCount,
    SUM(DurationMinutes) AS TotalMinutes,
    AVG(DurationMinutes) AS AvgMinutesPerAppointment
FROM vw_AppointmentDetails
GROUP BY
    DoctorId,
    DoctorCode,
    DoctorName,
    DepartmentId,
    DepartmentName,
    CAST(AppointmentDateTime AS DATE);
GO

-- =============================================
-- 뷰: 진료과별 예약 통계
-- =============================================
IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_DepartmentAppointmentStats')
    DROP VIEW vw_DepartmentAppointmentStats;
GO

CREATE VIEW vw_DepartmentAppointmentStats
AS
SELECT
    DepartmentId,
    DepartmentName,
    DepartmentCode,
    CAST(AppointmentDateTime AS DATE) AS AppointmentDate,
    DATEPART(WEEKDAY, AppointmentDateTime) AS DayOfWeek,
    COUNT(*) AS TotalAppointments,
    COUNT(CASE WHEN Status = 'Scheduled' THEN 1 END) AS ScheduledCount,
    COUNT(CASE WHEN Status = 'Completed' THEN 1 END) AS CompletedCount,
    COUNT(DISTINCT DoctorId) AS ActiveDoctorsCount,
    COUNT(DISTINCT PatientId) AS UniquePatients,
    AVG(DurationMinutes) AS AvgDuration
FROM vw_AppointmentDetails
GROUP BY
    DepartmentId,
    DepartmentName,
    DepartmentCode,
    CAST(AppointmentDateTime AS DATE),
    DATEPART(WEEKDAY, AppointmentDateTime);
GO

-- =============================================
-- 뷰: 의사 가용 시간 (근무 시간 - 예약 시간)
-- =============================================
IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_DoctorAvailability')
    DROP VIEW vw_DoctorAvailability;
GO

CREATE VIEW vw_DoctorAvailability
AS
SELECT
    d.DoctorId,
    d.DoctorCode,
    d.DoctorName,
    d.DepartmentId,
    dept.DepartmentName,
    ds.ScheduleId,
    ds.ScheduleType,
    ds.StartDateTime,
    ds.EndDateTime,
    DATEDIFF(MINUTE, ds.StartDateTime, ds.EndDateTime) AS TotalMinutes
FROM Doctors d
INNER JOIN Departments dept ON d.DepartmentId = dept.DepartmentId
INNER JOIN DoctorSchedules ds ON d.DoctorId = ds.DoctorId
WHERE d.IsActive = 1 AND ds.ScheduleType IN ('Available', 'Vacation', 'Meeting', 'ClosedDay');
GO

PRINT 'Views created successfully!';
GO
