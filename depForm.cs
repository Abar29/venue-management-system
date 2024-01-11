using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace VenueManagement
{
    public partial class depForm : Form
    {
        MySqlConnection con = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;");
        MySqlCommand cmd;
        MySqlDataAdapter adapt;


        public depForm()
        {
            InitializeComponent();
            DisplayData();
        }

        private void txtdep_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnsave_Click(object sender, EventArgs e)
        {

            // if the record is already in the database, it says 'Department already exists'
            // if the record is not in the database, it says 'Event Successfully Added'

            if (txtdep.Text != "")
            {
                cmd = new MySqlCommand("insert into venue_ms.department (id,dep) values (@id,@dep)", con);
                con.Open();
                cmd.Parameters.AddWithValue("@id", "");
                cmd.Parameters.AddWithValue("@dep", txtdep.Text);

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
            txtid.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtdep.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
        }

        private void DisplayData()
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new MySqlDataAdapter("select * from venue_ms.department", con);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void ClearData()
        {
            txtid.Text = "";
            txtdep.Text = "";
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            // if i click this picturebox, it will back into the adminForm
            new adminForm().Show();
            this.Hide();
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

        private void depForm_Load(object sender, EventArgs e)
        {
            panel1.Location = new Point(
               this.ClientSize.Width / 2 - panel1.Size.Width / 2,
               this.ClientSize.Height / 2 - panel1.Size.Height / 2
           );
            panel1.Anchor = AnchorStyles.None;

            panel1.Region = Region.FromHrgn(
                CreateRoundRectRgn(0, 0, panel1.Width, panel1.Height, 30, 30)
            );

        }

        private void btndel_Click(object sender, EventArgs e)
        {

            if (txtid.Text != "" && txtdep.Text != "")
            {
                cmd = new MySqlCommand("delete from venue_ms.department where id = @id", con);
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
    }
}

