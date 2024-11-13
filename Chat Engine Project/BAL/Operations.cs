using BEL;
using System.Reflection.Metadata;



namespace BAL
{
    public class Operations
    {
        public bool checkLogin(User user)
        {

            string strQuery = string.Empty;

            strQuery = $"SELECT * from User\n" +
                $"WHERE username = {user.username} AND password = {user.password};";


            return false;
        }
    }
}
