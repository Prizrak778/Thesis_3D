namespace Thesis_3D
{
    partial class Thesis3DForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.glControlThesis3D = new OpenTK.GLControl();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxShaders = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button11 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonNewAnFigure = new System.Windows.Forms.Button();
            this.buttonNewFigureFile = new System.Windows.Forms.Button();
            this.buttonChangeFigure = new System.Windows.Forms.Button();
            this.buttonRemoveFigure = new System.Windows.Forms.Button();
            this.buttonTrajectory = new System.Windows.Forms.Button();
            this.labelId = new System.Windows.Forms.Label();
            this.labelIdText = new System.Windows.Forms.Label();
            this.checkBoxFps = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // glControlThesis3D
            // 
            this.glControlThesis3D.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glControlThesis3D.BackColor = System.Drawing.Color.Black;
            this.glControlThesis3D.Location = new System.Drawing.Point(9, 12);
            this.glControlThesis3D.Name = "glControlThesis3D";
            this.glControlThesis3D.Size = new System.Drawing.Size(735, 638);
            this.glControlThesis3D.TabIndex = 0;
            this.glControlThesis3D.VSync = false;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(753, 322);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBox1.Size = new System.Drawing.Size(112, 17);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "Контур объектов";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(753, 345);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(189, 17);
            this.checkBox2.TabIndex = 2;
            this.checkBox2.Text = "Движение камеры за курсором";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(750, 365);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Варианты шейдеров";
            // 
            // comboBoxShaders
            // 
            this.comboBoxShaders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxShaders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxShaders.FormattingEnabled = true;
            this.comboBoxShaders.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboBoxShaders.Location = new System.Drawing.Point(753, 381);
            this.comboBoxShaders.Name = "comboBoxShaders";
            this.comboBoxShaders.Size = new System.Drawing.Size(189, 21);
            this.comboBoxShaders.TabIndex = 4;
            this.comboBoxShaders.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(754, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 82);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Передвижение камеры";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(134, 48);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(54, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "D";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(69, 49);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(59, 23);
            this.button5.TabIndex = 4;
            this.button5.Text = "S";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(7, 49);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(56, 23);
            this.button6.TabIndex = 3;
            this.button6.Text = "A";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(134, 19);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(54, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "E";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(69, 20);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(59, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "W";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(7, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Q";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.button8);
            this.groupBox2.Controls.Add(this.button9);
            this.groupBox2.Controls.Add(this.button10);
            this.groupBox2.Controls.Add(this.button7);
            this.groupBox2.Location = new System.Drawing.Point(754, 101);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 79);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Вращение камеры";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(134, 47);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(54, 23);
            this.button8.TabIndex = 8;
            this.button8.Text = "Вправо";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(69, 48);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(59, 23);
            this.button9.TabIndex = 7;
            this.button9.Text = "Вниз";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(7, 48);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(56, 23);
            this.button10.TabIndex = 6;
            this.button10.Text = "Влево";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(69, 19);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(59, 23);
            this.button7.TabIndex = 2;
            this.button7.Text = "Вверх";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.button11);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.textBox3);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.textBox2);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Location = new System.Drawing.Point(753, 187);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 129);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Установить коорлинаты камеры";
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(125, 98);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(63, 23);
            this.button11.TabIndex = 6;
            this.button11.Text = "Ok";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "z:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(28, 71);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(161, 20);
            this.textBox3.TabIndex = 4;
            this.textBox3.Text = "0";
            this.textBox3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "y:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(28, 45);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(161, 20);
            this.textBox2.TabIndex = 2;
            this.textBox2.Text = "0";
            this.textBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "x:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(28, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(161, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "0";
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(753, 589);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Fps: ";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(753, 611);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Position:";
            // 
            // buttonNewAnFigure
            // 
            this.buttonNewAnFigure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNewAnFigure.Location = new System.Drawing.Point(753, 409);
            this.buttonNewAnFigure.Name = "buttonNewAnFigure";
            this.buttonNewAnFigure.Size = new System.Drawing.Size(189, 23);
            this.buttonNewAnFigure.TabIndex = 10;
            this.buttonNewAnFigure.Text = "Новая фигура(аналитическая)";
            this.buttonNewAnFigure.UseVisualStyleBackColor = true;
            this.buttonNewAnFigure.Click += new System.EventHandler(this.buttonNewAnFigure_Click);
            // 
            // buttonNewFigureFile
            // 
            this.buttonNewFigureFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNewFigureFile.Location = new System.Drawing.Point(753, 439);
            this.buttonNewFigureFile.Name = "buttonNewFigureFile";
            this.buttonNewFigureFile.Size = new System.Drawing.Size(189, 23);
            this.buttonNewFigureFile.TabIndex = 11;
            this.buttonNewFigureFile.Text = "Новая фигура(из файла)";
            this.buttonNewFigureFile.UseVisualStyleBackColor = true;
            this.buttonNewFigureFile.Click += new System.EventHandler(this.buttonNewFigureFile_Click);
            // 
            // buttonChangeFigure
            // 
            this.buttonChangeFigure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonChangeFigure.Location = new System.Drawing.Point(753, 469);
            this.buttonChangeFigure.Name = "buttonChangeFigure";
            this.buttonChangeFigure.Size = new System.Drawing.Size(189, 23);
            this.buttonChangeFigure.TabIndex = 12;
            this.buttonChangeFigure.Text = "Изменить фигуру";
            this.buttonChangeFigure.UseVisualStyleBackColor = true;
            this.buttonChangeFigure.Click += new System.EventHandler(this.buttonChangeFigure_Click);
            // 
            // buttonRemoveFigure
            // 
            this.buttonRemoveFigure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveFigure.Location = new System.Drawing.Point(753, 498);
            this.buttonRemoveFigure.Name = "buttonRemoveFigure";
            this.buttonRemoveFigure.Size = new System.Drawing.Size(189, 23);
            this.buttonRemoveFigure.TabIndex = 13;
            this.buttonRemoveFigure.Text = "Удалить фигуру";
            this.buttonRemoveFigure.UseVisualStyleBackColor = true;
            this.buttonRemoveFigure.Click += new System.EventHandler(this.buttonRemoveFigure_Click);
            // 
            // buttonTrajectory
            // 
            this.buttonTrajectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTrajectory.Enabled = false;
            this.buttonTrajectory.Location = new System.Drawing.Point(754, 527);
            this.buttonTrajectory.Name = "buttonTrajectory";
            this.buttonTrajectory.Size = new System.Drawing.Size(188, 23);
            this.buttonTrajectory.TabIndex = 14;
            this.buttonTrajectory.Text = "Траектория движения объекта";
            this.buttonTrajectory.UseVisualStyleBackColor = true;
            this.buttonTrajectory.Click += new System.EventHandler(this.buttonTrajectory_Click);
            // 
            // labelId
            // 
            this.labelId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelId.AutoSize = true;
            this.labelId.Location = new System.Drawing.Point(753, 633);
            this.labelId.Name = "labelId";
            this.labelId.Size = new System.Drawing.Size(19, 13);
            this.labelId.TabIndex = 15;
            this.labelId.Text = "Id:";
            this.labelId.Visible = false;
            // 
            // labelIdText
            // 
            this.labelIdText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelIdText.AutoSize = true;
            this.labelIdText.Location = new System.Drawing.Point(778, 633);
            this.labelIdText.Name = "labelIdText";
            this.labelIdText.Size = new System.Drawing.Size(10, 13);
            this.labelIdText.TabIndex = 16;
            this.labelIdText.Text = "-";
            this.labelIdText.Visible = false;
            // 
            // checkBoxFps
            // 
            this.checkBoxFps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxFps.AutoSize = true;
            this.checkBoxFps.Location = new System.Drawing.Point(756, 556);
            this.checkBoxFps.Name = "checkBoxFps";
            this.checkBoxFps.Size = new System.Drawing.Size(124, 17);
            this.checkBoxFps.TabIndex = 17;
            this.checkBoxFps.Text = "Запись Fps в файл ";
            this.checkBoxFps.UseVisualStyleBackColor = true;
            // 
            // Thesis3DForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 662);
            this.Controls.Add(this.checkBoxFps);
            this.Controls.Add(this.labelIdText);
            this.Controls.Add(this.labelId);
            this.Controls.Add(this.buttonTrajectory);
            this.Controls.Add(this.buttonRemoveFigure);
            this.Controls.Add(this.buttonChangeFigure);
            this.Controls.Add(this.buttonNewFigureFile);
            this.Controls.Add(this.buttonNewAnFigure);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.comboBoxShaders);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.glControlThesis3D);
            this.MinimumSize = new System.Drawing.Size(970, 700);
            this.Name = "Thesis3DForm";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl glControlThesis3D;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxShaders;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonNewAnFigure;
        private System.Windows.Forms.Button buttonNewFigureFile;
        private System.Windows.Forms.Button buttonChangeFigure;
        private System.Windows.Forms.Button buttonRemoveFigure;
        private System.Windows.Forms.Button buttonTrajectory;
        private System.Windows.Forms.Label labelId;
        private System.Windows.Forms.Label labelIdText;
        private System.Windows.Forms.CheckBox checkBoxFps;
    }
}

