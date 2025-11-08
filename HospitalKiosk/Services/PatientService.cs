using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HospitalKiosk.Data;
using HospitalKiosk.Models;

namespace HospitalKiosk.Services
{
    public class PatientService
    {
        private readonly PatientRepository _patientRepo;

        public PatientService()
        {
            _patientRepo = new PatientRepository();
        }

        /// <summary>
        /// 환자 등록
        /// </summary>
        public PatientResult RegisterPatient(string patientName, DateTime birthDate, string gender,
            string phoneNumber, string address = null, string emergencyContact = null)
        {
            try
            {
                // 유효성 검증
                var validation = ValidatePatientInfo(patientName, birthDate, gender, phoneNumber);
                if (!validation.IsValid)
                {
                    return new PatientResult { Success = false, Message = validation.ErrorMessage };
                }

                // 환자 번호 생성
                var patientNumber = _patientRepo.GeneratePatientNumber();

                var patient = new Patient
                {
                    PatientNumber = patientNumber,
                    PatientName = patientName,
                    BirthDate = birthDate.Date,
                    Gender = gender,
                    PhoneNumber = phoneNumber,
                    Address = address,
                    EmergencyContact = emergencyContact,
                    IsActive = true
                };

                var patientId = _patientRepo.Insert(patient);
                patient.PatientId = patientId;

                return new PatientResult
                {
                    Success = true,
                    Message = "환자 등록이 완료되었습니다.",
                    Patient = patient
                };
            }
            catch (Exception ex)
            {
                return new PatientResult { Success = false, Message = "환자 등록 중 오류가 발생했습니다: " + ex.Message };
            }
        }

        /// <summary>
        /// 환자 정보 수정
        /// </summary>
        public PatientResult UpdatePatient(int patientId, string patientName, DateTime birthDate,
            string gender, string phoneNumber, string address, string emergencyContact)
        {
            try
            {
                var patient = _patientRepo.GetById(patientId);
                if (patient == null)
                {
                    return new PatientResult { Success = false, Message = "환자를 찾을 수 없습니다." };
                }

                // 유효성 검증
                var validation = ValidatePatientInfo(patientName, birthDate, gender, phoneNumber);
                if (!validation.IsValid)
                {
                    return new PatientResult { Success = false, Message = validation.ErrorMessage };
                }

                patient.PatientName = patientName;
                patient.BirthDate = birthDate.Date;
                patient.Gender = gender;
                patient.PhoneNumber = phoneNumber;
                patient.Address = address;
                patient.EmergencyContact = emergencyContact;

                _patientRepo.Update(patient);

                return new PatientResult
                {
                    Success = true,
                    Message = "환자 정보가 수정되었습니다.",
                    Patient = patient
                };
            }
            catch (Exception ex)
            {
                return new PatientResult { Success = false, Message = "환자 정보 수정 중 오류가 발생했습니다: " + ex.Message };
            }
        }

        /// <summary>
        /// 환자 번호로 조회
        /// </summary>
        public Patient GetPatientByNumber(string patientNumber)
        {
            return _patientRepo.GetByPatientNumber(patientNumber);
        }

        /// <summary>
        /// 환자 ID로 조회
        /// </summary>
        public Patient GetPatientById(int patientId)
        {
            return _patientRepo.GetById(patientId);
        }

        /// <summary>
        /// 환자 검색 (이름, 전화번호, 환자번호)
        /// </summary>
        public List<Patient> SearchPatients(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new List<Patient>();
            }

            return new List<Patient>(_patientRepo.SearchPatients(searchTerm.Trim()));
        }

        /// <summary>
        /// 모든 환자 조회
        /// </summary>
        public List<Patient> GetAllPatients()
        {
            return new List<Patient>(_patientRepo.GetAll());
        }

        /// <summary>
        /// 환자 정보 유효성 검증
        /// </summary>
        private ValidationResult ValidatePatientInfo(string patientName, DateTime birthDate,
            string gender, string phoneNumber)
        {
            // 이름 검증
            if (string.IsNullOrWhiteSpace(patientName))
            {
                return new ValidationResult { IsValid = false, ErrorMessage = "환자 이름을 입력해주세요." };
            }

            if (patientName.Length < 2 || patientName.Length > 100)
            {
                return new ValidationResult { IsValid = false, ErrorMessage = "환자 이름은 2자 이상 100자 이하로 입력해주세요." };
            }

            // 생년월일 검증
            if (birthDate > DateTime.Today)
            {
                return new ValidationResult { IsValid = false, ErrorMessage = "생년월일은 오늘 이전이어야 합니다." };
            }

            if (birthDate < DateTime.Today.AddYears(-150))
            {
                return new ValidationResult { IsValid = false, ErrorMessage = "올바른 생년월일을 입력해주세요." };
            }

            // 성별 검증
            if (gender != "M" && gender != "F")
            {
                return new ValidationResult { IsValid = false, ErrorMessage = "성별을 선택해주세요." };
            }

            // 전화번호 검증
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return new ValidationResult { IsValid = false, ErrorMessage = "전화번호를 입력해주세요." };
            }

            // 전화번호 형식 검증 (숫자와 하이픈만 허용)
            if (!Regex.IsMatch(phoneNumber, @"^[0-9-]+$"))
            {
                return new ValidationResult { IsValid = false, ErrorMessage = "전화번호는 숫자와 하이픈(-)만 입력 가능합니다." };
            }

            return new ValidationResult { IsValid = true };
        }
    }

    // 헬퍼 클래스들
    public class PatientResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Patient Patient { get; set; }
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
    }
}
