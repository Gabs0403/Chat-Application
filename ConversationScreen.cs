//using BAL;
//using BEL;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient; // Ensure this namespace is included
//using System.Drawing;
//using System.Linq;
//using System.Windows.Forms;

//namespace Chat_Engine_Project
//{
//    public partial class ConversationScreen : Form
//    {
//        private User loggedInUser; // The logged-in user
//        private List<User> allUsers; // List of all users
//        private Dictionary<string, List<string>> chatHistory = new Dictionary<string, List<string>>();
//        private string connectionString = "Data Source=HenriquePC\\SQLEXPRESS01;Initial Catalog=chatEngine;Integrated Security=True;TrustServerCertificate=True"; // Update with your actual connection string
//        private Operations operations = new Operations();    

//        public ConversationScreen(User loggedUser, List<User> allUsers)
//        {
//            InitializeComponent();

//            this.loggedInUser = loggedUser;
//            this.allUsers = allUsers;

//            label1.Text = $"Welcome, {loggedInUser.username}!";

//            comboBoxUsers.Items.Clear();
//            foreach (var user in allUsers)
//            {
//                if (user.username != loggedUser.username)
//                {
//                    comboBoxUsers.Items.Add(user.username);
//                }
//            }

//            operations.LoadExistingChats(loggedInUser.userID);
//            AddChatButton()
//        }


//        private void btnChat_Click(object sender, EventArgs e)
//        {
//            if (comboBoxUsers.SelectedItem != null)
//            {
//                string selectedUsername = comboBoxUsers.SelectedItem.ToString();
//                string conversationID = Guid.NewGuid().ToString();

//                if (FLPChatPanel.Controls.Find($"btn_{selectedUsername}", false).Length == 0)
//                {
//                    AddChatButton(selectedUsername, conversationID);
//                    operations.AddConversationToDatabase(conversationID, loggedInUser.userID, selectedUsername);
//                }
//                else
//                {
//                    MessageBox.Show($"Chat with {selectedUsername} already exists!");
//                }
//            }
//            else
//            {
//                MessageBox.Show("Please select a user to add to chat.");
//            }
//        }

//        private void AddChatButton(string username, string conversationID)
//        {
//            Button userButton = new Button
//            {
//                Name = $"btn_{username}",
//                Text = username,
//                Tag = conversationID,
//                Width = 200,
//                Height = 30
//            };
//            userButton.Click += (s, ev) => SwitchChat(username, conversationID);
//            FLPChatPanel.Controls.Add(userButton);
//        }



//        private void SwitchChat(string username, string conversationID)
//        {
//            FLPChat.Controls.Clear();
//            lblChatWith.Text = $"Chatting with: {username}";

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                string query = @"
//                    SELECT SenderID, MessageContent, SentAt 
//                    FROM Message 
//                    WHERE ConversationID = @ConversationID 
//                    ORDER BY SentAt";
//                SqlCommand command = new SqlCommand(query, connection);
//                command.Parameters.AddWithValue("@ConversationID", conversationID);

//                connection.Open();
//                SqlDataReader reader = command.ExecuteReader();

//                while (reader.Read())
//                {
//                    string sender = reader["SenderID"].ToString();
//                    string content = reader["MessageContent"].ToString();
//                    string message = $"{(sender == loggedInUser.UserId ? "You" : username)}: {content}";

//                    Label messageLabel = new Label
//                    {
//                        Text = message,
//                        AutoSize = true
//                    };
//                    FLPChat.Controls.Add(messageLabel);
//                }
//            }
//        }

//        private void btnSend_Click(object sender, EventArgs e)
//        {
//            var selectedButton = FLPChatPanel.Controls
//                .OfType<Button>()
//                .FirstOrDefault(btn => btn.BackColor == Color.LightBlue);

//            if (selectedButton != null)
//            {
//                string conversationID = selectedButton.Tag.ToString();
//                string message = txtBoxMessage.Text.Trim();

//                if (!string.IsNullOrEmpty(message))
//                {
//                    AddMessageToDatabase(conversationID, loggedInUser.UserId, message);

//                    Label messageLabel = new Label
//                    {
//                        Text = $"You: {message}",
//                        AutoSize = true
//                    };
//                    FLPChat.Controls.Add(messageLabel);
//                    txtBoxMessage.Clear();
//                }
//                else
//                {
//                    MessageBox.Show("Please enter a message to send.");
//                }
//            }
//        }

