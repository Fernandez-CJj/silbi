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
    public partial class ValidateRequestForm : Form
    {
        int requestId;
        int userId;
        public ValidateRequestForm(int id, int user_id)
        {
            InitializeComponent();
            requestId = id;
            userId = user_id;
        }

        private string GetUsername(int userId)
        {
            string username = "";
            string connectionString = "server=localhost;user=root;password=;database=im_etr";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT username FROM users WHERE id=@id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                        username = result.ToString();
                }
            }
            return username;
        }
        private void ValidateRequestForm_Load(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;password=;database=im_etr";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM document_requests WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", requestId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                firstNameValue.Text = reader["first_name"].ToString();
                                lastnameValue.Text = reader["last_name"].ToString();
                                middlenameValue.Text = reader["middle_name"].ToString();
                                suffixValue.Text = reader["suffix"].ToString();
                                dobValue.Text = Convert.ToDateTime(reader["dob"]).ToString("yyyy-MM-dd");
                                genderValue.Text = reader["gender"].ToString();
                                maritalStatusValue.Text = reader["marital_status"].ToString();
                                purokValue.Text = reader["purok"].ToString();
                                addressValue.Text = reader["address"].ToString();
                                mobileNumberValue.Text = reader["mobile_number"].ToString();
                                documentTypeValue.Text = reader["purpose"].ToString();
                                purposeValue.Text = reader["secondary_purpose"].ToString();
                                feeValue.Text = "₱" + reader["fee"].ToString();
                                pictureBox4.Image = Image.FromFile(reader["document_path"].ToString());
                            }
                            else
                            {
                                MessageBox.Show("Request not found.");
                                this.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading request: " + ex.Message);
                    this.Close();
                }
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            ExpandImageForm frm = new ExpandImageForm(requestId, userId);
            frm.Show();
            this.Hide();
        }

        private void uploadButton_Click(object sender, EventArgs e)
        {
            AdminDashboardForm frm = new AdminDashboardForm(userId);
            frm.Show();
            this.Hide();
        }

        private void rejectButton_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show(
                "Are you sure you want to reject this request?",
                "Confirm Reject",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmResult == DialogResult.Yes)
            {
                string adminUsername = GetUsername(userId);
                string connectionString = "server=localhost;user=root;password=;database=im_etr";
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "UPDATE document_requests SET status = @status, approved_by = @approvedBy WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@status", "rejected");
                        cmd.Parameters.AddWithValue("@approvedBy", adminUsername);
                        cmd.Parameters.AddWithValue("@id", requestId);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Request rejected!");
                            AdminDashboardForm frm = new AdminDashboardForm(userId);
                            frm.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("An error occurred updating the request.");
                        }
                    }
                }
            }
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
           var confirmResult = MessageBox.Show(
               "Are you sure you want to approve this request?",
               "Confirm Approve",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question
           );

            if (confirmResult == DialogResult.Yes)
            {
                string adminUsername = GetUsername(userId);
                string connectionString = "server=localhost;user=root;password=;database=im_etr";
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "UPDATE document_requests SET status = @status, approved_by = @approvedBy WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@status", "accepted");
                        cmd.Parameters.AddWithValue("@approvedBy", adminUsername);
                        cmd.Parameters.AddWithValue("@id", requestId);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Request approved!");
                            AdminDashboardForm frm = new AdminDashboardForm(userId);
                            frm.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("An error occurred updating the request.");
                        }
                    }
                }
            }
        }
    }
}
