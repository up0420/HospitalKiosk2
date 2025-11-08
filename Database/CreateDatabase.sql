-- =============================================
-- Hospital Kiosk Database Schema
-- =============================================

USE master;
GO

-- 데이터베이스 생성
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'HospitalKiosk')
BEGIN
    CREATE DATABASE HospitalKiosk;
END
GO

USE HospitalKiosk;
GO

-- =============================================
-- 진료과 테이블
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Departments')
BEGIN
    CREATE TABLE Departments
    (
        DepartmentId INT PRIMARY KEY IDENTITY(1,1),
        DepartmentName NVARCHAR(100) NOT NULL,
        DepartmentCode NVARCHAR(20) NOT NULL UNIQUE,
        Description NVARCHAR(500),
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

-- =============================================
-- 의사 테이블
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Doctors')
BEGIN
    CREATE TABLE Doctors
    (
        DoctorId INT PRIMARY KEY IDENTITY(1,1),
        DoctorCode NVARCHAR(20) NOT NULL UNIQUE,
        DoctorName NVARCHAR(100) NOT NULL,
        DepartmentId INT NOT NULL,
        LicenseNumber NVARCHAR(50) NOT NULL UNIQUE,
        PhoneNumber NVARCHAR(20),
        Email NVARCHAR(100),
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Doctors_Departments FOREIGN KEY (DepartmentId)
            REFERENCES Departments(DepartmentId)
    );
END
GO

-- =============================================
-- 환자 테이블
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Patients')
BEGIN
    CREATE TABLE Patients
    (
        PatientId INT PRIMARY KEY IDENTITY(1,1),
        PatientNumber NVARCHAR(20) NOT NULL UNIQUE,
        PatientName NVARCHAR(100) NOT NULL,
        BirthDate DATE NOT NULL,
        Gender NVARCHAR(10) NOT NULL CHECK (Gender IN ('M', 'F')),
        PhoneNumber NVARCHAR(20) NOT NULL,
        Address NVARCHAR(300),
        EmergencyContact NVARCHAR(20),
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

-- =============================================
-- 관리자 테이블
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Admins')
BEGIN
    CREATE TABLE Admins
    (
        AdminId INT PRIMARY KEY IDENTITY(1,1),
        AdminCode NVARCHAR(20) NOT NULL UNIQUE,
        AdminName NVARCHAR(100) NOT NULL,
        Username NVARCHAR(50) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(256) NOT NULL,
        Email NVARCHAR(100),
        PhoneNumber NVARCHAR(20),
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

-- =============================================
-- 의사 일정 유형
-- ScheduleType: 'Available'(근무), 'Vacation'(휴가), 'Meeting'(회의), 'ClosedDay'(휴진)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DoctorSchedules')
BEGIN
    CREATE TABLE DoctorSchedules
    (
        ScheduleId INT PRIMARY KEY IDENTITY(1,1),
        DoctorId INT NOT NULL,
        ScheduleType NVARCHAR(20) NOT NULL CHECK (ScheduleType IN ('Available', 'Vacation', 'Meeting', 'ClosedDay')),
        StartDateTime DATETIME NOT NULL,
        EndDateTime DATETIME NOT NULL,
        Title NVARCHAR(100),
        Description NVARCHAR(500),
        IsRecurring BIT NOT NULL DEFAULT 0,
        RecurrencePattern NVARCHAR(50), -- 예: 'Weekly', 'Daily', 'Monthly'
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_DoctorSchedules_Doctors FOREIGN KEY (DoctorId)
            REFERENCES Doctors(DoctorId),
        CONSTRAINT CK_DoctorSchedules_DateTime CHECK (EndDateTime > StartDateTime)
    );
END
GO

-- =============================================
-- 예약 상태
-- Status: 'Scheduled'(예약됨), 'Completed'(완료), 'Cancelled'(취소), 'NoShow'(부재)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Appointments')
BEGIN
    CREATE TABLE Appointments
    (
        AppointmentId INT PRIMARY KEY IDENTITY(1,1),
        AppointmentNumber NVARCHAR(20) NOT NULL UNIQUE,
        PatientId INT NOT NULL,
        DoctorId INT NOT NULL,
        AppointmentDateTime DATETIME NOT NULL,
        DurationMinutes INT NOT NULL DEFAULT 30,
        Status NVARCHAR(20) NOT NULL DEFAULT 'Scheduled'
            CHECK (Status IN ('Scheduled', 'Completed', 'Cancelled', 'NoShow')),
        Reason NVARCHAR(300),
        Notes NVARCHAR(500),
        CreatedBy NVARCHAR(50) NOT NULL, -- 'Patient' or 'Admin'
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        CancelledAt DATETIME,
        CancelledBy NVARCHAR(50),
        CancelReason NVARCHAR(300),
        CONSTRAINT FK_Appointments_Patients FOREIGN KEY (PatientId)
            REFERENCES Patients(PatientId),
        CONSTRAINT FK_Appointments_Doctors FOREIGN KEY (DoctorId)
            REFERENCES Doctors(DoctorId)
    );
END
GO

-- =============================================
-- 수납 테이블
-- PaymentStatus: 'Pending'(미납), 'Paid'(납부완료), 'PartiallyPaid'(부분납부), 'Refunded'(환불)
-- PaymentMethod: 'Cash'(현금), 'Card'(카드), 'Transfer'(계좌이체)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Payments')
BEGIN
    CREATE TABLE Payments
    (
        PaymentId INT PRIMARY KEY IDENTITY(1,1),
        PaymentNumber NVARCHAR(20) NOT NULL UNIQUE,
        AppointmentId INT NOT NULL,
        PatientId INT NOT NULL,
        TotalAmount DECIMAL(18,2) NOT NULL,
        PaidAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
        PaymentStatus NVARCHAR(20) NOT NULL DEFAULT 'Pending'
            CHECK (PaymentStatus IN ('Pending', 'Paid', 'PartiallyPaid', 'Refunded')),
        PaymentMethod NVARCHAR(20)
            CHECK (PaymentMethod IN ('Cash', 'Card', 'Transfer')),
        PaymentDateTime DATETIME,
        ReceiptNumber NVARCHAR(50),
        Notes NVARCHAR(500),
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Payments_Appointments FOREIGN KEY (AppointmentId)
            REFERENCES Appointments(AppointmentId),
        CONSTRAINT FK_Payments_Patients FOREIGN KEY (PatientId)
            REFERENCES Patients(PatientId)
    );
END
GO

-- =============================================
-- 인덱스 생성
-- =============================================

-- 예약 조회 최적화
CREATE NONCLUSTERED INDEX IX_Appointments_DateTime
    ON Appointments(AppointmentDateTime) INCLUDE (PatientId, DoctorId, Status);

CREATE NONCLUSTERED INDEX IX_Appointments_Patient
    ON Appointments(PatientId, AppointmentDateTime);

CREATE NONCLUSTERED INDEX IX_Appointments_Doctor
    ON Appointments(DoctorId, AppointmentDateTime);

CREATE NONCLUSTERED INDEX IX_Appointments_Status
    ON Appointments(Status, AppointmentDateTime);

-- 의사 일정 조회 최적화
CREATE NONCLUSTERED INDEX IX_DoctorSchedules_Doctor_DateTime
    ON DoctorSchedules(DoctorId, StartDateTime, EndDateTime);

CREATE NONCLUSTERED INDEX IX_DoctorSchedules_Type
    ON DoctorSchedules(ScheduleType, StartDateTime);

-- 수납 조회 최적화
CREATE NONCLUSTERED INDEX IX_Payments_Patient
    ON Payments(PatientId, PaymentStatus);

CREATE NONCLUSTERED INDEX IX_Payments_Status
    ON Payments(PaymentStatus, PaymentDateTime);

GO

-- =============================================
-- 기본 데이터 삽입
-- =============================================

-- 진료과 초기 데이터
IF NOT EXISTS (SELECT * FROM Departments)
BEGIN
    INSERT INTO Departments (DepartmentName, DepartmentCode, Description)
    VALUES
        (N'내과', 'IM', N'내과 진료'),
        (N'외과', 'GS', N'외과 진료'),
        (N'정형외과', 'OS', N'정형외과 진료'),
        (N'소아과', 'PED', N'소아과 진료'),
        (N'산부인과', 'OBGY', N'산부인과 진료'),
        (N'안과', 'OPH', N'안과 진료'),
        (N'이비인후과', 'ENT', N'이비인후과 진료'),
        (N'피부과', 'DERM', N'피부과 진료'),
        (N'정신건강의학과', 'PSY', N'정신건강의학과 진료'),
        (N'가정의학과', 'FM', N'가정의학과 진료');
END
GO

-- 관리자 초기 계정 (비밀번호: admin123)
-- 실제 운영시에는 반드시 변경 필요
IF NOT EXISTS (SELECT * FROM Admins WHERE Username = 'admin')
BEGIN
    INSERT INTO Admins (AdminCode, AdminName, Username, PasswordHash, Email)
    VALUES
        ('ADM001', N'시스템관리자', 'admin',
         '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9', -- SHA256('admin123')
         'admin@hospital.com');
END
GO

PRINT 'Database schema created successfully!';
GO
