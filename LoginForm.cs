using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VenueManagement
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            if (txtuser.Text == "admin" && txtpass.Text == "admin12345")
            {
                new adminForm().Show();
                this.Hide();
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if(txtpass.PasswordChar=='*')
            {
                pictureBox5.BringToFront();
                txtpass.PasswordChar = '\0';
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (txtpass.PasswordChar == '\0')
            {
                pictureBox4.BringToFront();
                txtpass.PasswordChar = '*';
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void txtuser_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
