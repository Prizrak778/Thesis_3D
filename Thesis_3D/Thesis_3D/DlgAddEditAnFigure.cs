using OpenTK;
using OpenTK.Graphics;
using System;
using System.Windows.Forms;

namespace Thesis_3D
{
    public partial class DlgAddEditAnFigure : Form
    {
        private TypeObjectCreate _typeObjectCreate = TypeObjectCreate.SolidCube;
        public Color4 colorObject = Color4.White;
        public Vector3 position;
        public int colBreakX;
        public int colBreakY;
        public int coeffSX;
        public int coeffSY;
        public int angleX;
        public int angleY;
        public int angleZ;
        public float side; //Его расстояние от центра до границы

        public Vertex[] figureVertex;
        public DlgAddEditAnFigure()
        {
            InitializeComponent();
        }
        public DlgAddEditAnFigure(TypeObjectCreate locTypeObjectCreate = TypeObjectCreate.SolidCube, float locAlphaObject = 1.0f, float locSide = 1.0f, float shiftx = 0, float shifty = 0, float shiftz = 0, int locColBreakX = 1, int locColBreakY = 1, int locCoeffSX = 1, int locCoeffSY = 1, int locAngleX = 0, int locAngleY = 0, int locAngleZ = 0)
        {
            InitializeComponent();
            _typeObjectCreate = locTypeObjectCreate;
            trackBarAlpha.Value = (int)(locAlphaObject * 10); //Округляем как можем
            textBoxSide.Text = locSide.ToString();
            textBoxShiftX.Text = shiftx.ToString();
            textBoxShiftY.Text = shifty.ToString();
            textBoxShiftZ.Text = shiftz.ToString();
            textBoxColBreakX.Text = locColBreakX.ToString();
            textBoxColBreakY.Text = locColBreakY.ToString();
            textBoxCoeffSX.Text = locCoeffSX.ToString();
            textBoxCoeffSY.Text = locCoeffSY.ToString();
            numericUpDownAngelX.Value = locAngleX;
            numericUpDownAngelY.Value = locAngleY;
            numericUpDownAngelZ.Value = locAngleZ;
            if (locTypeObjectCreate == TypeObjectCreate.SolidCube)
            {
                Height = 300;
                buttonOk.Top = 225;
                labelAngelX.Enabled = false;
                labelAngelY.Enabled = false;
                labelAngelZ.Enabled = false;
                numericUpDownAngelX.Enabled = false;
                numericUpDownAngelY.Enabled = false;
                numericUpDownAngelZ.Enabled = false;
                labelColBreakX.Enabled = false;
                labelColBreakY.Enabled = false;
                labelCoeffSX.Enabled = false;
                labelCoeffSY.Enabled = false;
                textBoxColBreakX.Enabled = false;
                textBoxColBreakY.Enabled = false;
                textBoxCoeffSX.Enabled = false;
                textBoxCoeffSY.Enabled = false;
            }
            if (locTypeObjectCreate == TypeObjectCreate.Plane)
            {
                Height = 300;
                buttonOk.Top = 225;
                labelAngelX.Enabled = true;
                labelAngelY.Enabled = true;
                labelAngelZ.Enabled = true;
                numericUpDownAngelX.Enabled = true;
                numericUpDownAngelY.Enabled = true;
                numericUpDownAngelZ.Enabled = true;
                labelColBreakX.Enabled = false;
                labelColBreakY.Enabled = false;
                labelCoeffSX.Enabled = false;
                labelCoeffSY.Enabled = false;
                textBoxColBreakX.Enabled = false;
                textBoxColBreakY.Enabled = false;
                textBoxCoeffSX.Enabled = false;
                textBoxCoeffSY.Enabled = false;
            }
            if(locTypeObjectCreate == TypeObjectCreate.Sphere)
            {
                Height = 330;
                buttonOk.Top = 255;
                labelAngelX.Enabled = false;
                labelAngelY.Enabled = false;
                labelAngelZ.Enabled = false;
                numericUpDownAngelX.Enabled = false;
                numericUpDownAngelY.Enabled = false;
                numericUpDownAngelZ.Enabled = false;
                labelColBreakX.Enabled = true;
                labelColBreakY.Enabled = true;
                labelCoeffSX.Enabled = true;
                labelCoeffSY.Enabled = true;
                textBoxColBreakX.Enabled = true;
                textBoxColBreakY.Enabled = true;
                textBoxCoeffSX.Enabled = true;
                textBoxCoeffSY.Enabled = true;
            }
        }
        public void SetColor(Color4 colorObj)
        {
            buttonColor.BackColor = System.Drawing.Color.FromArgb(colorObj.ToArgb());
        }
        private void buttonColor_Click(object sender, EventArgs e)
        {
            if (colorDialogObject.ShowDialog() == DialogResult.OK)
            {
                buttonColor.BackColor = colorDialogObject.Color;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            colBreakX = int.Parse(textBoxColBreakX.Text);
            colBreakY = int.Parse(textBoxColBreakY.Text);
            coeffSX = int.Parse(textBoxCoeffSX.Text);
            coeffSY = int.Parse(textBoxCoeffSY.Text);
            angleX = (int)numericUpDownAngelX.Value;
            angleY = (int)numericUpDownAngelY.Value;
            angleZ = (int)numericUpDownAngelZ.Value;
            side = float.Parse(textBoxSide.Text, System.Globalization.NumberStyles.Float);
            colorObject = buttonColor.BackColor;
            if (_typeObjectCreate == TypeObjectCreate.SolidCube)
            {
                position = new Vector3(float.Parse(textBoxShiftX.Text), float.Parse(textBoxShiftY.Text), float.Parse(textBoxShiftZ.Text));
                figureVertex = ObjectCreate.CreateSolidCube(side, position);
            }
            if (_typeObjectCreate == TypeObjectCreate.Plane)
            {
                position = new Vector3(float.Parse(textBoxShiftX.Text), float.Parse(textBoxShiftY.Text), float.Parse(textBoxShiftZ.Text));
                figureVertex = ObjectCreate.CreatePlane(side, position, angleX, angleY, angleZ);
            }
            if (_typeObjectCreate == TypeObjectCreate.Sphere)
            {
                position = new Vector3(float.Parse(textBoxShiftX.Text), float.Parse(textBoxShiftY.Text), float.Parse(textBoxShiftZ.Text));
                figureVertex = ObjectCreate.CreateSphere(side, position, colBreakX, colBreakY, coeffSX, coeffSY);
            }
            colorObject.A = trackBarAlpha.Value / 10f;
            Close();
        }

        private void textBoxSide_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!char.IsDigit(number))
            {
                e.Handled = true;
            }
        }
    }
}
