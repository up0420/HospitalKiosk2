-- =============================================
-- Hospital Kiosk - 샘플 예약 및 수납 데이터 추가
-- =============================================
-- 테스트용 예약 및 수납 데이터를 추가합니다.
-- 이미 환자, 의사, 일정 데이터가 있다고 가정합니다.
-- =============================================

USE HospitalKiosk;
GO

PRINT '샘플 예약 및 수납 데이터 추가 중...';
GO

-- =============================================
-- 샘플 예약 데이터 추가
-- =============================================

DECLARE @Today DATE = CAST(GETDATE() AS DATE);
DECLARE @Yesterday DATE = DATEADD(DAY, -1, @Today);
DECLARE @TwoDaysAgo DATE = DATEADD(DAY, -2, @Today);

-- 환자 ID 조회
DECLARE @Patient1Id INT = (SELECT PatientId FROM Patients WHERE PatientNumber = 'P20250001'); -- 홍길동
DECLARE @Patient2Id INT = (SELECT PatientId FROM Patients WHERE PatientNumber = 'P20250002'); -- 김영희
DECLARE @Patient3Id INT = (SELECT PatientId FROM Patients WHERE PatientNumber = 'P20250003'); -- 이철수

-- 의사 ID 조회
DECLARE @Doctor1Id INT = (SELECT DoctorId FROM Doctors WHERE DoctorCode = 'DOC001'); -- 김철수 (내과)
DECLARE @Doctor2Id INT = (SELECT DoctorId FROM Doctors WHERE DoctorCode = 'DOC002'); -- 이영희 (소아과)
DECLARE @Doctor3Id INT = (SELECT DoctorId FROM Doctors WHERE DoctorCode = 'DOC003'); -- 박민수 (정형외과)

-- 예약 데이터 삽입 (진료 완료된 예약)
IF NOT EXISTS (SELECT * FROM Appointments WHERE AppointmentNumber LIKE 'APT%')
BEGIN
    -- 1. 홍길동 - 김철수 의사 (내과) - 이틀 전 진료 완료
    INSERT INTO Appointments (
        AppointmentNumber, PatientId, DoctorId, AppointmentDateTime,
        DurationMinutes, Status, Reason, CreatedBy, CreatedAt, UpdatedAt
    )
    VALUES (
        'APT' + CONVERT(NVARCHAR(8), @TwoDaysAgo, 112) + '0001',
        @Patient1Id, @Doctor1Id,
        DATEADD(HOUR, 10, @TwoDaysAgo), -- 이틀 전 10:00
        30, 'Completed', N'정기 검진', 'Patient',
        DATEADD(HOUR, -48, GETDATE()), GETDATE()
    );

    -- 2. 홍길동 - 박민수 의사 (정형외과) - 어제 진료 완료
    INSERT INTO Appointments (
        AppointmentNumber, PatientId, DoctorId, AppointmentDateTime,
        DurationMinutes, Status, Reason, CreatedBy, CreatedAt, UpdatedAt
    )
    VALUES (
        'APT' + CONVERT(NVARCHAR(8), @Yesterday, 112) + '0001',
        @Patient1Id, @Doctor3Id,
        DATEADD(HOUR, 14, @Yesterday), -- 어제 14:00
        30, 'Completed', N'무릎 통증', 'Patient',
        DATEADD(HOUR, -24, GETDATE()), GETDATE()
    );

    -- 3. 김영희 - 이영희 의사 (소아과) - 이틀 전 진료 완료
    INSERT INTO Appointments (
        AppointmentNumber, PatientId, DoctorId, AppointmentDateTime,
        DurationMinutes, Status, Reason, CreatedBy, CreatedAt, UpdatedAt
    )
    VALUES (
        'APT' + CONVERT(NVARCHAR(8), @TwoDaysAgo, 112) + '0002',
        @Patient2Id, @Doctor2Id,
        DATEADD(HOUR, 11, @TwoDaysAgo), -- 이틀 전 11:00
        30, 'Completed', N'독감 예방접종', 'Patient',
        DATEADD(HOUR, -48, GETDATE()), GETDATE()
    );

    -- 4. 이철수 - 김철수 의사 (내과) - 어제 진료 완료
    INSERT INTO Appointments (
        AppointmentNumber, PatientId, DoctorId, AppointmentDateTime,
        DurationMinutes, Status, Reason, CreatedBy, CreatedAt, UpdatedAt
    )
    VALUES (
        'APT' + CONVERT(NVARCHAR(8), @Yesterday, 112) + '0002',
        @Patient3Id, @Doctor1Id,
        DATEADD(HOUR, 15, @Yesterday), -- 어제 15:00
        30, 'Completed', N'고혈압 검진', 'Patient',
        DATEADD(HOUR, -24, GETDATE()), GETDATE()
    );

    -- 5. 김영희 - 김철수 의사 (내과) - 오늘 예약 (아직 진료 전)
    INSERT INTO Appointments (
        AppointmentNumber, PatientId, DoctorId, AppointmentDateTime,
        DurationMinutes, Status, Reason, CreatedBy, CreatedAt, UpdatedAt
    )
    VALUES (
        'APT' + CONVERT(NVARCHAR(8), @Today, 112) + '0001',
        @Patient2Id, @Doctor1Id,
        DATEADD(HOUR, 14, @Today), -- 오늘 14:00
        30, 'Scheduled', N'감기 증상', 'Patient',
        GETDATE(), GETDATE()
    );

    -- 6. 이철수 - 이영희 의사 (소아과) - 내일 예약
    INSERT INTO Appointments (
        AppointmentNumber, PatientId, DoctorId, AppointmentDateTime,
        DurationMinutes, Status, Reason, CreatedBy, CreatedAt, UpdatedAt
    )
    VALUES (
        'APT' + CONVERT(NVARCHAR(8), DATEADD(DAY, 1, @Today), 112) + '0001',
        @Patient3Id, @Doctor2Id,
        DATEADD(HOUR, 10, DATEADD(DAY, 1, @Today)), -- 내일 10:00
        30, 'Scheduled', N'정기 검진', 'Patient',
        GETDATE(), GETDATE()
    );

    PRINT '  - 샘플 예약 데이터 6건 추가 완료';
    PRINT '    * 진료 완료: 4건 (수납 대상)';
    PRINT '    * 예약 중: 2건';
