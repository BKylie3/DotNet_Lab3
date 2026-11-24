using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace ScholarshipManagementSystem_27109
{
    public class ApplicationStatusForm : Form
    {
        private readonly string studentId;
        private DataGridView grid = new DataGridView();

        public ApplicationStatusForm(string id)
        {
            studentId = id;

            Text = "My Scholarship Applications";
            ClientSize = new Size(900, 500);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            Controls.Add(new Label
            {
                Text = "MY APPLICATION STATUS",
                Location = new Point(260, 25),
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                AutoSize = true
            });

            grid.Location = new Point(30, 100);
            grid.Size = new Size(840, 280);
            grid.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            grid.CellFormatting += FormatStatus;

            Controls.Add(grid);

            Button refresh = new Button
            {
                Text = "REFRESH",
                Location = new Point(30, 420),
                Size = new Size(150, 40)
            };

            refresh.Click += (s, e) => LoadApplications();

            Button back = new Button
            {
                Text = "BACK",
                Location = new Point(720, 420),
                Size = new Size(150, 40)
            };

            back.Click += (s, e) => Close();

            Controls.Add(refresh);
            Controls.Add(back);

            LoadApplications();
        }

        private void LoadApplications()
        {
            using var conn = Database.GetConnection();
            conn.Open();

            using var cmd = new SqliteCommand(
                @"SELECT a.ApplicationId,
                  s.Title AS Scholarship,
                  a.ApplicationDate,
                  a.Status
                  FROM Applications a
                  JOIN Scholarships s
                  ON a.ScholarshipId = s.ScholarshipId
                  WHERE a.StudentId = @id", conn);

            cmd.Parameters.AddWithValue("@id", studentId);

            using var reader = cmd.ExecuteReader();

            DataTable table = new DataTable();
            table.Load(reader);

            grid.DataSource = table;
        }

        private void FormatStatus(
            object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (grid.Columns[e.ColumnIndex].Name != "Status")
                return;

            string status = e.Value?.ToString() ?? "";

            if (status == "Approved")
                e.CellStyle.ForeColor = Color.Green;
            else if (status == "Rejected")
                e.CellStyle.ForeColor = Color.Red;
            else if (status == "Pending")
                e.CellStyle.ForeColor = Color.DarkOrange;
        }
    }
}