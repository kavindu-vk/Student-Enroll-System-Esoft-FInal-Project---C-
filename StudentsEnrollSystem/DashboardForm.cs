using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentsEnrollSystem
{
    public partial class DashboardForm : Form
    {
        public DashboardForm()
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

        

        private void button3_Click(object sender, EventArgs e)
        {
            RegistrationForm registrationForm = new RegistrationForm();
            registrationForm.Show();

            this.Hide();
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private int GetStudentsCount()
        {
            // Assuming you have a connection to your database
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vihan\Documents\Skill_international_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM StudentRegistration";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        private int GetCourseCount()
        {
            // Assuming you have a connection to your database
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vihan\Documents\Skill_international_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Course";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        private int GetTeachersCount()
        {
            // Assuming you have a connection to your database
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vihan\Documents\Skill_international_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Teachers";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    return (int)cmd.ExecuteScalar();
                }
            }
        }
        private int GetParentsCount()
        {
            // Assuming you have a connection to your database
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vihan\Documents\Skill_international_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM StudentRegistration";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        private void DisplayCount()
        {
            int studentsCount = GetStudentsCount();
            label_std.Text = $"{studentsCount}";

            int courseCount = GetCourseCount();
            label_courses.Text = $"{courseCount}";

            int teachersCount = GetTeachersCount();
            label_teachers.Text = $"{teachersCount}";

            int parentsCount = GetParentsCount();
            label_parents.Text = $"{parentsCount}";
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            DisplayCount();
        }

        private void button_dash_Click(object sender, EventArgs e)
        {

        }
    }
}
