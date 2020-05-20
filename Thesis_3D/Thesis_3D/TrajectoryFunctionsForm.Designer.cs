namespace Thesis_3D
{
    partial class TrajectoryFunctionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelX = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxCoeffX = new System.Windows.Forms.TextBox();
            this.textBoxCoeffY = new System.Windows.Forms.TextBox();
            this.labelY = new System.Windows.Forms.Label();
            this.textBoxCoeffZ = new System.Windows.Forms.TextBox();
            this.labelZ = new System.Windows.Forms.Label();
            this.comboBoxFuncX = new System.Windows.Forms.ComboBox();
            this.comboBoxFuncY = new System.Windows.Forms.ComboBox();
            this.comboBoxFuncZ = new System.Windows.Forms.ComboBox();
            this.textBoxStepX = new System.Windows.Forms.TextBox();
            this.textBoxStepY = new System.Windows.Forms.TextBox();
            this.textBoxStepZ = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxMinX = new System.Windows.Forms.TextBox();
            this.textBoxMaxX = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxMaxZ = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxMinZ = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxMaxY = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxMinY = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxStartValY = new System.Windows.Forms.TextBox();
            this.textBoxStartValZ = new System.Windows.Forms.TextBox();
            this.textBoxStartValX = new System.Windows.Forms.TextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.checkBoxMinMaxValX = new System.Windows.Forms.CheckBox();
            this.checkBoxMinMaxValY = new System.Windows.Forms.CheckBox();
            this.checkBoxMinMaxValZ = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(12, 39);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(14, 13);
            this.labelX.TabIndex = 0;
            this.labelX.Text = "X";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Коэффициент";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(145, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Функция";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(248, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Шаг";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(405, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Диапозон";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(647, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Начальное значение";
            // 
            // textBoxCoeffX
            // 
            this.textBoxCoeffX.Location = new System.Drawing.Point(42, 36);
            this.textBoxCoeffX.Name = "textBoxCoeffX";
            this.textBoxCoeffX.Size = new System.Drawing.Size(100, 20);
            this.textBoxCoeffX.TabIndex = 6;
            this.textBoxCoeffX.Text = "1";
            this.textBoxCoeffX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxCoeffX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCoeffX_KeyPress);
            // 
            // textBoxCoeffY
            // 
            this.textBoxCoeffY.Location = new System.Drawing.Point(42, 62);
            this.textBoxCoeffY.Name = "textBoxCoeffY";
            this.textBoxCoeffY.Size = new System.Drawing.Size(100, 20);
            this.textBoxCoeffY.TabIndex = 8;
            this.textBoxCoeffY.Text = "1";
            this.textBoxCoeffY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxCoeffY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCoeffX_KeyPress);
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Location = new System.Drawing.Point(12, 65);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(14, 13);
            this.labelY.TabIndex = 7;
            this.labelY.Text = "Y";
            // 
            // textBoxCoeffZ
            // 
            this.textBoxCoeffZ.Location = new System.Drawing.Point(42, 88);
            this.textBoxCoeffZ.Name = "textBoxCoeffZ";
            this.textBoxCoeffZ.Size = new System.Drawing.Size(100, 20);
            this.textBoxCoeffZ.TabIndex = 10;
            this.textBoxCoeffZ.Text = "1";
            this.textBoxCoeffZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxCoeffZ.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCoeffX_KeyPress);
            // 
            // labelZ
            // 
            this.labelZ.AutoSize = true;
            this.labelZ.Location = new System.Drawing.Point(12, 91);
            this.labelZ.Name = "labelZ";
            this.labelZ.Size = new System.Drawing.Size(14, 13);
            this.labelZ.TabIndex = 9;
            this.labelZ.Text = "Z";
            // 
            // comboBoxFuncX
            // 
            this.comboBoxFuncX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFuncX.FormattingEnabled = true;
            this.comboBoxFuncX.Location = new System.Drawing.Point(148, 36);
            this.comboBoxFuncX.Name = "comboBoxFuncX";
            this.comboBoxFuncX.Size = new System.Drawing.Size(97, 21);
            this.comboBoxFuncX.TabIndex = 11;
            // 
            // comboBoxFuncY
            // 
            this.comboBoxFuncY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFuncY.FormattingEnabled = true;
            this.comboBoxFuncY.Location = new System.Drawing.Point(148, 61);
            this.comboBoxFuncY.Name = "comboBoxFuncY";
            this.comboBoxFuncY.Size = new System.Drawing.Size(97, 21);
            this.comboBoxFuncY.TabIndex = 12;
            // 
            // comboBoxFuncZ
            // 
            this.comboBoxFuncZ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFuncZ.FormattingEnabled = true;
            this.comboBoxFuncZ.Location = new System.Drawing.Point(148, 87);
            this.comboBoxFuncZ.Name = "comboBoxFuncZ";
            this.comboBoxFuncZ.Size = new System.Drawing.Size(97, 21);
            this.comboBoxFuncZ.TabIndex = 13;
            // 
            // textBoxStepX
            // 
            this.textBoxStepX.Location = new System.Drawing.Point(251, 36);
            this.textBoxStepX.Name = "textBoxStepX";
            this.textBoxStepX.Size = new System.Drawing.Size(100, 20);
            this.textBoxStepX.TabIndex = 14;
            this.textBoxStepX.Text = "1";
            this.textBoxStepX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxStepX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCoeffX_KeyPress);
            // 
            // textBoxStepY
            // 
            this.textBoxStepY.Location = new System.Drawing.Point(251, 62);
            this.textBoxStepY.Name = "textBoxStepY";
            this.textBoxStepY.Size = new System.Drawing.Size(100, 20);
            this.textBoxStepY.TabIndex = 15;
            this.textBoxStepY.Text = "1";
            this.textBoxStepY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxStepY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCoeffX_KeyPress);
            // 
            // textBoxStepZ
            // 
            this.textBoxStepZ.Location = new System.Drawing.Point(251, 88);
            this.textBoxStepZ.Name = "textBoxStepZ";
            this.textBoxStepZ.Size = new System.Drawing.Size(100, 20);
            this.textBoxStepZ.TabIndex = 16;
            this.textBoxStepZ.Text = "1";
            this.textBoxStepZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxStepZ.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCoeffX_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(380, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Min:";
            // 
            // textBoxMinX
            // 
            this.textBoxMinX.Location = new System.Drawing.Point(408, 36);
            this.textBoxMinX.Name = "textBoxMinX";
            this.textBoxMinX.Size = new System.Drawing.Size(100, 20);
            this.textBoxMinX.TabIndex = 18;
            this.textBoxMinX.Text = "0";
            this.textBoxMinX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxMinX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCoeffX_KeyPress);
            // 
            // textBoxMaxX
            // 
            this.textBoxMaxX.Location = new System.Drawing.Point(544, 36);
            this.textBoxMaxX.Name = "textBoxMaxX";
            this.textBoxMaxX.Size = new System.Drawing.Size(100, 20);
            this.textBoxMaxX.TabIndex = 20;
            this.textBoxMaxX.Text = "1";
            this.textBoxMaxX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxMaxX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCoeffX_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(516, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Max:";
            // 
            // textBoxMaxZ
            // 
            this.textBoxMaxZ.Location = new System.Drawing.Point(544, 88);
            this.textBoxMaxZ.Name = "textBoxMaxZ";
            this.textBoxMaxZ.Size = new System.Drawing.Size(100, 20);
            this.textBoxMaxZ.TabIndex = 24;
            this.textBoxMaxZ.Text = "1";
            this.textBoxMaxZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxMaxZ.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCoeffX_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(516, 90);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Max:";
            // 
            // textBoxMinZ
            // 
            this.textBoxMinZ.Location = new System.Drawing.Point(408, 87);
            this.textBoxMinZ.Name = "textBoxMinZ";
            this.textBoxMinZ.Size = new System.Drawing.Size(100, 20);
            this.textBoxMinZ.TabIndex = 22;
            this.textBoxMinZ.Text = "0";
            this.textBoxMinZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxMinZ.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCoeffX_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(380, 90);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(27, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Min:";
            // 
            // textBoxMaxY
            // 
            this.textBoxMaxY.Location = new System.Drawing.Point(544, 61);
            this.textBoxMaxY.Name = "textBoxMaxY";
            this.textBoxMaxY.Size = new System.Drawing.Size(100, 20);
            this.textBoxMaxY.TabIndex = 28;
            this.textBoxMaxY.Text = "1";
            this.textBoxMaxY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxMaxY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCoeffX_KeyPress);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(516, 64);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "Max:";
            // 
            // textBoxMinY
            // 
            this.textBoxMinY.Location = new System.Drawing.Point(408, 61);
            this.textBoxMinY.Name = "textBoxMinY";
            this.textBoxMinY.Size = new System.Drawing.Size(100, 20);
            this.textBoxMinY.TabIndex = 26;
            this.textBoxMinY.Text = "0";
            this.textBoxMinY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxMinY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCoeffX_KeyPress);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(380, 64);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(27, 13);
            this.label11.TabIndex = 25;
            this.label11.Text = "Min:";
            // 
            // textBoxStartValY
            // 
            this.textBoxStartValY.Location = new System.Drawing.Point(650, 62);
            this.textBoxStartValY.Name = "textBoxStartValY";
            this.textBoxStartValY.Size = new System.Drawing.Size(100, 20);
            this.textBoxStartValY.TabIndex = 31;
            this.textBoxStartValY.Text = "0";
            this.textBoxStartValY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxStartValY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCoeffX_KeyPress);
            // 
            // textBoxStartValZ
            // 
            this.textBoxStartValZ.Location = new System.Drawing.Point(650, 89);
            this.textBoxStartValZ.Name = "textBoxStartValZ";
            this.textBoxStartValZ.Size = new System.Drawing.Size(100, 20);
            this.textBoxStartValZ.TabIndex = 30;
            this.textBoxStartValZ.Text = "0";
            this.textBoxStartValZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxStartValZ.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCoeffX_KeyPress);
            // 
            // textBoxStartValX
            // 
            this.textBoxStartValX.Location = new System.Drawing.Point(650, 37);
            this.textBoxStartValX.Name = "textBoxStartValX";
            this.textBoxStartValX.Size = new System.Drawing.Size(100, 20);
            this.textBoxStartValX.TabIndex = 29;
            this.textBoxStartValX.Text = "0";
            this.textBoxStartValX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxStartValX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCoeffX_KeyPress);
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(756, 113);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(100, 23);
            this.buttonOk.TabIndex = 32;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // checkBoxMinMaxValX
            // 
            this.checkBoxMinMaxValX.AutoSize = true;
            this.checkBoxMinMaxValX.Location = new System.Drawing.Point(756, 40);
            this.checkBoxMinMaxValX.Name = "checkBoxMinMaxValX";
            this.checkBoxMinMaxValX.Size = new System.Drawing.Size(307, 17);
            this.checkBoxMinMaxValX.TabIndex = 33;
            this.checkBoxMinMaxValX.Text = "Если значение равно максиму, то сделать минимумом";
            this.checkBoxMinMaxValX.UseVisualStyleBackColor = true;
            // 
            // checkBoxMinMaxValY
            // 
            this.checkBoxMinMaxValY.AutoSize = true;
            this.checkBoxMinMaxValY.Location = new System.Drawing.Point(756, 65);
            this.checkBoxMinMaxValY.Name = "checkBoxMinMaxValY";
            this.checkBoxMinMaxValY.Size = new System.Drawing.Size(307, 17);
            this.checkBoxMinMaxValY.TabIndex = 34;
            this.checkBoxMinMaxValY.Text = "Если значение равно максиму, то сделать минимумом";
            this.checkBoxMinMaxValY.UseVisualStyleBackColor = true;
            // 
            // checkBoxMinMaxValZ
            // 
            this.checkBoxMinMaxValZ.AutoSize = true;
            this.checkBoxMinMaxValZ.Location = new System.Drawing.Point(756, 91);
            this.checkBoxMinMaxValZ.Name = "checkBoxMinMaxValZ";
            this.checkBoxMinMaxValZ.Size = new System.Drawing.Size(307, 17);
            this.checkBoxMinMaxValZ.TabIndex = 35;
            this.checkBoxMinMaxValZ.Text = "Если значение равно максиму, то сделать минимумом";
            this.checkBoxMinMaxValZ.UseVisualStyleBackColor = true;
            // 
            // TrajectoryFunctionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1066, 148);
            this.Controls.Add(this.checkBoxMinMaxValZ);
            this.Controls.Add(this.checkBoxMinMaxValY);
            this.Controls.Add(this.checkBoxMinMaxValX);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.textBoxStartValY);
            this.Controls.Add(this.textBoxStartValZ);
            this.Controls.Add(this.textBoxStartValX);
            this.Controls.Add(this.textBoxMaxY);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxMinY);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBoxMaxZ);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxMinZ);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxMaxX);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxMinX);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxStepZ);
            this.Controls.Add(this.textBoxStepY);
            this.Controls.Add(this.textBoxStepX);
            this.Controls.Add(this.comboBoxFuncZ);
            this.Controls.Add(this.comboBoxFuncY);
            this.Controls.Add(this.comboBoxFuncX);
            this.Controls.Add(this.textBoxCoeffZ);
            this.Controls.Add(this.labelZ);
            this.Controls.Add(this.textBoxCoeffY);
            this.Controls.Add(this.labelY);
            this.Controls.Add(this.textBoxCoeffX);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelX);
            this.Name = "TrajectoryFunctionsForm";
            this.Text = "TrajectoryFunctionsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxCoeffX;
        private System.Windows.Forms.TextBox textBoxCoeffY;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.TextBox textBoxCoeffZ;
        private System.Windows.Forms.Label labelZ;
        private System.Windows.Forms.ComboBox comboBoxFuncX;
        private System.Windows.Forms.ComboBox comboBoxFuncY;
        private System.Windows.Forms.ComboBox comboBoxFuncZ;
        private System.Windows.Forms.TextBox textBoxStepX;
        private System.Windows.Forms.TextBox textBoxStepY;
        private System.Windows.Forms.TextBox textBoxStepZ;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxMinX;
        private System.Windows.Forms.TextBox textBoxMaxX;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxMaxZ;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxMinZ;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxMaxY;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxMinY;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxStartValY;
        private System.Windows.Forms.TextBox textBoxStartValZ;
        private System.Windows.Forms.TextBox textBoxStartValX;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.CheckBox checkBoxMinMaxValX;
        private System.Windows.Forms.CheckBox checkBoxMinMaxValY;
        private System.Windows.Forms.CheckBox checkBoxMinMaxValZ;
    }
}