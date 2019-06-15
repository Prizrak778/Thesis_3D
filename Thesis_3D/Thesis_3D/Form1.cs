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

        private int _program;

        private bool _contour = false;
        private int _SelectID = -1;
        private float angel = 90f;

        private double _framecount = 0;

        private List<RenderObject> _renderObjects = new List<RenderObject>();
        private List<Color4> color4s_unique = new List<Color4>();
        private Vector2 lastMousePos = new Vector2(30f, 140f);

        private Random rnd = new Random();

        #region RandomColor
        private Color4 RandomColor()
        {
            Color4 temp_color = Color4.Black;
            bool flag = true;
            while (flag)
            {
                flag = false;
                temp_color = new Color4(rnd.Next(256), rnd.Next(256), rnd.Next(256), 255);
                for (int i = 0; i < color4s_unique.Count && flag; i++)
                {
                    if (temp_color == color4s_unique[i] && temp_color == Color4.Black)
                    {
                        flag = true;
                    }
                }
                if(!flag)
                {
                    color4s_unique.Add(temp_color);
                }
            }
            return temp_color;
        }
        #endregion

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

        #region CreateProjection
        private void CreateProjection()
        {
            float aspectRatio = (float)Width / Height;
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                angel * (float)Math.PI / 180f,
                aspectRatio,
                0.01f,
                40f);
            _modelView = camera1.GetViewMatrix();
            _view = _modelView * _projectionMatrix;
        }
        #endregion

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
            Application.Idle += Application_Idle;

            String VertexShader = @"Components\Shaders\vertexShader_c.vert";
            String FragentShader = @"Components\Shaders\fragmentShader.frag";
            _program = CompileShaders(VertexShader, FragentShader);

            _renderObjects.Add(new RenderObject(ObjectCreate.CreateSolidCube(0.5f, 0.0f, 2.0f, 0.0f), Color4.LightCoral, RandomColor()));
            for(int i = 0; i < 10; i++)
            {
                _renderObjects.Add(new RenderObject(ObjectCreate.CreateSolidCube(0.5f, (float)i + 1, 2.0f, 0.0f), Color4.LightCoral, RandomColor()));
            }
               
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }
        void Application_Idle(object sender, EventArgs e)
        {
            while (glControl1.IsIdle)
            {
                Render();
            }
        }
        private void glControl_Load(object sender, EventArgs e)
        {
            CreateProjection();
            glControl1.Resize += new EventHandler(glControl_Resize);
            glControl1.Paint += new PaintEventHandler(glControl_Paint);
            glControl1.MouseMove += new MouseEventHandler(glControl_MouseMove);
            glControl1.MouseDown += new MouseEventHandler(glControl_MouseDown);
            glControl1.KeyPress += new KeyPressEventHandler(glControl_KeyPress);
            glControl1.MouseWheel += new MouseEventHandler(glControl_MouseWheel);
            glControl1.MakeCurrent();
            camera1.Position = new Vector3(0, 2.5f, 2);
            camera1.Orientation = new Vector3(-(float)Math.PI, -(float)Math.PI, 0);
            camera1.AddRotation(0, 0);
            glControl_Resize(glControl1, EventArgs.Empty);

            GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.TextureCubeMap);
            GL.DepthFunc(DepthFunction.Less);
            GL.ClearColor(new Color4(0.3f, 0.3f, 0.3f, 0.0f));
        }
        void glControl_Resize(object sender, EventArgs e)
        {
            
            OpenTK.GLControl c = sender as OpenTK.GLControl;
            CreateProjection();
            if (c.ClientSize.Height == 0)
                c.ClientSize = new System.Drawing.Size(c.ClientSize.Width, 1);

            GL.LoadIdentity();
            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            //float aspect_ratio = Width / (float)Height;

            //_view = camera1.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1f, aspect_ratio * 90, 1.0f, 40.0f);
            //Matrix4 perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect_ratio, 1, 64);
            //GL.MatrixMode(MatrixMode.Projection);
            GL.UniformMatrix4(21, false, ref _projectionMatrix);
            //GL.LoadMatrix(ref perpective);
        }

        void glControl_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }
        void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            Vector2 delta = lastMousePos - new Vector2(e.X, e.Y);
            camera1.AddRotation(delta.X, delta.Y);
            lastMousePos = new Vector2(e.X, e.Y);
        }
        void glControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if(angel > 1 && angel < 179)
            {
                angel += e.Delta/120;
            }
            else if(angel == 1 && e.Delta > 0)
            {
                angel += e.Delta / 120;
            }
            else if (angel == 179 && e.Delta < 0)
            {
                angel += e.Delta / 120;
            }
        }
        void glControl_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            char t = e.KeyChar;
            switch(e.KeyChar)
            {
                case (char)27:
                    this.Close();
                    break;
                case '6':
                    camera1.AddRotation(-10f, 0f);
                    break;
                case '4':
                    camera1.AddRotation(10f, 0f);
                    break;
                case '8':
                    camera1.AddRotation(0f, 10f);
                    break;
                case '2':
                    camera1.AddRotation(0f, -10f);
                    break;
                case 'w':
                    camera1.Move(0f, 0.05f, 0f);
                    break;
                case 'W':
                    camera1.Move(0f, 0.05f, 0f);
                    break;
                case 'A':
                    camera1.Move(-0.05f, 0f, 0f);
                    break;
                case 'a':
                    camera1.Move(-0.05f, 0f, 0f);
                    break;
                case 'D':
                    camera1.Move(0.05f, 0f, 0f);
                    break;
                case 'd':
                    camera1.Move(0.05f, 0f, 0f);
                    break;
                case 'Q'://Вверх по y
                    camera1.Move(0f, 0f, 0.05f);
                    break;
                case 'q'://Вверх по y
                    camera1.Move(0f, 0f, 0.05f);
                    break;
                case 'E'://Вниз по y
                    camera1.Move(0f, 0f, -0.05f);
                    break;
                case 'e'://Вниз по y
                    camera1.Move(0f, 0f, -0.05f);
                    break;
                case 'S':
                    camera1.Move(0f, -0.05f, 0f);
                    break;
                case 's':
                    camera1.Move(0f, -0.05f, 0f);
                    break;
                case 'L':
                    _contour = _contour ? false : true;
                    break;
                case 'l':
                    _contour = _contour ? false : true;
                    break;
            }
        }

        private void glControl_MouseDown(object sender, MouseEventArgs e)
        {
            int colorFBO;
            GL.GenBuffers(1, out colorFBO);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, colorFBO);
            {
                _SelectID = -1;
                GL.Enable(EnableCap.DepthTest);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                int pixel = new int();
                Render_select_color_buf();
                GL.ReadPixels(e.X, glControl1.Height - e.Y, 1, 1, PixelFormat.Bgra, PixelType.UnsignedByte, ref pixel);

                Color color = Color.FromArgb(pixel);
                Color4 temp_color;
                temp_color.R = color.R;
                temp_color.G = color.G;
                temp_color.B = color.B;
                temp_color.A = 255;
                for(int i = 0; i < _renderObjects.Count; i++)
                {
                    if(color4s_unique[i]== temp_color)
                    {
                        _SelectID = i;
                    }
                }
            }
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        private void Render_select_color_buf()
        {
            CreateProjection();
            GL.UniformMatrix4(21, false, ref _projectionMatrix);
            GL.ClearColor(new Color4(0.0f, 0.0f, 0.0f, 1.0f));
            GL.UseProgram(_program);
            int iter = 0;
            Vector4 temp_color;
            foreach (var renderObject in _renderObjects)
            {
                temp_color.X = color4s_unique[iter].R / 255;
                temp_color.Y = color4s_unique[iter].G / 255;
                temp_color.Z = color4s_unique[iter].B / 255;
                temp_color.W = color4s_unique[iter].A / 255;
                Render_figure(renderObject, PolygonMode.Fill);
                GL.Uniform4(19, ref temp_color);
                renderObject.Render();
                iter++;
            }
        }

        private void Render_figure(RenderObject renderObject, PolygonMode polygon)
        {
            renderObject.Bind();
            GL.UniformMatrix4(20, false, ref _view);
            GL.UniformMatrix4(21, false, ref _projectionMatrix);
            GL.UniformMatrix4(22, false, ref _modelView);
            renderObject.PolygonMode_now(polygon);
        }
        private void Render()
        {
            DateTime dateTime = DateTime.Now;

            GL.ClearColor(new Color4(0.3f, 0.3f, 0.3f, 1.0f));
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            CreateProjection();
            GL.UseProgram(_program);
            foreach (var renderObject in _renderObjects)
            {
                
                Render_figure(renderObject, PolygonMode.Fill);
                Vector4 color = renderObject.Color4;
                GL.Uniform4(19, ref color);

                renderObject.Render();
            }
            if(_contour)
            {
                GL.LineWidth(3);
                foreach (var renderObject in _renderObjects)
                {
                    
                    Render_figure(renderObject, PolygonMode.Line);
                    Vector4 color = new Vector4(0, 0, 0, 255);
                    GL.Uniform4(19, ref color);

                    renderObject.Render_line();
                }
            }
            if(_SelectID > -1)
            {
                GL.LineWidth(7);
                Render_figure(_renderObjects[_SelectID], PolygonMode.Line);
                Vector4 color = new Vector4(0, 0, 0, 255);
                GL.Uniform4(19, ref color);
                _renderObjects[_SelectID].Render_line();
            }
            glControl1.SwapBuffers();

            TimeSpan timeSpan = DateTime.Now - dateTime;
            _framecount = 1f / (timeSpan.TotalMilliseconds / 1000);
            Text =
                GL.GetString(StringName.Vendor) + " " +
                GL.GetString(StringName.Renderer) + " " +
                GL.GetString(StringName.Version);
            Text += $" (Vsync: {glControl1.VSync})";
            Text += $" (FPS: {_framecount:0})";
            Text += $"(Position:{camera1.Position})";
        }
    }
}
