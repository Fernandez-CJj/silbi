using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace information_technology_endterm_project
{
    public partial class ProfilerForm : Form
    {
        string gender = "";
        string age = "";
        string tof = "";
        string fipm = "";
        int hasSchools = 0;
        int daycare = 0;
        int elementary = 0;
        int highschool = 0;
        int college = 0;
        int hasHealthCare = 0;
        int healthCenter = 0;
        int clinic = 0;
        int publich = 0;
        int privateh = 0;

        int userId;
        public ProfilerForm(int id)
        {
            InitializeComponent();
            userId = id;
        }

        private void ProfilerForm_Load(object sender, EventArgs e)
        {
            cbHealthCenter.Enabled = !true;
            cbClinic.Enabled = !true;
            cbPublicH.Enabled = !true;
            cbPrivateH.Enabled = !true;
            cbDaycare.Enabled = false;
            cbElementary.Enabled = false;
            cbhighschool.Enabled = false;
            cbCollege.Enabled = false;
        }

        private bool ValidateForm()
        {
            // Name (required)
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter the Name.");
                txtName.Focus();
                return false;
            }

            // Date (required)
            if (txtDate.Value == null || txtDate.Text == "")
            {
                MessageBox.Show("Please enter the Date.");
                txtDate.Focus();
                return false;
            }

            // Barangay
            if (string.IsNullOrWhiteSpace(txtBarangay.Text))
            {
                MessageBox.Show("Please enter the Barangay.");
                txtBarangay.Focus();
                return false;
            }

            // Zone/Sitio
            if (string.IsNullOrWhiteSpace(txtZoneSitio.Text))
            {
                MessageBox.Show("Please enter the Zone or Sitio.");
                txtZoneSitio.Focus();
                return false;
            }

            // House No.
            if (string.IsNullOrWhiteSpace(txtHouseNo.Text))
            {
                MessageBox.Show("Please enter the House Number.");
                txtHouseNo.Focus();
                return false;
            }

            // Address
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Please enter the Complete Address.");
                txtAddress.Focus();
                return false;
            }

            // Sex (radio buttons)
            if (!rbMale.Checked && !rbFemale.Checked)
            {
                MessageBox.Show("Please select Sex.");
                rbMale.Focus();
                return false;
            }

            // Age Group (radio buttons)
            if (!rbBelow15.Checked && !rb15_16.Checked && !rb17_18.Checked && !rb19above.Checked)
            {
                MessageBox.Show("Please select Age Group.");
                rbBelow15.Focus();
                return false;
            }

            // Type of Family (radio buttons)
            if (!rbNuclear.Checked && !rbExtended.Checked && !rbSingleParent.Checked)
            {
                MessageBox.Show("Please select Type of Family.");
                rbNuclear.Focus();
                return false;
            }

            // Siblings (required, integer)
            int siblingsValue;
            if (string.IsNullOrWhiteSpace(txtNoS.Text) || !int.TryParse(txtNoS.Text, out siblingsValue) || siblingsValue < 0)
            {
                MessageBox.Show("Please enter a valid number of Siblings (0 or more).");
                txtNoS.Focus();
                return false;
            }

            // Family Income Per Month (radio buttons)
            if (!rb5k.Checked && !rb10k.Checked && !rb15k.Checked && !rb20k.Checked && !rb21kAbove.Checked)
            {
                MessageBox.Show("Please select Family Income Per Month.");
                rb5k.Focus();
                return false;
            }

            // Business Establishments
            if (string.IsNullOrWhiteSpace(txtBusiness.Text))
            {
                MessageBox.Show("Please enter the Business Establishments field.");
                txtBusiness.Focus();
                return false;
            }

            // Do you have schools in your community?
            if (!rbSchoolYes.Checked && !rbSchoolNo.Checked)
            {
                MessageBox.Show("Please select if you have schools in your community.");
                rbSchoolYes.Focus();
                return false;
            }
            // If SchoolYes, validate at least one school type
            if (rbSchoolYes.Checked && !cbDaycare.Checked && !cbElementary.Checked && !cbhighschool.Checked && !cbCollege.Checked)
            {
                MessageBox.Show("If schools exist, please check at least one school type.");
                cbDaycare.Focus();
                return false;
            }

            // Are health services available?
            if (!rbHealthYes.Checked && !rbHealthNo.Checked)
            {
                MessageBox.Show("Please select if health services are available.");
                rbHealthYes.Focus();
                return false;
            }
            // If HealthYes, validate at least one health type
            if (rbHealthYes.Checked && !cbHealthCenter.Checked && !cbClinic.Checked && !cbPublicH.Checked && !cbPrivateH.Checked)
            {
                MessageBox.Show("If health services exist, please check at least one health facility type.");
                cbHealthCenter.Focus();
                return false;
            }

            // All checks passed!
            return true;
        }

        // Call this method when you want to save the profile (e.g. from a Save button click)
        private void InsertProfile()
        {
            string connectionString = "server=localhost;user=root;password=;database=im_etr";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string sql = @"INSERT INTO community_profiles
                        (name, date, barangay, zone_or_sitio, house_no, address, sex, age_group, family_type, siblings, family_income, business_establishments,
                         has_schools, school_day_care, school_elementary, school_highschool, school_college,
                         health_services_available, health_center, clinic, public_hospital, private_hospital)
                        VALUES
                        (@name, @date, @barangay, @zone_or_sitio, @house_no, @address, @sex, @age_group, @family_type, @siblings, @family_income, @business_establishments,
                         @has_schools, @school_day_care, @school_elementary, @school_highschool, @school_college,
                         @health_services_available, @health_center, @clinic, @public_hospital, @private_hospital)";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@date", txtDate.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@barangay", txtBarangay.Text);
                    cmd.Parameters.AddWithValue("@zone_or_sitio", txtZoneSitio.Text);
                    cmd.Parameters.AddWithValue("@house_no", txtHouseNo.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@sex", gender);
                    cmd.Parameters.AddWithValue("@age_group", age);
                    cmd.Parameters.AddWithValue("@family_type", tof);
                    cmd.Parameters.AddWithValue("@siblings", int.Parse(txtNoS.Text));
                    cmd.Parameters.AddWithValue("@family_income", fipm);
                    cmd.Parameters.AddWithValue("@business_establishments", txtBusiness.Text);
                    cmd.Parameters.AddWithValue("@has_schools", hasSchools);
                    cmd.Parameters.AddWithValue("@school_day_care", daycare);
                    cmd.Parameters.AddWithValue("@school_elementary", elementary);
                    cmd.Parameters.AddWithValue("@school_highschool", highschool);
                    cmd.Parameters.AddWithValue("@school_college", college);
                    cmd.Parameters.AddWithValue("@health_services_available", hasHealthCare);
                    cmd.Parameters.AddWithValue("@health_center", healthCenter);
                    cmd.Parameters.AddWithValue("@clinic", clinic);
                    cmd.Parameters.AddWithValue("@public_hospital", publich);
                    cmd.Parameters.AddWithValue("@private_hospital", privateh);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Profile saved successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to save profile.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // Example: Call InsertProfile from a button click
        private void btnSave_Click(object sender, EventArgs e)
        {
            InsertProfile();
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMale.Checked)
            {
                gender = rbMale.Text;
            }
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFemale.Checked)
            {
                gender = rbFemale.Text;
            }
        }

        private void rbBelow15_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBelow15.Checked) {
                age = rbBelow15.Text;
            }
        }

        private void rb15_16_CheckedChanged(object sender, EventArgs e)
        {
            if (rb15_16.Checked)
            {
                age = rb15_16.Text;
            }
        }

        private void rb17_19_CheckedChanged(object sender, EventArgs e)
        {
            if (rb17_18.Checked)
            {
                age = rb17_18.Text;
            }
        }

        private void rb19above_CheckedChanged(object sender, EventArgs e)
        {
            if (rb19above.Checked)
            {
                age = rb19above.Text;
            }
        }

        private void rbNuclear_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNuclear.Checked)
            {
                tof = rbNuclear.Text;
            }
        }

        private void rbExtended_CheckedChanged(object sender, EventArgs e)
        {
            if (rbExtended.Checked)
            {
                tof = rbExtended.Text;
            }
        }

        private void rbSingeParent_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSingleParent.Checked)
            {
                tof = rbSingleParent.Text;
            }
        }

        private void rb5k_CheckedChanged(object sender, EventArgs e)
        {
            if (rb5k.Checked)
            {
                fipm = rb5k.Text;
            }
        }

        private void rb10k_CheckedChanged(object sender, EventArgs e)
        {
            if (rb10k.Checked)
            {
                fipm = rb10k.Text;
            }
        }

        private void rb15k_CheckedChanged(object sender, EventArgs e)
        {
            if (rb15k.Checked)
            {
                fipm = rb15k.Text;
            }
        }

        private void rb20k_CheckedChanged(object sender, EventArgs e)
        {
            if (rb20k.Checked){
                fipm = rb20k.Text;
            }
        }

        private void rb21kAbove_CheckedChanged(object sender, EventArgs e)
        {
            if (rb21kAbove.Checked)
            {
                fipm = rb21kAbove.Text;
            }
        }

        private void rbSchoolYes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSchoolYes.Checked)
            {
                hasSchools = 1;
            }
            cbDaycare.Enabled = !false;
            cbElementary.Enabled = !false;
            cbhighschool.Enabled = !false;
            cbCollege.Enabled = !false;
        }

        private void cbDaycare_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDaycare.Checked)
            {
                daycare = 1;
            }
            else
            {
                daycare = 0;
            }
        }

        private void cbElementary_CheckedChanged(object sender, EventArgs e)
        {
            if (cbElementary.Checked)
            {
                elementary = 1;
            }
            else
            {
                elementary = 0;
            }
        }

        private void cbhighschool_CheckedChanged(object sender, EventArgs e)
        {
            if (cbhighschool.Checked)
            {
                highschool = 1;
            }
            else
            {
                highschool = 0;
            }
        }

        private void cbCollege_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCollege.Checked)
            {
                college = 1;
            }
            else
            {
                college = 0;
            }
        }

        private void rbHealthYes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbHealthYes.Checked)
            {
                hasHealthCare = 1;
            }
            cbHealthCenter.Enabled = true;
            cbClinic.Enabled = true;
            cbPublicH.Enabled = true;
            cbPrivateH.Enabled = true;
        }

        private void rbHealthNo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbHealthNo.Checked)
            {
                hasHealthCare = 0;
            }
            cbHealthCenter.Enabled = !true;
            cbClinic.Enabled = !true;
            cbPublicH.Enabled = !true;
            cbPrivateH.Enabled = !true;
            cbHealthCenter.Checked = false;
            cbClinic.Checked = false;
            cbPublicH.Checked = false;
            cbPrivateH.Checked = false;
        }

        private void cbHealthCenter_CheckedChanged(object sender, EventArgs e)
        {
            if (cbHealthCenter.Checked)
            {
                healthCenter = 1;
            }
            else
            {
                healthCenter = 0;
            }
        }

        private void cbClinic_CheckedChanged(object sender, EventArgs e)
        {
            if (cbClinic.Checked)
            {
                clinic = 1;
            }
            else
            {
                clinic = 0;
            }
        }

        private void cbPublicH_CheckedChanged(object sender, EventArgs e)
        {
            if (cbPublicH.Checked)
            {
                publich = 1;
            }
            else
            {
                publich = 0;
            }
        }

        private void cbPrivateH_CheckedChanged(object sender, EventArgs e)
        {
            if (cbPrivateH.Checked)
            {
                privateh = 1;
            }
            else
            {
                privateh = 0;
            }
        }

        private void ClearForm()
        {
            // TextBoxes
            txtName.Clear();
            txtBarangay.Clear();
            txtZoneSitio.Clear();
            txtHouseNo.Clear();
            txtAddress.Clear();
            txtNoS.Clear();
            txtBusiness.Clear();

            // DateTimePicker: reset to today
            txtDate.Value = DateTime.Today;

            // RadioButtons — set defaults
            rbMale.Checked = false;
            rbFemale.Checked = false;

            rbBelow15.Checked = false;
            rb15_16.Checked = false;
            rb17_18.Checked = false;
            rb19above.Checked = false;

            rbNuclear.Checked = false;
            rbExtended.Checked = false;
            rbSingleParent.Checked = false;

            rb5k.Checked = false;
            rb10k.Checked = false;
            rb15k.Checked = false;
            rb20k.Checked = false;
            rb21kAbove.Checked = false;

            rbSchoolYes.Checked = false;
            rbSchoolNo.Checked = false;

            rbHealthYes.Checked = false;
            rbHealthNo.Checked = false;

            // CheckBoxes
            cbDaycare.Checked = false;
            cbElementary.Checked = false;
            cbhighschool.Checked = false;
            cbCollege.Checked = false;

            cbHealthCenter.Checked = false;
            cbClinic.Checked = false;
            cbPublicH.Checked = false;
            cbPrivateH.Checked = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (ValidateForm()) { 
                InsertProfile();
                ClearForm();
            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ProfilerForm frm = new ProfilerForm(userId);
            frm.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ViewProfiles frm = new ViewProfiles(userId);
            frm.Show();
            this.Hide();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void rbSchoolNo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSchoolYes.Checked)
            {
                hasSchools = 0;
            }
            cbDaycare.Enabled = false;
            cbElementary.Enabled = false;
            cbhighschool.Enabled = false;
            cbCollege.Enabled = false;
            cbDaycare.Checked = false;
            cbElementary.Checked = false;
            cbhighschool.Checked = false;
            cbCollege.Checked = false;
        }

        private void ProfilerForm_Load_1(object sender, EventArgs e)
        {
            cbHealthCenter.Enabled = !true;
            cbClinic.Enabled = !true;
            cbPublicH.Enabled = !true;
            cbPrivateH.Enabled = !true;
            cbDaycare.Enabled = false;
            cbElementary.Enabled = false;
            cbhighschool.Enabled = false;
            cbCollege.Enabled = false;
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
                            cmd.Parameters.AddWithValue("@id", userId);
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