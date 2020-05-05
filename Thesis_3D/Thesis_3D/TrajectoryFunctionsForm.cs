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
    public partial class TrajectoryFunctionsForm : Form
    {
        public TrajectoryFunctions trajectoryFunctionsX;
        public TrajectoryFunctions trajectoryFunctionsY;
        public TrajectoryFunctions trajectoryFunctionsZ;
        public TrajectoryFunctionsForm()
        {
            InitializeComponent();
            comboBoxFuncX.DataSource = new List<ComboboxDataSourceTrajectory> {
                new ComboboxDataSourceTrajectory { Text = "x",      mathFunc = (double x) => (x) } ,
                new ComboboxDataSourceTrajectory { Text = "cos(x)", mathFunc = (double x) => Math.Cos(x) } ,
                new ComboboxDataSourceTrajectory { Text = "sin(x)", mathFunc = (double x) => Math.Sin(x) }
            };
            comboBoxFuncY.DataSource = new List<ComboboxDataSourceTrajectory> {
                new ComboboxDataSourceTrajectory { Text = "y",      mathFunc = (double x) => (x) } ,
                new ComboboxDataSourceTrajectory { Text = "cos(y)", mathFunc = (double x) => Math.Cos(x) } ,
                new ComboboxDataSourceTrajectory { Text = "sin(y)", mathFunc = (double x) => Math.Sin(x) }
            };
            comboBoxFuncZ.DataSource = new List<ComboboxDataSourceTrajectory> {
                new ComboboxDataSourceTrajectory { Text = "z",      mathFunc = (double x) => (x) } ,
                new ComboboxDataSourceTrajectory { Text = "cos(z)", mathFunc = (double x) => Math.Cos(x) } ,
                new ComboboxDataSourceTrajectory { Text = "sin(z)", mathFunc = (double x) => Math.Sin(x) }
            };
            comboBoxFuncX.DisplayMember = comboBoxFuncY.DisplayMember = comboBoxFuncZ.DisplayMember = "Text";
            comboBoxFuncX.ValueMember = comboBoxFuncY.ValueMember = comboBoxFuncZ.ValueMember = "mathFunc";
        }

        private void textBoxKoeffX_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!char.IsDigit(number) || number == '-' || number == ',' || number == '.')
            {
                e.Handled = true;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            ComboboxDataSourceTrajectory selectItemX = (ComboboxDataSourceTrajectory)comboBoxFuncX.SelectedItem;
            TrajectoryFunctions locTrajectoryFunctionsX = new TrajectoryFunctions(float.Parse(textBoxKoeffX.Text), selectItemX.mathFunc, float.Parse(textBoxStepX.Text), float.Parse(textBoxMinX.Text), float.Parse(textBoxMaxX.Text), float.Parse(textBoxStartValX.Text), selectItemX.Text);
            if (!locTrajectoryFunctionsX.ValidateTrajectoryFunc())
            {
                MessageBox.Show("Неккоректно заданная траектория для X");
                return;
            }
            trajectoryFunctionsX = locTrajectoryFunctionsX;
            ComboboxDataSourceTrajectory selectItemY = (ComboboxDataSourceTrajectory)comboBoxFuncY.SelectedItem;
            TrajectoryFunctions locTrajectoryFunctionsY = new TrajectoryFunctions(float.Parse(textBoxKoeffY.Text), selectItemY.mathFunc, float.Parse(textBoxStepY.Text), float.Parse(textBoxMinY.Text), float.Parse(textBoxMaxY.Text), float.Parse(textBoxStartValY.Text), selectItemY.Text);
            if (!locTrajectoryFunctionsY.ValidateTrajectoryFunc())
            {
                MessageBox.Show("Неккоректно заданная траектория для Y");
                return;
            }
            trajectoryFunctionsY = locTrajectoryFunctionsY;
            ComboboxDataSourceTrajectory selectItemZ = (ComboboxDataSourceTrajectory)comboBoxFuncZ.SelectedItem;
            TrajectoryFunctions locTrajectoryFunctionsZ = new TrajectoryFunctions(float.Parse(textBoxKoeffZ.Text), selectItemZ.mathFunc, float.Parse(textBoxStepZ.Text), float.Parse(textBoxMinZ.Text), float.Parse(textBoxMaxZ.Text), float.Parse(textBoxStartValZ.Text), selectItemZ.Text);
            if (!locTrajectoryFunctionsZ.ValidateTrajectoryFunc())
            {
                MessageBox.Show("Неккоректно заданная траектория для X");
                return;
            }
            trajectoryFunctionsZ = locTrajectoryFunctionsZ;
        }
    }
    public class ComboboxDataSourceTrajectory
    {
        public string Text { get; set; }
        public Func<double, double> mathFunc { get; set; }
    }
    public class TrajectoryFunctions
    {
        public float koeff;
        public Func<double, double> mathFunc;
        public float step;
        public float min;
        public float max;
        public float startVal;
        public float Val;
        public float modifier;
        int colIter;
        int Iter;
        int signIter;

        public TrajectoryFunctions()
        {
            koeff = 1;
            mathFunc = (double x) => (x);
            step = 1;
            min = 0;
            max = 0;
            startVal = 0;
            Val = 0;
            colIter = 1;
            modifier = 1;
            Iter = 0;
            signIter = 1;
        }
        public TrajectoryFunctions(float locKoeff, Func<double, double> locMathFunc, float locStep, float locMin, float locMax, float locStartVal, string nameFunc)
        {
            koeff = locKoeff;
            mathFunc = locMathFunc;
            step = locStep;
            min = locMin;
            max = locMax;
            startVal = locStartVal;
            Val = locStartVal;
            Iter = 0;
            signIter = 1;
            colIter = (int)((max - min) / step);
            modifier = nameFunc.ToUpper().Equals("X") || nameFunc.ToUpper().Equals("Y") || nameFunc.ToUpper().Equals("Z") ? 1f: (float)(Math.PI * 2 / 180);
        }
        public void NextValueAndSetStep()
        {
            if (Iter >= colIter) signIter = -1;
            if (Iter <= 0) signIter = 1;
            if (Iter <= 0) signIter = 0;
            Val = Val + signIter * step;
        }
        public bool ValidateTrajectoryFunc()
        {
            return !(min > max || startVal > max || startVal < min);
        }
    }
}
