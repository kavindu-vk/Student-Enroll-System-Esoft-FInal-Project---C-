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
    public partial class ParentsForm : Form
    {
        public ParentsForm()
        {
            InitializeComponent();
            LoadParentsData();
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

        private void ParentsForm_Load(object sender, EventArgs e)
        {

        }

        private void LoadParentsData()
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vihan\Documents\Skill_international_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                try
                {
                    connection.Open();
                    string selectQuery = "SELECT regNo, parentName, nic, contactNo FROM StudentRegistration";

                    DataTable dataTable = new DataTable();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, connection))
                    {
                        adapter.Fill(dataTable);
                    }

                    DataGridView_allParents.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
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
