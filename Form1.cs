using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Windows.Forms;

namespace ScholarshipManagementSystem_27109
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            btnSave.Click += SaveStudent;
            btnUpdate.Click += UpdateStudent;
            btnDelete.Click += DeleteStudent;
            btnDisplay.Click += DisplayStudents;

            dgvStudents.CellClick += SelectStudent;

            dgvStudents.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvStudents.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvStudents.ReadOnly = true;
            dgvStudents.AllowUserToAddRows = false;
            dgvStudents.MultiSelect = false;

            LoadStudents();
        }

        // Validate input
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtStudentId.Text) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtAge.Text) ||
                cmbCourse.SelectedIndex == -1)
            {
                MessageBox.Show("Please complete all fields.");
                return false;
            }

            if (!int.TryParse(txtAge.Text, out int age) ||
                age <= 0 || age > 120)
            {
                MessageBox.Show("Please enter a valid age.");
                return false;
            }

            return true;
        }

        // SAVE
        private void SaveStudent(object? sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                using var conn = Database.GetConnection();
                conn.Open();

                string sql = @"INSERT INTO Students
                    (StudentId, Name, Age, Course)
                    VALUES (@id, @name, @age, @course)";

                using var cmd = new SqliteCommand(sql, conn);

                cmd.Parameters.AddWithValue(
                    "@id", txtStudentId.Text.Trim());

                cmd.Parameters.AddWithValue(
                    "@name", txtName.Text.Trim());

                cmd.Parameters.AddWithValue(
                    "@age", int.Parse(txtAge.Text));

                cmd.Parameters.AddWithValue(
                    "@course", cmbCourse.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Student saved successfully!");

                LoadStudents();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // UPDATE
        private void UpdateStudent(object? sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                using var conn = Database.GetConnection();
                conn.Open();

                string sql = @"UPDATE Students
                    SET Name = @name,
                        Age = @age,
                        Course = @course
                    WHERE StudentId = @id";

                using var cmd = new SqliteCommand(sql, conn);

                cmd.Parameters.AddWithValue(
                    "@id", txtStudentId.Text.Trim());

                cmd.Parameters.AddWithValue(
                    "@name", txtName.Text.Trim());

                cmd.Parameters.AddWithValue(
                    "@age", int.Parse(txtAge.Text));

                cmd.Parameters.AddWithValue(
                    "@course", cmbCourse.Text);

                int rows = cmd.ExecuteNonQuery();

                MessageBox.Show(rows > 0
                    ? "Student updated successfully!"
                    : "Student not found.");

                LoadStudents();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // DELETE
        private void DeleteStudent(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStudentId.Text))
            {
                MessageBox.Show("Select a student first.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this student?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes) return;

            try
            {
                using var conn = Database.GetConnection();
                conn.Open();

                string sql = @"DELETE FROM Students
                    WHERE StudentId = @id";

                using var cmd = new SqliteCommand(sql, conn);

                cmd.Parameters.AddWithValue(
                    "@id", txtStudentId.Text.Trim());

                int rows = cmd.ExecuteNonQuery();

                MessageBox.Show(rows > 0
                    ? "Student deleted successfully!"
                    : "Student not found.");

                LoadStudents();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // DISPLAY
        private void DisplayStudents(object? sender, EventArgs e)
        {
            LoadStudents();
        }

        // Load records into DataGridView
        private void LoadStudents()
        {
            try
            {
                using var conn = Database.GetConnection();
                conn.Open();

                string sql = "SELECT * FROM Students";

                using var cmd = new SqliteCommand(sql, conn);
                using var reader = cmd.ExecuteReader();

                DataTable table = new DataTable();
                table.Load(reader);

                dgvStudents.DataSource = table;
                dgvStudents.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Select a student from the table
        private void SelectStudent(
            object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row =
                dgvStudents.Rows[e.RowIndex];

            txtStudentId.Text =
                row.Cells["StudentId"].Value?.ToString();

            txtName.Text =
                row.Cells["Name"].Value?.ToString();

            txtAge.Text =
                row.Cells["Age"].Value?.ToString();

            cmbCourse.Text =
                row.Cells["Course"].Value?.ToString();
        }

        // Clear input fields
        private void ClearFields()
        {
            txtStudentId.Clear();
            txtName.Clear();
            txtAge.Clear();
            cmbCourse.SelectedIndex = -1;
            dgvStudents.ClearSelection();
            txtStudentId.Focus();
        }
    }
}