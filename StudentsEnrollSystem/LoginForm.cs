using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentsEnrollSystem
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void Button_login_Click(object sender, EventArgs e)
        {
            string username = textBox_username.Text;
            string password = textBox_pass.Text;

            if (IsValidUser(username, password))
            {
                try
                {
                    // Create and show the dashboard form
                    DashboardForm dashboardForm = new DashboardForm();
                    dashboardForm.Show();

                    
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening dashboard form: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Invalid login credentials, please check username and password. try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsValidUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\vihan\Documents\Skill_international_db.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False"))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM UsersInfo WHERE username = @username";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);

                        SqlDataReader reader = command.ExecuteReader();
                        return reader.HasRows;
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
           
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure, Do you really want to exit?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
            
        }

        
    }
}
