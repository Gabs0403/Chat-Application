using BEL;
using System.Data.Common;
using System.Reflection.Metadata;
using DAL;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BAL
{
    public class Operations
    {
        public Dbconnection Connection = new Dbconnection();

        public User checkLogin(User user)
        {

            string strQuery = string.Empty;

            strQuery = $"SELECT * from Users\n" +
                $"WHERE username = '{user.username}' AND password = '{user.password}';";

            var cmd = new SqlCommand();
            cmd.CommandText = strQuery;
            cmd.CommandType = CommandType.Text;

            User loggedInUser = null;

            try
            {
                using (var reader = Connection.ExeReader(cmd))
                {
                    if (reader.Rows.Count > 0)
                    {
                        DataRow row = reader.Rows[0];
                        loggedInUser = new User
                        {
                            userID = Convert.ToInt32(row["UserId"]),
                            username = row["UserName"].ToString(),
                            email = row["Email"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine(ex.Message);
            }

            return loggedInUser;
        }

        public int addUser(User user)
        {
            string strQuery = string.Empty;

            strQuery = $"INSERT INTO Users (Username,Email,Password)" +
                $"VALUES (" +
                $"'{user.username}'," +
                $"'{user.email}'," +
                $"'{user.password}')";

            var cmd = new SqlCommand();
            cmd.CommandText = strQuery;
            cmd.CommandType = CommandType.Text;
            try
            {

                int result = Connection.ExeNonQuery(cmd);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return -1;
            }

        }

        public bool isValidPassword(string password, string confirmPassword)
        {
            return password.Length >= 8 &&
                   password.Any(char.IsUpper) &&
                   password.Count(char.IsLetter) >= 2 &&
                   password.Any(char.IsDigit) &&
                   password == confirmPassword;
        }

        public bool userExists(string username)
        {
            var cmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE username = @username");
            cmd.Parameters.AddWithValue("@username", username);
            try
            {
                int count = Convert.ToInt32(Connection.ExeScalar(cmd));
                return count > 0;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

    }

    
}
