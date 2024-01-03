using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace VenueManagement
{

    public partial class fillForms : Form
    {
        MySqlConnection con = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;");
        MySqlCommand cmd;
        MySqlDataAdapter adapt;
        MySqlDataReader dr;


        public TextBox StartingDateTextBox
        {
            get { return startingdate; } // 'startingdate' should be the name of your TextBox
        }

        public object Startingdate { get; internal set; }

        public fillForms()
        {
            InitializeComponent();
            DisplayData();
            BindData();
            Binddep();



        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void fillForms_Load(object sender, EventArgs e)
        {
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

        }

        private void enddate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            adminForm adminForm = new adminForm();
            adminForm.Show();
            this.Hide();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Check if all necessary fields are filled
            if (txtfname.Text != "" && txtlname.Text != "" && actevent.Text != "" && purpose.Text != "" && cmbvenue.Text != "" && depcmb.Text != "" && txtcontact.Text != "" && startingdate.Text != "" && enddate.Text != "" && startingtime.Text != "" && endtime.Text != "")
            {

                // Convert start_time and end_time to 24-hour format
                DateTime startTime, endTime;
                if (!DateTime.TryParseExact(startingtime.Text, "h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out startTime) || !DateTime.TryParseExact(endtime.Text, "h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out endTime))
                {
                    // Handle the case where startingtime.Text or endtime.Text is not in the "h:mm:ss tt" format
                    MessageBox.Show("Starting time or end time is not in the correct format. Please enter the time in the format 'h:mm:ss AM/PM'.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Create a new MySqlCommand to check if a reservation already exists with the same venue, date, and time
                MySqlCommand cmdCheck = new MySqlCommand("SELECT * FROM venue_ms.re_venue WHERE venue = @venue AND ((start_date <= @start_date AND end_date >= @start_date) OR (start_date <= @end_date AND end_date >= @end_date) OR (start_date >= @start_date AND end_date <= @end_date)) AND ((TIME(start_time) <= TIME(@start_time) AND TIME(end_time) > TIME(@start_time)) OR (TIME(start_time) < TIME(@end_time) AND TIME(end_time) >= @end_time) OR (TIME(start_time) >= TIME(@start_time) AND TIME(end_time) <= @end_time))", con);
                cmdCheck.Parameters.AddWithValue("@venue", cmbvenue.Text);
                DateTime startDate;
                if (!DateTime.TryParseExact(startingdate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
                {
                    // Handle the case where startingdate.Text is not in the "dd/MM/yyyy" format
                    MessageBox.Show("Starting date is not in the correct format. Please enter the date in the format 'dd/MM/yyyy'.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                cmdCheck.Parameters.AddWithValue("@start_date", startDate.ToString("dd-MM-yyyy"));
                // Convert enddate to DateTime and format it as "dd-MM-yyyy"
                DateTime endDate = enddate.Value;
                cmdCheck.Parameters.AddWithValue("@end_date", endDate.ToString("dd-MM-yyyy"));

                cmdCheck.Parameters.AddWithValue("@start_time", startTime.ToString("h:mm:ss tt"));
                cmdCheck.Parameters.AddWithValue("@end_time", endTime.ToString("h:mm:ss tt"));

                con.Open();
                MySqlDataReader drCheck;
                using (drCheck = cmdCheck.ExecuteReader())
                {
                    if (drCheck.HasRows)
                    {
                        // If a reservation already exists, show an error message and close the connection
                        MessageBox.Show("A reservation already exists with the same venue, date, and time.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        con.Close();
                        return;
                    }
                }
                drCheck.Close();

                // If no reservation exists, insert the new record
                cmd = new MySqlCommand("insert into venue_ms.re_venue (id,lname,fname,act_event,nature_event,venue,department,contact,start_date,end_date,start_time,end_time) values (@id,@lname,@fname,@act_event,@nature_event,@venue,@department,@contact,@start_date,@end_date,@start_time,@end_time)", con);
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
                cmd.Parameters.AddWithValue("@start_time", startTime.ToString("h:mm:ss tt"));
                cmd.Parameters.AddWithValue("@end_time", endTime.ToString("h:mm:ss tt"));
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Event Successfully Added", "INSERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DisplayData();
                ClearData();
            }
            else
            {
                MessageBox.Show("Fill out all the information needed", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

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
            startingdate.Text = "";
            enddate.Text = "";
            startingtime.Text = "";
            endtime.Text = "";

        }

        private void BindData()
        {
            con.Open();

            MySqlCommand cmd = new MySqlCommand("select * from venue_ms.venue_table ", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                cmbvenue.Items.Add(dr[1].ToString());
            }
            dr.Close();
            con.Close();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (txtid.Text != "" && txtfname.Text != "" && txtlname.Text != "" && actevent.Text != "" && purpose.Text != "" && cmbvenue.Text != "" && depcmb.Text != "" && txtcontact.Text != "" && startingdate.Text != "" && enddate.Text != "" && startingtime.Text != "" && endtime.Text != "")
            {
                cmd = new MySqlCommand("delete from venue_ms.re_venue where id = @id", con);
                con.Open();
                cmd.Parameters.AddWithValue("@id", txtid.Text);

                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Record Successfully Deleted", "DELETE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DisplayData();
                ClearData();
            }
            else
            {
                MessageBox.Show("Select the record you want to Delete", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void startingdate_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void startingdate_MouseClick(object sender, MouseEventArgs e)
        {

            reservation reservationForm = new reservation();
            reservationForm.Show();

            this.Hide();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtid.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtfname.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtlname.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            actevent.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            purpose.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            cmbvenue.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            depcmb.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
            txtcontact.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();

            string startDateString = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
            DateTime startDate;
            if (!DateTime.TryParseExact(startDateString, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            {
                // Handle the case where startDateString is not in the "dd-MM-yyyy" format
                MessageBox.Show("Start date is not in the correct format. Please enter the date in the format 'yyyy-MM-dd'.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            startingdate.Text = startDate.ToString("dd-MM-yyyy"); // Use Text property instead of Value


            string endDateString = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
            DateTime endDate;
            if (!DateTime.TryParseExact(endDateString, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
            {
                // Handle the case where endDateString is not in the "dd-MM-yyyy" format
                MessageBox.Show("End date is not in the correct format. Please enter the date in the format 'yyyy-MM-dd'.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            enddate.Value = endDate; // Use Value property instead of Text

            startingtime.Text = dataGridView1.Rows[e.RowIndex].Cells[10].Value.ToString();
            endtime.Text = dataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString();
        }

        private void Binddep()
        {
            con.Open();

            MySqlCommand cmd = new MySqlCommand("select * from venue_ms.department ", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                depcmb.Items.Add(dr[1].ToString());
            }
            dr.Close();
            con.Close();


        }
    }
}
