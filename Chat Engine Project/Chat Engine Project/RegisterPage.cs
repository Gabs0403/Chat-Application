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
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();
            string email = txtEmail.Text.Trim();

            // Check if passwords match
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match. Please try again.");
                return;
            }

            User user = new User
            {
                username = username,
                password = password,
                email = email
            };

            // Validate password
            if (Operations.isValidPassword(password, confirmPassword))
            {
                // Check if the username already exists
                if (!Operations.userExists(user.username))
                {
                    try
                    {
                        // Try adding the user
                        int rows = Operations.addUser(user);
                        if (rows > 0)
                        {
                            MessageBox.Show("Registration successful!");
                            this.Close(); 
                        }
                        else
                        {
                            MessageBox.Show("Error during registration. Please try again.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while registering: {ex.Message}");
                    }
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
                lblShowPassword.Visible = false; // Hide the warning label if passwords match
            }
            else
            {
                lblShowPassword.Visible = true; // Show the warning label if passwords don't match
            }
        }
    }
}