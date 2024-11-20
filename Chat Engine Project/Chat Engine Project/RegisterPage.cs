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

            if ((Operations.isValidPassword(txtPassword.Text, txtConfirmPassword.Text)))
            {
                int rows = Operations.addUser(user);

                if (rows != -1)
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Username already exists!");
                }

            }
            else
            {
                string message = $"The password must have:\n" +
                    $"- At least 2 letters\n" +
                    $"- At least one uppercase letter\n" +
                    $"- At least one number\n" +
                    $"- At least eight characters";

                MessageBox.Show(message);
            }



        }

        private void txtConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtConfirmPassword.Text == txtPassword.Text)
            {
                lblShowPassword.Visible = false;
            }
            else
            {
                lblShowPassword.Visible = true;
            }
        }
    }
}
