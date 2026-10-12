namespace TiemTapHoa
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
            textBox1 = new TextBox();
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            numericUpDown1 = new NumericUpDown();
            numericUpDown2 = new NumericUpDown();
            dateTimePicker1 = new DateTimePicker();
            checkBox1 = new CheckBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            dataGridView1 = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(86, 28);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(503, 27);
            textBox1.TabIndex = 0;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(86, 75);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(151, 28);
            comboBox1.TabIndex = 1;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(86, 129);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(151, 28);
            comboBox2.TabIndex = 2;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(87, 183);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(150, 27);
            numericUpDown1.TabIndex = 3;
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new Point(366, 76);
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(150, 27);
            numericUpDown2.TabIndex = 4;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(366, 127);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(250, 27);
            dateTimePicker1.TabIndex = 5;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(366, 183);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(134, 24);
            checkBox1.TabIndex = 6;
            checkBox1.Text = "Còn kinh doanh";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(9, 35);
            label1.Name = "label1";
            label1.Size = new Size(69, 20);
            label1.TabIndex = 7;
            label1.Text = "Tên hàng";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(-1, 83);
            label2.Name = "label2";
            label2.Size = new Size(81, 20);
            label2.TabIndex = 8;
            label2.Text = "Loại hàng: ";
            label2.Click += label2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(9, 137);
            label3.Name = "label3";
            label3.Size = new Size(59, 20);
            label3.TabIndex = 9;
            label3.Text = "Đơn vị: ";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(9, 190);
            label4.Name = "label4";
            label4.Size = new Size(67, 20);
            label4.TabIndex = 10;
            label4.Text = "Giá bán: ";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(258, 78);
            label5.Name = "label5";
            label5.Size = new Size(102, 20);
            label5.TabIndex = 11;
            label5.Text = "Số lượng tồn: ";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(266, 132);
            label6.Name = "label6";
            label6.Size = new Size(88, 20);
            label6.TabIndex = 12;
            label6.Text = "Ngày nhập: ";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(33, 230);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(729, 199);
            dataGridView1.TabIndex = 13;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dataGridView1);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(checkBox1);
            Controls.Add(dateTimePicker1);
            Controls.Add(numericUpDown2);
            Controls.Add(numericUpDown1);
            Controls.Add(comboBox2);
            Controls.Add(comboBox1);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private DateTimePicker dateTimePicker1;
        private CheckBox checkBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private DataGridView dataGridView1;
    }
}
