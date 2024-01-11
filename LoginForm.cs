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
            // Check if the username field is empty
            if (string.IsNullOrEmpty(txtuser.Text))
            {
                MessageBox.Show("Please enter a username.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if the password field is empty
            if (string.IsNullOrEmpty(txtpass.Text))
            {
                MessageBox.Show("Please enter a password.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Existing login logic
            if (txtuser.Text == "admin" && txtpass.Text == "admin")
            {
                new adminForm().Show();
                this.Hide();
            }
            else
            {
                // If the credentials are incorrect, show an error message
                MessageBox.Show("Invalid username or password.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
