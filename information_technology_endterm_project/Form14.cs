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
    public partial class CreateAccountAdmin : Form
    {
        int userId;
        public CreateAccountAdmin(int id)
        {
            InitializeComponent();
            userId = id;
        }

        private void CreateAccountAdmin_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form13 frm = new Form13(userId);
            frm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;
            string role = comboBox1.SelectedItem.ToString();

            // Basic input validation
            if (username == "" || firstName == "" || lastName == "" || email == "" || password == "" || confirmPassword == "")
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }



            string connectionString = "server=localhost;user=root;password=;database=im_etr";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Check for duplicate username or email
                    string checkQuery = "SELECT * FROM users WHERE username = @username OR email = @email";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@username", username);
                        checkCmd.Parameters.AddWithValue("@email", email);

                        using (MySqlDataReader reader = checkCmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                MessageBox.Show("Username or Email already exists.");
                                return;
                            }
                        }
                    }

                    // Insert new user
                    string insertQuery = "INSERT INTO users (username, first_name, last_name, email, password, role) " +
                     "VALUES (@username, @firstName, @lastName, @email, @password, @role)";
                    using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@username", username);
                        insertCmd.Parameters.AddWithValue("@firstName", firstName);
                        insertCmd.Parameters.AddWithValue("@lastName", lastName);
                        insertCmd.Parameters.AddWithValue("@email", email);
                        insertCmd.Parameters.AddWithValue("@password", password); 
                        insertCmd.Parameters.AddWithValue("@role", role);

                        int result = insertCmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Registration successful!");
                            Form13 frm = new Form13(userId);
                            frm.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Registration failed. Please try again.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            } // closes using MySqlConnection
         // closes button1_Click

    }
}
}
