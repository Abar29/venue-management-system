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
using System.Globalization;

namespace VenueManagement
{
    public partial class UserControlDays : UserControl
    {
        MySqlConnection con = new MySqlConnection(
            "datasource=localhost;port=3306;username=root;password=;"
        );

        // Add new properties for the selected year and month
        public int SelectedYear { get; private set; }
        public int SelectedMonth { get; private set; }

        // Add a new property to store the original color
        public Color OriginalColor { get; private set; }

        // Add a new property to store the original text color
        public Color OriginalTextColor { get; private set; }

        public UserControlDays(int selectedYear, int selectedMonth)
        {
            InitializeComponent();

            SelectedYear = selectedYear;
            SelectedMonth = selectedMonth;

            // Add event handlers for MouseEnter and MouseLeave events
            this.MouseEnter += new EventHandler(UserControlDays_MouseEnter);
            this.MouseLeave += new EventHandler(UserControlDays_MouseLeave);
        }

        // lets us create another static variable for days
        public static string static_day;

        private void UserControlDays_MouseEnter(object sender, EventArgs e)
        {
            // Darken the background color
            this.BackColor = ControlPaint.Dark(this.BackColor);

            // Change the text color to a darker color
            this.ForeColor = Color.White;
        }

        // Event handler for MouseLeave event
        private void UserControlDays_MouseLeave(object sender, EventArgs e)
        {
            // Restore the original color
            this.BackColor = OriginalColor;

            // Restore the original text color
            this.ForeColor = OriginalTextColor;
        }

        private void UserControlDays_Load(object sender, EventArgs e)
        {
            OriginalTextColor = this.ForeColor;

            UpdateDayColor();
        }

        public void days(int numday)
        {
            lbldays.Text = numday + "";
            UpdateDayColor();
        }

        private void UpdateDayColor()
        {
            int day;
            if (!int.TryParse(lbldays.Text, out day))
            {
                // Invalid day number, set color to default
                this.BackColor = Color.White;
                return;
            }

            // Get the selected year and month
            int year = SelectedYear;
            int month = SelectedMonth;

            // Construct a DateTime object from the year, month, and day
            DateTime selectedDateTime = new DateTime(year, month, day);

            // Check the number of reservations for the day
            int numReservations = GetNumReservations(selectedDateTime.ToString("dd-MM-yyyy"));

            if (numReservations == 0)
            {
                // Light Green - that day is available for reservation
                this.BackColor = Color.FromArgb(144, 238, 144); // LightGreen
            }
            else if (numReservations <= 2)
            {
                // Light Yellow - that day is available but only limited to the other venues
                this.BackColor = Color.FromArgb(255, 255, 174); // LightYellow
            }
            else
            {
                // Light Coral - that day is not available
                this.BackColor = Color.FromArgb(240, 128, 128); // LightCoral
            }

            // Store the original color
            OriginalColor = this.BackColor;
        }

        // Function to get the number of reservations for the date
        private int GetNumReservations(string selectedDate)
        {
            try
            {
                con.Open();

                // Use parameterized query to avoid SQL injection
                MySqlCommand cmd1 = new MySqlCommand(
                    "SELECT COUNT(*) FROM venue_ms.re_venue WHERE start_date = @start_date",
                    con
                );
                cmd1.Parameters.AddWithValue("@start_date", DateTime.ParseExact(selectedDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy"));

                // Print the SQL query with actual parameter values
                string queryWithParameters = cmd1.CommandText;
                foreach (MySqlParameter param in cmd1.Parameters)
                {
                    queryWithParameters = queryWithParameters.Replace(param.ParameterName, param.Value.ToString());
                }
                Console.WriteLine($"SQL query: {queryWithParameters}");

                using (var dr1 = cmd1.ExecuteReader())
                {
                    if (dr1.Read())
                    {
                        int numReservations = dr1.GetInt32(0);
                        Console.WriteLine($"Number of reservations for {selectedDate}: {numReservations}");
                        return numReservations;
                    }
                    else
                    {
                        Console.WriteLine($"No reservations found for {selectedDate}");
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur
                Console.WriteLine($"Error getting number of reservations for {selectedDate}: {ex.Message}");
                return 0;
            }
            finally
            {
                con.Close();
            }
        }

        private void UserControlDays_Click(object sender, EventArgs e)
        {
            // Use SelectedYear and SelectedMonth instead of the current year and month
            int year = SelectedYear;
            int month = SelectedMonth;

            // Parse the day number from lbldays.Text into an integer
            int day;
            if (!int.TryParse(lbldays.Text, out day))
            {
                MessageBox.Show(
                    "Invalid day number!",
                    "ERROR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            // Construct a DateTime object from the year, month, and day
            DateTime selectedDateTime = new DateTime(year, month, day);

            // Set UserControlDays.static_day, reservation.static_month, and reservation.static_year
            reservation.static_month = month;
            reservation.static_year = year;

            // Check if the selected date is in the past
            if (selectedDateTime.Date < DateTime.Today)
            {
                MessageBox.Show(
                    "Past dates cannot be selected!",
                    "ERROR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            // Proceed to the fillForms form
            static_day = selectedDateTime.ToString("dd/MM/yyyy");
            fillForms fillFormsInstance = fillForms.Instance;
            fillFormsInstance.SetDate(selectedDateTime);
            fillFormsInstance.Show();

            // Close the reservation form
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Close();
            }
        }

        // Function to check if the date is reserved in the database
        private bool IsDateReserved(string selectedDate)
        {
            try
            {
                con.Open();

                // Use parameterized query to avoid SQL injection
                MySqlCommand cmd1 = new MySqlCommand(
                    "SELECT * FROM venue_ms.re_venue WHERE start_date = @start_date",
                    con
                );
                cmd1.Parameters.AddWithValue("@start_date", DateTime.ParseExact(selectedDate, "dd-MM-yyyy", CultureInfo.InvariantCulture));

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
