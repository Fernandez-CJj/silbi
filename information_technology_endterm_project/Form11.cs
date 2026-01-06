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
    public partial class ExpandImageForm : Form
    {
        int requestId;
        int userId;
        public ExpandImageForm(int id, int user_id)
        {
            InitializeComponent();
            requestId = id;
            userId = user_id;
        }

        private void ExpandImageForm_Load(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user=root;password=;database=im_etr";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT document_path FROM document_requests WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", requestId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string imagePath = reader["document_path"].ToString();

                                // For example: show in PictureBox
                                if (!string.IsNullOrEmpty(imagePath) && System.IO.File.Exists(imagePath))
                                {
                                    pictureBox1.Image = Image.FromFile(imagePath);
                                }
                                else
                                {
                                    // Maybe show a placeholder or an error message
                                    pictureBox1.Image = null;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Image path not found.");
                                this.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image path: " + ex.Message);
                    this.Close();
                }
            }
        }

        private void uploadButton_Click(object sender, EventArgs e)
        {
            ValidateRequestForm frm = new ValidateRequestForm(requestId, userId);
            frm.Show();
            this.Hide();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
