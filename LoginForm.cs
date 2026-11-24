using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using System.Security.Cryptography;

namespace ScholarshipManagementSystem_27109
{
    public partial class LoginForm : Form
    {
        private TextBox txtUsername = new TextBox();
        private TextBox txtPassword = new TextBox();
        private Button btnLogin = new Button();
        private Button btnRegister = new Button();

        public LoginForm()
        {
            BuildInterface();
            CreateAdmin();
        }

        private void BuildInterface()
        {
            Text = "Scholarship Management System - Login";
            ClientSize = new Size(500, 360);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;
            MaximizeBox = false;

            Label title = new Label()
            {
                Text = "SCHOLARSHIP MANAGEMENT",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(45, 30),
                AutoSize = true
            };

            Label subtitle = new Label()
            {
                Text = "Account Login",
                Font = new Font("Segoe UI", 12),
                Location = new Point(190, 75),
                AutoSize = true
            };

            Label lblUsername = new Label()
            {
                Text = "Username:",
                Location = new Point(65, 130),
                AutoSize = true
            };

            txtUsername.Location = new Point(180, 125);
            txtUsername.Size = new Size(240, 30);

            Label lblPassword = new Label()
            {
                Text = "Password:",
                Location = new Point(65, 185),
                AutoSize = true
            };

            txtPassword.Location = new Point(180, 180);
            txtPassword.Size = new Size(240, 30);
            txtPassword.UseSystemPasswordChar = true;

            btnLogin.Text = "LOGIN";
            btnLogin.Location = new Point(180, 235);
            btnLogin.Size = new Size(240, 35);
            btnLogin.BackColor = Color.SteelBlue;
            btnLogin.ForeColor = Color.White;
            btnLogin.Click += Login;

            btnRegister.Text = "REGISTER STUDENT";
            btnRegister.Location = new Point(180, 280);
            btnRegister.Size = new Size(240, 35);
            btnRegister.Click += Register;

            Controls.AddRange(new Control[]
            {
                title, subtitle,
                lblUsername, txtUsername,
                lblPassword, txtPassword,
                btnLogin, btnRegister
            });

            AcceptButton = btnLogin;
        }

        public static string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password, salt, 600000,
                HashAlgorithmName.SHA256, 32);

