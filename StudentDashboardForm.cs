using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace ScholarshipManagementSystem_27109
{
    public class StudentDashboardForm : Form
    {
        private readonly string studentId;
        private DataGridView grid = new DataGridView();

        public StudentDashboardForm(string id)
        {
            studentId = id;

            Text = "Student Dashboard";
            ClientSize = new Size(1000, 550);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            Controls.Add(new Label
            {
                Text = "AVAILABLE SCHOLARSHIPS",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Location = new Point(290, 25),
                AutoSize = true
            });

            Controls.Add(new Label
            {
                Text = "Student ID: " + studentId,
                Location = new Point(30, 80),
                AutoSize = true
            });

            grid.Location = new Point(30, 120);
            grid.Size = new Size(940, 330);
            grid.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            grid.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            grid.ReadOnly = true;
            grid.MultiSelect = false;
            grid.AllowUserToAddRows = false;

            Controls.Add(grid);

            AddButton("APPLY", 30, (s, e) =>
            {
                if (grid.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Select a scholarship.");
                    return;
                }

                var row = grid.SelectedRows[0];

                int scholarshipId = Convert.ToInt32(
                    row.Cells["ScholarshipId"].Value);

                string title =
                    row.Cells["Title"].Value.ToString()!;

                using var form = new ApplicationForm(
                    studentId, scholarshipId, title);

                form.ShowDialog();
                LoadScholarships();
            });

            AddButton("MY APPLICATIONS", 260, (s, e) =>
            {
                using var form =
                    new ApplicationStatusForm(studentId);

                form.ShowDialog();
            });

            AddButton("REFRESH", 490,
                (s, e) => LoadScholarships());

            AddButton("LOGOUT", 720,
                (s, e) => Close());

            LoadScholarships();
        }

        private void AddButton(
            string text, int x, EventHandler action)
        {
            Button button = new Button
            {
                Text = text,
                Location = new Point(x, 480),
                Size = new Size(200, 40),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White
            };

            button.Click += action;
            Controls.Add(button);
        }

        private void LoadScholarships()
        {
            using var conn = Database.GetConnection();
            conn.Open();

            using var cmd = new SqliteCommand(
                @"SELECT ScholarshipId, Title,
                  Description, Amount, Deadline
                  FROM Scholarships
                  WHERE Deadline >= @today", conn);

            cmd.Parameters.AddWithValue(
                "@today", DateTime.Today.ToString("yyyy-MM-dd"));

            using var reader = cmd.ExecuteReader();

            DataTable table = new DataTable();
            table.Load(reader);

            grid.DataSource = table;
        }
    }
}