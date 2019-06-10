using System;
using System.IO;
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
        Camera camera1 = new Camera();
        private Matrix4 _projectionMatrix;
        private Matrix4 _modelView;
        private Matrix4 _view;

        private List<RenderObject> _renderObjects = new List<RenderObject>();

        #region CompileShaders
        private int CompileShaders(String VertexString, String FragmentString, String GeometricString = "")
        {
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, File.ReadAllText(VertexString));
            GL.CompileShader(vertexShader);
            int geometryShader = 0;
            if (GeometricString != "")
            {
                geometryShader = GL.CreateShader(ShaderType.GeometryShader);
                GL.ShaderSource(geometryShader, File.ReadAllText(GeometricString));
                GL.CompileShader(geometryShader);
            }

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, File.ReadAllText(FragmentString));
            GL.CompileShader(fragmentShader);

            var program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            if (GeometricString != "")
            {
                GL.AttachShader(program, geometryShader);
            }
            GL.LinkProgram(program);

            GL.DetachShader(program, vertexShader);
            GL.DetachShader(program, fragmentShader);
            if (GeometricString != "")
            {
                GL.DetachShader(program, geometryShader);
                GL.DeleteProgram(geometryShader);
            }
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
            return program;
        }
        #endregion

        private void CreateProjection()
        {
            float aspectRatio = (float)Width / Height;
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                90f * (float)Math.PI / 180f,
                aspectRatio,
                0.1f,
                400f);
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
            glControl1.Load += new EventHandler(glControl_Load);
            glControl_Load(glControl1, EventArgs.Empty);
            GL.ClearColor(new Color4(0.3f, 0.3f, 0.3f, 0.0f));
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
            Application.Idle += Application_Idle;
            _renderObjects.Add(new RenderObject(ObjectCreate.CreateSolidCube(0.5f, 0.0f, 2.0f, 0.0f), Color4.LightCoral, Color4.Black));
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }
        void Application_Idle(object sender, EventArgs e)
        {

            String Text_glcontrol = Text;
            //Text += $" (FPS: {1f / time_now:0})";
            Text += $"(Position:{camera1.Position})";
            while (glControl1.IsIdle)
            {
                if (Focused)
                {

                }
                Render();
            }

            Text = Text_glcontrol;
        }
        private void glControl_Load(object sender, EventArgs e)
        {
            CreateProjection();
            glControl1.Resize += new EventHandler(glControl_Resize);
            glControl1.Paint += new PaintEventHandler(glControl_Paint);
            glControl1.MakeCurrent();
            camera1.Position = new Vector3(0, 2.5f, 2);
            camera1.Orientation = new Vector3(-(float)Math.PI, -(float)Math.PI, 0);
            glControl_Resize(glControl1, EventArgs.Empty);
        }
        void glControl_Resize(object sender, EventArgs e)
        {
            CreateProjection();
            OpenTK.GLControl c = sender as OpenTK.GLControl;

            if (c.ClientSize.Height == 0)
                c.ClientSize = new System.Drawing.Size(c.ClientSize.Width, 1);
        }
        void glControl_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }
        private void Render()
        {
            GL.ClearColor(new Color4(0.3f, 0.3f, 0.3f, 1.0f));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //Матрица проекции и вида
            CreateProjection();
        }
        
    }
}
