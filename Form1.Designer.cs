namespace ScholarshipManagementSystem_27109
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            txtStudentId = new TextBox();
            txtName = new TextBox();
            txtAge = new TextBox();
            cmbCourse = new ComboBox();
            btnSave = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            btnDisplay = new Button();
            dgvStudents = new DataGridView();
            label5 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvStudents).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(73, 98);
            label1.Name = "label1";
            label1.Size = new Size(78, 20);
            label1.TabIndex = 0;
            label1.Text = "StudentID:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(73, 183);
            label2.Name = "label2";
            label2.Size = new Size(52, 20);
            label2.TabIndex = 1;
            label2.Text = "Name:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(73, 260);
            label3.Name = "label3";
            label3.Size = new Size(39, 20);
            label3.TabIndex = 2;
            label3.Text = "Age:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(73, 339);
            label4.Name = "label4";
            label4.Size = new Size(57, 20);
            label4.TabIndex = 3;
            label4.Text = "Course:";
            // 
            // txtStudentId
            // 
            txtStudentId.Location = new Point(208, 91);
            txtStudentId.Name = "txtStudentId";
            txtStudentId.Size = new Size(158, 27);
            txtStudentId.TabIndex = 5;
            // 
            // txtName
            // 
            txtName.Location = new Point(208, 176);
            txtName.Name = "txtName";
            txtName.Size = new Size(158, 27);
            txtName.TabIndex = 6;
            // 
            // txtAge
            // 
            txtAge.Location = new Point(208, 253);
            txtAge.Name = "txtAge";
            txtAge.Size = new Size(158, 27);
            txtAge.TabIndex = 7;
            // 
            // cmbCourse
            // 
            cmbCourse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCourse.FormattingEnabled = true;
            cmbCourse.Items.AddRange(new object[] { "Software Engineering", "", "Information Technology", "", "Information Management", "", "Accounting", "", "Finance", "", "Business Administration" });
            cmbCourse.Location = new Point(208, 331);
            cmbCourse.Name = "cmbCourse";
            cmbCourse.Size = new Size(158, 28);
            cmbCourse.TabIndex = 8;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(73, 424);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(94, 29);
            btnSave.TabIndex = 9;
            btnSave.Text = "SAVE";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(191, 424);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(94, 29);
            btnUpdate.TabIndex = 10;
            btnUpdate.Text = "UPDATE";
            btnUpdate.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(305, 424);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(94, 29);
            btnDelete.TabIndex = 11;
            btnDelete.Text = "DELETE";
            btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnDisplay
            // 
            btnDisplay.Location = new Point(663, 424);
            btnDisplay.Name = "btnDisplay";
            btnDisplay.Size = new Size(94, 29);
            btnDisplay.TabIndex = 12;
            btnDisplay.Text = "DISPLAY";
            btnDisplay.UseVisualStyleBackColor = true;
            // 
            // dgvStudents
            // 
            dgvStudents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvStudents.Location = new Point(484, 91);
            dgvStudents.Name = "dgvStudents";
            dgvStudents.RowHeadersWidth = 51;
            dgvStudents.Size = new Size(414, 268);
            dgvStudents.TabIndex = 13;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(421, 37);
            label5.Name = "label5";
            label5.Size = new Size(144, 20);
            label5.TabIndex = 14;
            label5.Text = "Student Registration";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(982, 553);
            Controls.Add(label5);
            Controls.Add(dgvStudents);
            Controls.Add(btnDisplay);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnSave);
            Controls.Add(cmbCourse);
            Controls.Add(txtAge);
            Controls.Add(txtName);
            Controls.Add(txtStudentId);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "\tScholarship Management System";
            ((System.ComponentModel.ISupportInitialize)dgvStudents).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox txtStudentId;
        private TextBox txtName;
        private TextBox txtAge;
        private ComboBox cmbCourse;
        private Button btnSave;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnDisplay;
        private DataGridView dgvStudents;
        private Label label5;
    }
}
