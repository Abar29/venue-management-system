using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

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
                startingdate.Text = date.ToString("dd/MM/yyyy");
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
            if (txtfname.Text != "" && txtlname.Text != "" && actevent.Text != "" && purpose.Text != "" && cmbvenue.Text != "" && deptcmb.Text != "" && txtcontact.Text != "" && startingdate.Text != "" && enddate.Text != "" && startingtime.Text != "" && endtime.Text != "")
            {
                // Create a new MySqlCommand to check if the venue is already booked on the same date and at the same time
                MySqlCommand cmdCheck = new MySqlCommand("SELECT * FROM venue_ms.re_venue WHERE venue = @venue AND start_date = @start_date AND ((start_time <= @start_time AND end_time > @start_time) OR (start_time < @end_time AND end_time >= @end_time))", con);
                cmdCheck.Parameters.AddWithValue("@venue", cmbvenue.Text);
                cmdCheck.Parameters.AddWithValue("@start_date", startingdate.Text);
                cmdCheck.Parameters.AddWithValue("@start_time", startingtime.Text);
                cmdCheck.Parameters.AddWithValue("@end_time", endtime.Text);

                con.Open();
                using (var drCheck = cmdCheck.ExecuteReader())
                {
                    if (drCheck.HasRows)
                    {
                        // If the venue is already booked, show an error message and close the connection
                        MessageBox.Show("The venue is already booked at the same time and on the same date.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        con.Close();
                    }
                    else
                    {
                        // Close the DataReader before executing the second command
                        drCheck.Close();

                        // If the venue is not booked, insert the new record
                        cmd = new MySqlCommand("insert into venue_ms.re_venue (id,lname,fname,act_event,nature_event,venue,department,contact,start_date,end_date,start_time,end_time) values (@id,@lname,@fname,@act_event,@nature_event,@venue,@department,@contact,@start_date,@end_date,@start_time,@end_time)", con);
                        cmd.Parameters.AddWithValue("@id", txtid.Text);
                        cmd.Parameters.AddWithValue("@lname", txtlname.Text);
                        cmd.Parameters.AddWithValue("@fname", txtfname.Text);
                        cmd.Parameters.AddWithValue("@act_event", actevent.Text);
                        cmd.Parameters.AddWithValue("@nature_event", purpose.Text);
                        cmd.Parameters.AddWithValue("@venue", cmbvenue.Text);
                        cmd.Parameters.AddWithValue("@department", deptcmb.Text);
                        cmd.Parameters.AddWithValue("@contact", txtcontact.Text);
                        cmd.Parameters.AddWithValue("@start_date", startingdate.Text);
                        cmd.Parameters.AddWithValue("@end_date", enddate.Text);
                        cmd.Parameters.AddWithValue("@start_time", startingtime.Text);
                        cmd.Parameters.AddWithValue("@end_time", endtime.Text);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Event Successfully Added", "INSERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DisplayData();
                        ClearData();
                    }
                }
            }
            else
            {
                MessageBox.Show("Fill out all the information needed", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtid.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtfname.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtlname.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            actevent.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            purpose.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            cmbvenue.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            deptcmb.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
            txtcontact.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
            startingdate.Text = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
            enddate.Text = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
            startingtime.Text = dataGridView1.Rows[e.RowIndex].Cells[10].Value.ToString();
            endtime.Text = dataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString();
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
            deptcmb.Text = "";
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
            if (txtid.Text != "" && txtfname.Text != "" && txtlname.Text != "" && actevent.Text != "" && purpose.Text != "" && cmbvenue.Text != "" && deptcmb.Text != "" && txtcontact.Text != "" && startingdate.Text != "" && enddate.Text != "" && startingtime.Text != "" && endtime.Text != "")
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
    }
}