END
ELSE
BEGIN
    PRINT '  - 예약 데이터가 이미 존재합니다.';
END
GO

-- =============================================
-- 샘플 수납 데이터 추가
-- =============================================

-- 진료 완료된 예약 중 일부만 수납 처리
IF NOT EXISTS (SELECT * FROM Payments WHERE PaymentNumber LIKE 'PAY%')
BEGIN
    DECLARE @Appointment1Id INT, @Appointment2Id INT, @Appointment3Id INT;
    DECLARE @Patient1Id2 INT, @Patient2Id2 INT, @Patient3Id2 INT;
    DECLARE @Today2 DATE = CAST(GETDATE() AS DATE);
    DECLARE @Yesterday2 DATE = DATEADD(DAY, -1, @Today2);
    DECLARE @TwoDaysAgo2 DATE = DATEADD(DAY, -2, @Today2);

    -- 첫 번째 예약 (홍길동 - 내과 - 이틀 전) - 수납 완료
    SELECT @Appointment1Id = AppointmentId, @Patient1Id2 = PatientId
    FROM Appointments
    WHERE AppointmentNumber = 'APT' + CONVERT(NVARCHAR(8), @TwoDaysAgo2, 112) + '0001';

    IF @Appointment1Id IS NOT NULL
    BEGIN
        INSERT INTO Payments (
            PaymentNumber, AppointmentId, PatientId, TotalAmount,
            PaidAmount, PaymentStatus, PaymentMethod, PaymentDateTime,
            ReceiptNumber, Notes, CreatedAt, UpdatedAt
        )
        VALUES (
            'PAY' + CONVERT(NVARCHAR(8), @TwoDaysAgo2, 112) + '0001',
            @Appointment1Id, @Patient1Id2, 50000.00, 50000.00,
            'Paid', 'Card',
            DATEADD(HOUR, -47, GETDATE()), -- 이틀 전 수납
            'RCP' + CONVERT(NVARCHAR(8), @TwoDaysAgo2, 112) + '0001',
            N'정기 검진 - 혈액검사 포함', DATEADD(HOUR, -47, GETDATE()), GETDATE()
        );
        PRINT '  - 홍길동 (내과 검진) 수납 완료: 50,000원';
    END

    -- 두 번째 예약 (김영희 - 소아과 - 이틀 전) - 부분 납부
    SELECT @Appointment2Id = AppointmentId, @Patient2Id2 = PatientId
    FROM Appointments
    WHERE AppointmentNumber = 'APT' + CONVERT(NVARCHAR(8), @TwoDaysAgo2, 112) + '0002';

    IF @Appointment2Id IS NOT NULL
    BEGIN
        INSERT INTO Payments (
            PaymentNumber, AppointmentId, PatientId, TotalAmount,
            PaidAmount, PaymentStatus, PaymentMethod, PaymentDateTime,
            ReceiptNumber, Notes, CreatedAt, UpdatedAt
        )
        VALUES (
            'PAY' + CONVERT(NVARCHAR(8), @TwoDaysAgo2, 112) + '0002',
            @Appointment2Id, @Patient2Id2, 30000.00, 20000.00,
            'PartiallyPaid', 'Cash',
            DATEADD(HOUR, -47, GETDATE()), -- 이틀 전 부분 납부
            'RCP' + CONVERT(NVARCHAR(8), @TwoDaysAgo2, 112) + '0002',
            N'독감 예방접종 - 잔액: 10,000원', DATEADD(HOUR, -47, GETDATE()), GETDATE()
        );
        PRINT '  - 김영희 (소아과 예방접종) 부분 납부: 20,000원 / 30,000원';
    END

    -- 세 번째 예약 (이철수 - 내과 - 어제) - 미수납 (수납 대기 중)
    -- Payment 레코드를 생성하지 않음 (미수납 상태)

    -- 네 번째 예약 (홍길동 - 정형외과 - 어제) - 미수납 (수납 대기 중)
    -- Payment 레코드를 생성하지 않음 (미수납 상태)

    PRINT '  - 샘플 수납 데이터 2건 추가 완료';
    PRINT '    * 완납: 1건';
    PRINT '    * 부분납부: 1건';
    PRINT '    * 미수납: 2건 (수납 대기 중)';
