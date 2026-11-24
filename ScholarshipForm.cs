using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using Microsoft.Data.Sqlite;

namespace ScholarshipManagementSystem_27109
{
    public class ScholarshipForm : Form
    {
        private TextBox txtId = new TextBox();
        private TextBox txtTitle = new TextBox();
        private TextBox txtDescription = new TextBox();
        private TextBox txtAmount = new TextBox();

        private DateTimePicker deadline = new DateTimePicker();
        private DataGridView grid = new DataGridView();

        public ScholarshipForm()
        {
            Text = "Scholarship Management";
            ClientSize = new Size(1000, 550);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            Label heading = new Label
            {
                Text = "SCHOLARSHIP MANAGEMENT",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(300, 20),
                AutoSize = true
            };

            Controls.Add(heading);

            AddField("Scholarship ID:", txtId, 90);
            AddField("Title:", txtTitle, 150);
            AddField("Description:", txtDescription, 210);
            AddField("Amount:", txtAmount, 300);
            AddField("Deadline:", deadline, 360);

            txtId.ReadOnly = true;
            txtDescription.Multiline = true;
            txtDescription.Height = 65;
            deadline.Format = DateTimePickerFormat.Short;

            grid.Location = new Point(450, 90);
            grid.Size = new Size(520, 340);
            grid.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            grid.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.MultiSelect = false;

            grid.CellClick += SelectScholarship;
            Controls.Add(grid);

            AddButton("SAVE", 30, SaveScholarship);
            AddButton("UPDATE", 180, UpdateScholarship);
            AddButton("DELETE", 330, DeleteScholarship);
            AddButton("DISPLAY", 480,
                (s, e) => LoadScholarships());
            AddButton("CLEAR", 630,
                (s, e) => ClearFields());
            AddButton("BACK", 780,
                (s, e) => Close());

            LoadScholarships();
        }

        private void AddField(
            string label, Control input, int y)
        {
            Controls.Add(new Label
            {
                Text = label,
                Location = new Point(30, y + 5),
                AutoSize = true
            });

            input.Location = new Point(160, y);
            input.Size = new Size(240, 30);

            Controls.Add(input);
        }

        private void AddButton(
            string text, int x, EventHandler action)
        {
            Button button = new Button
            {
                Text = text,
                Location = new Point(x, 470),
                Size = new Size(120, 40),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White
            };

            button.Click += action;
            Controls.Add(button);
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Complete all required fields.");
                return false;
            }

            if (!decimal.TryParse(txtAmount.Text,
                out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Enter a valid amount.");
                return false;
            }

            if (deadline.Value.Date < DateTime.Today)
            {
                MessageBox.Show("Deadline cannot be in the past.");
                return false;
            }

            return true;
        }

        private void ExecuteCommand(string sql)
        {
            using var conn = Database.GetConnection();
            conn.Open();

            using var cmd = new SqliteCommand(sql, conn);

            if (sql.Contains("@title"))
            {
                cmd.Parameters.AddWithValue(
                    "@title", txtTitle.Text.Trim());

                cmd.Parameters.AddWithValue(
                    "@description", txtDescription.Text.Trim());

                cmd.Parameters.AddWithValue(
                    "@amount", decimal.Parse(txtAmount.Text));

                cmd.Parameters.AddWithValue(
                    "@deadline",
                    deadline.Value.ToString("yyyy-MM-dd"));
            }

            if (sql.Contains("@id"))
            {
                cmd.Parameters.AddWithValue(
                    "@id", int.Parse(txtId.Text));
            }

            cmd.ExecuteNonQuery();

            MessageBox.Show("Operation successful!");

            LoadScholarships();
            ClearFields();
        }

        private void SaveScholarship(object? s, EventArgs e)
        {
            if (!ValidateFields()) return;

            ExecuteCommand(@"INSERT INTO Scholarships
                (Title, Description, Amount, Deadline)
                VALUES
                (@title, @description, @amount, @deadline)");
        }

        private void UpdateScholarship(object? s, EventArgs e)
        {
            if (txtId.Text == "")
            {
                MessageBox.Show("Select a scholarship first.");
                return;
            }

            if (!ValidateFields()) return;

            ExecuteCommand(@"UPDATE Scholarships
                SET Title = @title,
                Description = @description,
                Amount = @amount,
                Deadline = @deadline
                WHERE ScholarshipId = @id");
        }

        private void DeleteScholarship(object? s, EventArgs e)
        {
            if (txtId.Text == "")
            {
                MessageBox.Show("Select a scholarship first.");
                return;
            }

            if (MessageBox.Show(
                "Delete this scholarship?",
                "Confirm",
                MessageBoxButtons.YesNo)
                != DialogResult.Yes) return;

            try
            {
                ExecuteCommand(@"DELETE FROM Scholarships
                    WHERE ScholarshipId = @id");
            }
            catch (SqliteException)
            {
                MessageBox.Show(
                    "Cannot delete a scholarship with applications.");
            }
        }

        private void LoadScholarships()
        {
            using var conn = Database.GetConnection();
            conn.Open();

            using var cmd = new SqliteCommand(
                "SELECT * FROM Scholarships", conn);

            using var reader = cmd.ExecuteReader();

            DataTable table = new DataTable();
            table.Load(reader);

            grid.DataSource = table;
        }

        private void SelectScholarship(
            object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = grid.Rows[e.RowIndex];

            txtId.Text =
                row.Cells["ScholarshipId"].Value?.ToString();

            txtTitle.Text =
                row.Cells["Title"].Value?.ToString();

            txtDescription.Text =
                row.Cells["Description"].Value?.ToString();

            txtAmount.Text =
                row.Cells["Amount"].Value?.ToString();

            deadline.Value = DateTime.Parse(
                row.Cells["Deadline"].Value.ToString()!);
        }

        private void ClearFields()
        {
            txtId.Clear();
            txtTitle.Clear();
            txtDescription.Clear();
            txtAmount.Clear();
            deadline.Value = DateTime.Today;
        }
    }
}