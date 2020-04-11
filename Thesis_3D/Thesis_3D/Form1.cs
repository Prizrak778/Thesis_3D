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
        private Matrix4 _ViewMatrix;
        private Matrix4 _MVP; //Modal * View * Matrix

        private int _program = -1;
        private int _program_contour = -1;
        private int _program_some_light = -1;
        private int _program_Fong_directed = -1;

        private bool _contour = false;
        private int _SelectID = -1;
        private float angel = 90f;

        private double _framecount = 0;

        private List<RenderObject> _renderObjects = new List<RenderObject>();
        private List<LightObject> _lightObjects = new List<LightObject>();
        private List<Color4> color4s_unique = new List<Color4>();
        private List<int> listProgram = new List<int>();
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
                if (!flag)
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
            GL.DeleteProgram(vertexShader);
            GL.DeleteProgram(fragmentShader);
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
            _ViewMatrix = camera1.GetViewMatrix();
            _MVP = _ViewMatrix * _projectionMatrix;
        }
        #endregion

        private void ChoiseShader(int Index)
        {
            _program = listProgram[Index];
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

        bool CompileAllShaders(out string error)
        {
            error = string.Empty;
            String VertexShader = @"Components\Shaders\vertexShader_c.vert";
            String FragentShader = @"Components\Shaders\fragmentShader.frag";
            if((_program_contour = _program = CompileShaders(VertexShader, FragentShader)) == -1)
            {
                error = "Ошибка при компиляции обычного шейдера";
                return false;
            }
            VertexShader = @"Components\Shaders\vertexShader.vert";
            FragentShader = @"Components\Shaders\fragmentShader.frag";
            listProgram.Add(_program);
            if ((_program = CompileShaders(VertexShader, FragentShader)) == -1)
            {
                error = "Ошибка при компиляции шейдера т.и. без отражения";
                return false;
            }
            listProgram.Add(_program);
            VertexShader = @"Components\Shaders\vertexShader_mirror.vert";
            FragentShader = @"Components\Shaders\fragmentShader.frag";
            if ((_program = CompileShaders(VertexShader, FragentShader)) == -1)
            {
                error = "Ошибка при компиляции шейдера т.и. с отражением";
                return false;
            }
            listProgram.Add(_program);
            VertexShader = @"Components\Shaders\vertexShader_double_mirror.vert";
            FragentShader = @"Components\Shaders\fragmentShader_double_mirror.frag";
            if ((_program = CompileShaders(VertexShader, FragentShader)) == -1)
            {
                error = "Ошибка при компиляции шейдера т.и. с двойным отражением";
                return false;
            }
            listProgram.Add(_program);
            VertexShader = @"Components\Shaders\vertexShader_flatshadow.vert";
            FragentShader = @"Components\Shaders\fragmentShader.frag";
            if ((_program = CompileShaders(VertexShader, FragentShader)) == -1)
            {
                error = "Ошибка при компиляции шейдера т.и. с плоским затенением";
                return false;
            }
            listProgram.Add(_program);
            VertexShader = @"Components\Shaders\vertexShader_some_light.vert";
            FragentShader = @"Components\Shaders\fragmentShader.frag";
            if ((_program = CompileShaders(VertexShader, FragentShader)) == -1)
            {
                error = "Ошибка при компиляции шейдера несколько т.и.";
                return false;
            }
            _program_some_light = _program;
            listProgram.Add(_program);
            VertexShader = @"Components\Shaders\vertexShader_Lgh_directed.vert";
            FragentShader = @"Components\Shaders\fragmentShader.frag";
            if ((_program = CompileShaders(VertexShader, FragentShader)) == -1)
            {
                error = "Ошибка при компиляции шейдера направленного т.и";
                return false;
            }
            listProgram.Add(_program);
            VertexShader = @"Components\Shaders\vertexShader_Fong.vert";
            FragentShader = @"Components\Shaders\fragmentShader_Fong.frag";
            if ((_program = CompileShaders(VertexShader, FragentShader)) == -1)
            {
                error = "Ошибка при компиляции шейдера затенение по Фонгу";
                return false;
            }
            listProgram.Add(_program);
            VertexShader = @"Components\Shaders\vertexShader_Fong.vert";
            FragentShader = @"Components\Shaders\fragmentShader_Fong_half.frag";
            if ((_program = CompileShaders(VertexShader, FragentShader)) == -1)
            {
                error = "Ошибка при компиляции шейдера затенение по Фонгу с использованием вектора полпути ";
                return false;
            }
            listProgram.Add(_program);
            VertexShader = @"Components\Shaders\vertexShader_Fong.vert";
            FragentShader = @"Components\Shaders\fragmentShader_Fong_directed.frag";
            if ((_program = CompileShaders(VertexShader, FragentShader)) == -1)
            {
                error = "Ошибка при компиляции шейдера узконаправленый источник";
                return false;
            }
            _program_Fong_directed = _program;
            listProgram.Add(_program);
            return true;
        }

        protected override void OnLoad(EventArgs e)
        {
            glControl1.Load += new EventHandler(glControl_Load);
            glControl_Load(glControl1, EventArgs.Empty);
            Application.Idle += Application_Idle;
            String ErrorText = string.Empty;
            if(!CompileAllShaders(out ErrorText))
            {
                throw new Exception(ErrorText);
            }
            comboBox1.Items.AddRange(new object[] { "Обычные цвета", "Т.И. без отражения", "Т.И. с отражением", "Т.И. с двойным отражением", "Т.И. с плоским затенением", "Несколько Т.И.", "Направленный источник", "Затенение по Фонгу", "Затенение по Фонгу с использованием вектора полпути", "Узконаправленный источник" });
            comboBox1.SelectedIndex = 0;
            
            _renderObjects.Add(new RenderObject(ObjectCreate.CreateSolidCube(0.5f, 0.0f, 2.0f, 0.0f), Color4.LightCoral, RandomColor()));
            for (int i = 0; i < 10; i++)
            {
                _renderObjects.Add(new RenderObject(ObjectCreate.CreateSolidCube(0.5f, (float)i + 1, 2.0f, 0.0f), Color4.LightCoral, RandomColor()));
            }
            for (int i = 0; i < 10; i++)
            {
                _renderObjects.Add(new RenderObject(ObjectCreate.CreateSolidCube(0.5f, 1, -(float)i + 2.0f, 0.0f), Color4.LightCoral, RandomColor()));
            }
            Vector3 position_light = new Vector3(1.0f, 3.0f, 1.0f);
            _lightObjects.Add(new LightObject(ObjectCreate.CreateSolidCube(0.1f, position_light.X, position_light.Y, position_light.Z), Color4.Yellow, RandomColor(), position_light, new Vector4(5.0f, 5.0f, 1.0f, 1.0f), new Vector3(0.2f, -1f, -0.3f), new Vector3(0.3f, 0.3f, 0.0f), new Vector3(1.0f, 0.0f, 5f), _program_Fong_directed));
            position_light = new Vector3(4.0f, 3.0f, 1.0f);
            _lightObjects.Add(new LightObject(ObjectCreate.CreateSolidCube(0.1f, position_light.X, position_light.Y, position_light.Z), Color4.Yellow, RandomColor(), position_light, new Vector4(5.0f, 5.0f, 1.0f, 1.0f), new Vector3(0.2f, -1f, -0.3f), new Vector3(0.0f, 0.3f, 0.3f), new Vector3(1.0f, 0.0f, 5f)));
            position_light = new Vector3(7.0f, 3.0f, 1.0f);
            _lightObjects.Add(new LightObject(ObjectCreate.CreateSolidCube(0.1f, position_light.X, position_light.Y, position_light.Z), Color4.Yellow, RandomColor(), position_light, new Vector4(5.0f, 5.0f, 1.0f, 1.0f), new Vector3(0.2f, -1f, -0.3f), new Vector3(0.3f, 0.0f, 0.3f), new Vector3(1.0f, 0.0f, 5f)));
            foreach (var obj in _lightObjects)
            {
                _renderObjects.Add(obj);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Application.Idle -= Application_Idle;
            foreach (var obj in _renderObjects)
                obj.Dispose();
            foreach (var obj in _lightObjects)
                obj.Dispose();
            GL.DeleteProgram(_program);
            GL.DeleteProgram(_program_contour);
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
            glControl1.KeyDown += new KeyEventHandler(glControl_KeyPressDown);
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

        #region MouseEvent
        void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            Vector2 delta = lastMousePos - new Vector2(e.X, e.Y);
            if (camera1.Rotation_status)
            {
                camera1.AddRotation(delta.X, delta.Y);
            }
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
                for (int i = 0; i < _renderObjects.Count; i++)
                {
                    if (color4s_unique[i] == temp_color)
                    {
                        _SelectID = i;
                    }
                }
            }
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
        #endregion

        #region KeyEvent
        void glControl_KeyPressDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    this.Close();
                    break;
                case Keys.J:
                    camera1.AddRotation(10f, 0f);
                    break;
                case Keys.L:
                    camera1.AddRotation(-10f, 0f);
                    break;
                case Keys.I:
                    camera1.AddRotation(0f, 10f);
                    break;
                case Keys.K:
                    camera1.AddRotation(0f, -10f);
                    break;
                case Keys.W:
                    camera1.Move(0f, 0.05f, 0f);
                    break;
                case Keys.S:
                    camera1.Move(0f, -0.05f, 0f);
                    break;
                case Keys.A:
                    camera1.Move(-0.05f, 0f, 0f);
                    break;
                case Keys.D:
                    camera1.Move(0.05f, 0f, 0f);
                    break;
                case Keys.Q://Вверх по y
                    camera1.Move(0f, 0f, 0.05f);
                    break;
                case Keys.E://Вниз по y
                    camera1.Move(0f, 0f, -0.05f);
                    break;
                case Keys.C:
                    camera1.Rotation_change_status();
                    checkBox2.Checked = camera1.Rotation_status;
                    break;
                case Keys.O:
                    _contour = _contour ? false : true;
                    checkBox1.Checked = _contour;
                    break;
            }
            if (_SelectID > -1)
            {
                switch (e.KeyData)
                {
                    case Keys.D8:
                        _renderObjects[_SelectID].changeModelMstrix(new Vector3(1, 0, 0));
                        break;
                    case Keys.D6:
                        _renderObjects[_SelectID].changeModelMstrix(new Vector3(0, 0, 1));
                        break;
                    case Keys.D4:
                        _renderObjects[_SelectID].changeModelMstrix(new Vector3(0, 0, -1));
                        break;
                    case Keys.D2:
                        _renderObjects[_SelectID].changeModelMstrix(new Vector3(-1, 0, 0));
                        break;
                    case Keys.D7:
                        _renderObjects[_SelectID].changeModelMstrix(new Vector3(0, -1, 0));
                        break;
                    case Keys.D9:
                        _renderObjects[_SelectID].changeModelMstrix(new Vector3(0, 1, 0));
                        break;
                }
                if(_renderObjects[_SelectID].TypeObject == TypeObjectRender.LightSourceObject)
                {
                    foreach(var lightObject in _lightObjects)
                    {
                        if(lightObject.Color_choice == _renderObjects[_SelectID].Color_choice)
                        {
                            lightObject.SetPositionLight(_renderObjects[_SelectID].ModelMatrix);
                            if (_program_Fong_directed != -1 && lightObject.uboHandle != -1) lightObject.UpdateBufferForBlock(_program_Fong_directed);
                            break; // -_-
                        }
                    }
                }
            }
        }
        #endregion

        
        private void Render_select_color_buf()
        {
            CreateProjection();
            GL.UniformMatrix4(21, false, ref _projectionMatrix);
            GL.ClearColor(new Color4(0.0f, 0.0f, 0.0f, 1.0f));
            GL.UseProgram(_program_contour);
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
            GL.UniformMatrix4(20, false, ref _MVP);
            //GL.UniformMatrix4(21, false, ref _projectionMatrix);
            GL.UniformMatrix4(22, false, ref renderObject.ModelMatrix);
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
                Vector4 color = renderObject.Color_obj;
                GL.Uniform4(19, ref color);
                if (_program == _program_some_light)
                {
                    foreach (var light in _lightObjects.Cast<LightObject>().Select((r, i) => new { Row = r, Index = i }))
                    {
                        
                            light.Row.PositionLightUniform(16 + light.Index);
                            light.Row.IntensityLightUniform(13 + light.Index);
                        
                    }
                }
                else if(_program == _program_Fong_directed)
                {
                    if(_lightObjects[0].uboHandle != -1) //GL.BindBufferBase(BufferTarget.UniformBuffer, 24, _lightObjects[0].uboHandle);
                    GL.BindBufferRange(BufferRangeTarget.UniformBuffer, 24, _lightObjects[0].uboHandle, (IntPtr)0, _lightObjects[0].blockSize);
                }
                else
                {
                    _lightObjects[0].PositionLightUniform(18);
                }
                renderObject.Render();
            }
            GL.UseProgram(_program_contour);
            if (_contour)
            {
                GL.LineWidth(4);
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
            Text += $" (Position:{camera1.Position})";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _contour = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            camera1.Rotation_status = checkBox2.Checked;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChoiseShader(comboBox1.SelectedIndex);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            camera1.Move(0, 0, 1.5f);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            camera1.Move(0, 1.5f, 0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            camera1.Move(0, 0, -1.5f);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            camera1.Move(-1.5f, 0.0f, 0);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            camera1.Move(0, -1.5f, 0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            camera1.Move(1.5f, 0, 0);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            camera1.AddRotation(0.0f, 10.0f);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            camera1.AddRotation(10.0f, 0.0f);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            camera1.AddRotation(0.0f, -10.0f);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            camera1.AddRotation(-10.0f, 0.0f);
        }

        private void textBox1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) && string.IsNullOrWhiteSpace(textBox2.Text) && string.IsNullOrWhiteSpace(textBox3.Text))
                camera1.Position = new Vector3((float)Convert.ToDouble(textBox1.Text), (float)Convert.ToDouble(textBox2.Text), (float)Convert.ToDouble(textBox3.Text));
        }
    }
}
