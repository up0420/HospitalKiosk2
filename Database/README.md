# Hospital Kiosk Database 설정 가이드

다른 로컬 환경에서 Hospital Kiosk 데이터베이스를 설정하는 방법입니다.

## 사전 요구사항

1. **SQL Server Express 2022 이상** 설치
   - [Microsoft SQL Server Express 다운로드](https://www.microsoft.com/ko-kr/sql-server/sql-server-downloads)
   - 기본 인스턴스명: `SQLEXPRESS`

2. **SQL Server Management Studio (SSMS)** 또는 **sqlcmd** 설치
   - [SSMS 다운로드](https://docs.microsoft.com/ko-kr/sql/ssms/download-sql-server-management-studio-ssms)

## 설치 방법

### 방법 1: 통합 스크립트 사용 (권장)

1. **SetupDatabase_Complete.sql** 파일 실행
   ```sql
   -- SSMS에서 실행하거나
   sqlcmd -S (local)\SQLEXPRESS -i SetupDatabase_Complete.sql
   ```

2. 이 스크립트는 다음을 자동으로 수행합니다:
   - ✅ 데이터베이스 생성
   - ✅ 모든 테이블 생성
   - ✅ 인덱스 생성
   - ✅ 기본 데이터 삽입 (진료과, 관리자, 샘플 의사, 샘플 환자)
   - ✅ 샘플 일정 데이터 삽입

### 방법 2: 개별 스크립트 실행

순서대로 실행하세요:

1. **CreateDatabase.sql** - 데이터베이스 및 테이블 생성
2. **CreateViews.sql** - 통계용 뷰 생성 (선택사항)
3. **CreateProcedures.sql** - 저장 프로시저 생성 (선택사항)

```sql
sqlcmd -S (local)\SQLEXPRESS -i CreateDatabase.sql
sqlcmd -S (local)\SQLEXPRESS -i CreateViews.sql
sqlcmd -S (local)\SQLEXPRESS -i CreateProcedures.sql
```

## 생성되는 데이터

### 진료과 (10개)
- 내과, 외과, 정형외과, 소아과, 산부인과
- 안과, 이비인후과, 피부과, 정신건강의학과, 가정의학과

### 관리자 계정 (1개)
- **Username**: `admin`
- **Password**: `admin123`
- 이메일: admin@hospital.com

### 샘플 의사 (3명)
모든 의사의 비밀번호는 `admin123` 입니다.

| Username | 이름 | 진료과 | 이메일 |
|----------|------|--------|--------|
| kim.cs | 김철수 | 내과 | kim.cs@hospital.com |
| lee.yh | 이영희 | 소아과 | lee.yh@hospital.com |
| park.ms | 박민수 | 정형외과 | park.ms@hospital.com |

### 샘플 환자 (3명)
- 홍길동 (P20250001)
- 김영희 (P20250002)
- 이철수 (P20250003)

### 샘플 일정 데이터
- 각 의사의 오늘/내일 근무 일정
- 박민수 의사의 휴가 일정

## 연결 문자열

애플리케이션의 `App.config` 파일에서 사용할 연결 문자열:

```xml
<connectionStrings>
    <add name="HospitalKioskDB"
         connectionString="Data Source=(local)\SQLEXPRESS;Initial Catalog=HospitalKiosk;Integrated Security=True;"
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

### SQL Server 인스턴스명이 다른 경우

SQL Server 인스턴스명이 `SQLEXPRESS`가 아닌 경우:

```xml
<!-- 기본 인스턴스 -->
<add name="HospitalKioskDB"
     connectionString="Data Source=(local);Initial Catalog=HospitalKiosk;Integrated Security=True;"
     providerName="System.Data.SqlClient" />

<!-- 명명된 인스턴스 -->
<add name="HospitalKioskDB"
     connectionString="Data Source=(local)\YOUR_INSTANCE_NAME;Initial Catalog=HospitalKiosk;Integrated Security=True;"
     providerName="System.Data.SqlClient" />
```

## 데이터베이스 구조

### 주요 테이블

- **Departments**: 진료과 정보
- **Doctors**: 의사 정보 (로그인 계정 포함)
- **Patients**: 환자 정보
- **Admins**: 관리자 정보
- **DoctorSchedules**: 의사 일정 (근무, 휴가, 회의, 휴진)
- **Appointments**: 예약 정보
- **Payments**: 수납 정보

### 주요 인덱스

성능 최적화를 위한 인덱스:
- 예약 조회용 인덱스 (날짜, 환자, 의사, 상태별)
- 의사 일정 조회용 인덱스
- 수납 조회용 인덱스

## 비밀번호 보안

⚠️ **중요**: 운영 환경에서는 반드시 기본 비밀번호를 변경하세요!

### 관리자 비밀번호 변경

```sql
USE HospitalKiosk;
GO

-- 새 비밀번호의 SHA256 해시값을 생성하여 업데이트
UPDATE Admins
SET PasswordHash = 'YOUR_NEW_PASSWORD_HASH',
    UpdatedAt = GETDATE()
WHERE Username = 'admin';
GO
```

### 의사 비밀번호 변경

```sql
-- 특정 의사의 비밀번호 변경
UPDATE Doctors
SET PasswordHash = 'YOUR_NEW_PASSWORD_HASH',
    UpdatedAt = GETDATE()
WHERE Username = 'kim.cs';
GO
```

## 문제 해결

### 1. 데이터베이스 생성 권한 오류

```
Error: CREATE DATABASE permission denied
```

**해결방법**: SQL Server에 관리자 권한으로 로그인하거나, sysadmin 역할이 있는 계정으로 실행하세요.

### 2. SQL Server 서비스가 실행되지 않음

```
Error: Cannot connect to (local)\SQLEXPRESS
```

**해결방법**: SQL Server 서비스를 시작하세요.

```cmd
# Windows Services에서 시작
services.msc

# 또는 명령 프롬프트에서
net start MSSQLSERVER
net start MSSQL$SQLEXPRESS
```

### 3. 인스턴스를 찾을 수 없음

**해결방법**: 설치된 SQL Server 인스턴스 확인

```cmd
# 명령 프롬프트에서 실행
sqlcmd -L
```

### 4. 기존 데이터베이스 삭제 후 재설정

```sql
USE master;
GO

-- 모든 연결 종료
ALTER DATABASE HospitalKiosk SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

-- 데이터베이스 삭제
DROP DATABASE HospitalKiosk;
GO

-- SetupDatabase_Complete.sql 다시 실행
```

## 추가 정보

- **SHA256 비밀번호 해싱**: 모든 비밀번호는 SHA256으로 해시되어 저장됩니다.
- **기본 데이터**: 개발/테스트용 샘플 데이터가 포함되어 있습니다.
- **확장성**: 필요에 따라 진료과, 의사, 일정 등을 추가할 수 있습니다.

## 라이선스

Copyright © 2025 Hospital Kiosk Project