//        private void AddMessageToDatabase(string conversationID, string senderID, string message)
//        {
//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                string query = @"
//                    INSERT INTO Message (MessageID, ConversationID, SenderID, MessageContent, SentAt) 
//                    VALUES (@MessageID, @ConversationID, @SenderID, @MessageContent, GETDATE())";
//                SqlCommand command = new SqlCommand(query, connection);
//                command.Parameters.AddWithValue("@MessageID", Guid.NewGuid().ToString());
//                command.Parameters.AddWithValue("@ConversationID", conversationID);
//                command.Parameters.AddWithValue("@SenderID", senderID);
//                command.Parameters.AddWithValue("@MessageContent", message);

//                connection.Open();
//                command.ExecuteNonQuery();
//            }
//        }
//    }
//}




using BAL;
using BEL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Chat_Engine_Project
{
    public partial class ConversationScreen : Form
    {
        private User loggedInUser; // The logged-in user
        private List<User> allUsers; // List of all users
        private Dictionary<string, List<string>> chatHistory = new Dictionary<string, List<string>>();
        private Operations operations = new Operations();

        public ConversationScreen(User loggedUser, List<User> allUsers)
        {
            InitializeComponent();

            this.loggedInUser = loggedUser;
            this.allUsers = allUsers;

            // Set welcome label
            label1.Text = $"Welcome, {loggedInUser.username}!";

            // Populate the combo box with all users
            comboBoxUsers.Items.Clear();
            foreach (var user in allUsers)
            {
                if (user.userID != loggedInUser.userID) // Exclude the logged-in user
                {
                    comboBoxUsers.Items.Add(user.username);
                }
            }

            // Load existing chats from the database
            LoadExistingChats();
        }

        private void LoadExistingChats()
        {
            var chats = operations.LoadExistingChats(loggedInUser.userID);
            foreach (var (conversationID, username) in chats)
            {
                AddChatButton(username, conversationID);
            }
        }

        private void btnChat_Click(object sender, EventArgs e)
        {
            if (comboBoxUsers.SelectedItem != null)
            {
                string selectedUsername = comboBoxUsers.SelectedItem.ToString();
                string conversationID = Guid.NewGuid().ToString(); // Generate a new unique conversation ID

                // Check if the chat button already exists
                if (FLPChatPanel.Controls.Find($"btn_{selectedUsername}", false).Length == 0)
                {
                    AddChatButton(selectedUsername, conversationID);
                    operations.AddConversationToDatabase(conversationID, loggedInUser.userID, selectedUsername);
                }
                else
                {
                    MessageBox.Show($"Chat with {selectedUsername} already exists!");
                }
            }
            else
            {
                MessageBox.Show("Please select a user to start a chat.");
            }
        }

        private void AddChatButton(string username, string conversationID)
        {
            Button userButton = new Button
            {
                Name = $"btn_{username}",
                Text = username,
                Tag = conversationID,
                Width = 200,
                Height = 30
            };
            userButton.Click += (s, ev) => SwitchChat(username, conversationID);
            FLPChatPanel.Controls.Add(userButton);
        }

        private void SwitchChat(string username, string conversationID)
        {
            // Clear current chat display
            FLPChat.Controls.Clear();

            // Update label for current chat
            lblChatWith.Text = $"Chatting with: {username}";

            // Load messages for the selected conversation
            var messages = operations.GetMessagesForConversation(conversationID);
            if (!chatHistory.ContainsKey(conversationID))
            {
                chatHistory[conversationID] = new List<string>();
            }

            foreach (var message in messages)
            {
                chatHistory[conversationID].Add(message);
                Label messageLabel = new Label
                {
                    Text = message,
                    AutoSize = true
                };
                FLPChat.Controls.Add(messageLabel);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            // Find the currently active chat button
            var selectedButton = FLPChatPanel.Controls
                .OfType<Button>()
                .FirstOrDefault(btn => btn.BackColor == Color.LightBlue); // Assuming you use this color to indicate active chat

            if (selectedButton != null)
            {
                string conversationID = selectedButton.Tag.ToString();
                string message = txtBoxMessage.Text.Trim();

                if (!string.IsNullOrEmpty(message))
                {
                    // Add message to the database
                    operations.AddMessageToDatabase(conversationID, loggedInUser.userID.ToString(), message);

                    // Add the message to the UI
                    chatHistory[conversationID].Add($"You: {message}");
                    Label messageLabel = new Label
                    {
                        Text = $"You: {message}",
                        AutoSize = true
                    };
                    FLPChat.Controls.Add(messageLabel);

                    // Clear the message box
                    txtBoxMessage.Clear();
                }
                else
                {
                    MessageBox.Show("Please enter a message to send.");
                }
            }
            else
            {
                MessageBox.Show("Please select a chat to send a message.");
            }
        }
    }
}

