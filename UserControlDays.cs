using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace VenueManagement
{
    public partial class UserControlDays : UserControl
    {
        MySqlConnection con = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;");

        // lets us create another static variable for days
        public static string static_day;

        public UserControlDays()
        {
            InitializeComponent();
        }

        private void UserControlDays_Load(object sender, EventArgs e)
        {
            
        }
        public void days(int numday)
        {
            lbldays.Text = numday + "";
        }

        private void UserControlDays_Click(object sender, EventArgs e)
        {
            // Assuming lbldays.Text is the selected date
            string selectedDate = lbldays.Text;

            // Check if the selected date is already reserved
            if (IsDateReserved(selectedDate))
            {
                MessageBox.Show("Event not available!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // If the date is not reserved, proceed to the fillForms form
                static_day = selectedDate;
                fillForms fillForms = new fillForms();
                fillForms.StartingDateTextBox.Text = selectedDate;
                fillForms.Show();
                this.Hide();
            }
        }

        // Function to check if the date is reserved in the database
        private bool IsDateReserved(string selectedDate)
        {
            try
            {
                con.Open();

                // Use parameterized query to avoid SQL injection
                MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM venue_ms.re_venue WHERE start_date = @start_date", con);
                cmd1.Parameters.AddWithValue("@start_date", selectedDate);

                using (var dr1 = cmd1.ExecuteReader())
                {
                    return dr1.HasRows;
                }
            }
            
            finally
            {
                con.Close();
            }
        }


    }
}
