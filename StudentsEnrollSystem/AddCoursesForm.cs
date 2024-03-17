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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Net;
using System.Text.RegularExpressions;

namespace StudentsEnrollSystem
{
    public partial class AddCoursesForm : Form

    {

        public AddCoursesForm()
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

        private void AddCoursesForm_Load(object sender, EventArgs e)
        {
            
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            string courseId = textBox_id.Text;
            string courseName = textBox_course.Text;
            string courseLevel = comboBox_courselevel.SelectedItem.ToString();
            string coursePeriod = comboBox_period.SelectedItem.ToString();

            //create connection
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vihan\Documents\Skill_international_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(courseId) || string.IsNullOrWhiteSpace(courseName) || string.IsNullOrWhiteSpace(courseLevel) || string.IsNullOrWhiteSpace(coursePeriod))
                    {
                        MessageBox.Show("Please fill in all the fields.");
                    }
                    else
                    {
                        //open connection
                        connection.Open();

                        // Check if the email already exists
                        string courseCheckQuery = "SELECT COUNT(*) FROM Course WHERE courseId = @courseId";
                        using (SqlCommand courseCheckCommand = new SqlCommand(courseCheckQuery, connection))
                        {
                            courseCheckCommand.Parameters.AddWithValue("@courseId", courseId);
                            int existingCourseCount = (int)courseCheckCommand.ExecuteScalar();

                            if (existingCourseCount > 0)
                            {
                                MessageBox.Show("Course ID is already added. Please use a different course ID.");
                                return;
                            }
                        }

                        // Insert data into the Users table
                        string query = "INSERT INTO Course (courseId, courseName, courseLevel, coursePeriod) VALUES (@courseId, @courseName, @courseLevel, @coursePeriod)";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            //Add parameters to prevent SQL injection
                            cmd.Parameters.AddWithValue("@courseId", courseId);
                            cmd.Parameters.AddWithValue("@courseName", courseName);
                            cmd.Parameters.AddWithValue("@courseLevel", courseLevel);
                            cmd.Parameters.AddWithValue("@coursePeriod", coursePeriod);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Record added successfuly");
                                ClearFormData();
                            }
                            else
                            {
                                MessageBox.Show("Record adding failed. Please try again.");
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

        private void ClearFormData()
        {
            textBox_id.Text = string.Empty;
            textBox_course.Text = string.Empty;
            comboBox_period.SelectedIndex = -1;
            

            comboBox_courselevel.SelectedIndex = -1;
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            ClearFormData();
        }



        private void button_search_Click(object sender, EventArgs e)
        {
            string searchKeyword = textBox_search.Text;

            if (int.TryParse(searchKeyword, out int courseID))
            {
                // Search by ID
                SearchByID(courseID);
            }
            else
            {
                // Search by Name
                SearchByName(searchKeyword);
            }
        }

        private void SearchByID(int courseId)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vihan\Documents\Skill_international_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Course WHERE courseId = @courseId";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@courseId", courseId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string courseid = reader["courseId"].ToString();
                                    string courseName = reader["courseName"].ToString();
                                    string courseLevel = reader["courseLevel"].ToString();
                                    string coursePeriod = reader["coursePeriod"].ToString();

                                    // Display or use the retrieved data (for example, update text boxes)
                                    textBox_id.Text = courseid;
                                    textBox_course.Text = courseName;
                                    comboBox_courselevel.Text = courseLevel;
                                    comboBox_period.Text = coursePeriod;
                                    
                                }
                            }
                            else
                            {
                                MessageBox.Show("No data found for the selected registration number.");
                            }
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void SearchByName(string courseName)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vihan\Documents\Skill_international_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Course WHERE courseName = @courseName";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@courseName", courseName);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string courseid = reader["courseId"].ToString();
                                    string coursename = reader["courseName"].ToString();
                                    string courseLevel = reader["courseLevel"].ToString();
                                    string coursePeriod = reader["coursePeriod"].ToString();

                                    // Display or use the retrieved data (for example, update text boxes)
                                    textBox_id.Text = courseid;
                                    textBox_course.Text = coursename;
                                    comboBox_courselevel.Text = courseLevel;
                                    comboBox_period.Text = coursePeriod;

                                }
                            }
                            else
                            {
                                MessageBox.Show("No data found for the selected registration number.");
                            }
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void DeleteRecords(string courseId)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vihan\Documents\Skill_international_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                try
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM Course WHERE courseId = @courseId";

                    using (SqlCommand cmd = new SqlCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@courseId", courseId);

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
            string selectedCourseId = textBox_id.Text?.ToString();

            DialogResult result = MessageBox.Show("Are you sure you want to delete all records for the selected course ID?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DeleteRecords(selectedCourseId);
            }
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
            hideSubmenu();

            AddTeachersForm addTeachersForm = new AddTeachersForm();
            addTeachersForm.Show();

            this.Close();
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
