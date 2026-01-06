using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;

namespace information_technology_endterm_project
{
    public partial class RequestDocumentForm : Form
    {
        int userId;
        String imageLocation = "";

        public RequestDocumentForm(int id)
        {
            InitializeComponent();
            userId = id;
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(firstNameBox.Text))
            {
                MessageBox.Show("Please enter First Name.");
                firstNameBox.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(lastNameBox.Text))
            {
                MessageBox.Show("Please enter Last Name.");
                lastNameBox.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(middleNameBox.Text))
            {
                MessageBox.Show("Please enter Middle Name.");
                middleNameBox.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(suffixBox.Text))
            {
                MessageBox.Show("Please enter Suffix.");
                suffixBox.Focus();
                return false;
            }
            if (genderComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Please select Gender.");
                genderComboBox.Focus();
                return false;
            }
            if (maritalStatusComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Please enter Marital Status.");
                maritalStatusComboBox.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(purokBox.Text))
            {
                MessageBox.Show("Please enter Purok.");
                purokBox.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(addressBox.Text))
            {
                MessageBox.Show("Please enter Address.");
                addressBox.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(mobileNumberBox.Text))
            {
                MessageBox.Show("Please enter Mobile Number.");
                mobileNumberBox.Focus();
                return false;
            }
            if (purposeComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Please select Purpose.");
                purposeComboBox.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(secondaryPurposeComboBox.Text))
            {
                MessageBox.Show("Please enter Secondary Purpose.");
                secondaryPurposeComboBox.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(PictureDocument.ImageLocation))
            {
                MessageBox.Show("Please upload a supporting document.");
                return false;
            }
            if (!agreeRB.Checked)
            {
                MessageBox.Show("You must agree to the terms and Data Privacy Act to submit your request.");
                return false;
            }
            return true;
        }

        private int GetFeeForPurpose(string purpose)
        {
            switch (purpose)
            {
                case "Barangay Clearance":
                    return 100;
                case "Certification of Indigency":
                    return 120;
                case "Certification of Good Moral Character":
                    return 140;
                case "Certification of No Pending Case":
                    return 160;
                case "Certification of Solo Parent":
                    return 180;
                case "Certification of Business Residency":
                    return 200;
                default:
                    return 0;
            }
        }

        private void uploadButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg files(*.jpg)|*.jpg| PNG files(*.png)|*.png| All Files(*.*)|*.*";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    imageLocation = dialog.FileName;
                    PictureDocument.ImageLocation = imageLocation;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An error has occurred", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                string destDir = Path.Combine(Application.StartupPath, "SupportingDocuments");
                Directory.CreateDirectory(destDir);

                string extension = Path.GetExtension(imageLocation);
                string uniqueName = $"support_{DateTime.Now:yyyyMMddHHmmssfff}{extension}";
                string destPath = Path.Combine(destDir, uniqueName);

                File.Copy(imageLocation, destPath, true);

                string documentRelativePath = @"SupportingDocuments\" + uniqueName;

                // --- Database Insert ---
                string connectionString = "server=localhost;user=root;password=;database=im_etr";
                string selectedPurpose = purposeComboBox.SelectedItem.ToString();
                int fee = GetFeeForPurpose(selectedPurpose);

                using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string sql = @"INSERT INTO document_requests
                            (user_id, first_name, middle_name, last_name, suffix, dob, gender, marital_status, purok, address, mobile_number,
                            purpose, secondary_purpose, document_path, declaration, fee)
                            VALUES
                            (@user_id, @first_name, @middle_name, @last_name, @suffix, @dob, @gender, @marital_status, @purok, @address, @mobile_number,
                            @purpose, @secondary_purpose, @document_path, @declaration, @fee)";
                        long newRequestId = 0;
                        using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@user_id", userId);
                            cmd.Parameters.AddWithValue("@first_name", firstNameBox.Text);
                            cmd.Parameters.AddWithValue("@middle_name", middleNameBox.Text);
                            cmd.Parameters.AddWithValue("@last_name", lastNameBox.Text);
                            cmd.Parameters.AddWithValue("@suffix", suffixBox.Text);
                            cmd.Parameters.AddWithValue("@dob", dobComboBox.Value.ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@gender", genderComboBox.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@marital_status", maritalStatusComboBox.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@purok", purokBox.Text);
                            cmd.Parameters.AddWithValue("@address", addressBox.Text);
                            cmd.Parameters.AddWithValue("@mobile_number", mobileNumberBox.Text);
                            cmd.Parameters.AddWithValue("@purpose", selectedPurpose);
                            cmd.Parameters.AddWithValue("@secondary_purpose", secondaryPurposeComboBox.Text);
                            cmd.Parameters.AddWithValue("@document_path", documentRelativePath);
                            cmd.Parameters.AddWithValue("@declaration", agreeRB.Checked ? 1 : 0);
                            cmd.Parameters.AddWithValue("@fee", fee);

                            cmd.ExecuteNonQuery();
                            newRequestId = cmd.LastInsertedId;

                        }

                        MessageBox.Show("Document request successfully submitted!");
                        ReviewRequestForm frm = new ReviewRequestForm((int)newRequestId, userId);
                        frm.Show();
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while saving to the database:\n" + ex.Message,
                                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
        

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UserDashboardForm frm = new UserDashboardForm(userId);
            frm.Show();
            this.Hide();
        }

        private void RequestDocumentForm_Load(object sender, EventArgs e)
        {

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