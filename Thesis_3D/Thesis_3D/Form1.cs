using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

namespace Thesis_3D
{
    public partial class Form1 : Form
    {
        Camera camera1;
        private Matrix4 _projectionMatrix;
        private Matrix4 _modelView;
        private Matrix4 _view;

        private void CreateProjection()
        {
            float aspectRatio = (float)Width / Height;
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                90f * (float)Math.PI / 180f,
                aspectRatio,
                0.1f,
                400f);
            //_projectionMatrix = Matrix4.CreatePerspectiveOffCenter(-0.5f, 0.5f, -0.5f, 0.5f, 0.1f, 400f);
            //_projectionMatrix = Matrix4.CreateOrthographicOffCenter(-4*glControl.Width / glControl.Height, 4*glControl.Width / glControl.Height, -4 / aspectRatio, 4 / aspectRatio, 0.7f, 400f);
            //_projectionMatrix = Matrix4.CreateOrthographic(10* aspectRatio, 10, 0.1f, 400f );
            _modelView = camera1.GetViewMatrix();
            _view = _modelView * _projectionMatrix;
        }
        public Form1()
        {
            OpenTK.Toolkit.Init();
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        #region Form1_Resize

        private void Form1_Resize(object sender, EventArgs e)
        {
            base.OnResize(e);
        }

        #endregion
        protected override void OnLoad(EventArgs e)
        {
            Text =
                GL.GetString(StringName.Vendor) + " " +
                GL.GetString(StringName.Renderer) + " " +
                GL.GetString(StringName.Version);
            Text += $" (Vsync: {glControl1.VSync})";
            GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.TextureCubeMap);
            GL.DepthFunc(DepthFunction.Less);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }
        private void glControl_Load(object sender, EventArgs e)
        {
            CreateProjection();
            camera1.Position = new Vector3(0, 2.5f, 2);
            camera1.Orientation = new Vector3(-(float)Math.PI, -(float)Math.PI, 0);
            // Ensure that the viewport and projection matrix are set correctly.
            glControl_Resize(glControl1, EventArgs.Empty);
        }
        void glControl_Resize(object sender, EventArgs e)
        {

        }
    }
}
