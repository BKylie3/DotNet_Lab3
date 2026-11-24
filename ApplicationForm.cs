using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace ScholarshipManagementSystem_27109
{
    public class ApplicationForm : Form
    {
        private readonly string studentId;
        private readonly int scholarshipId;
        private string[] selectedFiles = Array.Empty<string>();
        private ListBox documents = new ListBox();

        public ApplicationForm(string id, int sid, string title)
        {
            studentId = id;
            scholarshipId = sid;

            Text = "Scholarship Application";
            ClientSize = new Size(650, 480);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            AddLabel("SCHOLARSHIP APPLICATION", 130, 25, 18);
            AddLabel("Student ID: " + studentId, 40, 100, 12);
            AddLabel("Scholarship: " + title, 40, 145, 11);
            AddLabel("Required documents:", 40, 195, 12);

            documents.Location = new Point(40, 230);
            documents.Size = new Size(570, 100);
            Controls.Add(documents);

            AddButton("UPLOAD DOCUMENTS", 40, 350, Upload);
            AddButton("SUBMIT", 250, 350, Submit);
            AddButton("CANCEL", 460, 350, (s, e) => Close());
        }

        private void AddLabel(
            string text, int x, int y, int size)
        {
            Controls.Add(new Label
            {
                Text = text,
                Location = new Point(x, y),
                Font = new Font("Segoe UI", size),
                AutoSize = true
            });
        }

        private void AddButton(
            string text, int x, int y, EventHandler action)
        {
            Button button = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(150, 45),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White
            };

            button.Click += action;
            Controls.Add(button);
        }

        private void Upload(object? sender, EventArgs e)
        {
            using OpenFileDialog dialog = new OpenFileDialog();

            dialog.Multiselect = true;
            dialog.Filter =
                "Documents|*.pdf;*.docx;*.jpg;*.png";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                selectedFiles = dialog.FileNames;
                documents.Items.Clear();

                foreach (string file in selectedFiles)
                    documents.Items.Add(Path.GetFileName(file));
            }
        }

        private void Submit(object? sender, EventArgs e)
        {
            if (selectedFiles.Length == 0)
            {
                MessageBox.Show(
                    "Please upload the required documents.");
                return;
            }

            try
            {
                using var conn = Database.GetConnection();
                conn.Open();

                using var transaction = conn.BeginTransaction();

                using var cmd = new SqliteCommand(
                    @"INSERT INTO Applications
                    (StudentId, ScholarshipId,
                     ApplicationDate, Status)
                    SELECT @student, ScholarshipId,
                    @date, 'Pending'
                    FROM Scholarships
                    WHERE ScholarshipId = @scholarship
                    AND Deadline >= @date", conn, transaction);

                cmd.Parameters.AddWithValue(
                    "@student", studentId);

                cmd.Parameters.AddWithValue(
                    "@scholarship", scholarshipId);

                cmd.Parameters.AddWithValue(
                    "@date", DateTime.Today.ToString("yyyy-MM-dd"));

                if (cmd.ExecuteNonQuery() == 0)
                {
                    MessageBox.Show("Scholarship is no longer available.");
                    return;
                }

                using var idCommand = new SqliteCommand(
                    "SELECT last_insert_rowid()",
                    conn, transaction);

                long applicationId =
                    Convert.ToInt64(idCommand.ExecuteScalar());

                foreach (string file in selectedFiles)
                {
                    using var docCommand = new SqliteCommand(
                        @"INSERT INTO Documents
                        (ApplicationId, DocumentName, DocumentData)
                        VALUES (@id, @name, @data)",
                        conn, transaction);

                    docCommand.Parameters.AddWithValue(
                        "@id", applicationId);

                    docCommand.Parameters.AddWithValue(
                        "@name", Path.GetFileName(file));

                    docCommand.Parameters.AddWithValue(
                        "@data", File.ReadAllBytes(file));

                    docCommand.ExecuteNonQuery();
                }

                transaction.Commit();

                MessageBox.Show(
                    "Application submitted successfully!");

                Close();
            }
            catch (SqliteException ex) when (ex.SqliteExtendedErrorCode == 2067)
            {
                MessageBox.Show(
                    "You have already applied for this scholarship.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}