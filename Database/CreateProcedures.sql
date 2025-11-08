-- =============================================
-- Hospital Kiosk - Stored Procedures
-- =============================================

USE HospitalKiosk;
GO

-- =============================================
-- 프로시저: 병목 현상 예측 분석
-- =============================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_PredictBottlenecks')
    DROP PROCEDURE sp_PredictBottlenecks;
GO

CREATE PROCEDURE sp_PredictBottlenecks
    @StartDate DATE,
    @EndDate DATE,
    @DepartmentId INT = NULL,
    @ThresholdCount INT = 5  -- 한 시간당 임계값
AS
BEGIN
    SET NOCOUNT ON;

    -- 시간대별 예약 밀집도 분석
    SELECT
        AppointmentDate,
        HourOfDay,
        DepartmentName,
        AppointmentCount,
        ScheduledCount,
        CASE
            WHEN AppointmentCount >= @ThresholdCount * 1.5 THEN N'심각'
            WHEN AppointmentCount >= @ThresholdCount THEN N'주의'
            ELSE N'정상'
        END AS BottleneckLevel,
        CAST((AppointmentCount * 100.0 / @ThresholdCount) AS DECIMAL(5,2)) AS UtilizationRate
    FROM vw_HourlyAppointmentStats
    WHERE AppointmentDate BETWEEN @StartDate AND @EndDate
        AND (@DepartmentId IS NULL OR DepartmentId = @DepartmentId)
        AND AppointmentCount >= @ThresholdCount * 0.8  -- 임계값의 80% 이상만 표시
    ORDER BY AppointmentDate, HourOfDay, AppointmentCount DESC;
END
GO

-- =============================================
-- 프로시저: 의사 가용 시간 조회
-- =============================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_GetDoctorAvailableSlots')
    DROP PROCEDURE sp_GetDoctorAvailableSlots;
GO

CREATE PROCEDURE sp_GetDoctorAvailableSlots
    @DoctorId INT,
    @StartDate DATE,
    @EndDate DATE,
    @SlotDurationMinutes INT = 30
AS
BEGIN
    SET NOCOUNT ON;

    -- 의사의 근무 시간 조회 (Vacation, Meeting, ClosedDay 제외)
    -- 예약된 시간 제외하여 실제 가용 시간 반환
    WITH DoctorWorkHours AS (
        SELECT
            StartDateTime,
            EndDateTime
        FROM DoctorSchedules
        WHERE DoctorId = @DoctorId
            AND ScheduleType = 'Available'
            AND StartDateTime >= @StartDate
            AND EndDateTime <= DATEADD(DAY, 1, @EndDate)
    ),
    BlockedTimes AS (
        -- 의사의 휴가, 회의, 휴진 시간
        SELECT StartDateTime, EndDateTime
        FROM DoctorSchedules
        WHERE DoctorId = @DoctorId
            AND ScheduleType IN ('Vacation', 'Meeting', 'ClosedDay')
            AND StartDateTime >= @StartDate
            AND EndDateTime <= DATEADD(DAY, 1, @EndDate)

        UNION ALL

        -- 이미 예약된 시간
        SELECT AppointmentDateTime,
               DATEADD(MINUTE, DurationMinutes, AppointmentDateTime)
        FROM Appointments
        WHERE DoctorId = @DoctorId
            AND Status IN ('Scheduled', 'Completed')
            AND AppointmentDateTime >= @StartDate
            AND AppointmentDateTime <= DATEADD(DAY, 1, @EndDate)
    )
    SELECT
        wh.StartDateTime AS WorkStartTime,
        wh.EndDateTime AS WorkEndTime,
        bt.StartDateTime AS BlockedStartTime,
        bt.EndDateTime AS BlockedEndTime,
        CASE
            WHEN bt.StartDateTime IS NULL THEN 1
            ELSE 0
        END AS IsAvailable
    FROM DoctorWorkHours wh
    LEFT JOIN BlockedTimes bt
        ON bt.StartDateTime < wh.EndDateTime
        AND bt.EndDateTime > wh.StartDateTime
    ORDER BY wh.StartDateTime, bt.StartDateTime;
END
GO

-- =============================================
-- 프로시저: 예약 가능한 시간 슬롯 조회
-- =============================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_GetAvailableTimeSlots')
    DROP PROCEDURE sp_GetAvailableTimeSlots;
GO

CREATE PROCEDURE sp_GetAvailableTimeSlots
    @DoctorId INT,
    @Date DATE,
    @SlotDurationMinutes INT = 30
