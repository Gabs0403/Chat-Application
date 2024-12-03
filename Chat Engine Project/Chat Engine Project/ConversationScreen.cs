
using BAL;
using BEL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

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
                AddChatButton(username, conversationID.ToString());
            }
        }

        private async void btnChat_Click(object sender, EventArgs e)
        {
            // Disable the chat button to prevent multiple clicks
            btnChat.Enabled = false;

            try
            {
                if (comboBoxUsers.SelectedItem != null)
                {
                    string selectedUsername = comboBoxUsers.SelectedItem.ToString();

                    // Check if the chat button already exists
                    if (FLPChatPanel.Controls.Find($"btn_{selectedUsername}", false).Length == 0)
                    {
                        // Perform database operation asynchronously
                        int conversationID = await Task.Run(() =>
                            operations.AddConversationToDatabase(loggedInUser.userID, selectedUsername));

                        if (conversationID != -1)
                        {
                            AddChatButton(selectedUsername, conversationID.ToString());
                        }
                        else
                        {
                            MessageBox.Show("Error: Could not create conversation.");
                        }
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
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                // Re-enable the chat button after the operation is complete
                btnChat.Enabled = true;
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
                Height = 30,
                BackColor = Color.White // Default color
            };

            userButton.Click += (s, ev) =>
            {
                // Switch chat and set this button as active
                SwitchChat(username, conversationID);

                // Update the BackColor for the active button
                SetActiveButton(userButton);
            };

            FLPChatPanel.Controls.Add(userButton);
        }

        private async void SwitchChat(string username, string conversationID)
        {
            // Disable chat-related UI controls during the switch
            btnChat.Enabled = false;
            comboBoxUsers.Enabled = false;

            try
            {
                // Clear current chat display
                FLPChat.Controls.Clear();

                // Update label for current chat
                lblChatWith.Text = $"Chatting with: {username}";

                // Load messages for the selected conversation asynchronously
                var messages = await Task.Run(() => operations.GetMessagesForConversation(conversationID));

                // Ensure chat history is tracked for this conversation
                if (!chatHistory.ContainsKey(conversationID))
                {
                    chatHistory[conversationID] = new List<string>();
                }



                // Display each message and add to chat history
                foreach (var (message, senderID) in messages)
                {
                    Label messageLabel = new Label
                    {
                        Text = senderID == loggedInUser.userID ? "You: " + message : username + ": " + message,
                        AutoSize = true,
                        Padding = new Padding(0, 10, 0, 10),
                        ForeColor = !(senderID == loggedInUser.userID) ? Color.Red : Color.Black
                    };


                    // Add the messagePanel to the FLPChat panel
                    FLPChat.Controls.Add(messageLabel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while switching chats: {ex.Message}");
            }
            finally
            {
                // Re-enable UI controls after the operation completes
                btnChat.Enabled = true;
                comboBoxUsers.Enabled = true;
                btnUpdate.Visible = true;
            }
        }





        private void SetActiveButton(Button activeButton)
        {
            // Reset the BackColor of all buttons in FLPChatPanel
            foreach (var control in FLPChatPanel.Controls.OfType<Button>())
            {
                control.BackColor = Color.White; // Default color
            }

            // Set the BackColor of the active button
            activeButton.BackColor = Color.LightBlue;
        }



        private async void btnSend_Click(object sender, EventArgs e)
        {
            // Find the currently active chat button
            var selectedButton = FLPChatPanel.Controls
                .OfType<Button>()
                .FirstOrDefault(btn => btn.BackColor == Color.LightBlue); // Assuming LightBlue indicates active chat

            if (selectedButton != null)
            {
                string conversationID = selectedButton.Tag.ToString();
                string message = txtBoxMessage.Text.Trim();

                if (!string.IsNullOrEmpty(message))
                {
                    // Temporarily disable the send button to prevent duplicate clicks
                    btnSend.Enabled = false;

                    try
                    {
                        // Add the message to the database asynchronously
                        await Task.Run(() =>
                            operations.AddMessageToDatabase(conversationID, loggedInUser.userID.ToString(), message));

                        // Add the message to the UI
                        //chatHistory[conversationID].Add($"You: {message}");
                        Label messageLabel = new Label
                        {
                            Text = $"You: {message}",
                            AutoSize = true,
                            Padding = new Padding(0, 10, 0, 10)
                        };
                        FLPChat.Controls.Add(messageLabel);

                        // Clear the message box
                        txtBoxMessage.Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while sending the message: {ex.Message}");
                    }
                    finally
                    {
                        // Re-enable the send button
                        btnSend.Enabled = true;
                        txtBoxMessage.Focus();
                    }
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

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            var selectedButton = FLPChatPanel.Controls
                .OfType<Button>()
                .FirstOrDefault(btn => btn.BackColor == Color.LightBlue);


            try
            {
                string conversationID = selectedButton.Tag.ToString();

                // Clear current chat display
                FLPChat.Controls.Clear();

                // Load messages for the selected conversation asynchronously
                var messages = await Task.Run(() => operations.GetMessagesForConversation(conversationID));

                // Ensure chat history is tracked for this conversation
                if (!chatHistory.ContainsKey(conversationID))
                {
                    chatHistory[conversationID] = new List<string>();
                }



                // Display each message and add to chat history
                foreach (var (message, senderID) in messages)
                {
                    Label messageLabel = new Label
                    {
                        Text = senderID == loggedInUser.userID ? "You: " + message : selectedButton.Text + ": " + message,
                        AutoSize = true,
                        Padding = new Padding(0, 10, 0, 10),
                        ForeColor = !(senderID == loggedInUser.userID) ? Color.Red : Color.Black
                    };


                    // Add the messagePanel to the FLPChat panel
                    FLPChat.Controls.Add(messageLabel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while switching chats: {ex.Message}");
            }
            finally
            {
                // Re-enable UI controls after the operation completes
                btnChat.Enabled = true;
                comboBoxUsers.Enabled = true;
            }
        }
    }
}