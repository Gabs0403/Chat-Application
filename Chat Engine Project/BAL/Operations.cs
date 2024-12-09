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
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public (User, List<User>) CheckLoginAndFetchUsers(User user)
        {
            User loggedInUser = new User();
            List<User> allUsers = new List<User>();

            // Check Login
            string loginQuery = "SELECT * FROM Users WHERE UserName = @username AND Password = @password;";
            var loginCmd = new SqlCommand(loginQuery);
            loginCmd.Parameters.AddWithValue("@username", user.username);
            loginCmd.Parameters.AddWithValue("@password", user.password);

            try
            {
                DataTable loginTable = Connection.ExeReader(loginCmd);

                if (loginTable.Rows.Count > 0)
                {
                    DataRow row = loginTable.Rows[0];
                    loggedInUser = new User
                    {
                        userID = Convert.ToInt32(row["UserId"]),
                        username = row["UserName"].ToString(),
                        email = row["Email"].ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login check: {ex.Message}");
            }

            // Fetch All Users
            string fetchUsersQuery = "SELECT * FROM Users;";
            var fetchUsersCmd = new SqlCommand(fetchUsersQuery);

            try
            {
                DataTable usersTable = Connection.ExeReader(fetchUsersCmd);

                foreach (DataRow row in usersTable.Rows)
                {
                    var user1 = new User
                    {
                        userID = Convert.ToInt32(row["UserId"]),
                        username = row["UserName"].ToString(),
                        email = row["Email"].ToString()
                    };

                    allUsers.Add(user1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
            }

            return (loggedInUser, allUsers);
        }

        public int AddConversationToDatabase(int userID, string selectedUsername)
        {
            // Create a new conversation and get the ConversationID
            string conversationQuery = "INSERT INTO Conversation (LastMessageAt) VALUES (@lastMessageAt); SELECT SCOPE_IDENTITY();";
            using (var cmd = new SqlCommand(conversationQuery))
            {
                cmd.Parameters.AddWithValue("@lastMessageAt", DateTime.Now); 

                try
                {
                    // Get the new ConversationID after inserting
                    var conversationID = Convert.ToInt32(Connection.ExeScalar(cmd));

                    // Get the UserID of the selected user
                    string getUserIDQuery = "SELECT UserID FROM Users WHERE UserName = @selectedUsername;";
                    using (var userCmd = new SqlCommand(getUserIDQuery))
                    {
                        userCmd.Parameters.AddWithValue("@selectedUsername", selectedUsername);

                        var selectedUserID = Convert.ToInt32(Connection.ExeScalar(userCmd));

                        // Insert into User-Conversation table for the logged-in user
                        string userConversationQuery = "INSERT INTO [User_Conversation] (UserID, ConversationID, JoinedAt) VALUES (@userID, @conversationID, @joinedAt);";
                        using (var userConvCmd = new SqlCommand(userConversationQuery))
                        {
                            userConvCmd.Parameters.AddWithValue("@userID", userID);
                            userConvCmd.Parameters.AddWithValue("@conversationID", conversationID);
                            userConvCmd.Parameters.AddWithValue("@joinedAt", DateTime.Now);

                            Connection.ExeNonQuery(userConvCmd);
                        }

                        // Insert into User-Conversation table for the selected user
                        using (var userConvCmd = new SqlCommand(userConversationQuery))
                        {
                            userConvCmd.Parameters.AddWithValue("@userID", selectedUserID);
                            userConvCmd.Parameters.AddWithValue("@conversationID", conversationID);
                            userConvCmd.Parameters.AddWithValue("@joinedAt", DateTime.Now);

                            Connection.ExeNonQuery(userConvCmd); 
                        }
                    }

                    return conversationID;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating conversation: {ex.Message}");
                    return -1;
                }
            }
        }

        public List<Tuple<int, string>> LoadExistingChats(int userID)
        {
            var chats = new List<Tuple<int, string>>();

            string query = @"
                  SELECT uc.ConversationID, u.UserName
                  FROM [User_Conversation] uc
                  JOIN Users u ON uc.UserID = u.UserID
                  WHERE uc.ConversationID IN (
                      SELECT ConversationID
                      FROM [User_Conversation]
                      WHERE UserID = @userID
                  )
                  AND uc.UserID != @userID;
              ";

            using (var cmd = new SqlCommand(query))
            {
                cmd.Parameters.AddWithValue("@userID", userID);

                try
                {
                    var dt = Connection.ExeReader(cmd);

                    foreach (DataRow row in dt.Rows)
                    {
                        var conversationID = Convert.ToInt32(row["ConversationID"]);
                        var username = row["UserName"].ToString();
                        chats.Add(new Tuple<int, string>(conversationID, username));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading existing chats: {ex.Message}");
                }
            }

            return chats;
        }

        public List<Tuple<string, int>> GetMessagesForConversation(string conversationID)
        {
            // List to hold messages and their sender IDs
            List<Tuple<string, int>> messages = new List<Tuple<string, int>>();

            // SQL query to fetch all messages for a given conversation ID
            string query = @"
                  SELECT MessageContent, SenderID
                  FROM Message
                  WHERE ConversationID = @ConversationID
                  ORDER BY MessageID ASC";

            try
            {
                using (var cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@ConversationID", conversationID);

                    // Execute the query and retrieve the data
                    var dt = Connection.ExeReader(cmd);

                    foreach (DataRow row in dt.Rows)
                    {
                        string messageContent = row["MessageContent"].ToString();
                        int senderID = Convert.ToInt32(row["SenderID"]);
                        messages.Add(new Tuple<string, int>(messageContent, senderID));
                    }
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error retrieving messages for conversation {conversationID}: {ex.Message}");
            }

            return messages;
        }



        public void AddMessageToDatabase(string? conversationID, string userID, string message)
        {
            if (conversationID == null)
            {
                throw new ArgumentNullException(nameof(conversationID), "ConversationID cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message content cannot be empty.", nameof(message));
            }

            string updateConversationQuery = @"
                UPDATE Conversation
                SET LastMessageAt = @LastMessageAt
                WHERE ConversationID = @ConversationID";

            string insertMessageQuery = @"
                INSERT INTO Message (ConversationID, SenderID, MessageContent, SentAt)
                VALUES (@ConversationID, @SenderID, @MessageContent, @SentAt)";

            try
            {
                using (var updateCmd = new SqlCommand(updateConversationQuery))
                {
                    updateCmd.Parameters.AddWithValue("@LastMessageAt", DateTime.Now);
                    updateCmd.Parameters.AddWithValue("@ConversationID", conversationID);

                    Connection.ExeNonQuery(updateCmd);
                }

                using (var insertCmd = new SqlCommand(insertMessageQuery))
                {
                    insertCmd.Parameters.AddWithValue("@ConversationID", conversationID);
                    insertCmd.Parameters.AddWithValue("@SenderID", userID);
                    insertCmd.Parameters.AddWithValue("@MessageContent", message);
                    insertCmd.Parameters.AddWithValue("@SentAt", DateTime.Now);

                    Connection.ExeNonQuery(insertCmd); // Insert the message into the database
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding message to database: {ex.Message}");
            }
        }


    }


}