AS
BEGIN
    SET NOCOUNT ON;

    -- 임시 테이블: 해당 날짜의 모든 30분 슬롯 생성
    DECLARE @TimeSlots TABLE (
        SlotTime DATETIME,
        SlotEndTime DATETIME
    );

    DECLARE @CurrentTime DATETIME = @Date;
    DECLARE @EndTime DATETIME = DATEADD(DAY, 1, @Date);

    -- 09:00 ~ 18:00 시간대 생성
    SET @CurrentTime = DATEADD(HOUR, 9, @Date);
    SET @EndTime = DATEADD(HOUR, 18, @Date);

    WHILE @CurrentTime < @EndTime
    BEGIN
        INSERT INTO @TimeSlots (SlotTime, SlotEndTime)
        VALUES (@CurrentTime, DATEADD(MINUTE, @SlotDurationMinutes, @CurrentTime));

        SET @CurrentTime = DATEADD(MINUTE, @SlotDurationMinutes, @CurrentTime);
    END

    -- 가용 시간 슬롯 반환
    SELECT
        ts.SlotTime,
        ts.SlotEndTime,
        CASE
            WHEN ds.ScheduleId IS NOT NULL AND ds.ScheduleType <> 'Available' THEN 0
            WHEN a.AppointmentId IS NOT NULL THEN 0
            WHEN wh.ScheduleId IS NULL THEN 0
            ELSE 1
        END AS IsAvailable,
        CASE
            WHEN ds.ScheduleId IS NOT NULL AND ds.ScheduleType <> 'Available' THEN ds.ScheduleType
            WHEN a.AppointmentId IS NOT NULL THEN 'Booked'
            WHEN wh.ScheduleId IS NULL THEN 'NotWorking'
            ELSE 'Available'
        END AS SlotStatus
    FROM @TimeSlots ts
    -- 근무 시간 확인
    LEFT JOIN DoctorSchedules wh
        ON wh.DoctorId = @DoctorId
        AND wh.ScheduleType = 'Available'
        AND ts.SlotTime >= wh.StartDateTime
        AND ts.SlotEndTime <= wh.EndDateTime
    -- 휴가/회의/휴진 확인
    LEFT JOIN DoctorSchedules ds
        ON ds.DoctorId = @DoctorId
        AND ds.ScheduleType IN ('Vacation', 'Meeting', 'ClosedDay')
        AND ts.SlotTime >= ds.StartDateTime
        AND ts.SlotTime < ds.EndDateTime
    -- 예약 확인
    LEFT JOIN Appointments a
        ON a.DoctorId = @DoctorId
        AND a.Status IN ('Scheduled')
        AND ts.SlotTime >= a.AppointmentDateTime
        AND ts.SlotTime < DATEADD(MINUTE, a.DurationMinutes, a.AppointmentDateTime)
    ORDER BY ts.SlotTime;
END
GO

-- =============================================
-- 프로시저: 예약 생성
-- =============================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_CreateAppointment')
    DROP PROCEDURE sp_CreateAppointment;
GO

