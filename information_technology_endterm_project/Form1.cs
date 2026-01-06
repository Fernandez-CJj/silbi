using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace information_technology_endterm_project
{
    public partial class Form1 : Form
    {
        public Form1()        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '*';

        }
     
        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegisterPage frm = new RegisterPage();
            frm.Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;password=;database=im_etr";
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM users WHERE email = @email AND password = @password";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            reader.Read(); // Move to the first row
                            int userId = Convert.ToInt32(reader["id"]);

                            string role = reader["role"].ToString(); // Get the role value

                            if (role == "admin")
                            {
                                MessageBox.Show("Welcome, Admin!");
                                AdminDashboardForm frm = new AdminDashboardForm(userId);
                                frm.Show();
                                this.Hide();
                            }
                            else if (role == "profiler")
                            {
                                MessageBox.Show("Welcome, Profiler!");
                                ProfilerForm frm = new ProfilerForm(userId);
                                frm.Show();
                                this.Hide();
                            }
                            else if (role == "user")
                            {
                                MessageBox.Show("Welcome, Resident User!");
                                UserDashboardForm frm = new UserDashboardForm(userId);
                                frm.Show();
                                this.Hide();
                            }

                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password");
                        }
                    }
                }
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
