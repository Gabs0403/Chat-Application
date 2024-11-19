using BAL;
using BEL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat_Engine_Project
{
    public partial class RegisterPage : Form
    {
        public Operations Operations = new Operations();

        public RegisterPage()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            


            User user = new User
            {
                username = txtUsername.Text,
                password = txtPassword.Text,
                email = txtEmail.Text
            };

            

            if (Operations.addUser(user)) { 
                this.Close();
            }
            else
            {
                MessageBox.Show("erro");
            }

        }
    }
}
