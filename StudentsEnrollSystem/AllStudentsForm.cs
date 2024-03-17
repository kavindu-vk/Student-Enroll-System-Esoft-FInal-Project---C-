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
    public partial class AllStudentsForm : Form
    {
        public AllStudentsForm()
        {
            InitializeComponent();
            LoadStudentData();
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

        private void AllStudentsForm_Load(object sender, EventArgs e)
        {

        }

        private void LoadStudentData()
        {

            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vihan\Documents\Skill_international_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                try
                {
                    connection.Open();
                    string selectQuery = "SELECT regNo, firstName as F_Name, lastName as L_Name, dateOfBirth as Birthday, gender as Gender, email as Email, mobilePhone as MobilePhone FROM StudentRegistration";

                    DataTable dataTable = new DataTable();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, connection))
                    {
                        adapter.Fill(dataTable);
                    }

                    DataGridView_allStudents.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            RegistrationForm registrationForm = new RegistrationForm();
            registrationForm.Show();

            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
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

        private void button_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();


            DashboardForm dashboardForm = new DashboardForm();
            dashboardForm.Show();
        }
    }
}
