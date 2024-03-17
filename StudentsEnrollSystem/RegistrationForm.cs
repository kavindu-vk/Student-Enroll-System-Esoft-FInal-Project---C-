using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Net;

namespace StudentsEnrollSystem
{
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();

        }

        

        private void button_register_Click(object sender, EventArgs e)
        {
            // Get user input

            string firstName = textBox_fn.Text;
            string lastName = textBox_ln.Text;
            string birthday = dateTimePicker_db.Value.ToString("yyyy-MM-dd");
            string gender = "";
            if (radioButton_male.Checked)
            {
                gender = "Male";
            }
            else if (radioButton_female.Checked)
            {
                gender = "Female";
            }

            string address = textBox_address.Text;
            string email = textBox_email.Text;
            string mobilePhone = textBox_mp.Text;
            string homePhone = textBox_hp.Text;
            string parentName = textBox_pn.Text;
            string nic = textBox_nic.Text;
            string contact = textBox_cn.Text;

            //create connection
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vihan\Documents\Skill_international_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                try
                {
                    if(string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(birthday) || string.IsNullOrWhiteSpace(gender) || string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(mobilePhone) || string.IsNullOrWhiteSpace(homePhone) || string.IsNullOrWhiteSpace(parentName) || string.IsNullOrWhiteSpace(nic) || string.IsNullOrWhiteSpace(contact))
                    {
                        MessageBox.Show("Please fill in all the fields.");
                    } 
                    else
                    {
                        //open connection
                        connection.Open();

                        // Check if the email already exists
                        string emailCheckQuery = "SELECT COUNT(*) FROM StudentRegistration WHERE email = @email";
                        using (SqlCommand emailCheckCommand = new SqlCommand(emailCheckQuery, connection))
                        {
                            emailCheckCommand.Parameters.AddWithValue("@email", email);
                            int existingEmailCount = (int)emailCheckCommand.ExecuteScalar();

                            if (existingEmailCount > 0)
                            {
                                MessageBox.Show("Email is already registered. Please use a different email.");
                                return; 
                            }
                        }

                        // Insert data into the Users table
                        string query = "INSERT INTO StudentRegistration (firstName, lastName, dateOfBirth, gender, address, email, mobilePhone, homePhone, parentName, nic, contactNo) VALUES (@firstName, @lastName, @dateOfBirth, @gender, @address, @email, @mobilePhone, @homePhone, @parentName, @nic, @contactNo)";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            //Add parameters to prevent SQL injection
                            cmd.Parameters.AddWithValue("@firstName", firstName);
                            cmd.Parameters.AddWithValue("@lastName", lastName);
                            cmd.Parameters.AddWithValue("@dateOfBirth", birthday);
                            cmd.Parameters.AddWithValue("@gender", gender);
                            cmd.Parameters.AddWithValue("@address", address);
                            cmd.Parameters.AddWithValue("@email", email);
                            cmd.Parameters.AddWithValue("@mobilePhone", mobilePhone);
                            cmd.Parameters.AddWithValue("@homePhone", homePhone);
                            cmd.Parameters.AddWithValue("@parentName", parentName);
                            cmd.Parameters.AddWithValue("@nic", nic);
                            cmd.Parameters.AddWithValue("@contactNo", contact);

                            // Execute the query
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Record added successfuly");
                                ClearFormData();
                            }
                            else
                            {
                                MessageBox.Show("Registration failed. Please try again.");
                            }
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex.Message);
                }
            }
                
        }

        private void RegistrationForm_Load(object sender, EventArgs e)
        {
            PopulateComboBox();
        }

        private void PopulateComboBox()
        {
            // Create a list to store registration numbers
            List<string> registrationNumbers = new List<string>();
            registrationNumbers.Add(string.Empty);

            // Create connection
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vihan\Documents\Skill_international_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                try
                {
                    // Open connection
                    connection.Open();
                    string query = "SELECT regNo FROM StudentRegistration";

                    using(SqlCommand cmd = new SqlCommand(query, connection)) {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string regNo = reader["regNo"].ToString();
                                registrationNumbers.Add(regNo);
                            }
                        }
                    }

                    // Populate the ComboBox with the list of registration numbers
                    comboBox_reg.DataSource = registrationNumbers;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void comboBox_reg_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedRegNo = comboBox_reg.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedRegNo))
            {
                // Do something with the selected registration number
                FetchDataForRegistrationNumber(selectedRegNo);
            }
        }

        private void FetchDataForRegistrationNumber(string regNo)
        {
            // Create connection
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vihan\Documents\Skill_international_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM StudentRegistration WHERE regNo = @regNo";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@regNo", regNo);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check if there are rows returned
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string firstName = reader["firstName"].ToString();
                                    string lastName = reader["lastName"].ToString();
                                    string birthday = reader["dateOfBirth"].ToString();
                                    string gender = reader["gender"].ToString();

                                    if (gender.Equals("Male", StringComparison.OrdinalIgnoreCase))
                                    {
                                        radioButton_male.Checked = true;
                                        radioButton_female.Checked = false;
                                    }
                                    else if (gender.Equals("Female", StringComparison.OrdinalIgnoreCase))
                                    {
                                        radioButton_male.Checked = false;
                                        radioButton_female.Checked = true;
                                    }
                                    else
                                    {
                                        // Handle the case when the gender value is neither "Male" nor "Female"
                                        radioButton_male.Checked = false;
                                        radioButton_female.Checked = false;
                                    }

                                    string address = reader["address"].ToString();
                                    string email = reader["email"].ToString();
                                    string mobilePhone = reader["mobilePhone"].ToString();
                                    string homePhone = reader["homePhone"].ToString();
                                    string parentName = reader["parentName"].ToString();
                                    string nic = reader["nic"].ToString();
                                    string contact = reader["contactNo"].ToString();

                                    // Display or use the retrieved data (for example, update text boxes)
                                    textBox_fn.Text = firstName;
                                    textBox_ln.Text = lastName;
                                    textBox_address.Text = address;
                                    textBox_email.Text = email;
                                    textBox_mp.Text = mobilePhone;
                                    textBox_hp.Text = homePhone;
                                    textBox_pn.Text = parentName;
                                    textBox_nic.Text = nic;
                                    textBox_cn.Text = contact;
                                }
                            }
                            else
                            {
                                MessageBox.Show("No data found for the selected registration number.");
                            }
                        }
                    }
                } 
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button_update_Click(object sender, EventArgs e)
        {
            // Get the selected registration number from the ComboBox
            string selectedRegNo = comboBox_reg.SelectedItem?.ToString();

            // Get the updated values from the text boxes and radio buttons
            string updatedFirstName = textBox_fn.Text;
            string updatedLastName = textBox_ln.Text;
            string updatedGender = radioButton_male.Checked ? "Male" : "Female";
            string updatedAddress = textBox_address.Text;
            string updatedEmail = textBox_email.Text;
            string updatedMobilePhone = textBox_mp.Text;
            string updatedHomePhone = textBox_hp.Text;
            string updatedParentName = textBox_pn.Text;
            string updatedNic = textBox_nic.Text;
            string updatedContactNo = textBox_cn.Text;
            string UpdateBirthday = dateTimePicker_db.Value.ToString("yyyy-MM-dd");

            // Create connection
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vihan\Documents\Skill_international_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                try
                {
                    connection.Open();
                    string updateQuery = "UPDATE StudentRegistration SET firstName = @firstName, lastName = @lastName, dateOfBirth = @dateOfBirth, gender = @gender, address = @address, email = @email, mobilePhone = @mobilePhone, homePhone = @homePhone, parentName = @parentName, nic = @nic, contactNo = @contactNo WHERE regNo = @regNo";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@firstName", updatedFirstName);
                        cmd.Parameters.AddWithValue("@lastName", updatedLastName);
                        cmd.Parameters.AddWithValue("@gender", updatedGender);
                        cmd.Parameters.AddWithValue("@address", updatedAddress);
                        cmd.Parameters.AddWithValue("@email", updatedEmail);
                        cmd.Parameters.AddWithValue("@mobilePhone", updatedMobilePhone);
                        cmd.Parameters.AddWithValue("@homePhone", updatedHomePhone);
                        cmd.Parameters.AddWithValue("@parentName", updatedParentName);
                        cmd.Parameters.AddWithValue("@nic", updatedNic);
                        cmd.Parameters.AddWithValue("@contactNo", updatedContactNo);
                        cmd.Parameters.AddWithValue("@regNo", selectedRegNo);
                        cmd.Parameters.AddWithValue("@dateOfBirth", UpdateBirthday);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show(" Record Update successfuly");
                            ClearFormData();
                        }
                        else
                        {
                            MessageBox.Show("Update failed. Please try again.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void ClearFormData()
        {
            textBox_fn.Text = string.Empty;
            textBox_ln.Text = string.Empty;
            textBox_address.Text = string.Empty;
            textBox_email.Text = string.Empty;
            textBox_mp.Text = string.Empty;
            textBox_hp.Text = string.Empty;
            textBox_pn.Text = string.Empty;
            textBox_nic.Text = string.Empty;
            textBox_cn.Text = string.Empty;

            radioButton_male.Checked = false;
            radioButton_female.Checked = false;

            comboBox_reg.SelectedIndex = -1;
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            ClearFormData();
        }

        private void DeleteRecords(string regNo)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vihan\Documents\Skill_international_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                try
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM StudentRegistration WHERE regNo = @regNo";

                    using (SqlCommand cmd = new SqlCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@regNo", regNo);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Delete successful!");
                            // Clear form data after successful delete
                            ClearFormData();
                        }
                        else
                        {
                            MessageBox.Show("No records found for the selected registration number.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            // Get the selected registration number from the ComboBox
            string selectedRegNo = comboBox_reg.SelectedItem?.ToString();

            DialogResult result = MessageBox.Show("Are you sure you want to delete all records for the selected registration number?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DeleteRecords(selectedRegNo);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();

            this.Close();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();


            DashboardForm dashboardForm = new DashboardForm();
            dashboardForm.Show();
        }
    }
}
