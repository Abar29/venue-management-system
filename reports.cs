using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace VenueManagement
{
    public partial class reports : Form
    {

        MySqlConnection con = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;");
        MySqlCommand cmd;
        MySqlDataAdapter adapt;
        private string selectedVenue;

        public reports(string venueValue)
        {
            InitializeComponent();
            // Set the selected venue data
            selectedVenue = venueValue;

            // Call a method to display the data in a label, for example
            DisplayVenueData();
            DisplayData();
        }
        private void DisplayVenueData()
        {
            // Assuming you have a label named lblVenue in your reports form
            lblVenue.Text = $"Venue: {selectedVenue}";

            // Add additional logic to display other relevant data
        }

        // Method to retrieve and display data from the database
        private void DisplayData()
        {
            con.Open();
            DataTable dt = new DataTable();

            // Modify the SQL query to include a WHERE clause for the selected venue
            string query = $"SELECT act_event, start_date, end_date FROM venue_ms.re_venue WHERE venue = '{selectedVenue}'";

            adapt = new MySqlDataAdapter(query, con);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;

            UpdateCountAndDays();

            con.Close();
        }

        private void UpdateCountAndDays()
        {
            try
            {
                if (cmbmonth.SelectedItem == null || cmbyear.SelectedItem == null)
                {
                    // Handle the case where one of the ComboBoxes is not selected
                    return;
                }

                int selectedMonth = cmbmonth.SelectedIndex + 1; // Adding 1 because months are 1-based
                int selectedYear = int.Parse(cmbyear.SelectedItem.ToString());

                string query;

                if (selectedMonth > 0 && selectedYear > 0)
                {
                    // If a valid month and year are selected, fetch records for that month and year
                                    query = $@"
                    SELECT act_event, 
                           STR_TO_DATE(start_date, '%d-%m-%Y') AS start_date, 
                           STR_TO_DATE(end_date, '%d-%m-%Y') AS end_date 
                    FROM venue_ms.re_venue 
                    WHERE venue = '{selectedVenue}' 
                      AND MONTH(STR_TO_DATE(start_date, '%d-%m-%Y')) = {selectedMonth} 
                      AND YEAR(STR_TO_DATE(start_date, '%d-%m-%Y')) = {selectedYear}";
                }
                else
                {
                    // If "No Records" is selected or no valid month and year are selected, show no records
                    query = $"SELECT NULL AS act_event, NULL AS start_date, NULL AS end_date, NULL AS start_time, NULL AS end_time FROM venue_ms.re_venue WHERE 1 = 0";
                }

                DataTable dt = new DataTable();
                adapt = new MySqlDataAdapter(query, con);
                adapt.Fill(dt);

                int numActivities = dt.Rows.Count;
                int numDays = 0;

                foreach (DataRow row in dt.Rows)
                {
                    if (row["start_date"] != DBNull.Value && row["end_date"] != DBNull.Value)
                    {
                        DateTime startDate = (DateTime)row["start_date"];
                        DateTime endDate = (DateTime)row["end_date"];

                        numDays += (int)(endDate - startDate).TotalDays + 1;
                    }
                }

                lblact.Text = $"Number of Activities: {numActivities}";
                lblnumofdays.Text = $"Number of Days: {numDays}";

                // Set the DataTable as the DataSource for the DataGridView
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                // Handle the exception as needed
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            new reports_table().Show();
            this.Hide();
        }

        private void reports_Load(object sender, EventArgs e)
        {
            // Assuming comboBox1 is the name of your comboBox
            for (int i = 1; i <= 12; i++)
            {
                cmbmonth.Items.Add(new DateTime(2000, i, 1).ToString("MMMM"));
            }

            // Assuming cmbyear is the name of your ComboBox for years
            int currentYear = DateTime.Now.Year;

            for (int i = currentYear; i >= currentYear - 10; i--)
            {
                cmbyear.Items.Add(i.ToString());
            }

            // Call DisplayData after ComboBox controls are initialized
            DisplayData();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCountAndDays();
        }

        private void cmbyear_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCountAndDays();
        }
    }


}