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
    public partial class ViewProfiles : Form
    {
        string sex = "";
        int health = 0;
        int school = 0;
        string selectedId = "";
        int userId;
        public ViewProfiles(int id)
        {
            InitializeComponent();
            userId = id;
        }

        private void ViewProfiles_Load(object sender, EventArgs e)
        {
            LoadProfiles();
            LoadBarangays();
            LoadZones();
            deleteButton.Enabled = false;
            editButton.Enabled = false;
        }

        private void LoadProfiles()
        {
            string connectionString = "server=localhost;user=root;password=;database=im_etr";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM community_profiles";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Bind to your DataGridView from Designer!
                    dataGridView1.DataSource = dt;

                    // =============================================
                    // DESIGN IMPROVEMENTS - SHOW ALL DATA
                    // =============================================

                    // Colors matching rgb(12, 74, 134)
                    Color accentColor = Color.FromArgb(12, 74, 134);

                    // --- Column sizing:  Auto-size based on content, NOT fill ---
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

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
                    dataGridView1.DefaultCellStyle.Font = new Font("Arial", 11F);
                    dataGridView1.DefaultCellStyle.ForeColor = Color.FromArgb(44, 62, 80);
                    dataGridView1.DefaultCellStyle.BackColor = Color.White;
                    dataGridView1.DefaultCellStyle.Padding = new Padding(10, 15, 10, 15);
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
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading profiles: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadBarangays()
        {
            string connectionString = "server=localhost;user=root;password=;database=im_etr";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Get unique barangay values
                    string sql = "SELECT DISTINCT barangay FROM community_profiles WHERE barangay IS NOT NULL AND barangay <> ''";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    barangayCB.Items.Clear(); // Clear existing items
                    while (reader.Read())
                    {
                        // Add non-null/non-empty barangays
                        barangayCB.Items.Add(reader.GetString("barangay"));
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading barangays: " + ex.Message);
                }
            }
        }

        private void LoadZones()
        {
            string connectionString = "server=localhost;user=root;password=;database=im_etr";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT DISTINCT zone_or_sitio FROM community_profiles WHERE zone_or_sitio IS NOT NULL AND zone_or_sitio <> ''";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    zoneCB.Items.Clear(); // Clear existing items
                    while (reader.Read())
                    {
                        zoneCB.Items.Add(reader.GetString("zone_or_sitio"));
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading zones/sitios: " + ex.Message);
                }
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> filters = new List<string>();

            if (!string.IsNullOrEmpty(txtName.Text))
                filters.Add("name LIKE @name");

            if (barangayCB.SelectedItem != null && barangayCB.SelectedItem.ToString() != "")
                filters.Add("barangay LIKE @barangay");

            if (zoneCB.SelectedItem != null && zoneCB.SelectedItem.ToString() != "")
                filters.Add("zone_or_sitio LIKE @zone");

            if (ageCB.SelectedItem != null && ageCB.SelectedItem.ToString() != "")
                filters.Add("age_group = @ageGroup");

            if (familyTypeCB.SelectedItem != null && familyTypeCB.SelectedItem.ToString() != "")
                filters.Add("family_type = @familyType");

            if (familyIncomeCB.SelectedItem != null && familyIncomeCB.SelectedItem.ToString() != "")
                filters.Add("family_income = @familyIncome");

            if (!string.IsNullOrEmpty(sex))
                filters.Add("sex = @sex");

            if (school != 0)
                filters.Add("has_schools = @schools");

            if (health != 0)
                filters.Add("health_service_available = @health");

            string whereClause = filters.Count > 0 ? "WHERE " + string.Join(" AND ", filters) : "";
            string sql = $"SELECT * FROM community_profiles {whereClause}";

            string connectionString = "server=localhost;user=root;password=;database=im_etr";
            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                // Add parameters
                if (!string.IsNullOrEmpty(txtName.Text))
                    cmd.Parameters.AddWithValue("@name", $"%{txtName.Text}%");
                if (barangayCB.SelectedItem != null && barangayCB.SelectedItem.ToString() != "")
                    cmd.Parameters.AddWithValue("@barangay", $"%{barangayCB.SelectedItem}%");
                if (zoneCB.SelectedItem != null && zoneCB.SelectedItem.ToString() != "")
                    cmd.Parameters.AddWithValue("@zone", $"%{zoneCB.SelectedItem}%");
                if (ageCB.SelectedItem != null && ageCB.SelectedItem.ToString() != "")
                    cmd.Parameters.AddWithValue("@ageGroup", ageCB.SelectedItem.ToString());
                if (familyTypeCB.SelectedItem != null && familyTypeCB.SelectedItem.ToString() != "")
                    cmd.Parameters.AddWithValue("@familyType", familyTypeCB.SelectedItem.ToString());
                if (familyIncomeCB.SelectedItem != null && familyIncomeCB.SelectedItem.ToString() != "")
                    cmd.Parameters.AddWithValue("@familyIncome", familyIncomeCB.SelectedItem.ToString());
                if (!string.IsNullOrEmpty(sex))
                    cmd.Parameters.AddWithValue("@sex", sex);
                if (school != 0)
                    cmd.Parameters.AddWithValue("@schools", school);
                if (health != 0)
                    cmd.Parameters.AddWithValue("@health", health);

                // Execute query and bind to DataGridView
                DataTable dt = new DataTable();
                try
                {
                    conn.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving filtered profiles: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMale.Checked)
            {
                sex = rbMale.Text;
            }
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFemale.Checked)
            {
                sex = rbFemale.Text;
            }
        }

        private void rbSchoolYes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSchoolYes.Checked)
            {
                school = 1;
            }
        }

        private void rbSchoolNo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSchoolNo.Checked)
            {
                school = 0;
            }
        }

        private void rbHealthYes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbHealthYes.Checked)
            {
                health = 1;
            }
        }

        private void rbHealthNo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbHealthNo.Checked)
            {
                health = 0;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Clear textboxes
            txtName.Text = "";

            // Reset comboboxes
            barangayCB.SelectedIndex = -1;
            zoneCB.SelectedIndex = -1;
            ageCB.SelectedIndex = -1;
            familyTypeCB.SelectedIndex = -1;
            familyIncomeCB.SelectedIndex = -1;

            // Reset radiobuttons (assuming grouped properly)
            rbMale.Checked = false;
            rbFemale.Checked = false;
            rbSchoolYes.Checked = false;
            rbSchoolNo.Checked = false;
            rbHealthYes.Checked = false;
            rbHealthNo.Checked = false;

            // Also reset your filter variables!
            sex = "";
            school = 0;
            health = 0;

            // Reload all profiles in the grid
            LoadProfiles();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ignore clicks on header
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                // Assuming your id column is named "id"
                selectedId = row.Cells["id"].Value?.ToString();
                deleteButton.Enabled = true;
                editButton.Enabled = true;
            }
        }

       

        private void deleteButton_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedId))
            {
                MessageBox.Show("Please select a profile to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirmation dialog
            var result = MessageBox.Show("Are you sure you want to delete this profile?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string connectionString = "server=localhost;user=root;password=;database=im_etr";
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string sql = "DELETE FROM community_profiles WHERE id = @id";
                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", selectedId);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Profile deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadProfiles(); // Refresh grid
                                selectedId = ""; // Reset selection
                            }
                            else
                            {
                                MessageBox.Show("No record deleted. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting profile: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            deleteButton.Enabled = false;
            editButton.Enabled = false;
            selectedId = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(selectedId))
            {
                MessageBox.Show("Please select a profile to edit.", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            EditProfile frm = new EditProfile(selectedId, userId);
            frm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ProfilerForm frm = new ProfilerForm(userId);
            frm.Show();
            this.Hide();
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Show();
            this.Hide();
        }

        private void DeleteAccountButton_Click(object sender, EventArgs e)
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
                            cmd.Parameters.AddWithValue("@id", userId);
                            int rows = cmd.ExecuteNonQuery();

                            if (rows > 0)
                            {
                                MessageBox.Show("Account deleted successfully. Logging out...");

                              
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