CREATE PROCEDURE sp_CreateAppointment
    @PatientId INT,
    @DoctorId INT,
    @AppointmentDateTime DATETIME,
    @DurationMinutes INT = 30,
    @Reason NVARCHAR(300) = NULL,
    @CreatedBy NVARCHAR(50) = 'Patient',
    @AppointmentId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        -- 예약 가능 여부 확인
        DECLARE @IsAvailable BIT = 0;
        DECLARE @SlotStatus NVARCHAR(20);

        -- 의사 근무 시간 확인
        IF EXISTS (
            SELECT 1 FROM DoctorSchedules
            WHERE DoctorId = @DoctorId
                AND ScheduleType = 'Available'
                AND @AppointmentDateTime >= StartDateTime
                AND DATEADD(MINUTE, @DurationMinutes, @AppointmentDateTime) <= EndDateTime
        )
        BEGIN
            -- 중복 예약 확인
            IF NOT EXISTS (
                SELECT 1 FROM Appointments
                WHERE DoctorId = @DoctorId
                    AND Status = 'Scheduled'
                    AND @AppointmentDateTime < DATEADD(MINUTE, DurationMinutes, AppointmentDateTime)
                    AND DATEADD(MINUTE, @DurationMinutes, @AppointmentDateTime) > AppointmentDateTime
            )
            BEGIN
                -- 휴가/회의/휴진 확인
                IF NOT EXISTS (
                    SELECT 1 FROM DoctorSchedules
                    WHERE DoctorId = @DoctorId
                        AND ScheduleType IN ('Vacation', 'Meeting', 'ClosedDay')
                        AND @AppointmentDateTime >= StartDateTime
                        AND @AppointmentDateTime < EndDateTime
                )
                BEGIN
                    SET @IsAvailable = 1;
                END
            END
        END

        IF @IsAvailable = 0
        BEGIN
            RAISERROR (N'선택한 시간에 예약할 수 없습니다.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- 예약 번호 생성 (APT + YYYYMMDD + 일련번호)
        DECLARE @AppointmentNumber NVARCHAR(20);
        DECLARE @DatePart NVARCHAR(8) = CONVERT(NVARCHAR(8), @AppointmentDateTime, 112);
        DECLARE @SeqNum INT;

        SELECT @SeqNum = ISNULL(MAX(CAST(RIGHT(AppointmentNumber, 4) AS INT)), 0) + 1
        FROM Appointments
        WHERE AppointmentNumber LIKE 'APT' + @DatePart + '%';

        SET @AppointmentNumber = 'APT' + @DatePart + RIGHT('0000' + CAST(@SeqNum AS NVARCHAR), 4);

        -- 예약 생성
        INSERT INTO Appointments (
            AppointmentNumber, PatientId, DoctorId, AppointmentDateTime,
            DurationMinutes, Status, Reason, CreatedBy
        )
        VALUES (
            @AppointmentNumber, @PatientId, @DoctorId, @AppointmentDateTime,
            @DurationMinutes, 'Scheduled', @Reason, @CreatedBy
        );

        SET @AppointmentId = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- =============================================
-- 프로시저: 예약 수정 (관리자용)
-- =============================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_UpdateAppointment')
    DROP PROCEDURE sp_UpdateAppointment;
GO

CREATE PROCEDURE sp_UpdateAppointment
    @AppointmentId INT,
    @NewAppointmentDateTime DATETIME = NULL,
    @NewDoctorId INT = NULL,
    @NewDurationMinutes INT = NULL,
    @NewStatus NVARCHAR(20) = NULL,
    @UpdatedBy NVARCHAR(50) = 'Admin'
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE @CurrentDoctorId INT;
        DECLARE @CurrentDateTime DATETIME;
        DECLARE @CurrentDuration INT;

        -- 현재 예약 정보 조회
        SELECT
            @CurrentDoctorId = DoctorId,
            @CurrentDateTime = AppointmentDateTime,
            @CurrentDuration = DurationMinutes
        FROM Appointments
        WHERE AppointmentId = @AppointmentId;

        IF @CurrentDoctorId IS NULL
        BEGIN
            RAISERROR (N'예약을 찾을 수 없습니다.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- 변경할 값 설정
        SET @NewDoctorId = ISNULL(@NewDoctorId, @CurrentDoctorId);
        SET @NewAppointmentDateTime = ISNULL(@NewAppointmentDateTime, @CurrentDateTime);
        SET @NewDurationMinutes = ISNULL(@NewDurationMinutes, @CurrentDuration);

        -- 시간/의사가 변경되는 경우 가용성 확인
        IF (@NewDoctorId <> @CurrentDoctorId
            OR @NewAppointmentDateTime <> @CurrentDateTime
            OR @NewDurationMinutes <> @CurrentDuration)
        BEGIN
            -- 중복 예약 확인
            IF EXISTS (
                SELECT 1 FROM Appointments
                WHERE DoctorId = @NewDoctorId
                    AND AppointmentId <> @AppointmentId
                    AND Status = 'Scheduled'
                    AND @NewAppointmentDateTime < DATEADD(MINUTE, DurationMinutes, AppointmentDateTime)
                    AND DATEADD(MINUTE, @NewDurationMinutes, @NewAppointmentDateTime) > AppointmentDateTime
            )
            BEGIN
                RAISERROR (N'선택한 시간에 이미 다른 예약이 있습니다.', 16, 1);
                ROLLBACK TRANSACTION;
                RETURN;
            END
        END

        -- 예약 업데이트
        UPDATE Appointments
        SET
            DoctorId = @NewDoctorId,
            AppointmentDateTime = @NewAppointmentDateTime,
            DurationMinutes = @NewDurationMinutes,
            Status = ISNULL(@NewStatus, Status),
            UpdatedAt = GETDATE()
        WHERE AppointmentId = @AppointmentId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- =============================================
-- 프로시저: 예약 취소
-- =============================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_CancelAppointment')
    DROP PROCEDURE sp_CancelAppointment;
GO

CREATE PROCEDURE sp_CancelAppointment
    @AppointmentId INT,
    @CancelReason NVARCHAR(300) = NULL,
    @CancelledBy NVARCHAR(50) = 'Patient'
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Appointments
    SET
        Status = 'Cancelled',
        CancelledAt = GETDATE(),
        CancelledBy = @CancelledBy,
        CancelReason = @CancelReason,
        UpdatedAt = GETDATE()
    WHERE AppointmentId = @AppointmentId;
END
GO

-- =============================================
-- 프로시저: 수납 생성
-- =============================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_CreatePayment')
    DROP PROCEDURE sp_CreatePayment;
GO

CREATE PROCEDURE sp_CreatePayment
    @AppointmentId INT,
    @TotalAmount DECIMAL(18,2),
    @PaymentMethod NVARCHAR(20),
    @PaidAmount DECIMAL(18,2) = NULL,
    @PaymentId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        DECLARE @PatientId INT;
        DECLARE @PaymentNumber NVARCHAR(20);
        DECLARE @DatePart NVARCHAR(8) = CONVERT(NVARCHAR(8), GETDATE(), 112);
        DECLARE @SeqNum INT;

        -- 환자 ID 조회
        SELECT @PatientId = PatientId
        FROM Appointments
        WHERE AppointmentId = @AppointmentId;

        IF @PatientId IS NULL
        BEGIN
            RAISERROR (N'예약을 찾을 수 없습니다.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- 수납 번호 생성 (PAY + YYYYMMDD + 일련번호)
        SELECT @SeqNum = ISNULL(MAX(CAST(RIGHT(PaymentNumber, 4) AS INT)), 0) + 1
        FROM Payments
        WHERE PaymentNumber LIKE 'PAY' + @DatePart + '%';

        SET @PaymentNumber = 'PAY' + @DatePart + RIGHT('0000' + CAST(@SeqNum AS NVARCHAR), 4);

        -- PaidAmount 기본값 설정
        SET @PaidAmount = ISNULL(@PaidAmount, @TotalAmount);

        -- 수납 상태 결정
        DECLARE @PaymentStatus NVARCHAR(20);
        IF @PaidAmount >= @TotalAmount
            SET @PaymentStatus = 'Paid';
        ELSE IF @PaidAmount > 0
            SET @PaymentStatus = 'PartiallyPaid';
        ELSE
            SET @PaymentStatus = 'Pending';

        -- 수납 생성
        INSERT INTO Payments (
            PaymentNumber, AppointmentId, PatientId, TotalAmount,
            PaidAmount, PaymentStatus, PaymentMethod, PaymentDateTime
        )
        VALUES (
            @PaymentNumber, @AppointmentId, @PatientId, @TotalAmount,
            @PaidAmount, @PaymentStatus, @PaymentMethod, GETDATE()
        );

        SET @PaymentId = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- =============================================
-- 프로시저: 예약 통계 조회
-- =============================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_GetAppointmentStatistics')
    DROP PROCEDURE sp_GetAppointmentStatistics;
GO

CREATE PROCEDURE sp_GetAppointmentStatistics
    @StartDate DATE,
    @EndDate DATE,
    @DepartmentId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        DepartmentId,
        DepartmentName,
        COUNT(*) AS TotalAppointments,
        COUNT(CASE WHEN Status = 'Scheduled' THEN 1 END) AS ScheduledCount,
        COUNT(CASE WHEN Status = 'Completed' THEN 1 END) AS CompletedCount,
        COUNT(CASE WHEN Status = 'Cancelled' THEN 1 END) AS CancelledCount,
        COUNT(CASE WHEN Status = 'NoShow' THEN 1 END) AS NoShowCount,
        COUNT(DISTINCT DoctorId) AS ActiveDoctors,
        COUNT(DISTINCT PatientId) AS UniquePatients,
        AVG(CAST(DurationMinutes AS FLOAT)) AS AvgDuration
    FROM vw_AppointmentDetails
    WHERE CAST(AppointmentDateTime AS DATE) BETWEEN @StartDate AND @EndDate
        AND (@DepartmentId IS NULL OR DepartmentId = @DepartmentId)
    GROUP BY DepartmentId, DepartmentName
    ORDER BY TotalAppointments DESC;
END
GO

PRINT 'Stored procedures created successfully!';
GO
