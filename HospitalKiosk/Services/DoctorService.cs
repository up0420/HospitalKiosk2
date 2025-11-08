using System;
using System.Security.Cryptography;
using System.Text;
using HospitalKiosk.Data;
using HospitalKiosk.Models;

namespace HospitalKiosk.Services
{
    public class DoctorService
    {
        private readonly DoctorRepository _doctorRepo;

        public DoctorService()
        {
            _doctorRepo = new DoctorRepository();
        }

        /// <summary>
        /// 의사 로그인
        /// </summary>
        public DoctorLoginResult Login(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    return new DoctorLoginResult
                    {
                        Success = false,
                        Message = "사용자명과 비밀번호를 입력하세요."
                    };
                }

                var doctor = _doctorRepo.GetByUsername(username);
                if (doctor == null)
                {
                    return new DoctorLoginResult
                    {
                        Success = false,
                        Message = "사용자명 또는 비밀번호가 올바르지 않습니다."
                    };
                }

                // 비밀번호 확인
                var passwordHash = HashPassword(password);
                if (doctor.PasswordHash != passwordHash)
                {
                    return new DoctorLoginResult
                    {
                        Success = false,
                        Message = "사용자명 또는 비밀번호가 올바르지 않습니다."
                    };
                }

                if (!doctor.IsActive)
                {
                    return new DoctorLoginResult
                    {
                        Success = false,
                        Message = "비활성화된 계정입니다. 관리자에게 문의하세요."
                    };
                }

                return new DoctorLoginResult
                {
                    Success = true,
                    Message = "로그인 성공",
                    Doctor = doctor
                };
            }
            catch (Exception ex)
            {
                return new DoctorLoginResult
                {
                    Success = false,
                    Message = "로그인 중 오류가 발생했습니다: " + ex.Message
                };
            }
        }

        /// <summary>
        /// SHA256을 사용한 비밀번호 해싱
        /// </summary>
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "");
            }
        }

        /// <summary>
        /// 의사 정보 조회
        /// </summary>
        public Doctor GetDoctorById(int doctorId)
        {
            return _doctorRepo.GetById(doctorId);
        }

        /// <summary>
        /// 비밀번호 변경
        /// </summary>
        public DoctorOperationResult ChangePassword(int doctorId, string currentPassword, string newPassword)
        {
            try
            {
                var doctor = _doctorRepo.GetById(doctorId);
                if (doctor == null)
                {
                    return new DoctorOperationResult
                    {
                        Success = false,
                        Message = "의사를 찾을 수 없습니다."
                    };
                }

                // 현재 비밀번호 확인
                var currentPasswordHash = HashPassword(currentPassword);
                if (doctor.PasswordHash != currentPasswordHash)
                {
                    return new DoctorOperationResult
                    {
                        Success = false,
                        Message = "현재 비밀번호가 올바르지 않습니다."
                    };
                }

                // 새 비밀번호 유효성 검사
                if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
                {
                    return new DoctorOperationResult
                    {
                        Success = false,
                        Message = "새 비밀번호는 6자 이상이어야 합니다."
                    };
                }

                // 비밀번호 업데이트
                using (var connection = DatabaseHelper.GetConnection())
                using (var command = new System.Data.SqlClient.SqlCommand(@"
                    UPDATE Doctors
                    SET PasswordHash = @PasswordHash, UpdatedAt = @UpdatedAt
                    WHERE DoctorId = @DoctorId", connection))
                {
                    command.Parameters.AddWithValue("@PasswordHash", HashPassword(newPassword));
                    command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                    command.Parameters.AddWithValue("@DoctorId", doctorId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                return new DoctorOperationResult
                {
                    Success = true,
                    Message = "비밀번호가 변경되었습니다."
                };
            }
            catch (Exception ex)
            {
                return new DoctorOperationResult
                {
                    Success = false,
                    Message = "비밀번호 변경 중 오류가 발생했습니다: " + ex.Message
                };
            }
        }
    }

    // 헬퍼 클래스들
    public class DoctorLoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Doctor Doctor { get; set; }
    }

    public class DoctorOperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
