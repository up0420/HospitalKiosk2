using System;
using System.Configuration;
using System.Data.SqlClient;

namespace HospitalKiosk.Data
{
    public static class DatabaseHelper
    {
        private static string _connectionString;

        static DatabaseHelper()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["HospitalKioskDB"]?.ConnectionString;

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Connection string 'HospitalKioskDB' not found in App.config");
            }
        }

        public static string ConnectionString
        {
            get { return _connectionString; }
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public static bool TestConnection()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DB Connection Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Connection String: {_connectionString}");
                throw; // Re-throw to show detailed error in UI
            }
        }

        public static void ExecuteNonQuery(string query, Action<SqlCommand> configureCommand = null)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                configureCommand?.Invoke(command);
                command.ExecuteNonQuery();
            }
        }

        public static T ExecuteScalar<T>(string query, Action<SqlCommand> configureCommand = null)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();
                configureCommand?.Invoke(command);
                var result = command.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    return default(T);
                }

                return (T)Convert.ChangeType(result, typeof(T));
            }
        }
    }
}
