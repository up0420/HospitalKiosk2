using System;
using System.Security.Cryptography;
using System.Text;
using HospitalKiosk.Data;
using HospitalKiosk.Models;

namespace HospitalKiosk.Services
{
    public class AdminService
    {
        private readonly AdminRepository _adminRepo;

        public AdminService()
        {
            _adminRepo = new AdminRepository();
        }

        /// <summary>
        /// 관리자 로그인
        /// </summary>
        public LoginResult Login(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    return new LoginResult
                    {
                        Success = false,
                        Message = "사용자명과 비밀번호를 입력하세요."
                    };
                }

                var admin = _adminRepo.GetByUsername(username);
                if (admin == null)
                {
                    return new LoginResult
                    {
                        Success = false,
                        Message = "사용자명 또는 비밀번호가 올바르지 않습니다."
                    };
                }

                // 비밀번호 확인
                var passwordHash = HashPassword(password);
                if (admin.PasswordHash != passwordHash)
                {
                    return new LoginResult
                    {
                        Success = false,
                        Message = "사용자명 또는 비밀번호가 올바르지 않습니다."
                    };
                }

                if (!admin.IsActive)
                {
                    return new LoginResult
                    {
                        Success = false,
                        Message = "비활성화된 계정입니다. 관리자에게 문의하세요."
                    };
                }

                return new LoginResult
                {
                    Success = true,
                    Message = "로그인 성공",
                    Admin = admin
                };
            }
            catch (Exception ex)
            {
                return new LoginResult
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
        /// 관리자 정보 조회
        /// </summary>
        public Admin GetAdminById(int adminId)
        {
            return _adminRepo.GetById(adminId);
        }

        /// <summary>
        /// 관리자 정보 업데이트
        /// </summary>
        public OperationResult UpdateAdmin(Admin admin)
        {
            try
            {
                if (admin == null)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "관리자 정보가 없습니다."
                    };
                }

                _adminRepo.Update(admin);

                return new OperationResult
                {
                    Success = true,
                    Message = "관리자 정보가 업데이트되었습니다."
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "업데이트 중 오류가 발생했습니다: " + ex.Message
                };
            }
        }

        /// <summary>
        /// 비밀번호 변경
        /// </summary>
        public OperationResult ChangePassword(int adminId, string currentPassword, string newPassword)
        {
            try
            {
                var admin = _adminRepo.GetById(adminId);
                if (admin == null)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "관리자를 찾을 수 없습니다."
                    };
                }

                // 현재 비밀번호 확인
                var currentPasswordHash = HashPassword(currentPassword);
                if (admin.PasswordHash != currentPasswordHash)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "현재 비밀번호가 올바르지 않습니다."
                    };
                }

                // 새 비밀번호 유효성 검사
                if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "새 비밀번호는 6자 이상이어야 합니다."
                    };
                }

                // 비밀번호 업데이트
                admin.PasswordHash = HashPassword(newPassword);
                admin.UpdatedAt = DateTime.Now;

                using (var connection = DatabaseHelper.GetConnection())
                using (var command = new System.Data.SqlClient.SqlCommand(@"
                    UPDATE Admins
                    SET PasswordHash = @PasswordHash, UpdatedAt = @UpdatedAt
                    WHERE AdminId = @AdminId", connection))
                {
                    command.Parameters.AddWithValue("@PasswordHash", admin.PasswordHash);
                    command.Parameters.AddWithValue("@UpdatedAt", admin.UpdatedAt);
                    command.Parameters.AddWithValue("@AdminId", adminId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                return new OperationResult
                {
                    Success = true,
                    Message = "비밀번호가 변경되었습니다."
                };
            }
            catch (Exception ex)
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "비밀번호 변경 중 오류가 발생했습니다: " + ex.Message
                };
            }
        }
    }

    // 헬퍼 클래스들
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Admin Admin { get; set; }
    }

    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
