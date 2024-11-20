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
    public partial class ConversationScreen: Form
    {
        User user = new User();

        public ConversationScreen()
        {
            InitializeComponent();
        }
        public ConversationScreen(User user)
        {
            InitializeComponent();

            this.user = user;
            label1.Text = user.username.ToString();

        }
    }
}
