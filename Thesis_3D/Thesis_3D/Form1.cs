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
        private int _program_Fong_fog = -1;
        private int _program_shadow_project = -1;
        private int _program_Fong = -1;

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
            string VertexShader = @"Components\Shaders\vertexShader_c.vert";
            string FragentShader = @"Components\Shaders\fragmentShader.frag";
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
            _program_Fong = _program;
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
            VertexShader = @"Components\Shaders\vertexShader_Fong.vert";
            FragentShader = @"Components\Shaders\fragmentShader_Fong_fog.frag";
            if ((_program = CompileShaders(VertexShader, FragentShader)) == -1)
            {
                error = "Ошибка при компиляции шейдера туман";
                return false;
            }
            _program_Fong_fog = _program;
            listProgram.Add(_program);
            VertexShader = @"Components\Shaders\vertexShader_shadow.vert";
            FragentShader = @"Components\Shaders\fragmentShader_shadow.frag";
            if ((_program = CompileShaders(VertexShader, FragentShader)) == -1)
            {
                error = "Ошибка при компиляции шейдера плоских теней";
                return false;
            }
            _program_shadow_project = _program;
            listProgram.Add(_program);
            return true;
        }

        protected override void OnLoad(EventArgs e)
        {
            glControl1.Load += new EventHandler(glControl_Load);
            glControl_Load(glControl1, EventArgs.Empty);
            Application.Idle += Application_Idle;
            string ErrorText = string.Empty;
            if(!CompileAllShaders(out ErrorText))
            {
                throw new Exception(ErrorText);
            }
            comboBoxShaders.Items.AddRange(new object[] { "Обычные цвета", "Т.И. без отражения", "Т.И. с отражением", "Т.И. с двойным отражением", "Т.И. с плоским затенением", "Несколько Т.И.", "Направленный источник", "Затенение по Фонгу", "Затенение по Фонгу с использованием вектора полпути", "Узконаправленный источник", "Туман", "Плоское затенение для одного элемента" });
            comboBoxShaders.SelectedIndex = 0;
            _renderObjects.Add(new RenderObject(ObjectCreate.CreatePlane(1.5f, 0.0f, 0.0f, 0.0f, 0, 0, 45), Color4.LightCyan, RandomColor(), plane: true));
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
            buttonChangeFigure.Enabled = false;
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
            Text =
                GL.GetString(StringName.Vendor) + " " +
                GL.GetString(StringName.Renderer) + " " +
                GL.GetString(StringName.Version);
            Text += $" (Vsync: {glControl1.VSync})";
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
            if(_SelectID > -1)
            {
                buttonChangeFigure.Enabled = true;
            }
            else
            {
                buttonChangeFigure.Enabled = false;
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
                            if (_program_Fong_directed != -1 && lightObject.uboLightInfo != -1) lightObject.UpdatePositionForBlock(_program_Fong_directed);
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
            camera1.SetPositionCamerShader(23);
            var countLightObj = _lightObjects.Count;
            var countRenderObj = _renderObjects.Count;
            foreach (var renderObject in _renderObjects)
            {
                if (_program == _program_shadow_project)
                {
                    if (countLightObj > 0 && countRenderObj > 0 && _lightObjects[0].Color_choice != renderObject.Color_choice && _renderObjects[0].Color_choice != renderObject.Color_choice)
                    {
                        GL.UseProgram(_program_shadow_project);
                        GL.UniformMatrix4(23, false, ref _renderObjects[0].ModelMatrix);
                        GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 5, _renderObjects[0].ShadowProjectBuffer());
                        GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
                        Render_figure(renderObject, PolygonMode.Fill);
                        _lightObjects[0].PositionLightUniform(18);
                        renderObject.Render();
                        
                    }
                    GL.UseProgram(_program_Fong);
                }
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
                else if(_program == _program_Fong_fog && countLightObj > 0)
                {
                    _lightObjects[0].PositionLightUniform(18);
                    _lightObjects[0].SetAttrFog(25, 1f, 24, 9f, 26, new Vector3(0.3f, 0.3f, 0.3f));
                }
                else if(_program == _program_Fong_directed && countLightObj > 0)
                {
                    if(_lightObjects[0].uboLightInfo != -1) GL.BindBufferRange(BufferRangeTarget.UniformBuffer, 24, _lightObjects[0].uboLightInfo, (IntPtr)0, _lightObjects[0].blockSizeLightInfo);
                }
                else
                {
                    if (countLightObj > 0) _lightObjects[0].PositionLightUniform(18);
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
            
            label5.Text = $"Position:{camera1.Position:0}";
            label6.Text = $"FPS: {_framecount:0}";

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
            ChoiseShader(comboBoxShaders.SelectedIndex);
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

        private void buttonNewAnFigure_Click(object sender, EventArgs e)
        {
            //По хорошему форму стоит вынести отдельно
            Form dlgNewAnFigure = new Form()
            {
                Text = "Выбор фигуры",
                Width = 300,
                Height = 140,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label lblTypeFigure = new Label() { Text = "Тип фигуры:", Visible = true, Left = 30, Width = 100, Top = 30 };
            Button confirmation = new Button() { Text = "Ok", Left = 100, Width = 80, Top = 60, DialogResult = DialogResult.OK };
            Button close = new Button() { Text = "Отмена", Left = 192, Width = 80, Top = 60, DialogResult = DialogResult.Cancel };
            ComboBox comboBox = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Text = "Куб", Left = 110, Width = 160, Top = 27 };
            dlgNewAnFigure.Controls.Add(comboBox);
            comboBox.Items.AddRange(new object[] {
            "Плоскость",
            "Куб",
            "Сфера"});
            comboBox.SelectedIndex = 0;
            confirmation.Click += (senderOk, eOk) => { dlgNewAnFigure.Close(); };
            close.Click += (senderClose, eClose) => { dlgNewAnFigure.Close(); };
            dlgNewAnFigure.Controls.Add(lblTypeFigure);
            dlgNewAnFigure.Controls.Add(confirmation);
            dlgNewAnFigure.Controls.Add(close);
            dlgNewAnFigure.AcceptButton = confirmation;
            dlgNewAnFigure.CancelButton = close;
            if (dlgNewAnFigure.ShowDialog() == DialogResult.OK)
            {
                if (comboBox.Text == "Куб")
                {
                    dlgNewAnFigure = new Form()
                    {
                        Text = "Данные для фигуры",
                        Width = 350,
                        Height = 240,
                        FormBorderStyle = FormBorderStyle.FixedDialog,
                        StartPosition = FormStartPosition.CenterScreen
                    };
                    Label label_side = new Label() { Text = "Расстояние от центра то границы", Left = 10, Width = 190, Top = 30 };
                    Label label_shift_lr = new Label() { Text = "Смещение по x", Left = 10, Width = 190, Top = 60 };
                    Label label_shift_y = new Label() { Text = "Смещение по y", Left = 10, Width = 190, Top = 90 };
                    Label label_shift_ud = new Label() { Text = "Смещение по z", Left = 10, Width = 190, Top = 120 };
                    TextBox textBoxSide = new TextBox() { Text = "0", Left = 200, Width = 100, Top = 30 };
                    TextBox textBoxShift_lr = new TextBox() { Text = "0", Left = 200, Width = 100, Top = 60 };
                    TextBox textBoxShift_y = new TextBox() { Text = "0", Left = 200, Width = 100, Top = 90 };
                    TextBox textBoxShift_ud = new TextBox() { Text = "0", Left = 200, Width = 100, Top = 120 };
                    Button confirmation_new = new Button() { Text = "Ok", Left = 200, Width = 100, Top = 150, DialogResult = DialogResult.OK };
                    ColorDialog colorDialog = new ColorDialog();
                    dlgNewAnFigure.Controls.Add(label_side);
                    dlgNewAnFigure.Controls.Add(label_shift_lr);
                    dlgNewAnFigure.Controls.Add(label_shift_y);
                    dlgNewAnFigure.Controls.Add(label_shift_ud);
                    dlgNewAnFigure.Controls.Add(textBoxSide);
                    dlgNewAnFigure.Controls.Add(textBoxShift_lr);
                    dlgNewAnFigure.Controls.Add(textBoxShift_y);
                    dlgNewAnFigure.Controls.Add(textBoxShift_ud);
                    dlgNewAnFigure.Controls.Add(confirmation_new);
                    if (dlgNewAnFigure.ShowDialog() == DialogResult.OK)
                    {
                        Color4 colorcube = Color4.White;
                        if (colorDialog.ShowDialog() == DialogResult.OK)
                        {
                            colorcube = colorDialog.Color;
                        }
                        Vertex[] figure_vertex = ObjectCreate.CreateSolidCube(float.Parse(textBoxSide.Text, System.Globalization.NumberStyles.Float), float.Parse(textBoxShift_lr.Text), float.Parse(textBoxShift_y.Text), float.Parse(textBoxShift_ud.Text));
                        _renderObjects.Add(new RenderObject(figure_vertex, colorcube, RandomColor()));
                    }
                }
                else if (comboBox.Text == "Сфера")
                {
                    dlgNewAnFigure = new Form()
                    {
                        Text = "Данные для фигуры",
                        Width = 350,
                        Height = 340,
                        FormBorderStyle = FormBorderStyle.FixedDialog,
                        StartPosition = FormStartPosition.CenterScreen
                    };
                    Label label_side = new Label() { Text = "Расстояние от центра то границы", Left = 10, Width = 190, Top = 30 };
                    Label label_shift_lr = new Label() { Text = "Смещение по x", Left = 10, Width = 190, Top = 60 };
                    Label label_shift_y = new Label() { Text = "Смещение по y", Left = 10, Width = 190, Top = 90 };
                    Label label_shift_ud = new Label() { Text = "Смещение по z", Left = 10, Width = 190, Top = 120 };
                    Label label_nx = new Label() { Text = "Количество разбиений по x", Left = 10, Width = 180, Top = 150 };
                    Label label_ny = new Label() { Text = "Количество разбиений по y", Left = 10, Width = 180, Top = 180 };
                    Label label_k1 = new Label() { Text = "Коэффициент сжатия по x", Left = 10, Width = 180, Top = 210 };
                    Label label_k2 = new Label() { Text = "Коэффициент сжатия по y", Left = 10, Width = 180, Top = 240 };
                    TextBox textBoxSide = new TextBox() { Text = "0", Left = 200, Width = 100, Top = 30 };
                    TextBox textBoxShift_lr = new TextBox() { Text = "0", Left = 200, Width = 100, Top = 60 };
                    TextBox textBoxShift_y = new TextBox() { Text = "0", Left = 200, Width = 100, Top = 90 };
                    TextBox textBoxShift_ud = new TextBox() { Text = "0", Left = 200, Width = 100, Top = 120 };
                    TextBox textBoxNx = new TextBox() { Text = "0", Left = 200, Width = 100, Top = 150 };
                    TextBox textBoxNy = new TextBox() { Text = "0", Left = 200, Width = 100, Top = 180 };
                    TextBox textBoxk1 = new TextBox() { Text = "0", Left = 200, Width = 100, Top = 210 };
                    TextBox textBoxk2 = new TextBox() { Text = "0", Left = 200, Width = 100, Top = 240 };
                    Button confirmation_new = new Button() { Text = "Ok", Left = 150, Width = 100, Top = 270, DialogResult = DialogResult.OK };
                    ColorDialog colorDialog = new ColorDialog();
                    dlgNewAnFigure.Controls.Add(label_side);
                    dlgNewAnFigure.Controls.Add(label_shift_lr);
                    dlgNewAnFigure.Controls.Add(label_shift_y);
                    dlgNewAnFigure.Controls.Add(label_shift_ud);
                    dlgNewAnFigure.Controls.Add(label_nx);
                    dlgNewAnFigure.Controls.Add(label_ny);
                    dlgNewAnFigure.Controls.Add(label_k1);
                    dlgNewAnFigure.Controls.Add(label_k2);
                    dlgNewAnFigure.Controls.Add(textBoxSide);
                    dlgNewAnFigure.Controls.Add(textBoxShift_lr);
                    dlgNewAnFigure.Controls.Add(textBoxShift_y);
                    dlgNewAnFigure.Controls.Add(textBoxShift_ud);
                    dlgNewAnFigure.Controls.Add(textBoxNx);
                    dlgNewAnFigure.Controls.Add(textBoxNy);
                    dlgNewAnFigure.Controls.Add(textBoxk1);
                    dlgNewAnFigure.Controls.Add(textBoxk2);
                    dlgNewAnFigure.Controls.Add(confirmation_new);
                    if (dlgNewAnFigure.ShowDialog() == DialogResult.OK)
                    {
                        Color4 colorcube = Color4.White;
                        if (colorDialog.ShowDialog() == DialogResult.OK)
                        {
                            colorcube = colorDialog.Color;
                        }
                        Vertex[] figure_vertex = ObjectCreate.CreateSphere(float.Parse(textBoxSide.Text), float.Parse(textBoxShift_lr.Text), float.Parse(textBoxShift_y.Text), float.Parse(textBoxShift_ud.Text), Convert.ToInt32(textBoxNx.Text), Convert.ToInt32(textBoxNy.Text), Convert.ToInt32(textBoxk1.Text), Convert.ToInt32(textBoxk2.Text));
                        _renderObjects.Add(new RenderObject(figure_vertex, colorcube, RandomColor()));
                    }
                }
                else if (comboBox.Text == "Плоскость")
                {
                    dlgNewAnFigure = new Form()
                    {
                        Text = "Данные для фигуры",
                        Width = 350,
                        Height = 340,
                        FormBorderStyle = FormBorderStyle.FixedDialog,
                        StartPosition = FormStartPosition.CenterScreen
                    };
                    Label label_side = new Label() { Text = "Расстояние от центра то границы", Left = 10, Width = 190, Top = 30 };
                    Label label_shift_lr = new Label() { Text = "Смещение по x", Left = 10, Width = 190, Top = 60 };
                    Label label_shift_y =  new Label() { Text = "Смещение по y", Left = 10, Width = 190, Top = 90 };
                    Label label_shift_ud = new Label() { Text = "Смещение по z", Left = 10, Width = 190, Top = 120 };
                    Label label_angle_x = new Label() { Text = "Угол поворота по оси x", Left = 10, Width = 180, Top = 150 };
                    Label label_angle_y = new Label() { Text = "Угол поворота по оси y", Left = 10, Width = 180, Top = 180 };
                    Label label_angle_z = new Label() { Text = "Угол поворота по оси z", Left = 10, Width = 180, Top = 210 };
                    TextBox textBoxSide = new TextBox() { Text = "0", Left = 200, Width = 100, Top = 30 };
                    TextBox textBoxShift_lr = new TextBox() { Text = "0", Left = 200, Width = 100, Top = 60 };
                    TextBox textBoxShift_y = new TextBox() { Text = "0", Left = 200, Width = 100, Top = 90 };
                    TextBox textBoxShift_ud = new TextBox() { Text = "0", Left = 200, Width = 100, Top = 120 };
                    NumericUpDown textBoxAngleX = new NumericUpDown() { Value = 0, Minimum = -360, Maximum = 360, Left = 200, Width = 100, Top = 150 };
                    NumericUpDown textBoxAngleY = new NumericUpDown() { Value = 0, Minimum = -360, Maximum = 360, Left = 200, Width = 100, Top = 180 };
                    NumericUpDown textBoxAngleZ = new NumericUpDown() { Value = 0, Minimum = -360, Maximum = 360, Left = 200, Width = 100, Top = 210 };
                    Button confirmation_new = new Button() { Text = "Ok", Left = 150, Width = 100, Top = 270, DialogResult = DialogResult.OK };
                    ColorDialog colorDialog = new ColorDialog();
                    dlgNewAnFigure.Controls.Add(label_side);
                    dlgNewAnFigure.Controls.Add(label_shift_lr);
                    dlgNewAnFigure.Controls.Add(label_shift_y);
                    dlgNewAnFigure.Controls.Add(label_shift_ud);
                    dlgNewAnFigure.Controls.Add(label_angle_x);
                    dlgNewAnFigure.Controls.Add(label_angle_y);
                    dlgNewAnFigure.Controls.Add(label_angle_z);
                    dlgNewAnFigure.Controls.Add(textBoxSide);
                    dlgNewAnFigure.Controls.Add(textBoxShift_lr);
                    dlgNewAnFigure.Controls.Add(textBoxShift_y);
                    dlgNewAnFigure.Controls.Add(textBoxShift_ud);
                    dlgNewAnFigure.Controls.Add(textBoxAngleX);
                    dlgNewAnFigure.Controls.Add(textBoxAngleY);
                    dlgNewAnFigure.Controls.Add(textBoxAngleZ);
                    dlgNewAnFigure.Controls.Add(confirmation_new);
                    if (dlgNewAnFigure.ShowDialog() == DialogResult.OK)
                    {
                        Color4 colorcube = Color4.White;
                        if (colorDialog.ShowDialog() == DialogResult.OK)
                        {
                            colorcube = colorDialog.Color;
                        }
                        Vertex[] figure_vertex = ObjectCreate.CreatePlane(float.Parse(textBoxSide.Text), float.Parse(textBoxShift_lr.Text), float.Parse(textBoxShift_y.Text), float.Parse(textBoxShift_ud.Text), (int)textBoxAngleX.Value, (int)textBoxAngleY.Value, (int)textBoxAngleZ.Value);
                        _renderObjects.Add(new RenderObject(figure_vertex, colorcube, RandomColor()));
                    }
                }
            }
        }
        public class DataCoordElem
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
        }
        public class DataFinitElem
        {
            public int first { get; set; }
            public int second { get; set; }
            public int third { get; set; }
        }

        private void buttonNewFigureFile_Click(object sender, EventArgs e)
        {
            List<DataCoordElem> list_coord = new List<DataCoordElem>();
            List<DataFinitElem> list_finit_elem = new List<DataFinitElem>();
            Form dlgNewFigureFile = new Form()
            {
                Text = "Считать из файла",
                Width = 350,
                Height = 160,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label label_coord = new Label() { Text = "Координаты", Left = 10, Width = 60, Top = 30 };
            TextBox textBox_finit_elem = new TextBox { Text = "", Left = 100, Width = 120, Top = 60 };
            TextBox textBox_coord_file = new TextBox { Text = "", Left = 100, Width = 120, Top = 30 };
            Label label_finit_elem = new Label() { Text = "Элементы", Left = 10, Width = 60, Top = 60 };
            OpenFileDialog file_coord = new OpenFileDialog();
            file_coord.InitialDirectory = Application.ExecutablePath;
            file_coord.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            OpenFileDialog file_finit_elem = new OpenFileDialog();
            file_finit_elem.InitialDirectory = Application.ExecutablePath;
            file_finit_elem.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            Button file_coord_open = new Button() { Text = "Открыть файл", Left = 230, Width = 100, Top = 30 };
            Color4 color_surface = Color4.White;
            ColorDialog colorDialog = new ColorDialog();
            Button button_color = new Button() { Text = "", Left = 30, Width = 60, Top = 90, BackColor = (System.Drawing.Color)color_surface };
            Button button_ok = new Button() { Text = "Ok", Left = 230, Width = 60, Top = 90, DialogResult = DialogResult.OK };
            Button file_finit_elem_open = new Button() { Text = "Открыть файл", Left = 230, Width = 100, Top = 60 };
            file_coord_open.Click += (sender1, e1) => { file_coord.ShowDialog(); };
            button_color.Click += (sender1, e1) => { if (colorDialog.ShowDialog() == DialogResult.OK) { color_surface = button_color.BackColor = colorDialog.Color; } };
            file_coord.FileOk += (sender1, e1) => { textBox_coord_file.Text = file_coord.FileName; };
            file_finit_elem.FileOk += (sender1, e1) => { textBox_finit_elem.Text = file_finit_elem.FileName; };
            file_finit_elem_open.Click += (sender1, e1) => { file_finit_elem.ShowDialog(); };
            dlgNewFigureFile.Controls.Add(button_color);
            dlgNewFigureFile.Controls.Add(label_coord);
            dlgNewFigureFile.Controls.Add(textBox_coord_file);
            dlgNewFigureFile.Controls.Add(textBox_finit_elem);
            dlgNewFigureFile.Controls.Add(label_finit_elem);
            dlgNewFigureFile.Controls.Add(file_coord_open);
            dlgNewFigureFile.Controls.Add(file_finit_elem_open);
            dlgNewFigureFile.Controls.Add(button_ok);
            //textBox_coord_file.Text = "coord_default.txt";
            //textBox_finit_elem.Text = "finit_default.txt";
            if (dlgNewFigureFile.ShowDialog() == DialogResult.OK && textBox_coord_file.Text != "" && textBox_finit_elem.Text != "")
            {
                string file_content = string.Empty;
                StreamReader reader_coord = new StreamReader(textBox_coord_file.Text);

                bool error_flag = false;
                while ((file_content = reader_coord.ReadLine()) != null && !error_flag)
                {
                    string[] x_y_z_coord = file_content.Split(' ');
                    if (x_y_z_coord.Length == 3)
                    {
                        DataCoordElem dataCoord = new DataCoordElem();
                        dataCoord.X = double.Parse(x_y_z_coord[0]);
                        dataCoord.Y = double.Parse(x_y_z_coord[1]);
                        dataCoord.Z = double.Parse(x_y_z_coord[2]);
                        list_coord.Add(dataCoord);
                    }
                    else
                    {
                        error_flag = true;
                    }
                }
                reader_coord = new StreamReader(textBox_finit_elem.Text);
                file_content = string.Empty;

                while ((file_content = reader_coord.ReadLine()) != null && !error_flag)
                {
                    string[] finit_elem = file_content.Split(' ');
                    if (finit_elem.Length == 3)
                    {
                        DataFinitElem data_temp = new DataFinitElem();
                        data_temp.first = int.Parse(finit_elem[0]);
                        data_temp.second = int.Parse(finit_elem[1]);
                        data_temp.third = int.Parse(finit_elem[2]);
                        list_finit_elem.Add(data_temp);
                    }
                    else
                    {
                        error_flag = true;
                    }
                }
                if (!error_flag)
                {
                    Vertex[] figure_vertex = new Vertex[list_finit_elem.Count * 3];
                    Vertex[] figure_vertex_contur = new Vertex[list_finit_elem.Count * 3];
                    int col = 0;
                    foreach (DataFinitElem fin in list_finit_elem)
                    {
                        Vector4 norm_vec = new Vector4();
                        norm_vec.X = (float)(list_coord[fin.first].Y * list_coord[fin.third].Z - list_coord[fin.second].Y * list_coord[fin.third].Z - list_coord[fin.first].Y * list_coord[fin.second].Z - list_coord[fin.first].Z * list_coord[fin.third].Y + list_coord[fin.second].Z * list_coord[fin.third].Y + list_coord[fin.first].Z * list_coord[fin.second].Y);
                        norm_vec.Y = (float)(list_coord[fin.first].Z * list_coord[fin.third].X - list_coord[fin.second].Z * list_coord[fin.third].X - list_coord[fin.first].Z * list_coord[fin.second].X - list_coord[fin.first].X * list_coord[fin.third].Z + list_coord[fin.second].X * list_coord[fin.third].Z + list_coord[fin.first].X * list_coord[fin.second].Z);
                        norm_vec.Z = (float)(list_coord[fin.first].X * list_coord[fin.third].Y - list_coord[fin.second].X * list_coord[fin.third].Y - list_coord[fin.first].X * list_coord[fin.second].Y - list_coord[fin.first].Y * list_coord[fin.third].X + list_coord[fin.second].Y * list_coord[fin.third].X + list_coord[fin.first].Y * list_coord[fin.second].X);
                        norm_vec.W = 0.0f;
                        float len = Math.Abs(norm_vec.X) + Math.Abs(norm_vec.Y) + Math.Abs(norm_vec.Z);
                        norm_vec.X = norm_vec.X / len;
                        norm_vec.Y = norm_vec.Y / len;
                        norm_vec.Z = norm_vec.Z / len;
                        figure_vertex[col] = new Vertex(new Vector4((float)list_coord[fin.first].X, (float)list_coord[fin.first].Y, (float)list_coord[fin.first].Z, 1.0f), new Vector4(norm_vec.X, norm_vec.Y, norm_vec.Z, 0.0f), new Vector2(0, 0));
                        figure_vertex[col + 1] = new Vertex(new Vector4((float)list_coord[fin.second].X, (float)list_coord[fin.second].Y, (float)list_coord[fin.second].Z, 1.0f), new Vector4(norm_vec.X, norm_vec.Y, norm_vec.Z, 0.0f), new Vector2(0, 0));
                        figure_vertex[col + 2] = new Vertex(new Vector4((float)list_coord[fin.third].X, (float)list_coord[fin.third].Y, (float)list_coord[fin.third].Z, 1.0f), new Vector4(norm_vec.X, norm_vec.Y, norm_vec.Z, 0.0f), new Vector2(0, 0));
                        figure_vertex_contur[col] = new Vertex(new Vector4((float)list_coord[fin.first].X, (float)list_coord[fin.first].Y, (float)list_coord[fin.first].Z, 1.0f), new Vector4(norm_vec.X, norm_vec.Y, norm_vec.Z, 0.0f), new Vector2(0, 0));
                        figure_vertex_contur[col + 1] = new Vertex(new Vector4((float)list_coord[fin.second].X, (float)list_coord[fin.second].Y, (float)list_coord[fin.second].Z, 1.0f), new Vector4(norm_vec.X, norm_vec.Y, norm_vec.Z, 0.0f), new Vector2(0, 0));
                        figure_vertex_contur[col + 2] = new Vertex(new Vector4((float)list_coord[fin.third].X, (float)list_coord[fin.third].Y, (float)list_coord[fin.third].Z, 1.0f), new Vector4(norm_vec.X, norm_vec.Y, norm_vec.Z, 0.0f), new Vector2(0, 0));
                        col += 3;
                    }
                    _renderObjects.Add(new RenderObject(figure_vertex, color_surface, RandomColor()));
                }
                else
                {
                    MessageBox.Show("Ошибка во формате входных данных", "Ошибка");
                }
            }
            else
            {
                MessageBox.Show("Не были выбраны файлы", "Ошибка");
            }
        }

        private void buttonChangeFigure_Click(object sender, EventArgs e)
        {
            if (_SelectID > 0)
            {
                Vertex[] vertexObject = new Vertex[_renderObjects[_SelectID].BufferSize()];
                _renderObjects[_SelectID].ReadBuffer(vertexObject);
                Form dlgChangeFigure = new Form()
                {
                    Text = "Изменение объекта",
                    Width = 680,
                    Height = 500,
                    FormBorderStyle = FormBorderStyle.Sizable,
                    StartPosition = FormStartPosition.CenterScreen,
                };
                Label lblTrackBar = new Label() { Text = "Прозрачность объекта", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 170, Height = 30, Top = 415, Left = 20 };
                TrackBar trackBar = new TrackBar() { Value = (int)(_renderObjects[_SelectID].Color_obj.W * 10f), Minimum = 0, Maximum = 10, Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 170, Height = 30, Top = 415, Left = 150 };
                CheckBox checkBox = new CheckBox() { Checked = false, Text = "Изменить структуру фигуры", Width = 170, Height = 30, Top = 375, Left = 20, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                TextBox textBoxChangeCoord = new TextBox() { Enabled = false, Multiline = true, Width = 250, Height = 350, Top = 10, Left = 10, Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom, ScrollBars = ScrollBars.Vertical };
                TextBox textBoxChangeFinit = new TextBox() { Enabled = false, Multiline = true, Width = 250, Height = 350, Top = 10, Left = 10, Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom, ScrollBars = ScrollBars.Vertical };
                SplitContainer splitterText = new SplitContainer() { Width = 540, Height = 400, Left = 10, Top = 10, Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom, SplitterDistance = 260 };
                Button buttonSave = new Button() { Text = "Save", Top = 20, Left = 550, Width = 100, Height = 25, Anchor = AnchorStyles.Top | AnchorStyles.Right };
                SaveFileDialog saveFileCoord = new SaveFileDialog();
                SaveFileDialog saveFileFinit = new SaveFileDialog();
                checkBox.CheckedChanged += (sender1, e1) => { textBoxChangeCoord.Enabled = textBoxChangeFinit.Enabled = checkBox.Checked; };
                buttonSave.Click += (sender1, e1) => { saveFileCoord.ShowDialog(); saveFileFinit.ShowDialog(); };
                saveFileFinit.Filter = saveFileCoord.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileCoord.FileName = "coord_default.txt";
                saveFileFinit.FileName = "finit_default.txt";
                Button button_ok = new Button() { Text = "Ok", Top = 50, Left = 550, Width = 100, Height = 25, Anchor = AnchorStyles.Top | AnchorStyles.Right, DialogResult = DialogResult.OK };
                int i = vertexObject.Length;
                int colFinElement = i / 3;
                Triangls[] triangls_select = new Triangls[colFinElement];
                PointUnik[] pointUnik = new PointUnik[i];
                int colUnik = 0;
                for (int iter = 0; iter < vertexObject.Length; iter++)
                {
                    bool flag = true;
                    for (int jter = 0; jter < colUnik; jter++)
                    {
                        if (vertexObject[iter]._Position == pointUnik[jter].point)
                        {
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        pointUnik[colUnik].point = vertexObject[iter]._Position;
                        colUnik++;
                    }
                }
                for (int iter = 0; iter < vertexObject.Length; iter += 3)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        bool flag = true;
                        for (int jter = 0; jter < colUnik && flag; jter++)
                        {
                            if (pointUnik[jter].point == vertexObject[iter + k]._Position)
                            {
                                flag = false;
                                if (k == 2)
                                {
                                    textBoxChangeFinit.Text += jter.ToString();
                                }
                                else
                                {
                                    textBoxChangeFinit.Text += jter.ToString() + " ";
                                }
                            }
                        }
                    }
                    textBoxChangeFinit.Text += "\r\n";
                }
                for (int jter = 0; jter < colUnik; jter++)
                {
                    textBoxChangeCoord.Text += pointUnik[jter].point.Xyz.ToString().Replace("(", "").Replace(")", "").Replace(";", "") + "\r\n";
                }
                splitterText.Panel1.Controls.Add(textBoxChangeCoord);
                splitterText.Panel2.Controls.Add(textBoxChangeFinit);
                dlgChangeFigure.Controls.Add(checkBox);
                dlgChangeFigure.Controls.Add(splitterText);
                dlgChangeFigure.Controls.Add(button_ok);
                dlgChangeFigure.Controls.Add(buttonSave); 
                dlgChangeFigure.Controls.Add(trackBar);
                dlgChangeFigure.Controls.Add(lblTrackBar);
                saveFileCoord.FileOk += (sender1, e1) =>
                {
                    if (saveFileCoord.FileName.Length != 0)
                    {
                        StreamWriter write_coord = new StreamWriter(saveFileCoord.FileName);
                        write_coord.Write(textBoxChangeCoord.Text);
                        write_coord.Close();
                    }
                };
                saveFileFinit.FileOk += (sender1, e1) =>
                {
                    if (saveFileFinit.FileName.Length != 0)
                    {
                        StreamWriter write_finit = new StreamWriter(saveFileFinit.FileName);
                        write_finit.Write(textBoxChangeFinit.Text);
                        write_finit.Close();
                    }
                };
                if (dlgChangeFigure.ShowDialog() == DialogResult.OK)
                {
                    if (checkBox.Checked)
                    {
                        List<DataCoordElem> list_coord = new List<DataCoordElem>();
                        List<DataFinitElem> list_finit_elem = new List<DataFinitElem>();
                        string[] string_line = textBoxChangeCoord.Text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                        bool error_flag = false;
                        for (int iter = 0; iter < string_line.Length && !error_flag; iter++)
                        {
                            string[] x_y_z_coord = string_line[iter].Split(' ');
                            if (x_y_z_coord.Length == 3)
                            {
                                DataCoordElem dataCoord = new DataCoordElem();
                                dataCoord.X = double.Parse(x_y_z_coord[0]);
                                dataCoord.Y = double.Parse(x_y_z_coord[1]);
                                dataCoord.Z = double.Parse(x_y_z_coord[2]);
                                list_coord.Add(dataCoord);
                            }
                            else
                            {
                                error_flag = true;
                            }
                        }
                        string_line = textBoxChangeFinit.Text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int iter = 0; iter < string_line.Length && !error_flag; iter++)
                        {
                            string[] finit_elem = string_line[iter].Split(' ');
                            if (finit_elem.Length == 3)
                            {
                                DataFinitElem data_temp = new DataFinitElem();
                                data_temp.first = int.Parse(finit_elem[0]);
                                data_temp.second = int.Parse(finit_elem[1]);
                                data_temp.third = int.Parse(finit_elem[2]);
                                list_finit_elem.Add(data_temp);
                            }
                            else
                            {
                                error_flag = true;
                            }
                        }
                        if (!error_flag)
                        {
                            Vertex[] figure_vertex = new Vertex[list_finit_elem.Count * 3];
                            int col = 0;
                            foreach (DataFinitElem fin in list_finit_elem)
                            {
                                Vector4 norm_vec = new Vector4();
                                norm_vec.X = (float)(list_coord[fin.first].Y * list_coord[fin.third].Z - list_coord[fin.second].Y * list_coord[fin.third].Z - list_coord[fin.first].Y * list_coord[fin.second].Z - list_coord[fin.first].Z * list_coord[fin.third].Y + list_coord[fin.second].Z * list_coord[fin.third].Y + list_coord[fin.first].Z * list_coord[fin.second].Y);
                                norm_vec.Y = (float)(list_coord[fin.first].Z * list_coord[fin.third].X - list_coord[fin.second].Z * list_coord[fin.third].X - list_coord[fin.first].Z * list_coord[fin.second].X - list_coord[fin.first].X * list_coord[fin.third].Z + list_coord[fin.second].X * list_coord[fin.third].Z + list_coord[fin.first].X * list_coord[fin.second].Z);
                                norm_vec.Z = (float)(list_coord[fin.first].X * list_coord[fin.third].Y - list_coord[fin.second].X * list_coord[fin.third].Y - list_coord[fin.first].X * list_coord[fin.second].Y - list_coord[fin.first].Y * list_coord[fin.third].X + list_coord[fin.second].Y * list_coord[fin.third].X + list_coord[fin.first].Y * list_coord[fin.second].X);
                                norm_vec.W = 0.0f;
                                float len = Math.Abs(norm_vec.X) + Math.Abs(norm_vec.Y) + Math.Abs(norm_vec.Z);
                                norm_vec.X = norm_vec.X / len;
                                norm_vec.Y = norm_vec.Y / len;
                                norm_vec.Z = norm_vec.Z / len;
                                figure_vertex[col] = new Vertex(new Vector4((float)list_coord[fin.first].X, (float)list_coord[fin.first].Y, (float)list_coord[fin.first].Z, 1.0f), new Vector4(norm_vec.X, norm_vec.Y, norm_vec.Z, 0.0f), new Vector2(0, 0));
                                figure_vertex[col + 1] = new Vertex(new Vector4((float)list_coord[fin.second].X, (float)list_coord[fin.second].Y, (float)list_coord[fin.second].Z, 1.0f), new Vector4(norm_vec.X, norm_vec.Y, norm_vec.Z, 0.0f), new Vector2(0, 0));
                                figure_vertex[col + 2] = new Vertex(new Vector4((float)list_coord[fin.third].X, (float)list_coord[fin.third].Y, (float)list_coord[fin.third].Z, 1.0f), new Vector4(norm_vec.X, norm_vec.Y, norm_vec.Z, 0.0f), new Vector2(0, 0));
                                col += 3;
                            }
                            _renderObjects[_SelectID].WriteBuffer(figure_vertex);
                        }
                        else
                        {
                            MessageBox.Show("Ошибка во формате входных данных", "Ошибка");
                        }
                    }
                    _renderObjects[_SelectID].Color_obj.W = trackBar.Value / 10f;
                }
            }
            else
            {
                MessageBox.Show("Этот объект нельзя изменить");
            }
        }

        private void buttonRemoveFigure_Click(object sender, EventArgs e)
        {
            if (_SelectID > 0)
            {
                int buffer = _renderObjects[_SelectID].ShadowProjectBuffer();
                if (buffer > -1) GL.DeleteBuffer(buffer);
                buffer = _renderObjects[_SelectID].RenderBuffer();
                if (buffer > -1) GL.DeleteBuffer(buffer);
                var lightObj = _lightObjects.Where(x => x.Color_choice == _renderObjects[_SelectID].Color_choice).FirstOrDefault();
                buffer = -1;
                if (lightObj != null) buffer = lightObj.uboLightInfo;
                if (buffer > -1) GL.DeleteBuffer(buffer);
                _lightObjects.Remove(_lightObjects.Where(x => x.Color_choice == _renderObjects[_SelectID].Color_choice).FirstOrDefault());
                _renderObjects.Remove(_renderObjects[_SelectID]);
                _SelectID = -1;
            }
            else
            {
                MessageBox.Show("Этот объект нельзя удалить");
            }
        }
    }
}
