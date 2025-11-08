using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using HospitalKiosk.Models;

namespace HospitalKiosk.Data
{
    public class PaymentRepository : IRepository<Payment>
    {
        public Payment GetById(int id)
        {
            Payment payment = null;

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand("SELECT * FROM Payments WHERE PaymentId = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        payment = MapToPayment(reader);
                    }
                }
            }

            return payment;
        }

        public Payment GetByAppointmentId(int appointmentId)
        {
            Payment payment = null;

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand("SELECT * FROM Payments WHERE AppointmentId = @AppointmentId", connection))
            {
                command.Parameters.AddWithValue("@AppointmentId", appointmentId);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        payment = MapToPayment(reader);
                    }
                }
            }

            return payment;
        }

        public IEnumerable<Payment> GetAll()
        {
            var payments = new List<Payment>();

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand("SELECT * FROM Payments ORDER BY CreatedAt DESC", connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        payments.Add(MapToPayment(reader));
                    }
                }
            }

            return payments;
        }

        public IEnumerable<Payment> GetByPatientId(int patientId)
        {
            var payments = new List<Payment>();

            var query = @"SELECT * FROM Payments
                         WHERE PatientId = @PatientId
                         ORDER BY CreatedAt DESC";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PatientId", patientId);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        payments.Add(MapToPayment(reader));
                    }
                }
            }

            return payments;
        }

        public IEnumerable<Payment> GetPendingPayments()
        {
            var payments = new List<Payment>();

            var query = @"SELECT * FROM Payments
                         WHERE PaymentStatus IN ('Pending', 'PartiallyPaid')
                         ORDER BY CreatedAt DESC";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        payments.Add(MapToPayment(reader));
                    }
                }
            }

            return payments;
        }

        public int Insert(Payment entity)
        {
            var query = @"INSERT INTO Payments (
                             PaymentNumber, AppointmentId, PatientId, TotalAmount, PaidAmount,
                             PaymentStatus, PaymentMethod, PaymentDateTime, ReceiptNumber, Notes,
                             CreatedAt, UpdatedAt
                         )
                         VALUES (
                             @PaymentNumber, @AppointmentId, @PatientId, @TotalAmount, @PaidAmount,
                             @PaymentStatus, @PaymentMethod, @PaymentDateTime, @ReceiptNumber, @Notes,
                             @CreatedAt, @UpdatedAt
                         );
                         SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                AddParameters(command, entity);
                connection.Open();

                return (int)command.ExecuteScalar();
            }
        }

        public void Update(Payment entity)
        {
            var query = @"UPDATE Payments SET
                         TotalAmount = @TotalAmount,
                         PaidAmount = @PaidAmount,
                         PaymentStatus = @PaymentStatus,
                         PaymentMethod = @PaymentMethod,
                         PaymentDateTime = @PaymentDateTime,
                         ReceiptNumber = @ReceiptNumber,
                         Notes = @Notes,
                         UpdatedAt = @UpdatedAt
                         WHERE PaymentId = @Id";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                AddParameters(command, entity);
                command.Parameters.AddWithValue("@Id", entity.PaymentId);
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            var query = "DELETE FROM Payments WHERE PaymentId = @Id";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public void ProcessPayment(int paymentId, decimal amount, string paymentMethod)
        {
            var query = @"UPDATE Payments SET
                         PaidAmount = PaidAmount + @Amount,
                         PaymentMethod = @PaymentMethod,
                         PaymentDateTime = @PaymentDateTime,
                         PaymentStatus = CASE
                             WHEN PaidAmount + @Amount >= TotalAmount THEN 'Paid'
                             WHEN PaidAmount + @Amount > 0 THEN 'PartiallyPaid'
                             ELSE 'Pending'
                         END,
                         UpdatedAt = @UpdatedAt
                         WHERE PaymentId = @Id";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", paymentId);
                command.Parameters.AddWithValue("@Amount", amount);
                command.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                command.Parameters.AddWithValue("@PaymentDateTime", DateTime.Now);
                command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public string GeneratePaymentNumber()
        {
            var datePart = DateTime.Now.ToString("yyyyMMdd");
            var query = @"SELECT ISNULL(MAX(CAST(RIGHT(PaymentNumber, 4) AS INT)), 0) + 1
                         FROM Payments
                         WHERE PaymentNumber LIKE @Prefix";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Prefix", "PAY" + datePart + "%");
                connection.Open();

                var seqNum = (int)command.ExecuteScalar();
                return "PAY" + datePart + seqNum.ToString("D4");
            }
        }

        public string GenerateReceiptNumber()
        {
            var datePart = DateTime.Now.ToString("yyyyMMdd");
            var query = @"SELECT ISNULL(MAX(CAST(RIGHT(ReceiptNumber, 4) AS INT)), 0) + 1
                         FROM Payments
                         WHERE ReceiptNumber LIKE @Prefix";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Prefix", "RCP" + datePart + "%");
                connection.Open();

                var seqNum = (int)command.ExecuteScalar();
                return "RCP" + datePart + seqNum.ToString("D4");
            }
        }

        private Payment MapToPayment(SqlDataReader reader)
        {
            return new Payment
            {
                PaymentId = reader.GetInt32(reader.GetOrdinal("PaymentId")),
                PaymentNumber = reader.GetString(reader.GetOrdinal("PaymentNumber")),
                AppointmentId = reader.GetInt32(reader.GetOrdinal("AppointmentId")),
                PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                PaidAmount = reader.GetDecimal(reader.GetOrdinal("PaidAmount")),
                PaymentStatus = reader.GetString(reader.GetOrdinal("PaymentStatus")),
                PaymentMethod = reader.IsDBNull(reader.GetOrdinal("PaymentMethod")) ? null : reader.GetString(reader.GetOrdinal("PaymentMethod")),
                PaymentDateTime = reader.IsDBNull(reader.GetOrdinal("PaymentDateTime")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("PaymentDateTime")),
                ReceiptNumber = reader.IsDBNull(reader.GetOrdinal("ReceiptNumber")) ? null : reader.GetString(reader.GetOrdinal("ReceiptNumber")),
                Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
            };
        }

        private void AddParameters(SqlCommand command, Payment entity)
        {
            command.Parameters.AddWithValue("@PaymentNumber", entity.PaymentNumber);
            command.Parameters.AddWithValue("@AppointmentId", entity.AppointmentId);
            command.Parameters.AddWithValue("@PatientId", entity.PatientId);
            command.Parameters.AddWithValue("@TotalAmount", entity.TotalAmount);
            command.Parameters.AddWithValue("@PaidAmount", entity.PaidAmount);
            command.Parameters.AddWithValue("@PaymentStatus", entity.PaymentStatus);
            command.Parameters.AddWithValue("@PaymentMethod", (object)entity.PaymentMethod ?? DBNull.Value);
            command.Parameters.AddWithValue("@PaymentDateTime", (object)entity.PaymentDateTime ?? DBNull.Value);
            command.Parameters.AddWithValue("@ReceiptNumber", (object)entity.ReceiptNumber ?? DBNull.Value);
            command.Parameters.AddWithValue("@Notes", (object)entity.Notes ?? DBNull.Value);
            command.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);
            command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
        }
    }
}
