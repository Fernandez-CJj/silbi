using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient; 

namespace information_technology_endterm_project
{
    public partial class ReviewRequestForm : Form
    {
        int requestId;
        int user_id;
        public ReviewRequestForm(int id, int userId)
        {
            InitializeComponent();
            requestId = id;
            user_id = userId;
        }

        private void ReviewRequestForm_Load(object sender, EventArgs e)
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void uploadButton_Click(object sender, EventArgs e)
        {
            UserDashboardForm frm = new UserDashboardForm(user_id);
            frm.Show();
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }
    }
}
