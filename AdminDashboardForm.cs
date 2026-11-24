using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using Microsoft.Data.Sqlite;

namespace ScholarshipManagementSystem_27109
{
    public class AdminDashboardForm : Form
    {
        private DataGridView grid = new DataGridView();

        public AdminDashboardForm()
        {
            Text = "Administrator Dashboard";
            ClientSize = new Size(1000, 600);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            Label title = new Label
            {
                Text = "ADMINISTRATOR DASHBOARD",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Location = new Point(300, 25),
                AutoSize = true
            };

            Button students = MakeButton(
                "Manage Students", 30, 100);

            Button scholarships = MakeButton(
                "Manage Scholarships", 220, 100);

            Button display = MakeButton(
                "View Applications", 410, 100);

            Button approve = MakeButton(
                "Approve", 30, 510);

            Button reject = MakeButton(
                "Reject", 220, 510);

            Button logout = MakeButton(
                "Logout", 790, 100);

            grid.Location = new Point(30, 160);
            grid.Size = new Size(940, 320);
            grid.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            grid.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            grid.ReadOnly = true;
            grid.MultiSelect = false;
            grid.AllowUserToAddRows = false;

            students.Click += (s, e) =>
            {
                using var form = new Form1();
                form.ShowDialog();
            };

            scholarships.Click += (s, e) =>
            {
                using var form = new ScholarshipForm();
                form.ShowDialog();
                LoadApplications();
            };

            display.Click += (s, e) =>
                LoadApplications();

            approve.Click += (s, e) =>
                ChangeStatus("Approved");

            reject.Click += (s, e) =>
                ChangeStatus("Rejected");

            logout.Click += (s, e) => Close();

            Controls.AddRange(new Control[]
            {
                title, students, scholarships,
                display, approve, reject, logout, grid
            });

            LoadApplications();
        }

        private Button MakeButton(string text, int x, int y)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(170, 40),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
        }

        private void LoadApplications()
        {
            using var conn = Database.GetConnection();
            conn.Open();

            string sql = @"SELECT a.ApplicationId,
                s.StudentId, s.Name,
                sc.Title AS Scholarship,
                a.ApplicationDate, a.Status
                FROM Applications a
                JOIN Students s
                ON a.StudentId = s.StudentId
                JOIN Scholarships sc
                ON a.ScholarshipId = sc.ScholarshipId";

            using var cmd = new SqliteCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            DataTable table = new DataTable();
            table.Load(reader);

            grid.DataSource = table;
        }

        private void ChangeStatus(string status)
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select an application first.");
                return;
            }

            int id = Convert.ToInt32(
                grid.SelectedRows[0]
                .Cells["ApplicationId"].Value);

            using var conn = Database.GetConnection();
            conn.Open();

            using var cmd = new SqliteCommand(
                @"UPDATE Applications
                SET Status = @status
                WHERE ApplicationId = @id
                AND Status = 'Pending'", conn);

            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@id", id);

            int rows = cmd.ExecuteNonQuery();

            MessageBox.Show(rows > 0
                ? "Application " + status + " successfully."
                : "Application has already been reviewed.");

            LoadApplications();
        }
    }
}