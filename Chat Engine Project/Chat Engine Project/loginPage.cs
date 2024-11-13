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
            User user = new User();

            user.username = txtUsername.Text;
            user.password = txtPassword.Text;

            if(operations.checkLogin(user))
            {
                // create new form
            }
            else
            {
                MessageBox.Show("Username and/or password invalid!");
            }

        }
    }
}
