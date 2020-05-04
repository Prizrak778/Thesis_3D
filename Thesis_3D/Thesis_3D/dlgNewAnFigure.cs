using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Thesis_3D
{
    public partial class DlgNewAnFigure : Form
    {
        private TypeObjectCreate _typeObjectCreate = TypeObjectCreate.SolidCube;
        public Color4 colorObject = Color4.White;
        public Vector3 position;
        public Vertex[] figureVertex;
        public DlgNewAnFigure()
        {
            InitializeComponent();
        }
        public DlgNewAnFigure(TypeObjectCreate typeObjectCreate = TypeObjectCreate.SolidCube)
        {
            InitializeComponent();
            _typeObjectCreate = typeObjectCreate;
            if (typeObjectCreate == TypeObjectCreate.SolidCube)
            {
                Height = 220;
                buttonOk.Top = 145;
                labelAngelX.Visible = false;
                labelAngelY.Visible = false;
                labelAngelZ.Visible = false;
                numericUpDownAngelX.Visible = false;
                numericUpDownAngelY.Visible = false;
                numericUpDownAngelZ.Visible = false;
                labelColBreakX.Visible = false;
                labelColBreakY.Visible = false;
                labelKoeffSX.Visible = false;
                labelKoeffSY.Visible = false;
                textBoxColBreakX.Visible = false;
                textBoxColBreakY.Visible = false;
                textBoxKoeffSX.Visible = false;
                textBoxKoeffSY.Visible = false;
            }
            if (typeObjectCreate == TypeObjectCreate.Plane)
            {
                Height = 300;
                buttonOk.Top = 225;
                labelAngelX.Visible = true;
                labelAngelY.Visible = true;
                labelAngelZ.Visible = true;
                numericUpDownAngelX.Visible = true;
                numericUpDownAngelY.Visible = true;
                numericUpDownAngelZ.Visible = true;
                labelColBreakX.Visible = false;
                labelColBreakY.Visible = false;
                labelKoeffSX.Visible = false;
                labelKoeffSY.Visible = false;
                textBoxColBreakX.Visible = false;
                textBoxColBreakY.Visible = false;
                textBoxKoeffSX.Visible = false;
                textBoxKoeffSY.Visible = false;
            }
            if(typeObjectCreate == TypeObjectCreate.Sphere)
            {
                Height = 330;
                buttonOk.Top = 255;
                labelAngelX.Visible = false;
                labelAngelY.Visible = false;
                labelAngelZ.Visible = false;
                numericUpDownAngelX.Visible = false;
                numericUpDownAngelY.Visible = false;
                numericUpDownAngelZ.Visible = false;
                labelColBreakX.Visible = true;
                labelColBreakY.Visible = true;
                labelKoeffSX.Visible = true;
                labelKoeffSY.Visible = true;
                textBoxColBreakX.Visible = true;
                textBoxColBreakY.Visible = true;
                textBoxKoeffSX.Visible = true;
                textBoxKoeffSY.Visible = true;
            }
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
            colorObject = buttonColor.BackColor;
            if (_typeObjectCreate == TypeObjectCreate.SolidCube)
            {
                position = new Vector3(float.Parse(textBoxShiftX.Text), float.Parse(textBoxShiftY.Text), float.Parse(textBoxShiftZ.Text));
                figureVertex = ObjectCreate.CreateSolidCube(float.Parse(textBoxSide.Text, System.Globalization.NumberStyles.Float), position);
            }
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
