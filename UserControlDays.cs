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
            // Get the current year and month
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            // Parse the day number from lbldays.Text into an integer
            int day;
            if (!int.TryParse(lbldays.Text, out day))
            {
                MessageBox.Show("Invalid day number!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Construct a DateTime object from the year, month, and day
            DateTime selectedDateTime = new DateTime(year, month, day);

            // Check if the selected date is in the past
            if (selectedDateTime.Date < DateTime.Today)
            {
                MessageBox.Show("Past dates cannot be selected!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Proceed to the fillForms form
            static_day = selectedDateTime.ToString("dd/MM/yyyy");
            fillForms fillForms = new fillForms();
            fillForms.StartingDateTextBox.Text = static_day;
            fillForms.Show();
            this.Hide();
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
