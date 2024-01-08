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


namespace VenueManagement
{
    
    public partial class reservation : Form
    {
        public PictureBox PictureBox4 { get { return pictureBox4; } }
        public PictureBox PicBack { get { return picback; } }

        int month, year;
        public static int static_month, static_year;
        public reservation()
        {
            InitializeComponent();
            
        }

        private void reservation_Load(object sender, EventArgs e)
        {
            displaDays();

        }

        

        private void displaDays()
        {
            DateTime now = DateTime.Now;
            month = now.Month;
            year = now.Year;

            string monthname = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
            lbdate.Text = monthname + " " + year;
            static_month = month;
            static_year = year;

            //lets get the first day of the month
            DateTime startofthemonth = new DateTime(year,month,1);
            // get the count of the days of the month
            int days = DateTime.DaysInMonth(year, month);
            //convert the start of the month to integer
            int dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d")) + 1;
            // lets create blank userControl
            for (int i = 1; i < dayoftheweek; i++)
            {
                UserControlBlank ucblank = new UserControlBlank();
                daycontainer.Controls.Add(ucblank);
            }
            //lets create user control for days
            for(int i  = 1; i <= days; i++)
            {
                UserControlDays ucdays = new UserControlDays();
                ucdays.days(i);
                daycontainer.Controls.Add(ucdays);

            }

        }

        private void btnprevios_Click(object sender, EventArgs e)
        {
            daycontainer.Controls.Clear();
            month--;
            static_month = month;
            static_year = year;


            string monthname = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
            lbdate.Text = monthname + " " + year;

            //lets get the first day of the month
            DateTime startofthemonth = new DateTime(year, month, 1);
            // get the count of the days of the month
            int days = DateTime.DaysInMonth(year, month);
            //convert the start of the month to integer
            int dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d")) + 1;
            // lets create blank userControl
            for (int i = 1; i < dayoftheweek; i++)
            {
                UserControlBlank ucblank = new UserControlBlank();
                daycontainer.Controls.Add(ucblank);
            }
            //lets create user control for days
            for (int i = 1; i <= days; i++)
            {
                UserControlDays ucdays = new UserControlDays();
                ucdays.days(i);
                daycontainer.Controls.Add(ucdays);

            }
        }

       

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            adminForm adminForm = new adminForm();
            adminForm.Show();
            this.Hide();
        }

        private void picback_Click_1(object sender, EventArgs e)
        {
            // Hide the current form (reservation form)
            this.Hide();
            fillForms fillForms = fillForms.Instance;
            fillForms.Show();
        }

        private void btnnext_Click(object sender, EventArgs e)
        {
            daycontainer.Controls.Clear();
            month++;
            static_month = month;
            static_year = year;

            string monthname = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
            lbdate.Text = monthname + " " + year;

            //lets get the first day of the month
            DateTime startofthemonth = new DateTime(year, month, 1);
            // get the count of the days of the month
            int days = DateTime.DaysInMonth(year, month);
            //convert the start of the month to integer
            int dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d")) + 1;
            // lets create blank userControl
            for (int i = 1; i < dayoftheweek; i++)
            {
                UserControlBlank ucblank = new UserControlBlank();
                daycontainer.Controls.Add(ucblank);
            }
            //lets create user control for days
            for (int i = 1; i <= days; i++)
            {
                UserControlDays ucdays = new UserControlDays();
                ucdays.days(i);
                daycontainer.Controls.Add(ucdays);

            }
        }

        


    }
}
