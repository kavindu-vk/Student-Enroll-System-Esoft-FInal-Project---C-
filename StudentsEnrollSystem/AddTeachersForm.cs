using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace StudentsEnrollSystem
{
    public partial class AddTeachersForm : Form
    {
        public AddTeachersForm()
        {
            InitializeComponent();
            customizeDesign();
        }

        private void customizeDesign()
        {
            panel_std_menu.Visible = false;
            panel_teacher_menu.Visible = false;
            panel_course_menu.Visible = false;
        }

        private void hideSubmenu()
        {
            if (panel_std_menu.Visible == true)
                panel_std_menu.Visible = false;
            if (panel_teacher_menu.Visible == true)
                panel_teacher_menu.Visible = false;
            if (panel_course_menu.Visible == true)
                panel_course_menu.Visible = false;
        }

        private void showSubmenu(Panel submenu)
        {
            if (submenu.Visible == false)
            {
                hideSubmenu();
                submenu.Visible = true;
            }
            else
                submenu.Visible = false;
        }

        private void AddTeachersForm_Load(object sender, EventArgs e)
        {
            PopulateComboBox();
            PopulateCourseComboBox();
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            // Get user input

            string fullname = textBox_fn.Text;
            string birthday = dateTimePicker_bd.Value.ToString("yyyy-MM-dd");
            string gender = textBox_gender.Text;
            string mobileNumber = textBox_mn.Text;
            string course = "";

            if (comboBox_course.SelectedItem != null)
            {
                course = comboBox_course.SelectedItem.ToString();
            }
            else
            {
                MessageBox.Show("Please select a course");
            }


            string address = textBox_address.Text;
            string email = textBox_email.Text;

            //create connection
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vihan\Documents\Skill_international_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                try
                {
                    if(string.IsNullOrWhiteSpace(fullname) || string.IsNullOrWhiteSpace(birthday) || string.IsNullOrWhiteSpace(birthday) || string.IsNullOrWhiteSpace(gender) || string.IsNullOrWhiteSpace(mobileNumber) || string.IsNullOrWhiteSpace(course) || string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(email))
                    {
                        MessageBox.Show("Please fill in all the fields.");
                    }
                    else
                    {
                        connection.Open();
                        // Check if the email already exists
                        string emailCheckQuery = "SELECT COUNT(*) FROM Teachers WHERE email = @email";
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
                        string query = "INSERT INTO Teachers (fullname, dateOfBirth, gender, mobileNumber, course, address, email) VALUES (@fullname, @dateOfBirth, @gender, @mobileNumber, @course, @address, @email)";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            //Add parameters to prevent SQL injection
                            cmd.Parameters.AddWithValue("@fullname", fullname);
                            cmd.Parameters.AddWithValue("@dateOfBirth", birthday);
                            cmd.Parameters.AddWithValue("@gender", gender);
                            cmd.Parameters.AddWithValue("@mobileNumber", mobileNumber);
                            cmd.Parameters.AddWithValue("@course", course);
                            cmd.Parameters.AddWithValue("@address", address);
                            cmd.Parameters.AddWithValue("@email", email);

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
                    string query = "SELECT RegNum FROM Teachers";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string regNum = reader["RegNum"].ToString();
                                registrationNumbers.Add(regNum);
                            }
                        }
                    }

                    // Populate the ComboBox with the list of registration numbers
                    comboBox_regNo.DataSource = registrationNumbers;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void PopulateCourseComboBox()
        {
            // Create a list to store registration numbers
            List<string> courseInfoList = new List<string>();
            courseInfoList.Add(string.Empty);

            // Create connection
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vihan\Documents\Skill_international_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                try
                {
                    // Open connection
                    connection.Open();
                    string query = "SELECT courseName, courseLevel FROM Course";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string courseInfo = $"{reader["courseName"]} - {reader["courseLevel"]}";
                                courseInfoList.Add(courseInfo);
                            }
                        }
                    }

                    // Populate the ComboBox with the list of registration numbers
                    comboBox_course.DataSource = courseInfoList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void comboBox_regNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedRegNo = comboBox_regNo.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedRegNo))
            {
                // Do something with the selected registration number
                FetchDataForRegistrationNumber(selectedRegNo);
            }
        }

        private void FetchDataForRegistrationNumber(string regNo)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vihan\Documents\Skill_international_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Teachers WHERE RegNum = @RegNum";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@RegNum", regNo);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string fullname = reader["fullname"].ToString();
                                    string birthday = reader["dateOfBirth"].ToString();
                                    string gender = reader["gender"].ToString();
                                    string mobileNumber = reader["mobileNumber"].ToString();
                                    string course = reader["course"].ToString();
                                    string address = reader["address"].ToString();
                                    string email = reader["email"].ToString();

                                    

                                    // Display or use the retrieved data (for example, update text boxes)
                                    textBox_fn.Text = fullname;
                                    dateTimePicker_bd.Text = birthday;
                                    textBox_gender.Text = gender;
                                    textBox_mn.Text = mobileNumber;
                                    comboBox_course.Text = course;
                                    textBox_address.Text = address;
                                    textBox_email.Text = email;
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
            string selectedRegNo = comboBox_regNo.SelectedItem?.ToString();

            string UpdateFullname = textBox_fn.Text;
            string UpdateBirthday = dateTimePicker_bd.Value.ToString("yyyy-MM-dd");
            string UpdateGender = textBox_gender.Text;
            string UpdateMobileNumber = textBox_mn.Text;
            string UpdateCourse = comboBox_course.Text;
            string UpdateAddress = textBox_address.Text;
            string UpdateEmail = textBox_email.Text;

            // Create connection
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vihan\Documents\Skill_international_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                try
                {
                    connection.Open();
                    string updateQuery = "UPDATE Teachers SET fullname = @fullname, dateOfBirth = @dateOfBirth, gender = @gender, course = @course, address = @address, email = @email, mobileNumber = @mobileNumber WHERE regNum = @regNum";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@fullname", UpdateFullname);
                        cmd.Parameters.AddWithValue("@dateOfBirth", UpdateBirthday);
                        cmd.Parameters.AddWithValue("@gender", UpdateGender);
                        cmd.Parameters.AddWithValue("@mobileNumber", UpdateMobileNumber);
                        cmd.Parameters.AddWithValue("@course", UpdateCourse);
                        cmd.Parameters.AddWithValue("@address", UpdateAddress);
                        cmd.Parameters.AddWithValue("@email", UpdateEmail);
                        cmd.Parameters.AddWithValue("@regNum", selectedRegNo);

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
            dateTimePicker_bd.Text = string.Empty;
            textBox_gender.Text = string.Empty;
            textBox_mn.Text = string.Empty;
            comboBox_course.Text = string.Empty;
            textBox_address.Text = string.Empty;
            textBox_email.Text = string.Empty;

            comboBox_regNo.SelectedIndex = -1;
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            ClearFormData();
        }

        private void DeleteRecords(string regNum)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vihan\Documents\Skill_international_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                try
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM Teachers WHERE regNum = @regNum";

                    using (SqlCommand cmd = new SqlCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@regNum", regNum);

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
            string selectedRegNo = comboBox_regNo.SelectedItem?.ToString();

            DialogResult result = MessageBox.Show("Are you sure you want to delete all records for the selected registration number?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DeleteRecords(selectedRegNo);
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox_email_TextChanged(object sender, EventArgs e)
        {

        }

        private void button_std_Click(object sender, EventArgs e)
        {
            showSubmenu(panel_std_menu);
        }

        private void button_teacher_Click(object sender, EventArgs e)
        {
            showSubmenu(panel_teacher_menu);
        }

        private void button_courses_Click(object sender, EventArgs e)
        {
            showSubmenu(panel_course_menu);
        }

        private void button_std_reg_Click(object sender, EventArgs e)
        {
            RegistrationForm registrationForm = new RegistrationForm();
            registrationForm.Show();

            this.Hide();
        }

        private void button_all_students_Click(object sender, EventArgs e)
        {
            hideSubmenu();

            AllStudentsForm allStudentsForm = new AllStudentsForm();
            allStudentsForm.Show();

            this.Close();
        }

        private void button_techer_add_Click(object sender, EventArgs e)
        {
            
        }

        private void button_teacher_all_Click(object sender, EventArgs e)
        {

            hideSubmenu();

            AllTeachersForm allTeachersForm = new AllTeachersForm();
            allTeachersForm.Show();

            this.Close();
        }

        private void button_addCourses_Click(object sender, EventArgs e)
        {
            hideSubmenu();

            AddCoursesForm addCoursesForm = new AddCoursesForm();
            addCoursesForm.Show();

            this.Close();
        }

        private void button_allCourses_Click(object sender, EventArgs e)
        {
            hideSubmenu();

            AllCoursesForm allCoursesForm = new AllCoursesForm();
            allCoursesForm.Show();

            this.Close();
        }

        private void button_parents_Click(object sender, EventArgs e)
        {
            ParentsForm parentsForm = new ParentsForm();
            parentsForm.Show();

            this.Close();
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button_dash_Click(object sender, EventArgs e)
        {
            this.Close();


            DashboardForm dashboardForm = new DashboardForm();
            dashboardForm.Show();
        }
    }
}
