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
            InitCombobox();
        }
        public TrajectoryFunctionsForm(TrajectoryFunctions trajectoryFunctionsX, TrajectoryFunctions trajectoryFunctionsY, TrajectoryFunctions trajectoryFunctionsZ)
        {
            InitializeComponent();
            InitCombobox();
            if(trajectoryFunctionsX != null)
            {
                comboBoxFuncX.SelectedItem = ((List<ComboboxDataSourceTrajectory>)(comboBoxFuncX.DataSource)).Where(x => x.mathFunc == trajectoryFunctionsX.mathFunc).FirstOrDefault();
                textBoxKoeffX.Text = Convert.ToString(trajectoryFunctionsX.koeff);
                textBoxStepX.Text = Convert.ToString(trajectoryFunctionsX.step);
                textBoxMinX.Text = Convert.ToString(trajectoryFunctionsX.min);
                textBoxMaxX.Text = Convert.ToString(trajectoryFunctionsX.max);
                textBoxStartValX.Text = Convert.ToString(trajectoryFunctionsX.startVal);
                checkBoxMinMaxValX.Checked = trajectoryFunctionsX.reversMove;
            }
            if (trajectoryFunctionsY != null)
            {
                comboBoxFuncY.SelectedItem = ((List<ComboboxDataSourceTrajectory>)(comboBoxFuncY.DataSource)).Where(x => x.mathFunc == trajectoryFunctionsY.mathFunc).FirstOrDefault();
                textBoxKoeffY.Text = Convert.ToString(trajectoryFunctionsY.koeff);
                textBoxStepY.Text = Convert.ToString(trajectoryFunctionsY.step);
                textBoxMinY.Text = Convert.ToString(trajectoryFunctionsY.min);
                textBoxMaxY.Text = Convert.ToString(trajectoryFunctionsY.max);
                textBoxStartValY.Text = Convert.ToString(trajectoryFunctionsY.startVal);
                checkBoxMinMaxValY.Checked = trajectoryFunctionsY.reversMove;
            }
            if (trajectoryFunctionsZ != null)
            {
                comboBoxFuncZ.SelectedItem = ((List<ComboboxDataSourceTrajectory>)(comboBoxFuncZ.DataSource)).Where(x => x.mathFunc == trajectoryFunctionsZ.mathFunc).FirstOrDefault();
                textBoxKoeffZ.Text = Convert.ToString(trajectoryFunctionsZ.koeff);
                textBoxStepZ.Text = Convert.ToString(trajectoryFunctionsZ.step);
                textBoxMinZ.Text = Convert.ToString(trajectoryFunctionsZ.min);
                textBoxMaxZ.Text = Convert.ToString(trajectoryFunctionsZ.max);
                textBoxStartValZ.Text = Convert.ToString(trajectoryFunctionsZ.startVal);
                checkBoxMinMaxValZ.Checked = trajectoryFunctionsZ.reversMove;
            }
        }
        private void InitCombobox()
        {
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
            char symbolT = e.KeyChar;

            if (!char.IsDigit(symbolT) && symbolT != '-' && symbolT != ',' && symbolT != '\b')
            {
                e.Handled = true;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            ComboboxDataSourceTrajectory selectItemX = (ComboboxDataSourceTrajectory)comboBoxFuncX.SelectedItem;
            TrajectoryFunctions locTrajectoryFunctionsX = new TrajectoryFunctions(float.Parse(textBoxKoeffX.Text), selectItemX.mathFunc, float.Parse(textBoxStepX.Text), float.Parse(textBoxMinX.Text), float.Parse(textBoxMaxX.Text), float.Parse(textBoxStartValX.Text), selectItemX.Text, checkBoxMinMaxValX.Checked);
            if (!locTrajectoryFunctionsX.ValidateTrajectoryFunc())
            {
                MessageBox.Show("Неккоректно заданная траектория для X");
                return;
            }
            trajectoryFunctionsX = locTrajectoryFunctionsX;
            ComboboxDataSourceTrajectory selectItemY = (ComboboxDataSourceTrajectory)comboBoxFuncY.SelectedItem;
            TrajectoryFunctions locTrajectoryFunctionsY = new TrajectoryFunctions(float.Parse(textBoxKoeffY.Text), selectItemY.mathFunc, float.Parse(textBoxStepY.Text), float.Parse(textBoxMinY.Text), float.Parse(textBoxMaxY.Text), float.Parse(textBoxStartValY.Text), selectItemY.Text, checkBoxMinMaxValY.Checked);
            if (!locTrajectoryFunctionsY.ValidateTrajectoryFunc())
            {
                MessageBox.Show("Неккоректно заданная траектория для Y");
                return;
            }
            trajectoryFunctionsY = locTrajectoryFunctionsY;
            ComboboxDataSourceTrajectory selectItemZ = (ComboboxDataSourceTrajectory)comboBoxFuncZ.SelectedItem;
            TrajectoryFunctions locTrajectoryFunctionsZ = new TrajectoryFunctions(float.Parse(textBoxKoeffZ.Text), selectItemZ.mathFunc, float.Parse(textBoxStepZ.Text), float.Parse(textBoxMinZ.Text), float.Parse(textBoxMaxZ.Text), float.Parse(textBoxStartValZ.Text), selectItemZ.Text, checkBoxMinMaxValZ.Checked);
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
        public bool reversMove = false;
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
        public TrajectoryFunctions(float locKoeff, Func<double, double> locMathFunc, float locStep, float locMin, float locMax, float locStartVal, string nameFunc, bool locReversMove)
        {
            reversMove = locReversMove;
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
        public void NextValueAndSetSignStep()
        {
            if (!reversMove)
            {
                if (Iter >= colIter)
                {
                    signIter = -1;
                }
                if(Iter <= 0)
                {
                    signIter = 1;
                }
            }
            else
            {
                if (Iter >= colIter)
                {
                    Val = min;
                    Iter = 0;
                }
            }
            Val = Val + signIter * step;
            Iter += signIter;
        }
        public bool ValidateTrajectoryFunc()
        {
            return !(min > max || startVal > max || startVal < min);
        }
        public double getValue()
        {
            var localVal = koeff * mathFunc(Val * modifier);
            NextValueAndSetSignStep();
            return localVal;
        }
    }
}
