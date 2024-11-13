namespace BEL
{
    public class User
    {
        public string username { get; set; }
        
        public string password { get; set; }
        
        public string email { get; set; }

        public int userID { get; set; }

        public User() { 
        
            userID = 0;
            username = string.Empty;
            password = string.Empty;
            email = string.Empty;
        }


    }
}
