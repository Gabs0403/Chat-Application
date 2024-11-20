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

            User newUser = new User();

            try
            {
                using (var reader = Connection.ExeReader(cmd))
                {
                    foreach (DataRow row in reader.Rows)
                    {
                        // Retrieve and parse the data
                        newUser.userID = Convert.ToInt32(row["UserId"]); // Convert Id to integer
                        newUser.username = row["UserName"].ToString(); // Convert to string
                        newUser.password = row["Password"].ToString(); // Convert to string
                        newUser.email = row["Email"].ToString(); // Convert to string

                    }

                }
            }
            catch (Exception ex)
            {

            }


            return newUser;
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

        

    }
}
