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
    public partial class UserDashboardForm : Form
    {
        int Userid;
        int requestId;
        public UserDashboardForm(int id)
        {
            InitializeComponent();
            Userid = id;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RequestDocumentForm frm = new RequestDocumentForm(Userid);
            frm.Show();
            this.Hide();
        }

        private void UserDashboardForm_Load(object sender, EventArgs e)
        {
            uploadButton.Enabled = false;

            string connectionString = "server=localhost;user=root;password=;database=im_etr";
            string sql = "SELECT id, purpose, status, request_date FROM document_requests WHERE user_id=@id ORDER BY request_date desc";
            DataTable dt = new DataTable();

            using (var con = new MySqlConnection(connectionString))
            {
                con.Open();
                using(var cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", Userid);
                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["id"].HeaderText = "ID";
            dataGridView1.Columns["purpose"].HeaderText = "Document Type";
            dataGridView1.Columns["status"].HeaderText = "Status";
            dataGridView1.Columns["request_date"].HeaderText = "Requested At";

            Color accentColor = Color.FromArgb(12, 74, 134);

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // --- Enable horizontal scrolling ---
            dataGridView1.ScrollBars = ScrollBars.Both;

            // --- Row sizing (SET ONCE!) ---
            dataGridView1.RowTemplate.Height = 45; // Tall enough for font + padding
            dataGridView1.AllowUserToResizeRows = false;

            // --- Header styling ---
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersHeight = 45;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = accentColor; // changed!
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);

            // --- Cell styling ---
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 12F);

            
            dataGridView1.DefaultCellStyle.ForeColor = Color.FromArgb(44, 62, 80);
            dataGridView1.DefaultCellStyle.BackColor = Color.White;
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // --- Alternating row color ---
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(236, 240, 241);

            // --- Selection styling ---
            dataGridView1.DefaultCellStyle.SelectionBackColor = accentColor; // changed!
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;

            // --- Grid lines ---
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.GridColor = accentColor; // changed!

            // --- Behavior --- 
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeColumns = true;

            // --- Border style ---
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.BackgroundColor = Color.White;
        }

        private void uploadButton_Click(object sender, EventArgs e)
        {
            ReviewRequestForm frm = new ReviewRequestForm(requestId, Userid);
            frm.Show();
            this.Hide();
            uploadButton.Enabled = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) 
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                var idValue = row.Cells["id"].Value;
                if (idValue != null)
                {
                    requestId = Convert.ToInt32(idValue);
                }
            }
            uploadButton.Enabled = true;
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Show();
            this.Hide(); 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
        "Are you sure you want to DELETE your account? This cannot be undone.",
        "Confirm Delete Account",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning
    );

            if (confirm == DialogResult.Yes)
            {
                string connectionString = "server=localhost;user=root;password=;database=im_etr";
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string sql = "DELETE FROM users WHERE id = @id";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", Userid);
                            int rows = cmd.ExecuteNonQuery();

                            if (rows > 0)
                            {
                                MessageBox.Show("Account deleted successfully. Logging out...");

                                // Show login form and close this form
                                Form1 login = new Form1();
                                login.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Delete failed: Account not found.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting account: " + ex.Message);
                    }
                }
            }
        }
    }
}
