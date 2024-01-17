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
    public partial class adminForm : Form
    {
        MySqlConnection con = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;");
    
        MySqlDataAdapter adapt;
       
        public adminForm()
        {
            InitializeComponent();
            DisplayData();
        }
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

        private static extern IntPtr CreateRoundRectRgn
          (
          int nLeftRect,
          int nTopRect,
          int nRightRect,
          int nWidthEllipse,
          int nHeigthEllipse
,
          int v);

        private void btn3_Click(object sender, EventArgs e)
        {
            if (btn3.Text == btn3.Text)
            {

                new reservation().Show();
                
                this.Hide();
            }
        }

        private void addevent_Click(object sender, EventArgs e)
        {
            
                new addvenuecs().Show();
                this.Hide();
            
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

        private void searchpic_Click(object sender, EventArgs e)
        {

        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("lname like '%{0}%' OR fname like '%{0}%' OR venue like '%{0}%'", txtsearch.Text);
            searchpic.Text = dataGridView1.RowCount.ToString();
        }

        private void adminForm_Load(object sender, EventArgs e)
        {
            panel1.Location = new Point(
             this.ClientSize.Width / 2 - panel1.Size.Width / 2,
             this.ClientSize.Height / 2 - panel1.Size.Height / 2);
            panel1.Anchor = AnchorStyles.None;

            panel1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel1.Width,
                panel1.Height, 30, 30));
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            fillForms fillForms = fillForms.Instance;
            fillForms.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            depForm depForm = new depForm();
            depForm.Show();
            this.Close();
        }

        private void txtrep_Click(object sender, EventArgs e)
        {
            if (txtrep.Text == txtrep.Text)
            {

                new reports_table().Show();

                this.Hide();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            new aboutus().Show();
            this.Hide();
        }
    }
}
