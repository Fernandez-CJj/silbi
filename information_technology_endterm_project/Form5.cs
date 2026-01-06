using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace information_technology_endterm_project
{
    public partial class EditProfile : Form
    {
        private string profileId;
        private string connectionString = "server=localhost;user=root;password=;database=im_etr";
        int userId;
        public EditProfile(string id, int user_id)
        {
            InitializeComponent();
            profileId = id;
            this.Load += EditProfile_Load;
            userId = user_id;
        }

        private void EditProfile_Load(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT * FROM community_profiles WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", profileId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtName.Text = reader["name"].ToString();

                                // DateTimePicker
                                if (DateTime.TryParse(reader["date"].ToString(), out DateTime dateValue))
                                    txtDate.Value = dateValue;
                                else
                                    txtDate.Value = DateTime.Today;

                                txtBarangay.Text = reader["barangay"].ToString();
                                txtZoneSitio.Text = reader["zone_or_sitio"].ToString();
                                txtHouseNo.Text = reader["house_no"].ToString();
                                txtAddress.Text = reader["address"].ToString();

                                // Sex
                                string sex = reader["sex"].ToString();
                                rbMale.Checked = (sex == "Male");
                                rbFemale.Checked = (sex == "Female");

                                // Age group
                                string age = reader["age_group"].ToString();
                                rbBelow15.Checked = (age == rbBelow15.Text);
                                rb15_16.Checked = (age == rb15_16.Text);
                                rb17_18.Checked = (age == rb17_18.Text);
                                rb19above.Checked = (age == rb19above.Text);

                                // Type of Family
                                string tof = reader["family_type"].ToString();
                                rbNuclear.Checked = (tof == rbNuclear.Text);
                                rbExtended.Checked = (tof == rbExtended.Text);
                                rbSingleParent.Checked = (tof == rbSingleParent.Text);

                                // Siblings
                                txtNoS.Text = reader["siblings"].ToString();

                                // Family Income
                                string fipm = reader["family_income"].ToString();
                                rb5k.Checked = (fipm == rb5k.Text);
                                rb10k.Checked = (fipm == rb10k.Text);
                                rb15k.Checked = (fipm == rb15k.Text);
                                rb20k.Checked = (fipm == rb20k.Text);
                                rb21kAbove.Checked = (fipm == rb21kAbove.Text);

                                // Business Establishments
                                txtBusiness.Text = reader["business_establishments"].ToString();

                                // Has Schools
                                int hasSchools = Convert.ToInt32(reader["has_schools"]);
                                rbSchoolYes.Checked = (hasSchools == 1);
                                rbSchoolNo.Checked = (hasSchools == 0);

                                // School types
                                cbDaycare.Checked = Convert.ToInt32(reader["school_day_care"]) == 1;
                                cbElementary.Checked = Convert.ToInt32(reader["school_elementary"]) == 1;
                                cbhighschool.Checked = Convert.ToInt32(reader["school_highschool"]) == 1;
                                cbCollege.Checked = Convert.ToInt32(reader["school_college"]) == 1;

                                // Health Services
                                int hasHealthCare = Convert.ToInt32(reader["health_services_available"]);
                                rbHealthYes.Checked = (hasHealthCare == 1);
                                rbHealthNo.Checked = (hasHealthCare == 0);

                                cbHealthCenter.Checked = Convert.ToInt32(reader["health_center"]) == 1;
                                cbClinic.Checked = Convert.ToInt32(reader["clinic"]) == 1;
                                cbPublicH.Checked = Convert.ToInt32(reader["public_hospital"]) == 1;
                                cbPrivateH.Checked = Convert.ToInt32(reader["private_hospital"]) == 1;
                            }
                            else
                            {
                                MessageBox.Show("Profile not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load profile data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter the Name.");
                txtName.Focus();
                return false;
            }
            if (txtDate.Value == null || txtDate.Text == "")
            {
                MessageBox.Show("Please enter the Date.");
                txtDate.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtBarangay.Text))
            {
                MessageBox.Show("Please enter the Barangay.");
                txtBarangay.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtZoneSitio.Text))
            {
                MessageBox.Show("Please enter the Zone or Sitio.");
                txtZoneSitio.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtHouseNo.Text))
            {
                MessageBox.Show("Please enter the House Number.");
                txtHouseNo.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Please enter the Complete Address.");
                txtAddress.Focus();
                return false;
            }
            if (!rbMale.Checked && !rbFemale.Checked)
            {
                MessageBox.Show("Please select Sex.");
                rbMale.Focus();
                return false;
            }
            if (!rbBelow15.Checked && !rb15_16.Checked && !rb17_18.Checked && !rb19above.Checked)
            {
                MessageBox.Show("Please select Age Group.");
                rbBelow15.Focus();
                return false;
            }
            if (!rbNuclear.Checked && !rbExtended.Checked && !rbSingleParent.Checked)
            {
                MessageBox.Show("Please select Type of Family.");
                rbNuclear.Focus();
                return false;
            }
            int siblingsValue;
            if (string.IsNullOrWhiteSpace(txtNoS.Text) || !int.TryParse(txtNoS.Text, out siblingsValue) || siblingsValue < 0)
            {
                MessageBox.Show("Please enter a valid number of Siblings (0 or more).");
                txtNoS.Focus();
                return false;
            }
            if (!rb5k.Checked && !rb10k.Checked && !rb15k.Checked && !rb20k.Checked && !rb21kAbove.Checked)
            {
                MessageBox.Show("Please select Family Income Per Month.");
                rb5k.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtBusiness.Text))
            {
                MessageBox.Show("Please enter the Business Establishments field.");
                txtBusiness.Focus();
                return false;
            }
            if (!rbSchoolYes.Checked && !rbSchoolNo.Checked)
            {
                MessageBox.Show("Please select if you have schools in your community.");
                rbSchoolYes.Focus();
                return false;
            }
            if (rbSchoolYes.Checked && !cbDaycare.Checked && !cbElementary.Checked && !cbhighschool.Checked && !cbCollege.Checked)
            {
                MessageBox.Show("If schools exist, please check at least one school type.");
                cbDaycare.Focus();
                return false;
            }
            if (!rbHealthYes.Checked && !rbHealthNo.Checked)
            {
                MessageBox.Show("Please select if health services are available.");
                rbHealthYes.Focus();
                return false;
            }
            if (rbHealthYes.Checked && !cbHealthCenter.Checked && !cbClinic.Checked && !cbPublicH.Checked && !cbPrivateH.Checked)
            {
                MessageBox.Show("If health services exist, please check at least one health facility type.");
                cbHealthCenter.Focus();
                return false;
            }
            return true;
        }

       

        private void saveChangesButton_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = @"UPDATE community_profiles SET
                        name=@name, date=@date, barangay=@barangay, zone_or_sitio=@zone_or_sitio, house_no=@house_no, address=@address, 
                        sex=@sex, age_group=@age_group, family_type=@family_type, siblings=@siblings, family_income=@family_income, business_establishments=@business_establishments, 
                        has_schools=@has_schools, school_day_care=@school_day_care, school_elementary=@school_elementary, school_highschool=@school_highschool, school_college=@school_college, 
                        health_services_available=@health_services_available, health_center=@health_center, clinic=@clinic, public_hospital=@public_hospital, private_hospital=@private_hospital
                        WHERE id=@id";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@date", txtDate.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@barangay", txtBarangay.Text);
                    cmd.Parameters.AddWithValue("@zone_or_sitio", txtZoneSitio.Text);
                    cmd.Parameters.AddWithValue("@house_no", txtHouseNo.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@sex", rbMale.Checked ? "Male" : (rbFemale.Checked ? "Female" : ""));
                    cmd.Parameters.AddWithValue("@age_group",
                        rbBelow15.Checked ? rbBelow15.Text :
                        rb15_16.Checked ? rb15_16.Text :
                        rb17_18.Checked ? rb17_18.Text :
                        rb19above.Checked ? rb19above.Text : "");
                    cmd.Parameters.AddWithValue("@family_type",
                        rbNuclear.Checked ? rbNuclear.Text :
                        rbExtended.Checked ? rbExtended.Text :
                        rbSingleParent.Checked ? rbSingleParent.Text : "");
                    cmd.Parameters.AddWithValue("@siblings", int.TryParse(txtNoS.Text, out int sibVal) ? sibVal : 0);
                    cmd.Parameters.AddWithValue("@family_income",
                        rb5k.Checked ? rb5k.Text :
                        rb10k.Checked ? rb10k.Text :
                        rb15k.Checked ? rb15k.Text :
                        rb20k.Checked ? rb20k.Text :
                        rb21kAbove.Checked ? rb21kAbove.Text : "");
                    cmd.Parameters.AddWithValue("@business_establishments", txtBusiness.Text);
                    cmd.Parameters.AddWithValue("@has_schools", rbSchoolYes.Checked ? 1 : 0);
                    cmd.Parameters.AddWithValue("@school_day_care", cbDaycare.Checked ? 1 : 0);
                    cmd.Parameters.AddWithValue("@school_elementary", cbElementary.Checked ? 1 : 0);
                    cmd.Parameters.AddWithValue("@school_highschool", cbhighschool.Checked ? 1 : 0);
                    cmd.Parameters.AddWithValue("@school_college", cbCollege.Checked ? 1 : 0);
                    cmd.Parameters.AddWithValue("@health_services_available", rbHealthYes.Checked ? 1 : 0);
                    cmd.Parameters.AddWithValue("@health_center", cbHealthCenter.Checked ? 1 : 0);
                    cmd.Parameters.AddWithValue("@clinic", cbClinic.Checked ? 1 : 0);
                    cmd.Parameters.AddWithValue("@public_hospital", cbPublicH.Checked ? 1 : 0);
                    cmd.Parameters.AddWithValue("@private_hospital", cbPrivateH.Checked ? 1 : 0);

                    cmd.Parameters.AddWithValue("@id", profileId);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ViewProfiles frm = new ViewProfiles(userId);
                        frm.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No changes were made.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to update profile: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
           
        }

        private void rbSchoolNo_CheckedChanged(object sender, EventArgs e)
        {
            cbDaycare.Checked = false;
            cbElementary.Checked = false;
            cbhighschool.Checked = false;
            cbCollege.Checked = false;
        }

        private void rbHealthNo_CheckedChanged(object sender, EventArgs e)
        {
            cbHealthCenter.Checked = false;
            cbClinic.Checked = false;
            cbPublicH.Checked = false;
            cbPrivateH.Checked = false;
        }
    }
}