using BAL;
using BEL;
using ChatEngine;
using System.Buffers;

namespace Chat_Engine_Project
{
    public partial class loginPage : Form
    {
        Operations operations = new Operations();

        public loginPage()
        {
            InitializeComponent();
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegisterPage register = new RegisterPage();
            register.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            User user = new User
            {
                username = txtUsername.Text,
                password = txtPassword.Text
            };

            // Fetch logged-in user and all users in one operation
            var (loggedUser, allUsers) = operations.CheckLoginAndFetchUsers(user);

            if (loggedUser != null)
            {
                // Pass the logged user and all users to ConversationScreen
                ConversationScreen conversationScreen = new ConversationScreen(loggedUser, allUsers);
                conversationScreen.Show();
            }
            else
            {
                MessageBox.Show("Username and/or password invalid!");
            }
        }
    }
}