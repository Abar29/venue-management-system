using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VenueManagement
{
    public partial class reports_table : Form
    {
        MySqlConnection con = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;");

        MySqlDataAdapter adapt;
        private List<string> venueData;
        public reports_table()
        {
            InitializeComponent();
            venueData = Properties.Settings.Default.VenueData?.Cast<string>().ToList() ?? new List<string>();

            // Initialize DataGridView with stored data
            InitializeDataGridView();

            // Attach the CellClick event handler
            dataGridView1.CellClick += dataGridView1_CellClick;

            // Attach the CellFormatting event handler
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;

            // Display data from the database
            DisplayData();

        }

        private void DisplayData()
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new MySqlDataAdapter("select * from venue_ms.venue_table", con);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Check if the clicked cell is in the "viewReports" column
                if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["viewReports"].Index)
                {
                    // Handle the "View Reports" button click for the selected row
                    DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                    string venueValue = selectedRow.Cells["venue"].Value.ToString();
                    MessageBox.Show($"Viewing reports for venue: {venueValue}", "View Reports", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Show the reports form
                    ShowReportsForm(venueValue);

                    // Hide the reports_table form
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}\nStackTrace: {ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowReportsForm(string venueValue)
        {
            // Create an instance of your reports form
            reports reportsForm = new reports(venueValue);

            // Show the reports form
            reportsForm.Show();
        }

        // Initialize DataGridView with stored data
        private void InitializeDataGridView()
        {

            // Add the "viewReports" column if it doesn't exist
            if (!dataGridView1.Columns.Contains("viewReports"))
            {
                DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn
                {
                    Name = "viewReports",
                    UseColumnTextForButtonValue = true
                };

                dataGridView1.Columns.Add(buttonColumn);
            }

            // Add rows with venue data
            foreach (string venueValue in venueData)
            {
                // Add the value to the dataGridView1 in the "venue" column
                int rowIndex = dataGridView1.Rows.Add(venueValue);
            }
        }
        // Save venue data to settings
       
        private void reports_table_Load(object sender, EventArgs e)
        {
           
        }
        // Helper method to show panel2
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the formatting is for the "viewReports" column
            if (e.ColumnIndex == dataGridView1.Columns["viewReports"].Index && e.RowIndex >= 0)
            {
                // Set the button text for the "viewReports" column
                e.Value = "View Reports";
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

            new adminForm().Show();
            this.Hide();
        }
    }
}
