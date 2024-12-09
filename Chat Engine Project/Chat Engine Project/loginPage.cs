using BAL;
using BEL;
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


        private async void btnLogin_Click(object sender, EventArgs e)
        {
            // Disable the button to prevent multiple clicks
            btnLogin.Enabled = false;

            try
            {
                User user = new User
                {
                    username = txtUsername.Text,
                    password = txtPassword.Text
                };

                // Fetch logged-in user and all users
                var (loggedUser, allUsers) = await Task.Run(() => operations.CheckLoginAndFetchUsers(user));

                if (loggedUser.userID != 0)
                {
                    ConversationScreen conversationScreen = new ConversationScreen(loggedUser, allUsers);
                    conversationScreen.Show();
                }
                else
                {
                    MessageBox.Show("Username and/or password invalid!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                // Re-enable the button after the operation is complete
                btnLogin.Enabled = true;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Disable the link to prevent multiple clicks
            linkLabel1.Enabled = false;

            try
            {
                RegisterPage register = new RegisterPage();
                register.Show();
            }
            finally
            {
                // Re-enable the link
                linkLabel1.Enabled = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            // Disable the button to prevent multiple clicks
            btnExit.Enabled = false;

            try
            {
                this.Close();
            }
            finally
            {
                btnExit.Enabled = true;
            }
        }

    }
}