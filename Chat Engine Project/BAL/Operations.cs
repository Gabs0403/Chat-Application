using BEL;
using System.Data.Common;
using System.Reflection.Metadata;
using DAL;
using Microsoft.Data.SqlClient;

namespace BAL
{
    public class Operations
    {
        public DAL.DbConnection Connection = new DAL.DbConnection();

        public bool checkLogin(User user)
        {

            string strQuery = string.Empty;

            strQuery = $"SELECT * from Users\n" +
                $"WHERE username = {user.username} AND password = {user.password};";


            return false;
        }


        public bool addUser(User user)
        {
            string strQuery = "INSERT INTO Users (Username, Email, Password) VALUES ('ique', 'abc', '1234')";

            var cmd = new SqlCommand(strQuery);
            //cmd.Parameters.AddWithValue("@username", user.username);
            //cmd.Parameters.AddWithValue("@email", user.email);
            //cmd.Parameters.AddWithValue("@password", user.password);

            try
            {
                Connection.ExeNonQuery(cmd);
                return true;
            }
            catch (Exception ex)
            {
                // Optionally log the exception (ex.Message) here for more details
                return false;
            }
        }

        //public bool addUser(User user)
        //{
        //    string strQuery = string.Empty ;

        //    strQuery = $"INSERT INTO Users (Username,Email,Password)" +
        //        $"VALUES (" +
        //        $"'{user.username}'," +
        //        $"'{user.email}'," +
        //        $"'{user.password}')";

        //    var cmd = new SqlCommand();
        //    cmd.CommandText = strQuery;
        //    try
        //    {

        //        Connection.ExeNonQuery(cmd);
        //        return true;
        //    }
        //    catch (Exception ex) {

        //        return false;
        //    }

        //}

    }
}
