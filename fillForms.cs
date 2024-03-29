﻿using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace VenueManagement
{
    public partial class fillForms : Form
    {
        // Singleton instance
        private static fillForms instance = null;

        MySqlConnection con = new MySqlConnection(
            "datasource=localhost;port=3306;username=root;password=;database=venue_ms"
        );
        MySqlCommand cmd;
        MySqlDataAdapter adapt;
        MySqlDataReader dr;

        private string txtIdValue;
        private string txtFnameValue;
        private string txtLnameValue;
        private string actEventValue;
        private string purposeValue;
        private string cmbVenueValue;
        private string depCmbValue;
        private string txtContactValue;
        private string startingDateValue;
        private string endDateValue;
        private string startingTimeValue;
        private string endTimeValue;

        private DateTime? lastSetDate = null;

        public TextBox StartingDateTextBox
        {
            get { return startingdate; } // 'startingdate' should be the name of your TextBox
        }

        // Private constructor
        private fillForms()
        {
            InitializeComponent();
            DisplayData();

            Binddep();

            button2.Visible = false;

            UpdateStoredValues();

            // Attach the CloseUp event handler
            enddate.CloseUp += new EventHandler(enddate_CloseUp);

            txtcontact.MaxLength = 11;
        }

        // Public static property to get the instance
        public static fillForms Instance
        {
            get
            {
                if (instance == null || instance.IsDisposed)
                {
                    instance = new fillForms();
                }
                return instance;
            }
        }

        public void SetDate(DateTime? selectedDate)
        {
            // Check if selectedDate is null
            if (!selectedDate.HasValue)
            {
                Console.WriteLine("SetDate called with null date.");
                return;
            }

            // Check if SetDate has already been called with the same date
            if (lastSetDate.HasValue && lastSetDate.Value.Date == selectedDate.Value.Date)
            {
                return;
            }

            Console.WriteLine($"SetDate called with: {selectedDate.Value}");

            // Convert the date to a string in the format you want
            string dateString = selectedDate.Value.ToString("dd-MM-yyyy");

            Console.WriteLine($"SetDate executed with: {startingdate.Text}");

            // Set the Text property of the TextBox
            startingdate.Text = dateString;

            Console.WriteLine($"startingdate.Text after setting: {startingdate.Text}");

            // Call BindData
            BindData();

            // Update the lastSetDate
            lastSetDate = selectedDate;
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nWidthEllipse,
            int nHeigthEllipse,
            int v
        );

        private void textBox1_TextChanged(object sender, EventArgs e) { }

        private void fillForms_Load(object sender, EventArgs e)
        {
            panel1.Location = new Point(
                this.ClientSize.Width / 2 - panel1.Size.Width / 2,
                this.ClientSize.Height / 2 - panel1.Size.Height / 2
            );
            panel1.Anchor = AnchorStyles.None;

            panel1.Region = Region.FromHrgn(
                CreateRoundRectRgn(0, 0, panel1.Width, panel1.Height, 30, 30)
            );

            // Assuming UserControlDays.static_day, reservation.static_month, and reservation.static_year are strings
            if (int.TryParse(UserControlDays.static_day, out int day))
            {
                int month = reservation.static_month;
                int year = reservation.static_year;

                // Create a DateTime object with the given values
                DateTime date = new DateTime(year, month, day);

                // Format the date as "dd/MM/yyyy" and set it to the startingdate.Text property
                startingdate.Text = date.ToString("dd-MM-yyyy");
            }
            else
            {
                // Handle the case where UserControlDays.static_day is not a valid integer
                // You might want to show an error message or take appropriate action
            }
        }

        private void startingdate_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine($"startingdate_TextChanged called with: {startingdate.Text}");
        }

        private void enddate_ValueChanged(object sender, EventArgs e)
        {
            if (DateTime.TryParseExact(startingdate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate))
            {
                if (enddate.Value.Date < startDate.Date)
                {
                    MessageBox.Show("End date cannot be earlier than start date.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    enddate.Value = startDate;
                }
                else
                {
                    endDateValue = enddate.Value.ToString("dd-MM-yyyy"); // Update the endDateValue
                }
            }
            else
            {
                MessageBox.Show("Start date is not in the correct format. Please enter a valid date.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void enddate_CloseUp(object sender, EventArgs e)
        {
            if (DateTime.TryParseExact(startingdate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate))
            {
                if (enddate.Value.Date < startDate.Date)
                {
                    MessageBox.Show("End date cannot be earlier than start date.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    enddate.Value = startDate;
                }
                else
                {
                    endDateValue = enddate.Value.ToString("dd-MM-yyyy"); // Update the endDateValue
                }
            }
            else
            {
                MessageBox.Show("Start date is not in the correct format. Please enter a valid date.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            adminForm adminForm = new adminForm();
            adminForm.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Check if all necessary fields are filled
            if (
                txtfname.Text != ""
                && txtlname.Text != ""
                && actevent.Text != ""
                && purpose.Text != ""
                && cmbvenue.Text != ""
                && depcmb.Text != ""
                && txtcontact.Text != ""
                && startingdate.Text != ""
                && enddate.Text != ""
                && startingtime.Text != ""
                && endtime.Text != ""
            )

            {
                // Add validation for Philippine phone number
                Regex phoneNumpattern = new Regex(@"^09\d{9}$");
                if (!phoneNumpattern.IsMatch(txtcontact.Text))
                {
                    MessageBox.Show(
                        "Invalid phone number. Please enter a valid Philippine phone number starting with 09 and consisting of 11 numbers.",
                        "ERROR",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                // Convert start_time and end_time to 24-hour format
                DateTime startTime,
                    endTime;
                if (
                    !DateTime.TryParseExact(
                        startingtime.Text,
                        "h:mm:ss tt",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out startTime
                    )
                    || !DateTime.TryParseExact(
                        endtime.Text,
                        "h:mm:ss tt",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out endTime
                    )
                )
                {
                    // Handle the case where startingtime.Text or endtime.Text is not in the "h:mm:ss tt" format
                    MessageBox.Show(
                        "Starting time or end time is not in the correct format. Please enter the time in the format 'h:mm:ss AM/PM'.",
                        "ERROR",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                // Add validation for time range
                TimeSpan start = new TimeSpan(8, 0, 0); // 8 AM
                TimeSpan end = new TimeSpan(17, 0, 0); // 5 PM
                TimeSpan currentTimeStart = startTime.TimeOfDay;
                TimeSpan currentTimeEnd = endTime.TimeOfDay;

                if (currentTimeStart < start || currentTimeEnd > end || currentTimeEnd < currentTimeStart)
                {
                    MessageBox.Show(
                        "Invalid time. Please enter a time between 8:00 AM and 5:00 PM.",
                        "ERROR",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                // Create a new MySqlCommand to check if a reservation already exists with the same venue, date, and time
                MySqlCommand cmdCheck = new MySqlCommand(
                    "SELECT * FROM venue_ms.re_venue WHERE venue = @venue AND ((start_date <= @start_date AND end_date >= @start_date) OR (start_date <= @end_date AND end_date >= @end_date) OR (start_date >= @start_date AND end_date <= @end_date)) AND ((TIME(start_time) <= TIME(@start_time) AND TIME(end_time) > TIME(@start_time)) OR (TIME(start_time) < TIME(@end_time) AND TIME(end_time) >= TIME(@end_time)) OR (TIME(start_time) >= TIME(@start_time) AND TIME(end_time) <= TIME(@end_time)) OR (TIME(start_time) = TIME(@start_time) AND TIME(end_time) > TIME(@end_time)) OR (TIME(start_time) < TIME(@start_time) AND TIME(end_time) = TIME(@end_time)))",
                    con
                );
                cmdCheck.Parameters.AddWithValue("@venue", cmbvenue.Text);
                // parse the startingdate.Text to DateTime and format it as "dd-MM-yyyy"
                DateTime startDate;
                if (
                    !DateTime.TryParseExact(
                        startingdate.Text,
                        "dd-MM-yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out startDate
                    )
                )
                {
                    // Handle the case where startingdate.Text is not in the "dd-MM-yyyy" format
                    MessageBox.Show(
                        "Starting date is not in the correct format. Please enter the date in the format 'dd-MM-yyyy'.",
                        "ERROR",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                // Convert enddate to DateTime
                DateTime endDate = enddate.Value;

                // Check if enddate is earlier than startingdate
                if (endDate.Date < startDate.Date)
                {
                    MessageBox.Show("End date cannot be earlier than start date.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                cmdCheck.Parameters.AddWithValue("@start_date", startDate.ToString("dd-MM-yyyy"));
                cmdCheck.Parameters.AddWithValue("@end_date", endDate.ToString("dd-MM-yyyy"));

                cmdCheck.Parameters.AddWithValue("@start_time", startTime.ToString("h:mm:ss tt"));
                cmdCheck.Parameters.AddWithValue("@end_time", endTime.ToString("h:mm:ss tt"));

                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlDataReader drCheck;
                using (drCheck = cmdCheck.ExecuteReader())
                {
                    if (drCheck.HasRows)
                    {
                        // If a reservation already exists, show an error message and close the connection
                        MessageBox.Show(
                            "A reservation already exists with the same venue, date, and time.",
                            "ERROR",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        con.Close();
                        return;
                    }
                }
                drCheck.Close();

                // Create a new MySqlCommand to count the number of reservations for the selected venue on the selected date
                MySqlCommand cmdCount = new MySqlCommand(
                    "SELECT COUNT(*) FROM venue_ms.re_venue WHERE venue = @venue AND start_date = @start_date",
                    con
                );
                cmdCount.Parameters.AddWithValue("@venue", cmbvenue.Text);
                cmdCount.Parameters.AddWithValue("@start_date", startDate.ToString("yyyy-MM-dd"));

                int reservationCount;
                using (var drCount = cmdCount.ExecuteReader())
                {
                    if (drCount.Read())
                    {
                        reservationCount = drCount.GetInt32(0);
                    }
                    else
                    {
                        // Handle the case where the query did not return a result
                        MessageBox.Show(
                            "An error occurred while checking the number of reservations.",
                            "ERROR",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        return;
                    }
                }

                if (reservationCount >= 2)
                {
                    // If the venue has already been reserved twice on the selected date, show an error message and return
                    MessageBox.Show(
                        "This venue has already been reserved twice on the selected date. Please select a different venue or date.",
                        "ERROR",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                // If the venue has been reserved less than twice on the selected date, proceed with inserting the new reservation

                // If no reservation exists, insert the new record
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    cmd = new MySqlCommand(
                        "insert into venue_ms.re_venue (id,lname,fname,act_event,nature_event,venue,department,contact,start_date,end_date,start_time,end_time) values (@id,@lname,@fname,@act_event,@nature_event,@venue,@department,@contact,@start_date,@end_date,@start_time,@end_time)",
                        con
                    );
                    cmd.Parameters.AddWithValue("@id", txtid.Text);
                    cmd.Parameters.AddWithValue("@lname", txtlname.Text);
                    cmd.Parameters.AddWithValue("@fname", txtfname.Text);
                    cmd.Parameters.AddWithValue("@act_event", actevent.Text);
                    cmd.Parameters.AddWithValue("@nature_event", purpose.Text);
                    cmd.Parameters.AddWithValue("@venue", cmbvenue.Text);
                    cmd.Parameters.AddWithValue("@department", depcmb.Text);
                    cmd.Parameters.AddWithValue("@contact", txtcontact.Text);
                    cmd.Parameters.AddWithValue("@start_date", date.ToString("dd-MM-yyyy"));
                    cmd.Parameters.AddWithValue("@end_date", date.ToString("dd-MM-yyyy"));
                    cmd.Parameters.AddWithValue("@start_time", startTime.ToString("h:mm:ss tt"));
                    cmd.Parameters.AddWithValue("@end_time", endTime.ToString("h:mm:ss tt"));
                    cmd.ExecuteNonQuery();
                }
                con.Close();
                MessageBox.Show(
                    "Event Successfully Added",
                    "INSERT",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                DisplayData();
                ClearData();
            }
            else
            {
                MessageBox.Show(
                    "Fill out all the information needed",
                    "ERROR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void DisplayData()
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new MySqlDataAdapter("select * from venue_ms.re_venue", con);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void ClearData()
        {
            txtid.Text = "";
            txtfname.Text = "";
            txtlname.Text = "";
            actevent.Text = "";
            purpose.Text = "";
            cmbvenue.Text = "";
            depcmb.Text = "";
            txtcontact.Text = "";
            // startingdate.Text = "";
            enddate.Text = "";
            startingtime.Text = "";
            endtime.Text = "";
        }

        private void BindData()
        {
            Console.WriteLine($"BindData called with: {startingdate.Text}");

            if (string.IsNullOrEmpty(startingdate.Text))
            {
                MessageBox.Show("Please enter a date.", "No Date", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if the connection is not already open
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            // Try to convert the date to the correct format
            DateTime date;
            bool isDateParsed = DateTime.TryParseExact(startingdate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

            Console.WriteLine($"Date parsing successful: {isDateParsed}");

            if (!isDateParsed)
            {
                MessageBox.Show("Please enter a valid date.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string formattedDate = date.ToString("dd-MM-yyyy");

            string query = @"
                SELECT venue 
                FROM venue_table 
                WHERE venue NOT IN (
                    SELECT venue 
                    FROM re_venue 
                    WHERE start_date = @date
                    GROUP BY venue
                    HAVING COUNT(*) >= 2
                )
            ";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@date", formattedDate);

            Console.WriteLine($"Executing query: {query} with date: {formattedDate}");

            try
            {
                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    Console.WriteLine("Query executed successfully.");
                }
                else
                {
                    Console.WriteLine("No venues available for this date.");
                }

                cmbvenue.Items.Clear();
                while (dr.Read())
                {
                    cmbvenue.Items.Add(dr[0].ToString());
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (
                txtid.Text != ""
                && txtfname.Text != ""
                && txtlname.Text != ""
                && actevent.Text != ""
                && purpose.Text != ""
                && cmbvenue.Text != ""
                && depcmb.Text != ""
                && txtcontact.Text != ""
                && startingdate.Text != ""
                && enddate.Text != ""
                && startingtime.Text != ""
                && endtime.Text != ""
            )
            {
                cmd = new MySqlCommand("DELETE FROM venue_ms.re_venue WHERE id = @id", con);
                con.Open();
                cmd.Parameters.AddWithValue("@id", txtid.Text);

                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();

                if (rowsAffected > 0)
                {
                    MessageBox.Show(
                        "Record Successfully Deleted",
                        "DELETE",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    // Toggle button visibility after successful delete
                    button1.Visible = true; // Save button
                    button2.Visible = false; // Update button
                }
                else
                {
                    MessageBox.Show(
                        "No record found with the specified ID.",
                        "ERROR",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }

                DisplayData();
                ClearData();
            }
            else
            {
                MessageBox.Show(
                    "Select the record you want to Delete",
                    "ERROR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

        }

        private void startingdate_TextChanged_1(object sender, EventArgs e)
        {
            Console.WriteLine($"startingdate_TextChanged_1 called with: {startingdate.Text}");
        }

        private void startingdate_MouseClick(object sender, MouseEventArgs e)
        {
            // Create an instance of the reservation form
            reservation reservationForm = new reservation();

            // Set the owner of the reservation form to the current form
            reservationForm.Owner = this;

            // Access the PictureBox4 and PicBack properties from the reservation form
            PictureBox pictureBox4 = reservationForm.PictureBox4;
            PictureBox picBack = reservationForm.PicBack;

            // Set the visibility of PictureBox4 to false
            pictureBox4.Visible = false;

            // Set the visibility of PicBack to true
            picBack.Visible = true;

            // Wire up an event handler for the PicBack click event
            picBack.Click += (s, ev) => NavigateBack();

            // Show the reservation form
            reservationForm.Show();

            // Hide the current form
            this.Hide();
        }

        // New method to navigate back to the fillForms form
        private void NavigateBack()
        {
            // Show the current form
            this.Show();

            // Update stored values
            UpdateStoredValues();
        }

        private void UpdateStoredValues()
        {
            txtIdValue = txtid.Text;
            txtFnameValue = txtfname.Text;
            txtLnameValue = txtlname.Text;
            actEventValue = actevent.Text;
            purposeValue = purpose.Text;
            cmbVenueValue = cmbvenue.Text;
            depCmbValue = depcmb.Text;
            txtContactValue = txtcontact.Text;
            startingDateValue = startingdate.Text;
            endDateValue = enddate.Text;
            startingTimeValue = startingtime.Text;
            endTimeValue = endtime.Text;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a row is selected
            if (e.RowIndex >= 0)
            {
                // Populate data from the selected row to your form fields
                txtid.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtfname.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtlname.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                actevent.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                purpose.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                cmbvenue.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                depcmb.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                txtcontact.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();

                if (
                DateTime.TryParseExact(
                dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString(),
                "dd-MM-yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime startDate
                    )
                )
                {
                    startingdate.Text = startDate.ToString("dd-MM-yyyy");
                }
                else
                {
                    MessageBox.Show(
                        "Start date is not in the correct format. Please enter a valid date.",
                        "ERROR",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                if (
                    DateTime.TryParseExact(
                        dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString(),
                        "dd-MM-yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime endDate
                    )
                )
                {
                    enddate.Format = DateTimePickerFormat.Custom;
                    enddate.CustomFormat = "dd-MM-yyyy";
                    enddate.Value = endDate;
                }
                else
                {
                    MessageBox.Show(
                        "End date is not in the correct format. Please enter a valid date.",
                        "ERROR",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                startingtime.Text = dataGridView1.Rows[e.RowIndex].Cells[10].Value.ToString();
                endtime.Text = dataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString();

                // Hide the "Save" button
                button1.Visible = false;

                // Show the "Update" button
                button2.Visible = true;
            }
            else
            {
                // If no row is selected, show the "Save" button and hide the "Update" button
                button1.Visible = true;
                button2.Visible = false;
            }
        }

        private void Binddep()
        {
            // Check if the connection is not already open
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            MySqlCommand cmd = new MySqlCommand("select * from venue_ms.department ", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                depcmb.Items.Add(dr[1].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (
                txtlname.Text != ""
                && txtfname.Text != ""
                && actevent.Text != ""
                && purpose.Text != ""
                && cmbvenue.Text != ""
                && depcmb.Text != ""
                && txtcontact.Text != ""
                && startingtime.Text != ""
                && endtime.Text != ""
            )
            {
                // Add validation for Philippine phone number
                Regex phoneNumpattern = new Regex(@"^09\d{9}$");
                if (!phoneNumpattern.IsMatch(txtcontact.Text))
                {
                    MessageBox.Show(
                        "Invalid phone number. Please enter a valid Philippine phone number starting with 09 and consisting of 11 numbers.",
                        "ERROR",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                // Convert start_time and end_time to 24-hour format
                DateTime startTime, endTime;
                if (
                    !DateTime.TryParseExact(
                        startingtime.Text,
                        "h:mm:ss tt",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out startTime
                    )
                    || !DateTime.TryParseExact(
                        endtime.Text,
                        "h:mm:ss tt",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out endTime
                    )
                )
                {
                    // Handle the case where startingtime.Text or endtime.Text is not in the "h:mm:ss tt" format
                    MessageBox.Show(
                        "Starting time or end time is not in the correct format. Please enter the time in the format 'h:mm:ss AM/PM'.",
                        "ERROR",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                // Add validation for time range
                TimeSpan start = new TimeSpan(8, 0, 0); // 8 AM
                TimeSpan end = new TimeSpan(17, 0, 0); // 5 PM
                TimeSpan currentTimeStart = startTime.TimeOfDay;
                TimeSpan currentTimeEnd = endTime.TimeOfDay;

                if (currentTimeStart < start || currentTimeEnd > end || currentTimeEnd < currentTimeStart)
                {
                    MessageBox.Show(
                        "Invalid time. Please enter a time between 8:00 AM and 5:00 PM.",
                        "ERROR",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                MySqlCommand cmdCheck = new MySqlCommand(
                    "SELECT * FROM venue_ms.re_venue WHERE id != @id AND venue = @venue AND ((start_date <= @start_date AND end_date >= @start_date) OR (start_date <= @end_date AND end_date >= @end_date) OR (start_date >= @start_date AND end_date <= @end_date)) AND ((TIME(start_time) <= TIME(@start_time) AND TIME(end_time) > TIME(@start_time)) OR (TIME(start_time) < TIME(@end_time) AND TIME(end_time) >= TIME(@end_time)) OR (TIME(start_time) >= TIME(@start_time) AND TIME(end_time) <= TIME(@end_time)) OR (TIME(start_time) <= TIME(@start_time) AND TIME(end_time) >= TIME(@end_time)))",
                    con
                );
                cmdCheck.Parameters.AddWithValue("@id", txtid.Text);
                cmdCheck.Parameters.AddWithValue("@venue", cmbvenue.Text);

                DateTime startDate;
                string dateFormat = "dd-MM-yyyy";

                // Validate the startingdate.Text format before parsing
                if (
                    !DateTime.TryParseExact(
                        startingdate.Text,
                        dateFormat,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out startDate
                    )
                )
                {
                    MessageBox.Show(
                        $"Starting date is not in the correct format. Please enter the date in the format '{dateFormat}'.",
                        "ERROR",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }
                // Get the end date from the DateTimePicker control
                DateTime endDate = enddate.Value;

                cmdCheck.Parameters.AddWithValue("@start_date", startDate.ToString("dd-MM-yyyy"));
                cmdCheck.Parameters.AddWithValue("@end_date", endDate.ToString("dd-MM-yyyy"));
                cmdCheck.Parameters.AddWithValue("@start_time", startingtime.Text);
                cmdCheck.Parameters.AddWithValue("@end_time", endtime.Text);

                MySqlDataReader drCheck;
                using (drCheck = cmdCheck.ExecuteReader())
                {
                    if (drCheck.HasRows)
                    {
                        MessageBox.Show(
                            "A reservation already exists with the same venue, date, and time.",
                            "ERROR",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        con.Close();
                        return;
                    }
                }
                drCheck.Close();

                cmd = new MySqlCommand(
                    "UPDATE venue_ms.re_venue SET lname = @lname, fname = @fname, act_event = @act_event, nature_event = @nature_event, venue = @venue, department = @department, contact = @contact, start_date = @start_date, end_date = @end_date, start_time = @start_time, end_time = @end_time WHERE id = @id",
                    con
                );
                cmd.Parameters.AddWithValue("@id", txtid.Text);
                cmd.Parameters.AddWithValue("@lname", txtlname.Text);
                cmd.Parameters.AddWithValue("@fname", txtfname.Text);
                cmd.Parameters.AddWithValue("@act_event", actevent.Text);
                cmd.Parameters.AddWithValue("@nature_event", purpose.Text);
                cmd.Parameters.AddWithValue("@venue", cmbvenue.Text);
                cmd.Parameters.AddWithValue("@department", depcmb.Text);
                cmd.Parameters.AddWithValue("@contact", txtcontact.Text);
                cmd.Parameters.AddWithValue("@start_date", startDate.ToString("dd-MM-yyyy"));
                cmd.Parameters.AddWithValue("@end_date", endDate.ToString("dd-MM-yyyy"));
                cmd.Parameters.AddWithValue("@start_time", startingtime.Text);
                cmd.Parameters.AddWithValue("@end_time", endtime.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show(
                    "Record Successfully Updated",
                    "UPDATE",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                con.Close();
                DisplayData();
                ClearData();

                // Set the visibility of the "Save" button to true after successful update
                button1.Visible = true; // Set the visibility of the "Update" button to false after successful update
                button2.Visible = false;
            }
            else
            {
                MessageBox.Show(
                    "Select the record you want to Update",
                    "ERROR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void enddate_ValueChanged_1(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtcontact_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtcontact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }
    }
}
