-- =============================================
-- Hospital Kiosk Database - Complete Setup Script
-- 다른 로컬 환경에서 실행할 수 있는 통합 설정 스크립트
-- =============================================
-- 실행 방법:
-- 1. SQL Server Management Studio 또는 sqlcmd에서 실행
-- 2. SQL Server Express가 설치되어 있어야 함
-- 3. 관리자 권한으로 실행 권장
-- =============================================

USE master;
GO

-- =============================================
-- 데이터베이스 생성
-- =============================================
PRINT '데이터베이스 생성 중...';
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'HospitalKiosk')
BEGIN
    CREATE DATABASE HospitalKiosk;
    PRINT 'HospitalKiosk 데이터베이스가 생성되었습니다.';
END
ELSE
BEGIN
    PRINT 'HospitalKiosk 데이터베이스가 이미 존재합니다.';
END
GO

USE HospitalKiosk;
GO

-- =============================================
-- 테이블 생성
-- =============================================
PRINT '테이블 생성 중...';
GO

-- 진료과 테이블
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
    PRINT '  - Departments 테이블 생성 완료';
END
GO

-- 의사 테이블
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
        Username NVARCHAR(50),
        PasswordHash NVARCHAR(256),
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Doctors_Departments FOREIGN KEY (DepartmentId)
            REFERENCES Departments(DepartmentId)
    );
    PRINT '  - Doctors 테이블 생성 완료';
END
ELSE
BEGIN
    -- 기존 테이블이 있는 경우 Username, PasswordHash 컬럼 추가
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'Username')
    BEGIN
        ALTER TABLE Doctors ADD Username NVARCHAR(50);
        PRINT '  - Doctors 테이블에 Username 컬럼 추가';
    END

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Doctors') AND name = 'PasswordHash')
    BEGIN
        ALTER TABLE Doctors ADD PasswordHash NVARCHAR(256);
        PRINT '  - Doctors 테이블에 PasswordHash 컬럼 추가';
    END
END
GO

-- 환자 테이블
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
    PRINT '  - Patients 테이블 생성 완료';
END
GO

-- 관리자 테이블
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
    PRINT '  - Admins 테이블 생성 완료';
END
GO

-- 의사 일정 테이블
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
        RecurrencePattern NVARCHAR(50),
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_DoctorSchedules_Doctors FOREIGN KEY (DoctorId)
            REFERENCES Doctors(DoctorId),
        CONSTRAINT CK_DoctorSchedules_DateTime CHECK (EndDateTime > StartDateTime)
    );
    PRINT '  - DoctorSchedules 테이블 생성 완료';
END
GO

-- 예약 테이블
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
        CreatedBy NVARCHAR(50) NOT NULL,
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
    PRINT '  - Appointments 테이블 생성 완료';
END
GO

-- 수납 테이블
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
    PRINT '  - Payments 테이블 생성 완료';
END
GO

-- =============================================
-- 인덱스 생성
-- =============================================
PRINT '인덱스 생성 중...';
GO

-- 예약 조회 최적화
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Appointments_DateTime')
    CREATE NONCLUSTERED INDEX IX_Appointments_DateTime
        ON Appointments(AppointmentDateTime) INCLUDE (PatientId, DoctorId, Status);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Appointments_Patient')
    CREATE NONCLUSTERED INDEX IX_Appointments_Patient
        ON Appointments(PatientId, AppointmentDateTime);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Appointments_Doctor')
    CREATE NONCLUSTERED INDEX IX_Appointments_Doctor
        ON Appointments(DoctorId, AppointmentDateTime);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Appointments_Status')
    CREATE NONCLUSTERED INDEX IX_Appointments_Status
        ON Appointments(Status, AppointmentDateTime);

-- 의사 일정 조회 최적화
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_DoctorSchedules_Doctor_DateTime')
    CREATE NONCLUSTERED INDEX IX_DoctorSchedules_Doctor_DateTime
        ON DoctorSchedules(DoctorId, StartDateTime, EndDateTime);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_DoctorSchedules_Type')
    CREATE NONCLUSTERED INDEX IX_DoctorSchedules_Type
        ON DoctorSchedules(ScheduleType, StartDateTime);

-- 수납 조회 최적화
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Payments_Patient')
    CREATE NONCLUSTERED INDEX IX_Payments_Patient
        ON Payments(PatientId, PaymentStatus);

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Payments_Status')
    CREATE NONCLUSTERED INDEX IX_Payments_Status
        ON Payments(PaymentStatus, PaymentDateTime);

PRINT '인덱스 생성 완료';
GO

-- =============================================
-- 기본 데이터 삽입
-- =============================================
PRINT '기본 데이터 삽입 중...';
GO

-- 진료과 초기 데이터
IF NOT EXISTS (SELECT * FROM Departments)
BEGIN
    SET IDENTITY_INSERT Departments ON;

    INSERT INTO Departments (DepartmentId, DepartmentName, DepartmentCode, Description)
    VALUES
        (1, N'내과', 'IM', N'내과 진료'),
        (2, N'외과', 'GS', N'외과 진료'),
        (3, N'정형외과', 'OS', N'정형외과 진료'),
        (4, N'소아과', 'PED', N'소아과 진료'),
        (5, N'산부인과', 'OBGY', N'산부인과 진료'),
        (6, N'안과', 'OPH', N'안과 진료'),
        (7, N'이비인후과', 'ENT', N'이비인후과 진료'),
        (8, N'피부과', 'DERM', N'피부과 진료'),
        (9, N'정신건강의학과', 'PSY', N'정신건강의학과 진료'),
        (10, N'가정의학과', 'FM', N'가정의학과 진료');

    SET IDENTITY_INSERT Departments OFF;
    PRINT '  - 진료과 데이터 삽입 완료 (10개)';