            return Convert.ToBase64String(salt) + ":" +
                   Convert.ToBase64String(hash);
        }

        private static bool VerifyPassword(
            string password, string stored)
        {
            string[] parts = stored.Split(':');

            if (parts.Length != 2)
                return false;

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] expected = Convert.FromBase64String(parts[1]);

            byte[] actual = Rfc2898DeriveBytes.Pbkdf2(
                password, salt, 600000,
                HashAlgorithmName.SHA256, 32);

            return CryptographicOperations.FixedTimeEquals(
                actual, expected);
        }

        private void CreateAdmin()
        {
            using var conn = Database.GetConnection();
            conn.Open();

            string sql = @"INSERT OR IGNORE INTO Users
                (Username, PasswordHash, Role)
                VALUES (@username, @password, 'Admin')";

            using var cmd = new SqliteCommand(sql, conn);

            cmd.Parameters.AddWithValue("@username", "admin");
            cmd.Parameters.AddWithValue(
                "@password", HashPassword("Admin@27109"));

            cmd.ExecuteNonQuery();
        }

        private void Login(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Enter username and password.");
                return;
            }

            string role = "";
            string studentId = "";

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string sql = @"SELECT Role, StudentId, PasswordHash
                       FROM Users
                       WHERE Username = @username";

                using var cmd = new SqliteCommand(sql, conn);

                cmd.Parameters.AddWithValue(
                    "@username", txtUsername.Text.Trim());

                using var reader = cmd.ExecuteReader();

                if (reader.Read() &&
                    VerifyPassword(txtPassword.Text,
                        reader["PasswordHash"].ToString()!))
                {
                    role = reader["Role"].ToString()!;
                    studentId = reader["StudentId"].ToString() ?? "";
                }
                else
                {
                    MessageBox.Show("Invalid username or password.");
                    return;
                }
            }

            MessageBox.Show("Login successful! Welcome " + role);

            txtPassword.Clear();

            Hide();

            try
            {
                if (role == "Admin")
                {
                    using var dashboard = new AdminDashboardForm();
                    dashboard.ShowDialog();
                }
                else if (role == "Student")
                {
                    using var dashboard =
                        new StudentDashboardForm(studentId);

                    dashboard.ShowDialog();
                }
            }
            finally
            {
                Show();
            }
        }

        private void Register(object? sender, EventArgs e)
        {
            using var form = new Form();

            form.Text = "Create Student Account";
            form.ClientSize = new Size(450, 440);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.BackColor = Color.WhiteSmoke;

            string[] labels =
            {
        "Student ID:",
        "Full Name:",
        "Age:",
        "Course:",
        "Username:",
        "Password:"
    };

            TextBox[] fields = new TextBox[6];

            for (int i = 0; i < labels.Length; i++)
            {
                form.Controls.Add(new Label
                {
                    Text = labels[i],
                    Location = new Point(25, 35 + i * 50),
                    AutoSize = true
                });

                fields[i] = new TextBox
                {
                    Location = new Point(150, 30 + i * 50),
                    Size = new Size(260, 30)
                };

                form.Controls.Add(fields[i]);
            }

            fields[5].UseSystemPasswordChar = true;

            Button register = new Button
            {
                Text = "CREATE ACCOUNT",
                Location = new Point(150, 350),
                Size = new Size(260, 40),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White
            };

            form.Controls.Add(register);

            register.Click += (s, args) =>
            {
                foreach (var field in fields)
                {
                    if (string.IsNullOrWhiteSpace(field.Text))
                    {
                        MessageBox.Show("Complete all fields.");
                        return;
                    }
                }

                if (!int.TryParse(fields[2].Text, out int age)
                    || age <= 0 || age > 120)
                {
                    MessageBox.Show("Enter a valid age.");
                    return;
                }

                if (fields[5].Text.Length < 8)
                {
                    MessageBox.Show(
                        "Password must contain at least 8 characters.");
                    return;
                }

                try
                {
                    using var conn = Database.GetConnection();
                    conn.Open();

                    using var transaction = conn.BeginTransaction();

                    using var studentCmd = new SqliteCommand(
                        @"INSERT INTO Students
                  (StudentId, Name, Age, Course)
                  VALUES (@id, @name, @age, @course)",
                        conn, transaction);

                    studentCmd.Parameters.AddWithValue(
                        "@id", fields[0].Text.Trim());

                    studentCmd.Parameters.AddWithValue(
                        "@name", fields[1].Text.Trim());

                    studentCmd.Parameters.AddWithValue(
                        "@age", age);

                    studentCmd.Parameters.AddWithValue(
                        "@course", fields[3].Text.Trim());

                    studentCmd.ExecuteNonQuery();

                    using var userCmd = new SqliteCommand(
                        @"INSERT INTO Users
                  (Username, PasswordHash, Role, StudentId)
                  VALUES (@username, @password, 'Student', @id)",
                        conn, transaction);

                    userCmd.Parameters.AddWithValue(
                        "@username", fields[4].Text.Trim());

                    userCmd.Parameters.AddWithValue(
                        "@password", HashPassword(fields[5].Text));

                    userCmd.Parameters.AddWithValue(
                        "@id", fields[0].Text.Trim());

                    userCmd.ExecuteNonQuery();

                    transaction.Commit();

                    MessageBox.Show(
                        "Student account created successfully!");

                    form.Close();
                }
                catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
                {
                    MessageBox.Show(
                        "Student ID or username already exists.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            };

            form.ShowDialog();
        }
    }
}