END
ELSE
BEGIN
    PRINT '  - 수납 데이터가 이미 존재합니다.';
END
GO

-- =============================================
-- 추가된 데이터 요약
-- =============================================
PRINT '';
PRINT '========================================';
PRINT '샘플 데이터 추가 완료!';
PRINT '========================================';
PRINT '';
PRINT '추가된 데이터:';
PRINT '  [예약 데이터]';
PRINT '    - 총 6건';
PRINT '    - 진료 완료 (Completed): 4건';
PRINT '    - 예약 중 (Scheduled): 2건';
PRINT '';
PRINT '  [수납 데이터]';
PRINT '    - 총 2건 처리';
PRINT '    - 완납 (Paid): 1건';
PRINT '    - 부분납부 (PartiallyPaid): 1건';
PRINT '    - 미수납: 2건 (수납 화면에 표시됨)';
PRINT '';
PRINT '수납 테스트 방법:';
PRINT '  1. 키오스크에서 "진료비 수납" 선택';
PRINT '  2. 환자 검색:';
PRINT '     - 홍길동 (P20250001) - 정형외과 미수납 1건';
PRINT '     - 김영희 (P20250002) - 잔액 10,000원';
PRINT '     - 이철수 (P20250003) - 내과 미수납 1건';
PRINT '';
PRINT '예약 진료비:';
PRINT '  - 내과 정기검진: 50,000원';
PRINT '  - 소아과 예방접종: 30,000원';
PRINT '  - 정형외과 진료: 80,000원';
PRINT '  - 내과 고혈압검진: 70,000원';
PRINT '========================================';
GO