END
GO

-- 관리자 초기 계정
-- Username: admin
-- Password: admin123
-- SHA256 해시값: 240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9
IF NOT EXISTS (SELECT * FROM Admins WHERE Username = 'admin')
BEGIN
    INSERT INTO Admins (AdminCode, AdminName, Username, PasswordHash, Email)
    VALUES
        ('ADM001', N'시스템관리자', 'admin',
         '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9',
         'admin@hospital.com');
    PRINT '  - 관리자 계정 생성 완료 (Username: admin, Password: admin123)';
END
GO

-- 샘플 의사 데이터 (로그인 계정 포함)
-- 모든 의사 비밀번호: admin123
IF NOT EXISTS (SELECT * FROM Doctors)
BEGIN
    INSERT INTO Doctors (DoctorCode, DoctorName, DepartmentId, LicenseNumber, PhoneNumber, Email, Username, PasswordHash)
    VALUES
        ('DOC001', N'김철수', 1, 'LIC001', '010-1111-1111', 'kim.cs@hospital.com', 'kim.cs', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9'),
        ('DOC002', N'이영희', 4, 'LIC002', '010-2222-2222', 'lee.yh@hospital.com', 'lee.yh', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9'),
        ('DOC003', N'박민수', 3, 'LIC003', '010-3333-3333', 'park.ms@hospital.com', 'park.ms', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9');

    PRINT '  - 샘플 의사 데이터 삽입 완료 (3명)';
    PRINT '    * Username: kim.cs (김철수 - 내과)';
    PRINT '    * Username: lee.yh (이영희 - 소아과)';
    PRINT '    * Username: park.ms (박민수 - 정형외과)';
    PRINT '    * 모든 비밀번호: admin123';
END
GO

-- 샘플 의사 일정 데이터
IF NOT EXISTS (SELECT * FROM DoctorSchedules)
BEGIN
    DECLARE @Today DATE = CAST(GETDATE() AS DATE);
    DECLARE @NextWeek DATE = DATEADD(DAY, 7, @Today);

    INSERT INTO DoctorSchedules (DoctorId, ScheduleType, StartDateTime, EndDateTime, Title, Description)
    VALUES
        -- 김철수 의사 (내과) - 평일 근무
        (1, 'Available', DATEADD(HOUR, 9, @Today), DATEADD(HOUR, 18, @Today), N'근무', N'평일 정규 근무'),
        (1, 'Available', DATEADD(HOUR, 9, DATEADD(DAY, 1, @Today)), DATEADD(HOUR, 18, DATEADD(DAY, 1, @Today)), N'근무', N'평일 정규 근무'),

        -- 이영희 의사 (소아과) - 평일 근무
        (2, 'Available', DATEADD(HOUR, 9, @Today), DATEADD(HOUR, 17, @Today), N'근무', N'평일 정규 근무'),
        (2, 'Available', DATEADD(HOUR, 9, DATEADD(DAY, 1, @Today)), DATEADD(HOUR, 17, DATEADD(DAY, 1, @Today)), N'근무', N'평일 정규 근무'),

        -- 박민수 의사 (정형외과) - 평일 근무 및 휴가
        (3, 'Available', DATEADD(HOUR, 9, @Today), DATEADD(HOUR, 18, @Today), N'근무', N'평일 정규 근무'),
        (3, 'Vacation', DATEADD(HOUR, 9, DATEADD(DAY, 2, @Today)), DATEADD(HOUR, 18, DATEADD(DAY, 2, @Today)), N'휴가', N'개인 휴가');

    PRINT '  - 샘플 의사 일정 데이터 삽입 완료 (6건)';
END
GO

-- 샘플 환자 데이터
IF NOT EXISTS (SELECT * FROM Patients)
BEGIN
    INSERT INTO Patients (PatientNumber, PatientName, BirthDate, Gender, PhoneNumber, Address, EmergencyContact)
    VALUES
        ('P20250001', N'홍길동', '1980-01-15', 'M', '010-1234-5678', N'서울시 강남구', '010-8765-4321'),
        ('P20250002', N'김영희', '1990-05-20', 'F', '010-2345-6789', N'서울시 서초구', '010-9876-5432'),
        ('P20250003', N'이철수', '1975-12-10', 'M', '010-3456-7890', N'서울시 송파구', '010-6543-2109');

    PRINT '  - 샘플 환자 데이터 삽입 완료 (3명)';
END
GO

-- =============================================
-- 데이터베이스 설정 완료
-- =============================================
PRINT '';
PRINT '========================================';
PRINT 'Hospital Kiosk 데이터베이스 설정 완료!';
PRINT '========================================';
PRINT '';
PRINT '로그인 계정 정보:';
PRINT '  [관리자]';
PRINT '    - Username: admin';
PRINT '    - Password: admin123';
PRINT '';
PRINT '  [의사]';
PRINT '    - Username: kim.cs (김철수 - 내과)';
PRINT '    - Username: lee.yh (이영희 - 소아과)';
PRINT '    - Username: park.ms (박민수 - 정형외과)';
PRINT '    - Password: admin123 (공통)';
PRINT '';
PRINT '경고: 운영 환경에서는 반드시 비밀번호를 변경하세요!';
PRINT '';
PRINT '연결 문자열:';
PRINT '  Data Source=(local)\SQLEXPRESS;';
PRINT '  Initial Catalog=HospitalKiosk;';
PRINT '  Integrated Security=True;';
PRINT '========================================';
GO
