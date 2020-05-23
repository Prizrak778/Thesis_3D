namespace Thesis_3D
{
    partial class DlgAddEditAnFigure
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
            this.labelSide = new System.Windows.Forms.Label();
            this.textBoxSide = new System.Windows.Forms.TextBox();
            this.labelShiftX = new System.Windows.Forms.Label();
            this.labelShiftY = new System.Windows.Forms.Label();
            this.labelShiftZ = new System.Windows.Forms.Label();
            this.textBoxShiftX = new System.Windows.Forms.TextBox();
            this.textBoxShiftY = new System.Windows.Forms.TextBox();
            this.textBoxShiftZ = new System.Windows.Forms.TextBox();
            this.buttonColor = new System.Windows.Forms.Button();
            this.labelColor = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.labelAngelX = new System.Windows.Forms.Label();
            this.numericUpDownAngelX = new System.Windows.Forms.NumericUpDown();
            this.labelAngelY = new System.Windows.Forms.Label();
            this.numericUpDownAngelY = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownAngelZ = new System.Windows.Forms.NumericUpDown();
            this.labelAngelZ = new System.Windows.Forms.Label();
            this.colorDialogObject = new System.Windows.Forms.ColorDialog();
            this.labelColBreakX = new System.Windows.Forms.Label();
            this.labelColBreakY = new System.Windows.Forms.Label();
            this.labelCoeffSX = new System.Windows.Forms.Label();
            this.labelCoeffSY = new System.Windows.Forms.Label();
            this.textBoxCoeffSY = new System.Windows.Forms.TextBox();
            this.textBoxCoeffSX = new System.Windows.Forms.TextBox();
            this.textBoxColBreakY = new System.Windows.Forms.TextBox();
            this.textBoxColBreakX = new System.Windows.Forms.TextBox();
            this.trackBarAlpha = new System.Windows.Forms.TrackBar();
            this.labelAlpha = new System.Windows.Forms.Label();
            this.labelTypeFigure = new System.Windows.Forms.Label();
            this.comboBoxTypeFigure = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAngelX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAngelY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAngelZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAlpha)).BeginInit();
            this.SuspendLayout();
            // 
            // labelSide
            // 
            this.labelSide.AutoSize = true;
            this.labelSide.Location = new System.Drawing.Point(15, 39);
            this.labelSide.Name = "labelSide";
            this.labelSide.Size = new System.Drawing.Size(180, 13);
            this.labelSide.TabIndex = 0;
            this.labelSide.Text = "Расстояние от центра до границы";
            // 
            // textBoxSide
            // 
            this.textBoxSide.Location = new System.Drawing.Point(201, 36);
            this.textBoxSide.Name = "textBoxSide";
            this.textBoxSide.Size = new System.Drawing.Size(101, 20);
            this.textBoxSide.TabIndex = 1;
            this.textBoxSide.Text = "1";
            this.textBoxSide.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxSide_KeyPress);
            // 
            // labelShiftX
            // 
            this.labelShiftX.AutoSize = true;
            this.labelShiftX.Location = new System.Drawing.Point(15, 64);
            this.labelShiftX.Name = "labelShiftX";
            this.labelShiftX.Size = new System.Drawing.Size(107, 13);
            this.labelShiftX.TabIndex = 2;
            this.labelShiftX.Text = "Смещение по оси X";
            // 
            // labelShiftY
            // 
            this.labelShiftY.AutoSize = true;
            this.labelShiftY.Location = new System.Drawing.Point(15, 90);
            this.labelShiftY.Name = "labelShiftY";
            this.labelShiftY.Size = new System.Drawing.Size(107, 13);
            this.labelShiftY.TabIndex = 3;
            this.labelShiftY.Text = "Смещение по оси Y";
            // 
            // labelShiftZ
            // 
            this.labelShiftZ.AutoSize = true;
            this.labelShiftZ.Location = new System.Drawing.Point(15, 116);
            this.labelShiftZ.Name = "labelShiftZ";
            this.labelShiftZ.Size = new System.Drawing.Size(107, 13);
            this.labelShiftZ.TabIndex = 4;
            this.labelShiftZ.Text = "Смещение по оси Z";
            // 
            // textBoxShiftX
            // 
            this.textBoxShiftX.Location = new System.Drawing.Point(201, 61);
            this.textBoxShiftX.Name = "textBoxShiftX";
            this.textBoxShiftX.Size = new System.Drawing.Size(101, 20);
            this.textBoxShiftX.TabIndex = 5;
            this.textBoxShiftX.Text = "0";
            this.textBoxShiftX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxSide_KeyPress);
            // 
            // textBoxShiftY
            // 
            this.textBoxShiftY.Location = new System.Drawing.Point(201, 87);
            this.textBoxShiftY.Name = "textBoxShiftY";
            this.textBoxShiftY.Size = new System.Drawing.Size(101, 20);
            this.textBoxShiftY.TabIndex = 6;
            this.textBoxShiftY.Text = "0";
            this.textBoxShiftY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxSide_KeyPress);
            // 
            // textBoxShiftZ
            // 
            this.textBoxShiftZ.Location = new System.Drawing.Point(201, 113);
            this.textBoxShiftZ.Name = "textBoxShiftZ";
            this.textBoxShiftZ.Size = new System.Drawing.Size(101, 20);
            this.textBoxShiftZ.TabIndex = 7;
            this.textBoxShiftZ.Text = "0";
            this.textBoxShiftZ.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxSide_KeyPress);
            // 
            // buttonColor
            // 
            this.buttonColor.BackColor = System.Drawing.SystemColors.GrayText;
            this.buttonColor.Location = new System.Drawing.Point(201, 143);
            this.buttonColor.Name = "buttonColor";
            this.buttonColor.Size = new System.Drawing.Size(104, 23);
            this.buttonColor.TabIndex = 8;
            this.buttonColor.UseVisualStyleBackColor = false;
            this.buttonColor.Click += new System.EventHandler(this.buttonColor_Click);
            // 
            // labelColor
            // 
            this.labelColor.AutoSize = true;
            this.labelColor.Location = new System.Drawing.Point(15, 149);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(32, 13);
            this.labelColor.TabIndex = 9;
            this.labelColor.Text = "Цвет";
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(583, 263);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(100, 23);
            this.buttonOk.TabIndex = 10;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // labelAngelX
            // 
            this.labelAngelX.AutoSize = true;
            this.labelAngelX.Location = new System.Drawing.Point(449, 176);
            this.labelAngelX.Name = "labelAngelX";
            this.labelAngelX.Size = new System.Drawing.Size(128, 13);
            this.labelAngelX.TabIndex = 11;
            this.labelAngelX.Text = "Угол поворота по оси X";
            // 
            // numericUpDownAngelX
            // 
            this.numericUpDownAngelX.Location = new System.Drawing.Point(583, 174);
            this.numericUpDownAngelX.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numericUpDownAngelX.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.numericUpDownAngelX.Name = "numericUpDownAngelX";
            this.numericUpDownAngelX.Size = new System.Drawing.Size(101, 20);
            this.numericUpDownAngelX.TabIndex = 12;
            // 
            // labelAngelY
            // 
            this.labelAngelY.AutoSize = true;
            this.labelAngelY.Location = new System.Drawing.Point(449, 202);
            this.labelAngelY.Name = "labelAngelY";
            this.labelAngelY.Size = new System.Drawing.Size(128, 13);
            this.labelAngelY.TabIndex = 13;
            this.labelAngelY.Text = "Угол поворота по оси Y";
            // 
            // numericUpDownAngelY
            // 
            this.numericUpDownAngelY.Location = new System.Drawing.Point(583, 200);
            this.numericUpDownAngelY.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numericUpDownAngelY.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.numericUpDownAngelY.Name = "numericUpDownAngelY";
            this.numericUpDownAngelY.Size = new System.Drawing.Size(101, 20);
            this.numericUpDownAngelY.TabIndex = 14;
            // 
            // numericUpDownAngelZ
            // 
            this.numericUpDownAngelZ.Location = new System.Drawing.Point(583, 229);
            this.numericUpDownAngelZ.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numericUpDownAngelZ.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.numericUpDownAngelZ.Name = "numericUpDownAngelZ";
            this.numericUpDownAngelZ.Size = new System.Drawing.Size(101, 20);
            this.numericUpDownAngelZ.TabIndex = 15;
            // 
            // labelAngelZ
            // 
            this.labelAngelZ.AutoSize = true;
            this.labelAngelZ.Location = new System.Drawing.Point(449, 231);
            this.labelAngelZ.Name = "labelAngelZ";
            this.labelAngelZ.Size = new System.Drawing.Size(128, 13);
            this.labelAngelZ.TabIndex = 16;
            this.labelAngelZ.Text = "Угол поворота по оси Z";
            // 
            // labelColBreakX
            // 
            this.labelColBreakX.AutoSize = true;
            this.labelColBreakX.Location = new System.Drawing.Point(15, 176);
            this.labelColBreakX.Name = "labelColBreakX";
            this.labelColBreakX.Size = new System.Drawing.Size(148, 13);
            this.labelColBreakX.TabIndex = 17;
            this.labelColBreakX.Text = "Количество разбиений по X";
            // 
            // labelColBreakY
            // 
            this.labelColBreakY.AutoSize = true;
            this.labelColBreakY.Location = new System.Drawing.Point(15, 202);
            this.labelColBreakY.Name = "labelColBreakY";
            this.labelColBreakY.Size = new System.Drawing.Size(148, 13);
            this.labelColBreakY.TabIndex = 18;
            this.labelColBreakY.Text = "Количество разбиений по Y";
            // 
            // labelCoeffSX
            // 
            this.labelCoeffSX.AutoSize = true;
            this.labelCoeffSX.Location = new System.Drawing.Point(15, 227);
            this.labelCoeffSX.Name = "labelCoeffSX";
            this.labelCoeffSX.Size = new System.Drawing.Size(142, 13);
            this.labelCoeffSX.TabIndex = 19;
            this.labelCoeffSX.Text = "Коэффициент сжатия по X";
            // 
            // labelCoeffSY
            // 
            this.labelCoeffSY.AutoSize = true;
            this.labelCoeffSY.Location = new System.Drawing.Point(15, 253);
            this.labelCoeffSY.Name = "labelCoeffSY";
            this.labelCoeffSY.Size = new System.Drawing.Size(142, 13);
            this.labelCoeffSY.TabIndex = 20;
            this.labelCoeffSY.Text = "Коэффициент сжатия по Y";
            // 
            // textBoxCoeffSY
            // 
            this.textBoxCoeffSY.Location = new System.Drawing.Point(201, 250);
            this.textBoxCoeffSY.Name = "textBoxCoeffSY";
            this.textBoxCoeffSY.Size = new System.Drawing.Size(100, 20);
            this.textBoxCoeffSY.TabIndex = 21;
            this.textBoxCoeffSY.Text = "1";
            this.textBoxCoeffSY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxSide_KeyPress);
            // 
            // textBoxCoeffSX
            // 
            this.textBoxCoeffSX.Location = new System.Drawing.Point(201, 224);
            this.textBoxCoeffSX.Name = "textBoxCoeffSX";
            this.textBoxCoeffSX.Size = new System.Drawing.Size(100, 20);
            this.textBoxCoeffSX.TabIndex = 22;
            this.textBoxCoeffSX.Text = "1";
            this.textBoxCoeffSX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxSide_KeyPress);
            // 
            // textBoxColBreakY
            // 
            this.textBoxColBreakY.Location = new System.Drawing.Point(201, 198);
            this.textBoxColBreakY.Name = "textBoxColBreakY";
            this.textBoxColBreakY.Size = new System.Drawing.Size(100, 20);
            this.textBoxColBreakY.TabIndex = 23;
            this.textBoxColBreakY.Text = "1";
            this.textBoxColBreakY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxSide_KeyPress);
            // 
            // textBoxColBreakX
            // 
            this.textBoxColBreakX.Location = new System.Drawing.Point(202, 173);
            this.textBoxColBreakX.Name = "textBoxColBreakX";
            this.textBoxColBreakX.Size = new System.Drawing.Size(100, 20);
            this.textBoxColBreakX.TabIndex = 24;
            this.textBoxColBreakX.Text = "1";
            this.textBoxColBreakX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxSide_KeyPress);
            // 
            // trackBarAlpha
            // 
            this.trackBarAlpha.LargeChange = 3;
            this.trackBarAlpha.Location = new System.Drawing.Point(583, 43);
            this.trackBarAlpha.Name = "trackBarAlpha";
            this.trackBarAlpha.Size = new System.Drawing.Size(101, 45);
            this.trackBarAlpha.TabIndex = 25;
            this.trackBarAlpha.Value = 10;
            // 
            // labelAlpha
            // 
            this.labelAlpha.AutoSize = true;
            this.labelAlpha.Location = new System.Drawing.Point(449, 52);
            this.labelAlpha.Name = "labelAlpha";
            this.labelAlpha.Size = new System.Drawing.Size(124, 13);
            this.labelAlpha.TabIndex = 26;
            this.labelAlpha.Text = "Прозрачность объекта";
            // 
            // labelTypeFigure
            // 
            this.labelTypeFigure.AutoSize = true;
            this.labelTypeFigure.Location = new System.Drawing.Point(15, 12);
            this.labelTypeFigure.Name = "labelTypeFigure";
            this.labelTypeFigure.Size = new System.Drawing.Size(67, 13);
            this.labelTypeFigure.TabIndex = 27;
            this.labelTypeFigure.Text = "Тип фигуры";
            // 
            // comboBoxTypeFigure
            // 
            this.comboBoxTypeFigure.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTypeFigure.FormattingEnabled = true;
            this.comboBoxTypeFigure.Location = new System.Drawing.Point(201, 9);
            this.comboBoxTypeFigure.Name = "comboBoxTypeFigure";
            this.comboBoxTypeFigure.Size = new System.Drawing.Size(103, 21);
            this.comboBoxTypeFigure.TabIndex = 28;
            this.comboBoxTypeFigure.SelectedIndexChanged += new System.EventHandler(this.comboBoxTypeFigure_SelectedIndexChanged);
            // 
            // DlgAddEditAnFigure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 298);
            this.Controls.Add(this.comboBoxTypeFigure);
            this.Controls.Add(this.labelTypeFigure);
            this.Controls.Add(this.labelAlpha);
            this.Controls.Add(this.trackBarAlpha);
            this.Controls.Add(this.textBoxColBreakX);
            this.Controls.Add(this.textBoxColBreakY);
            this.Controls.Add(this.textBoxCoeffSX);
            this.Controls.Add(this.textBoxCoeffSY);
            this.Controls.Add(this.labelCoeffSY);
            this.Controls.Add(this.labelCoeffSX);
            this.Controls.Add(this.labelColBreakY);
            this.Controls.Add(this.labelColBreakX);
            this.Controls.Add(this.labelAngelZ);
            this.Controls.Add(this.numericUpDownAngelZ);
            this.Controls.Add(this.numericUpDownAngelY);
            this.Controls.Add(this.labelAngelY);
            this.Controls.Add(this.numericUpDownAngelX);
            this.Controls.Add(this.labelAngelX);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.labelColor);
            this.Controls.Add(this.buttonColor);
            this.Controls.Add(this.textBoxShiftZ);
            this.Controls.Add(this.textBoxShiftY);
            this.Controls.Add(this.textBoxShiftX);
            this.Controls.Add(this.labelShiftZ);
            this.Controls.Add(this.labelShiftY);
            this.Controls.Add(this.labelShiftX);
            this.Controls.Add(this.textBoxSide);
            this.Controls.Add(this.labelSide);
            this.Name = "DlgAddEditAnFigure";
            this.Text = "Добавление аналитической фигуры";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAngelX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAngelY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAngelZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAlpha)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSide;
        private System.Windows.Forms.TextBox textBoxSide;
        private System.Windows.Forms.Label labelShiftX;
        private System.Windows.Forms.Label labelShiftY;
        private System.Windows.Forms.Label labelShiftZ;
        private System.Windows.Forms.TextBox textBoxShiftX;
        private System.Windows.Forms.TextBox textBoxShiftY;
        private System.Windows.Forms.TextBox textBoxShiftZ;
        private System.Windows.Forms.Button buttonColor;
        private System.Windows.Forms.Label labelColor;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label labelAngelX;
        private System.Windows.Forms.NumericUpDown numericUpDownAngelX;
        private System.Windows.Forms.Label labelAngelY;
        private System.Windows.Forms.NumericUpDown numericUpDownAngelY;
        private System.Windows.Forms.NumericUpDown numericUpDownAngelZ;
        private System.Windows.Forms.Label labelAngelZ;
        private System.Windows.Forms.ColorDialog colorDialogObject;
        private System.Windows.Forms.Label labelColBreakX;
        private System.Windows.Forms.Label labelColBreakY;
        private System.Windows.Forms.Label labelCoeffSX;
        private System.Windows.Forms.Label labelCoeffSY;
        private System.Windows.Forms.TextBox textBoxCoeffSY;
        private System.Windows.Forms.TextBox textBoxCoeffSX;
        private System.Windows.Forms.TextBox textBoxColBreakY;
        private System.Windows.Forms.TextBox textBoxColBreakX;
        private System.Windows.Forms.TrackBar trackBarAlpha;
        private System.Windows.Forms.Label labelAlpha;
        private System.Windows.Forms.Label labelTypeFigure;
        private System.Windows.Forms.ComboBox comboBoxTypeFigure;
    }
}