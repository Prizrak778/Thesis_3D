using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Thesis_3D
{
    public partial class Thesis3DForm : Form
    {
        Camera cameraFirstFace = new Camera();
        private Matrix4 _projectionMatrix;
        private Matrix4 _ViewMatrix;
        private Matrix4 _MVP; //Modal * View * Matrix

        private int _program = -1;
        private int _program_contour = -1;
        private int _program_some_light = -1;
        private int _program_Lgh_directed_solar_effect = -1;
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
        private RenderObject primaryRenderObject;
        private RenderObject primarySphereAt;
        private LightObject primaryLightObject;
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
                400f);
            _ViewMatrix = cameraFirstFace.GetViewMatrix();
            _MVP = _ViewMatrix * _projectionMatrix;
        }
        #endregion

        private void ChoiseShader(int Index)
        {
            _program = listProgram[Index];
        }

        public Thesis3DForm()
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
            VertexShader = @"Components\Shaders\vertexShader_Lgh_directed_solar_effect.vert";
            FragentShader = @"Components\Shaders\fragmentShader_Lgh_directed_solar_effect.frag";
            if ((_program = CompileShaders(VertexShader, FragentShader)) == -1)
            {
                error = "Ошибка при компиляции шейдера плоских теней";
                return false;
            }
            _program_Lgh_directed_solar_effect = _program;
            return true;
        }

        protected override void OnLoad(EventArgs e)
        {
            FileStream filteTest = File.Create("FPSData.txt");
            filteTest.Close();
            glControlThesis3D.Load += new EventHandler(glControl_Load);
            glControl_Load(glControlThesis3D, EventArgs.Empty);
            Application.Idle += Application_Idle;
            string ErrorText = string.Empty;
            if(!CompileAllShaders(out ErrorText))
            {
                throw new Exception(ErrorText);
            }
            comboBoxShaders.Items.AddRange(new object[] { "Обычные цвета", "Т.И. без отражения", "Т.И. с отражением", "Т.И. с двойным отражением", "Т.И. с плоским затенением", "Несколько Т.И.", "Направленный источник", "Затенение по Фонгу", "Затенение по Фонгу с использованием вектора полпути", "Узконаправленный источник", "Туман", "Плоское затенение для одного элемента" });
            comboBoxShaders.SelectedIndex = 0;
            Vector3 positionObject = new Vector3(-1.0f, 1.0f, 0.0f);
            primarySphereAt = new RenderObject(ObjectCreate.CreateSphere(40f, positionObject, 60, 60, 1, 1), positionObject, Color4.DeepSkyBlue, RandomColor(), locSide: 40f, locTypeObjectCreate: TypeObjectCreate.Sphere, locColBreakX: 60, locColBreakY: 60, locCoeffSX: 1, locCoeffSY: 1);
            _renderObjects.Add(new RenderObject(ObjectCreate.CreatePlane(1.5f, positionObject, 0, 0, 45), positionObject, Color4.LightCyan, RandomColor(), plane: true, locSide: 1.5f, locTypeObjectCreate: TypeObjectCreate.Plane, locAngleZ: 45));
            primaryRenderObject = _renderObjects[0];
            positionObject = new Vector3(0.0f, 0.0f, 0.0f);
            _renderObjects.Add(new RenderObject(ObjectCreate.CreatePlane(15f, positionObject, 0, 0, 0), positionObject, Color4.Green, RandomColor(), plane: true, locSide: 40f, locTypeObjectCreate: TypeObjectCreate.Plane));
            positionObject = new Vector3(0.0f, 2.0f, 0.0f);
            _renderObjects.Add(new RenderObject(ObjectCreate.CreateSolidCube(0.5f, positionObject), positionObject, Color4.LightCoral, RandomColor(), locSide: 0.5f, locTypeObjectCreate: TypeObjectCreate.SolidCube));
            for (int i = 0; i < 10; i++)
            {
                positionObject = new Vector3((float)i + 1, 2.0f, 0.0f);
                _renderObjects.Add(new RenderObject(ObjectCreate.CreateSolidCube(0.5f, positionObject), positionObject, Color4.LightCoral, RandomColor(), locSide: 0.5f, locTypeObjectCreate: TypeObjectCreate.SolidCube));
            }
            for (int i = 0; i < 10; i++)
            {
                positionObject = new Vector3(1, -(float)i + 2.0f, 0.0f);
                _renderObjects.Add(new RenderObject(ObjectCreate.CreateSolidCube(0.5f, positionObject), positionObject, Color4.LightCoral, RandomColor(), locSide: 0.5f, locTypeObjectCreate: TypeObjectCreate.SolidCube));
            }
            positionObject = new Vector3(1, 4.0f, 0.0f);
            _renderObjects.Add(new RenderObject(ObjectCreate.CreateSphere(1.5f, positionObject, 60, 60, 1, 1), positionObject, Color4.Brown, RandomColor(), locSide: 1.5f, locTypeObjectCreate: TypeObjectCreate.Sphere, locColBreakX: 60, locColBreakY: 60, locCoeffSX: 1, locCoeffSY: 1));
            Vector3 positionLight = new Vector3(1.0f, 3.0f, 1.0f);
            _lightObjects.Add(new LightObject(ObjectCreate.CreateSphere(5.0f, positionObject, 10, 10, 1, 1), Color4.Yellow, RandomColor(), positionLight, new Vector4(5.0f, 5.0f, 1.0f, 1.0f), new Vector3(-0.2f, -1f, -0.3f), new Vector3(0.3f, 0.3f, 0.0f), new Vector3(1.0f, 0.0f, 5f), _program_Fong_directed, side: 0.5f, locTypeObjectCreate: TypeObjectCreate.SolidCube, locColBreakX: 10, locColBreakY: 10, locCoeffSX: 1, locCoeffSY: 1));
            primaryLightObject = _lightObjects[0];
            primaryLightObject.trajctoryRenderObject = new TrajctoryRenderObject(
                new TrajectoryFunctions(300, (double x) => (Math.Cos(x)), 0.03f, -180, 180, 0, "cos(x)", true),
                new TrajectoryFunctions(300, (double x) => (Math.Sin(x)), 0.03f, -180, 180, 0, "sin(y)", true),
                new TrajectoryFunctions(100, (double x) => (x), 0.001f, -1, 1, 0, "z", false),
                TargetTrajectory.Point,
                new Vector4(0, 0, 0, 1f)
                );
            primaryLightObject.Ambient = new Vector3(0.0f, 0.15f, 0.0f);
            positionLight = new Vector3(4.0f, 3.0f, 1.0f);                                                                                                                                                                                     
            _lightObjects.Add(new LightObject(ObjectCreate.CreateSolidCube(0.1f, positionLight), Color4.Yellow, RandomColor(), positionLight, new Vector4(5.0f, 5.0f, 1.0f, 1.0f), new Vector3(-0.2f, -1f, -0.3f), new Vector3(0.0f, 0.3f, 0.3f), new Vector3(1.0f, 0.0f, 5f), side: 0.1f, locTypeObjectCreate: TypeObjectCreate.SolidCube));
            positionLight = new Vector3(7.0f, 3.0f, 1.0f);                                                                                                                                                                                     
            _lightObjects.Add(new LightObject(ObjectCreate.CreateSolidCube(0.1f, positionLight), Color4.Yellow, RandomColor(), positionLight, new Vector4(5.0f, 5.0f, 1.0f, 1.0f), new Vector3(-0.2f, -1f, -0.3f), new Vector3(0.3f, 0.0f, 0.3f), new Vector3(1.0f, 0.0f, 5f), side: 0.1f, locTypeObjectCreate: TypeObjectCreate.SolidCube));
            /*for (int i = 0; i < 147; i++)
            {
                positionLight = new Vector3(10.0f + 3*i, 3.0f, 1.0f);
                _lightObjects.Add(new LightObject(ObjectCreate.CreateSolidCube(0.1f, positionLight), Color4.Yellow, RandomColor(), positionLight, new Vector4(5.0f, 5.0f, 1.0f, 1.0f), new Vector3(-0.2f, -1f, -0.3f), new Vector3(0.3f, 0.0f, 0.3f), new Vector3(1.0f, 0.0f, 5f), side: 0.1f, locTypeObjectCreate: TypeObjectCreate.SolidCube));
            }*/
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
            while (glControlThesis3D.IsIdle)
            {
                Render();
            }
        }
        private void glControl_Load(object sender, EventArgs e)
        {
            CreateProjection();
            buttonChangeFigure.Enabled = false;
            buttonRemoveFigure.Enabled = false;
            glControlThesis3D.Resize += new EventHandler(glControl_Resize);
            glControlThesis3D.Paint += new PaintEventHandler(glControl_Paint);
            glControlThesis3D.MouseMove += new MouseEventHandler(glControl_MouseMove);
            glControlThesis3D.MouseDown += new MouseEventHandler(glControl_MouseDown);
            glControlThesis3D.KeyDown += new KeyEventHandler(glControl_KeyPressDown);
            glControlThesis3D.MouseWheel += new MouseEventHandler(glControl_MouseWheel);
            glControlThesis3D.MakeCurrent();
            cameraFirstFace.Position = new Vector3(0, 2.5f, 2);
            cameraFirstFace.Orientation = new Vector3(-(float)Math.PI, -(float)Math.PI, 0);
            cameraFirstFace.AddRotation(0, 0);
            glControl_Resize(glControlThesis3D, EventArgs.Empty);
            GL.Enable(EnableCap.AlphaTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.DstAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);
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
            Text += $" (Vsync: {glControlThesis3D.VSync})";
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
            if (cameraFirstFace.Rotation_status)
            {
                cameraFirstFace.AddRotation(delta.X, delta.Y);
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
                RenderSelectColorBuf();
                GL.ReadPixels(e.X, glControlThesis3D.Height - e.Y, 1, 1, PixelFormat.Bgra, PixelType.UnsignedByte, ref pixel);

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
                buttonRemoveFigure.Enabled = true;
                buttonTrajectory.Enabled = true;
                labelId.Visible = true;
                labelIdText.Visible = true;
                labelIdText.Text = Convert.ToString(_renderObjects[_SelectID].ColorСhoice.Xyz).Replace("(", "").Replace(")", "").Replace(";", "").Replace(" ", "") + " №" + Convert.ToString(_SelectID);
            }
            else
            {
                buttonChangeFigure.Enabled = false;
                buttonRemoveFigure.Enabled = false;
                buttonTrajectory.Enabled = false;
                labelId.Visible = false; ;
                labelIdText.Visible = false; ;
                labelIdText.Text = "-";
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
                    cameraFirstFace.AddRotation(10f, 0f);
                    break;
                case Keys.L:
                    cameraFirstFace.AddRotation(-10f, 0f);
                    break;
                case Keys.I:
                    cameraFirstFace.AddRotation(0f, 10f);
                    break;
                case Keys.K:
                    cameraFirstFace.AddRotation(0f, -10f);
                    break;
                case Keys.W:
                    cameraFirstFace.Move(0f, 0.05f, 0f);
                    break;
                case Keys.S:
                    cameraFirstFace.Move(0f, -0.05f, 0f);
                    break;
                case Keys.A:
                    cameraFirstFace.Move(-0.05f, 0f, 0f);
                    break;
                case Keys.D:
                    cameraFirstFace.Move(0.05f, 0f, 0f);
                    break;
                case Keys.Q://Вверх по y
                    cameraFirstFace.Move(0f, 0f, 0.05f);
                    break;
                case Keys.E://Вниз по y
                    cameraFirstFace.Move(0f, 0f, -0.05f);
                    break;
                case Keys.C:
                    cameraFirstFace.Rotation_change_status();
                    checkBox2.Checked = cameraFirstFace.Rotation_status;
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
                if(_renderObjects[_SelectID].TypeObject == TypeObjectRenderLight.LightSourceObject)
                {
                    foreach(var lightObject in _lightObjects)
                    {
                        if(lightObject.ColorСhoice == _renderObjects[_SelectID].ColorСhoice)
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

        
        private void RenderSelectColorBuf()
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
                RenderFigure(renderObject, PolygonMode.Fill);
                GL.Uniform4(19, ref temp_color);
                renderObject.Render();
                iter++;
            }
        }
        private RenderObject RenderObjectFind(Vector4 colorChoice)
        {
            return _renderObjects.Where(x => x.ColorСhoice == colorChoice).FirstOrDefault();
        }

        private void RenderFigure(RenderObject renderObject, PolygonMode polygon)
        {
            renderObject.Bind();
            GL.UniformMatrix4(20, false, ref _MVP);
            //GL.UniformMatrix4(21, false, ref _projectionMatrix);
            GL.UniformMatrix4(22, false, ref renderObject.ModelMatrix);
            renderObject.PolygonMode_now(polygon);
        }
        private void Render()
        {
            var startTime = System.Diagnostics.Stopwatch.StartNew();
            GL.ClearColor(new Color4(0.3f, 0.3f, 0.3f, 1.0f));
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            CreateProjection();


            GL.UseProgram(_program);
            cameraFirstFace.SetPositionCamerShader(23);
            var countLightObj = _lightObjects.Count;
            var countRenderObj = _renderObjects.Count;
            foreach (var renderObject in _renderObjects)
            {
                if (_program == _program_shadow_project)
                {
                    if (countLightObj > 0 && countRenderObj > 0 && primaryLightObject.ColorСhoice != renderObject.ColorСhoice && primaryRenderObject.ColorСhoice != renderObject.ColorСhoice)
                    {
                        GL.UseProgram(_program_shadow_project);
                        GL.UniformMatrix4(23, false, ref primaryRenderObject.ModelMatrix);
                        GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 5, primaryRenderObject.ShadowProjectBuffer());
                        GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
                        RenderFigure(renderObject, PolygonMode.Fill);
                        primaryLightObject.PositionLightUniform(18);
                        renderObject.Render();
                        
                    }
                    GL.UseProgram(_program_Fong);
                }
                RenderFigure(renderObject, PolygonMode.Fill);
                Vector4 color = renderObject.ColorObj;
                GL.Uniform4(19, ref color);
                if (_program == _program_some_light)
                {
                    foreach (var light in _lightObjects.Cast<LightObject>().Select((r, i) => new { Row = r, Index = i }))
                    {
                        light.Row.PositionLightUniform(175 + light.Index);
                        light.Row.IntensityLightVectorUniform(24 + light.Index);
                    }
                }
                else if(_program == _program_Fong_fog && countLightObj > 0)
                {
                    primaryLightObject.PositionLightUniform(18);
                    primaryLightObject.SetAttrFog(25, 1f, 24, 9f, 26, new Vector3(0.3f, 0.3f, 0.3f));
                }
                else if(_program == _program_Fong_directed && countLightObj > 0)
                {
                    if(primaryLightObject.uboLightInfo != -1) GL.BindBufferRange(BufferRangeTarget.UniformBuffer, 24, primaryLightObject.uboLightInfo, (IntPtr)0, primaryLightObject.blockSizeLightInfo);
                }
                else
                {
                    if (countLightObj > 0)
                    {
                        primaryLightObject.PositionLightUniform(18);
                        primaryLightObject.IntensityLightVectorUniform(24);
                        primaryLightObject.IntensityAmbient(26);
                        renderObject.ambientUnifrom(27);
                        renderObject.diffusionUnifrom(25);
                    }
                }
                if (renderObject.trajctoryRenderObject.useTrajectory)
                {
                    renderObject.ModelMatrix.ClearTranslation();
                    Vector3 pointTarget = Vector3.Zero;
                    if (renderObject.trajctoryRenderObject.target == TargetTrajectory.Object)
                    {
                        RenderObject renderObjectTarget = RenderObjectFind(renderObject.trajctoryRenderObject.GetObject());
                        pointTarget = renderObjectTarget != null ? renderObjectTarget.getPositionRenderObject().Xyz : Vector3.Zero;
                    }
                    else pointTarget = renderObject.trajctoryRenderObject.GetPoint().Xyz;
                    Vector3 translation = -(renderObject.getStartPosition()) + renderObject.trajctoryRenderObject.getValue() + pointTarget;
                    renderObject.ModelMatrix = Matrix4.CreateTranslation(translation);
                    if (renderObject.TypeObject == TypeObjectRenderLight.LightSourceObject)
                    {
                        foreach (var lightObject in _lightObjects)
                        {
                            if (lightObject.ColorСhoice == renderObject.ColorСhoice)
                            {
                                lightObject.SetPositionLight(renderObject.ModelMatrix);
                                if (_program_Fong_directed != -1 && lightObject.uboLightInfo != -1) lightObject.UpdatePositionForBlock(_program_Fong_directed);
                                break; // -_-
                            }
                        }
                    }

                }
                renderObject.Render();
            }
            GL.UseProgram(_program_contour);
            if (_contour)
            {
                GL.LineWidth(4);
                foreach (var renderObject in _renderObjects)
                {
                    
                    RenderFigure(renderObject, PolygonMode.Line);
                    Vector4 color = new Vector4(0, 0, 0, 255);
                    GL.Uniform4(19, ref color);

                    renderObject.Render_line();
                }
            }
            if(_SelectID > -1)
            {
                GL.LineWidth(7);
                RenderFigure(_renderObjects[_SelectID], PolygonMode.Line);
                Vector4 color = new Vector4(0, 0, 0, 255);
                GL.Uniform4(19, ref color);
                _renderObjects[_SelectID].Render_line();
            }

            GL.UseProgram(_program_Lgh_directed_solar_effect);
            if (true)//Временно
            {
                RenderFigure(primarySphereAt, PolygonMode.Fill);
                Vector4 color = primarySphereAt.ColorObj;
                GL.Uniform4(19, ref color);
                if (countLightObj > 0)
                {
                    primaryLightObject.PositionLightUniform(18);
                    primaryLightObject.IntensityLightVectorUniform(24);
                    primarySphereAt.diffusionUnifrom(25);
                }
                primarySphereAt.Render();
            }
            glControlThesis3D.SwapBuffers();
            startTime.Stop();
            var resultTime = startTime.Elapsed;
            _framecount = 1f / (resultTime.Ticks / 10000000f);
            if(checkBoxFps.Checked) File.AppendAllText("FPSData.txt", $"FPS:\t{_framecount:0}" + Environment.NewLine);
            label5.Text = $"Position:{cameraFirstFace.Position:0}";
            label6.Text = $"FPS: {_framecount:0}";

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _contour = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            cameraFirstFace.Rotation_status = checkBox2.Checked;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChoiseShader(comboBoxShaders.SelectedIndex);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cameraFirstFace.Move(0, 0, 1.5f);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cameraFirstFace.Move(0, 1.5f, 0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cameraFirstFace.Move(0, 0, -1.5f);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            cameraFirstFace.Move(-1.5f, 0.0f, 0);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            cameraFirstFace.Move(0, -1.5f, 0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cameraFirstFace.Move(1.5f, 0, 0);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            cameraFirstFace.AddRotation(0.0f, 10.0f);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            cameraFirstFace.AddRotation(10.0f, 0.0f);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            cameraFirstFace.AddRotation(0.0f, -10.0f);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            cameraFirstFace.AddRotation(-10.0f, 0.0f);
        }

        private void textBox1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            char symbolT = e.KeyChar;

            if (!char.IsDigit(symbolT) && symbolT != '-' && symbolT != '.' && symbolT != '\b')
            {
                e.Handled = true;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox3.Text))
                cameraFirstFace.Position = new Vector3((float)Convert.ToDouble(textBox1.Text), (float)Convert.ToDouble(textBox2.Text), (float)Convert.ToDouble(textBox3.Text));
        }

        private void buttonNewAnFigure_Click(object sender, EventArgs e)
        {
            TypeObjectCreate typeObjectCreate = TypeObjectCreate.SolidCube;
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
            Button buttonOk = new Button() { Text = "Ok", Left = 100, Width = 80, Top = 60, DialogResult = DialogResult.OK };
            Button buttonClose = new Button() { Text = "Закрыть", Left = 192, Width = 80, Top = 60, DialogResult = DialogResult.Cancel };
            ComboBox comboBoxTypeFigure = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Text = "Куб", Left = 110, Width = 160, Top = 27 };
            dlgNewAnFigure.Controls.Add(comboBoxTypeFigure);
            comboBoxTypeFigure.Items.AddRange(new object[] {
            "Плоскость",
            "Куб",
            "Сфера"});
            comboBoxTypeFigure.SelectedIndex = 0;
            buttonOk.Click += (senderOk, eOk) => { dlgNewAnFigure.Close(); };
            buttonClose.Click += (senderClose, eClose) => { dlgNewAnFigure.Close(); };
            dlgNewAnFigure.Controls.Add(lblTypeFigure);
            dlgNewAnFigure.Controls.Add(buttonOk);
            dlgNewAnFigure.Controls.Add(buttonClose);
            dlgNewAnFigure.AcceptButton = buttonOk;
            dlgNewAnFigure.CancelButton = buttonClose;
            if (dlgNewAnFigure.ShowDialog() == DialogResult.OK)
            {
                if (comboBoxTypeFigure.Text == "Куб")
                {
                    typeObjectCreate = TypeObjectCreate.SolidCube;
                }
                else if (comboBoxTypeFigure.Text == "Сфера")
                {
                    typeObjectCreate = TypeObjectCreate.Sphere;
                }
                else if (comboBoxTypeFigure.Text == "Плоскость")
                {
                    typeObjectCreate = TypeObjectCreate.Plane;
                }
                DlgAddEditAnFigure dlgNewAn = new DlgAddEditAnFigure(typeObjectCreate);
                if (dlgNewAn.ShowDialog() == DialogResult.OK)
                {
                    Color4 colorcube = dlgNewAn.colorObject;
                    Vector3 position = dlgNewAn.position;
                    Vertex[] figure_vertex = dlgNewAn.figureVertex;
                    _renderObjects.Add(new RenderObject(figure_vertex, position, colorcube, RandomColor(), locTypeObjectCreate: typeObjectCreate, locSide: dlgNewAn.side, locColBreakX: dlgNewAn.colBreakX, locColBreakY: dlgNewAn.colBreakY, locCoeffSX: dlgNewAn.coeffSX, locCoeffSY: dlgNewAn.coeffSY, locAngleX: dlgNewAn.angleX, locAngleY: dlgNewAn.angleY, locAngleZ: dlgNewAn.angleZ));
                }
            }
        }
        //Это всё нужно вынести в отельный модуль
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
            List<DataCoordElem> listCoord = new List<DataCoordElem>();
            List<DataFinitElem> listFinitElem = new List<DataFinitElem>();
            Form dlgNewFigureFile = new Form()
            {
                Text = "Считать из файла",
                Width = 350,
                Height = 160,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label lblCoord = new Label() { Text = "Координаты", Left = 10, Width = 60, Top = 30 };
            TextBox textBoxFinitElem = new TextBox { Text = "", Left = 100, Width = 120, Top = 60 };
            TextBox textBoxCoordFile = new TextBox { Text = "", Left = 100, Width = 120, Top = 30 };
            Label lblFinitElem = new Label() { Text = "Элементы", Left = 10, Width = 60, Top = 60 };
            OpenFileDialog fileDialogCoord = new OpenFileDialog
            {
                InitialDirectory = Application.ExecutablePath,
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
            };
            OpenFileDialog fileDialogFinitElem = new OpenFileDialog
            {
                InitialDirectory = Application.ExecutablePath,
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
            };
            Button buttonFileCoordOpen = new Button() { Text = "Открыть файл", Left = 230, Width = 100, Top = 30 };
            Color4 colorSurface = Color4.White;
            ColorDialog colorDialog = new ColorDialog();
            Button buttonColor = new Button() { Text = "", Left = 30, Width = 60, Top = 90, BackColor = (System.Drawing.Color)colorSurface };
            Button buttonOk = new Button() { Text = "Ok", Left = 230, Width = 60, Top = 90, DialogResult = DialogResult.OK };
            Button fileFinitElemOpen = new Button() { Text = "Открыть файл", Left = 230, Width = 100, Top = 60 };
            buttonFileCoordOpen.Click += (sender1, e1) => { fileDialogCoord.ShowDialog(); };
            buttonColor.Click += (sender1, e1) => { if (colorDialog.ShowDialog() == DialogResult.OK) { colorSurface = buttonColor.BackColor = colorDialog.Color; } };
            fileDialogCoord.FileOk += (sender1, e1) => { textBoxCoordFile.Text = fileDialogCoord.FileName; };
            fileDialogFinitElem.FileOk += (sender1, e1) => { textBoxFinitElem.Text = fileDialogFinitElem.FileName; };
            fileFinitElemOpen.Click += (sender1, e1) => { fileDialogFinitElem.ShowDialog(); };
            dlgNewFigureFile.Controls.Add(buttonColor);
            dlgNewFigureFile.Controls.Add(lblCoord);
            dlgNewFigureFile.Controls.Add(textBoxCoordFile);
            dlgNewFigureFile.Controls.Add(textBoxFinitElem);
            dlgNewFigureFile.Controls.Add(lblFinitElem);
            dlgNewFigureFile.Controls.Add(buttonFileCoordOpen);
            dlgNewFigureFile.Controls.Add(fileFinitElemOpen);
            dlgNewFigureFile.Controls.Add(buttonOk);
            //textBox_coord_file.Text = "coord_default.txt";
            //textBox_finit_elem.Text = "finit_default.txt";
            if (dlgNewFigureFile.ShowDialog() == DialogResult.OK && textBoxCoordFile.Text != "" && textBoxFinitElem.Text != "")
            {
                string fileContent = string.Empty;
                StreamReader readerCoord = new StreamReader(textBoxCoordFile.Text);

                bool errorFlag = false;
                while ((fileContent = readerCoord.ReadLine()) != null && !errorFlag)
                {
                    string[] strXYZCoord = fileContent.Split(' ');
                    if (strXYZCoord.Length == 3)
                    {
                        DataCoordElem dataCoord = new DataCoordElem();
                        dataCoord.X = double.Parse(strXYZCoord[0]);
                        dataCoord.Y = double.Parse(strXYZCoord[1]);
                        dataCoord.Z = double.Parse(strXYZCoord[2]);
                        listCoord.Add(dataCoord);
                    }
                    else
                    {
                        errorFlag = true;
                    }
                }
                readerCoord = new StreamReader(textBoxFinitElem.Text);
                fileContent = string.Empty;

                while ((fileContent = readerCoord.ReadLine()) != null && !errorFlag)
                {
                    string[] strFinitElem = fileContent.Split(' ');
                    if (strFinitElem.Length == 3)
                    {
                        DataFinitElem dataFinitElem = new DataFinitElem();
                        dataFinitElem.first = int.Parse(strFinitElem[0]);
                        dataFinitElem.second = int.Parse(strFinitElem[1]);
                        dataFinitElem.third = int.Parse(strFinitElem[2]);
                        listFinitElem.Add(dataFinitElem);
                    }
                    else
                    {
                        errorFlag = true;
                    }
                }
                if (!errorFlag)
                {
                    Vertex[] figureVertex = new Vertex[listFinitElem.Count * 3];
                    int col = 0;
                    foreach (DataFinitElem fin in listFinitElem)
                    {
                        Vector4 normVec = new Vector4();
                        normVec.X = (float)(listCoord[fin.first].Y * listCoord[fin.third].Z - listCoord[fin.second].Y * listCoord[fin.third].Z - listCoord[fin.first].Y * listCoord[fin.second].Z - listCoord[fin.first].Z * listCoord[fin.third].Y + listCoord[fin.second].Z * listCoord[fin.third].Y + listCoord[fin.first].Z * listCoord[fin.second].Y);
                        normVec.Y = (float)(listCoord[fin.first].Z * listCoord[fin.third].X - listCoord[fin.second].Z * listCoord[fin.third].X - listCoord[fin.first].Z * listCoord[fin.second].X - listCoord[fin.first].X * listCoord[fin.third].Z + listCoord[fin.second].X * listCoord[fin.third].Z + listCoord[fin.first].X * listCoord[fin.second].Z);
                        normVec.Z = (float)(listCoord[fin.first].X * listCoord[fin.third].Y - listCoord[fin.second].X * listCoord[fin.third].Y - listCoord[fin.first].X * listCoord[fin.second].Y - listCoord[fin.first].Y * listCoord[fin.third].X + listCoord[fin.second].Y * listCoord[fin.third].X + listCoord[fin.first].Y * listCoord[fin.second].X);
                        normVec.W = 0.0f;
                        float len = Math.Abs(normVec.X) + Math.Abs(normVec.Y) + Math.Abs(normVec.Z);
                        normVec.X = normVec.X / len;
                        normVec.Y = normVec.Y / len;
                        normVec.Z = normVec.Z / len;
                        figureVertex[col] = new Vertex(new Vector4((float)listCoord[fin.first].X, (float)listCoord[fin.first].Y, (float)listCoord[fin.first].Z, 1.0f), new Vector4(normVec.X, normVec.Y, normVec.Z, 0.0f), new Vector2(0, 0));
                        figureVertex[col + 1] = new Vertex(new Vector4((float)listCoord[fin.second].X, (float)listCoord[fin.second].Y, (float)listCoord[fin.second].Z, 1.0f), new Vector4(normVec.X, normVec.Y, normVec.Z, 0.0f), new Vector2(0, 0));
                        figureVertex[col + 2] = new Vertex(new Vector4((float)listCoord[fin.third].X, (float)listCoord[fin.third].Y, (float)listCoord[fin.third].Z, 1.0f), new Vector4(normVec.X, normVec.Y, normVec.Z, 0.0f), new Vector2(0, 0));
                        col += 3;
                    }
                    _renderObjects.Add(new RenderObject(figureVertex, figureVertex[0]._Position.Xyz, colorSurface, RandomColor(), locTypeObjectCreate: TypeObjectCreate.NonTypeObject));
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
                var typeObject = _renderObjects[_SelectID].TypeObject;
                Vector3 diff = _renderObjects[_SelectID].getDiffusion();
                if (_renderObjects[_SelectID].typeObjectCreate == TypeObjectCreate.NonTypeObject)
                {
                    Form dlgChangeFigure = new Form()
                    {
                        Text = "Изменение объекта",
                        Width = 680,
                        Height = 550,
                        FormBorderStyle = FormBorderStyle.Sizable,
                        StartPosition = FormStartPosition.CenterScreen,
                    };
                    Label lblCoords = new Label() { Text = "Смещение по осям:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 120, Height = 30, Top = 423, Left = 190 };
                    Label lblCoordX = new Label() { Text = "X:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 423, Left = 315 };
                    Label lblCoordY = new Label() { Text = "Y:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 423, Left = 375 };
                    Label lblCoordZ = new Label() { Text = "Z:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 423, Left = 435 };
                    TextBox textBoxCoordX = new TextBox() { Text = "0", Width = 40, Height = 30, Top = 420, Left = 335, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    TextBox textBoxCoordY = new TextBox() { Text = "0", Width = 40, Height = 30, Top = 420, Left = 395, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    TextBox textBoxCoordZ = new TextBox() { Text = "0", Width = 40, Height = 30, Top = 420, Left = 455, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    Label lblDiffs = new Label() { Text = typeObject == TypeObjectRenderLight.LightSourceObject ? "Интенсивность освещения" : "Коэффициенты рассеивание:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 120, Height = 30, Top = 380, Left = 190 };
                    Label lblDiffR = new Label() { Text = "R:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 382, Left = 315 };
                    Label lblDiffG = new Label() { Text = "G:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 382, Left = 375 };
                    Label lblDiffB = new Label() { Text = "B:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 382, Left = 435 };
                    TextBox textBoxDiffR = new TextBox() { Text = diff.X.ToString(), Width = 40, Height = 30, Top = 380, Left = 335, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    TextBox textBoxDiffG = new TextBox() { Text = diff.Y.ToString(), Width = 40, Height = 30, Top = 380, Left = 395, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    TextBox textBoxDiffB = new TextBox() { Text = diff.Z.ToString(), Width = 40, Height = 30, Top = 380, Left = 455, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    Color4 colorSurface = new Color4();
                    colorSurface.R = _renderObjects[_SelectID].ColorObj.X;
                    colorSurface.G = _renderObjects[_SelectID].ColorObj.Y;
                    colorSurface.B = _renderObjects[_SelectID].ColorObj.Z;
                    colorSurface.A = _renderObjects[_SelectID].ColorObj.W;
                    ColorDialog colorDialog = new ColorDialog();
                    Label lblButtonColor = new Label() { Text = "Цвет объекта:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 170, Height = 30, Top = 423, Left = 20 };
                    Button buttonColor = new Button() { Text = "", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 70, Height = 30, Top = 415, Left = 100, BackColor = (System.Drawing.Color)colorSurface };
                    buttonColor.Click += (sender1, e1) => { if (colorDialog.ShowDialog() == DialogResult.OK) { colorSurface = buttonColor.BackColor = colorDialog.Color; } };
                    Label lblTrackBar = new Label() { Text = "Прозрачность объекта:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 170, Height = 30, Top = 470, Left = 20 };
                    TrackBar trackBar = new TrackBar() { Value = (int)(_renderObjects[_SelectID].ColorObj.W * 10f), Minimum = 0, Maximum = 10, Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 170, Height = 10, Top = 470, Left = 150 };
                    CheckBox checkBox = new CheckBox() { Checked = false, Text = "Изменить структуру фигуры", Width = 170, Height = 30, Top = 375, Left = 20, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    TextBox textBoxChangeCoord = new TextBox() { Enabled = false, Multiline = true, Width = 250, Height = 350, Top = 10, Left = 10, Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom, ScrollBars = ScrollBars.Vertical };
                    TextBox textBoxChangeFinit = new TextBox() { Enabled = false, Multiline = true, Width = 250, Height = 350, Top = 10, Left = 10, Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom, ScrollBars = ScrollBars.Vertical };
                    SplitContainer splitterText = new SplitContainer() { Width = 540, Height = 360, Left = 10, Top = 10, Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom, SplitterDistance = 260 };
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
                    Triangls[] trianglsSelect = new Triangls[colFinElement];
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
                    dlgChangeFigure.Controls.Add(lblDiffs);
                    dlgChangeFigure.Controls.Add(lblDiffR);
                    dlgChangeFigure.Controls.Add(lblDiffG);
                    dlgChangeFigure.Controls.Add(lblDiffB);
                    dlgChangeFigure.Controls.Add(textBoxDiffR);
                    dlgChangeFigure.Controls.Add(textBoxDiffG);
                    dlgChangeFigure.Controls.Add(textBoxDiffB);
                    dlgChangeFigure.Controls.Add(splitterText);
                    dlgChangeFigure.Controls.Add(button_ok);
                    dlgChangeFigure.Controls.Add(buttonSave);
                    dlgChangeFigure.Controls.Add(trackBar);
                    dlgChangeFigure.Controls.Add(lblTrackBar);
                    dlgChangeFigure.Controls.Add(buttonColor);
                    dlgChangeFigure.Controls.Add(lblButtonColor);
                    dlgChangeFigure.Controls.Add(lblCoords);
                    dlgChangeFigure.Controls.Add(lblCoordX);
                    dlgChangeFigure.Controls.Add(lblCoordY);
                    dlgChangeFigure.Controls.Add(lblCoordZ);
                    dlgChangeFigure.Controls.Add(textBoxCoordX);
                    dlgChangeFigure.Controls.Add(textBoxCoordY);
                    dlgChangeFigure.Controls.Add(textBoxCoordZ);

                    saveFileCoord.FileOk += (senderCoord, eCoord) =>
                    {
                        if (saveFileCoord.FileName.Length != 0)
                        {
                            StreamWriter write_coord = new StreamWriter(saveFileCoord.FileName);
                            write_coord.Write(textBoxChangeCoord.Text);
                            write_coord.Close();
                        }
                    };
                    saveFileFinit.FileOk += (senderFinit, eFinit) =>
                    {
                        if (saveFileFinit.FileName.Length != 0)
                        {
                            StreamWriter writeFinit = new StreamWriter(saveFileFinit.FileName);
                            writeFinit.Write(textBoxChangeFinit.Text);
                            writeFinit.Close();
                        }
                    };
                    if (dlgChangeFigure.ShowDialog() == DialogResult.OK)
                    {
                        if (checkBox.Checked)
                        {
                            List<DataCoordElem> listCoord = new List<DataCoordElem>();
                            List<DataFinitElem> listFinitElem = new List<DataFinitElem>();
                            string[] stringLine = textBoxChangeCoord.Text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                            bool errorFlag = false;
                            for (int iter = 0; iter < stringLine.Length && !errorFlag; iter++)
                            {
                                string[] strXYZCoord = stringLine[iter].Split(' ');
                                if (strXYZCoord.Length == 3)
                                {
                                    DataCoordElem dataCoord = new DataCoordElem();
                                    dataCoord.X = double.Parse(strXYZCoord[0]);
                                    dataCoord.Y = double.Parse(strXYZCoord[1]);
                                    dataCoord.Z = double.Parse(strXYZCoord[2]);
                                    listCoord.Add(dataCoord);
                                }
                                else
                                {
                                    errorFlag = true;
                                }
                            }
                            stringLine = textBoxChangeFinit.Text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int iter = 0; iter < stringLine.Length && !errorFlag; iter++)
                            {
                                string[] finit_elem = stringLine[iter].Split(' ');
                                if (finit_elem.Length == 3)
                                {
                                    DataFinitElem data_temp = new DataFinitElem();
                                    data_temp.first = int.Parse(finit_elem[0]);
                                    data_temp.second = int.Parse(finit_elem[1]);
                                    data_temp.third = int.Parse(finit_elem[2]);
                                    listFinitElem.Add(data_temp);
                                }
                                else
                                {
                                    errorFlag = true;
                                }
                            }
                            if (!errorFlag)
                            {
                                Vertex[] figureVertex = new Vertex[listFinitElem.Count * 3];
                                int col = 0;
                                foreach (DataFinitElem fin in listFinitElem)
                                {
                                    Vector4 normVec = new Vector4();
                                    normVec.X = (float)(listCoord[fin.first].Y * listCoord[fin.third].Z - listCoord[fin.second].Y * listCoord[fin.third].Z - listCoord[fin.first].Y * listCoord[fin.second].Z - listCoord[fin.first].Z * listCoord[fin.third].Y + listCoord[fin.second].Z * listCoord[fin.third].Y + listCoord[fin.first].Z * listCoord[fin.second].Y);
                                    normVec.Y = (float)(listCoord[fin.first].Z * listCoord[fin.third].X - listCoord[fin.second].Z * listCoord[fin.third].X - listCoord[fin.first].Z * listCoord[fin.second].X - listCoord[fin.first].X * listCoord[fin.third].Z + listCoord[fin.second].X * listCoord[fin.third].Z + listCoord[fin.first].X * listCoord[fin.second].Z);
                                    normVec.Z = (float)(listCoord[fin.first].X * listCoord[fin.third].Y - listCoord[fin.second].X * listCoord[fin.third].Y - listCoord[fin.first].X * listCoord[fin.second].Y - listCoord[fin.first].Y * listCoord[fin.third].X + listCoord[fin.second].Y * listCoord[fin.third].X + listCoord[fin.first].Y * listCoord[fin.second].X);
                                    normVec.W = 0.0f;
                                    float len = Math.Abs(normVec.X) + Math.Abs(normVec.Y) + Math.Abs(normVec.Z);
                                    normVec.X = normVec.X / len;
                                    normVec.Y = normVec.Y / len;
                                    normVec.Z = normVec.Z / len;
                                    figureVertex[col] = new Vertex(new Vector4((float)listCoord[fin.first].X, (float)listCoord[fin.first].Y, (float)listCoord[fin.first].Z, 1.0f), new Vector4(normVec.X, normVec.Y, normVec.Z, 0.0f), new Vector2(0, 0));
                                    figureVertex[col + 1] = new Vertex(new Vector4((float)listCoord[fin.second].X, (float)listCoord[fin.second].Y, (float)listCoord[fin.second].Z, 1.0f), new Vector4(normVec.X, normVec.Y, normVec.Z, 0.0f), new Vector2(0, 0));
                                    figureVertex[col + 2] = new Vertex(new Vector4((float)listCoord[fin.third].X, (float)listCoord[fin.third].Y, (float)listCoord[fin.third].Z, 1.0f), new Vector4(normVec.X, normVec.Y, normVec.Z, 0.0f), new Vector2(0, 0));
                                    col += 3;
                                }
                                _renderObjects[_SelectID].WriteBuffer(figureVertex);
                            }
                            else
                            {
                                MessageBox.Show("Ошибка во формате входных данных", "Ошибка");
                            }
                        }
                        _renderObjects[_SelectID].ColorObj = new Vector4(colorSurface.R, colorSurface.G, colorSurface.B, trackBar.Value / 10f);
                        _renderObjects[_SelectID].changeModelMstrix(new Vector3(float.Parse(textBoxCoordX.Text), float.Parse(textBoxCoordY.Text), float.Parse(textBoxCoordZ.Text)));
                        if (typeObject == TypeObjectRenderLight.LightSourceObject)
                        {
                            var lightObject = _lightObjects.Where(x => x.ColorСhoice == _renderObjects[_SelectID].ColorСhoice).FirstOrDefault();
                            lightObject.DiffusionIntensity = new Vector3(float.Parse(textBoxDiffR.Text), float.Parse(textBoxDiffG.Text), float.Parse(textBoxDiffB.Text));
                            if (lightObject != null)
                            {
                                lightObject.SetPositionLight(_renderObjects[_SelectID].ModelMatrix);
                                if (_program_Fong_directed != -1 && lightObject.uboLightInfo != -1) lightObject.UpdatePositionForBlock(_program_Fong_directed);
                            }
                        }
                        else _renderObjects[_SelectID].setDiffusion(new Vector3(float.Parse(textBoxDiffR.Text), float.Parse(textBoxDiffG.Text), float.Parse(textBoxDiffB.Text)));
                        _renderObjects[_SelectID].typeObjectCreate = TypeObjectCreate.NonTypeObject;
                    }
                }
                else
                {
                    Vector3 position = _renderObjects[_SelectID].getStartPosition();
                    DlgAddEditAnFigure dlgNewAn = new DlgAddEditAnFigure(_renderObjects[_SelectID].typeObjectCreate, _renderObjects[_SelectID].ColorObj.W, _renderObjects[_SelectID].side, position.X, position.Y, position.Z, locColBreakX: _renderObjects[_SelectID].colBreakX, locColBreakY: _renderObjects[_SelectID].colBreakY, locCoeffSX: _renderObjects[_SelectID].coeffSX, locCoeffSY: _renderObjects[_SelectID].coeffSY, locAngleX: _renderObjects[_SelectID].angleX, locAngleY: _renderObjects[_SelectID].angleY, locAngleZ: _renderObjects[_SelectID].angleZ);
                    dlgNewAn.SetColor(new Color4(_renderObjects[_SelectID].ColorObj.X, _renderObjects[_SelectID].ColorObj.Y, _renderObjects[_SelectID].ColorObj.Z, _renderObjects[_SelectID].ColorObj.W));
                    if (dlgNewAn.ShowDialog() == DialogResult.OK)
                    {
                        Color4 colorcube = dlgNewAn.colorObject;
                        position = dlgNewAn.position;
                        Vertex[] figure_vertex = dlgNewAn.figureVertex;
                        _renderObjects[_SelectID].WriteBuffer(figure_vertex);
                        _renderObjects[_SelectID].ColorObj.X = colorcube.R;
                        _renderObjects[_SelectID].ColorObj.Y = colorcube.G;
                        _renderObjects[_SelectID].ColorObj.Z = colorcube.B;
                        _renderObjects[_SelectID].ColorObj.W = colorcube.A;
                        _renderObjects[_SelectID].side = dlgNewAn.side;
                        _renderObjects[_SelectID].angleX = dlgNewAn.angleX;
                        _renderObjects[_SelectID].angleY = dlgNewAn.angleY;
                        _renderObjects[_SelectID].angleZ = dlgNewAn.angleZ;
                        _renderObjects[_SelectID].colBreakX = dlgNewAn.colBreakX;
                        _renderObjects[_SelectID].colBreakY = dlgNewAn.colBreakY;
                        _renderObjects[_SelectID].coeffSX = dlgNewAn.coeffSX;
                        _renderObjects[_SelectID].coeffSY = dlgNewAn.coeffSY;
                    }
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
                var lightObj = _lightObjects.Where(x => x.ColorСhoice == _renderObjects[_SelectID].ColorСhoice).FirstOrDefault();
                buffer = -1;
                if (lightObj != null) buffer = lightObj.uboLightInfo;
                if (buffer > -1) GL.DeleteBuffer(buffer);
                _lightObjects.Remove(_lightObjects.Where(x => x.ColorСhoice == _renderObjects[_SelectID].ColorСhoice).FirstOrDefault());
                _renderObjects.Remove(_renderObjects[_SelectID]);
                _SelectID = -1;
            }
            else
            {
                MessageBox.Show("Этот объект нельзя удалить");
            }
        }

        class IdRenderObject
        {
            public string Text { get; set; }
            public Vector4 colorChoices { get; set; }
        }
            

        private void buttonTrajectory_Click(object sender, EventArgs e)
        {
            if (_SelectID > 0)
            {
                bool useTrajection = _renderObjects[_SelectID].trajctoryRenderObject.useTrajectory;
                Vector3 target = new Vector3(_renderObjects[_SelectID].trajctoryRenderObject.GetPoint());
                Vector4 colorObject = _renderObjects[_SelectID].trajctoryRenderObject.GetObject();
                TargetTrajectory targetEnum = _renderObjects[_SelectID].trajctoryRenderObject.target;
                Form dlgChangeTrajectory = new Form()
                {
                    Text = "Изменение траектории движения",
                    Width = 480,
                    Height = 270,
                    FormBorderStyle = FormBorderStyle.Sizable,
                    StartPosition = FormStartPosition.CenterScreen,
                };
                CheckBox checkBoxUseTrajectory = new CheckBox() { Checked = useTrajection, Text = "Использовать траекторию движения", Width = 170, Height = 30, Top = 15, Left = 20, Anchor = AnchorStyles.Left | AnchorStyles.Top };
                ComboBox comboBoxTargetObject = new ComboBox() { Enabled = checkBoxUseTrajectory.Checked, DropDownStyle = ComboBoxStyle.DropDownList, Text = "Объект", Left = 140, Width = 145, Top = 50 };
                Label lblTargetObject = new Label() { Text = "Целевой объект:", Anchor = AnchorStyles.Left | AnchorStyles.Top, Width = 120, Height = 30, Top = 53, Left = 20 };
                Label lblCoords = new Label() { Text = "Координаты точки", Anchor = AnchorStyles.Left | AnchorStyles.Top, Width = 120, Height = 30, Top = 80, Left = 20 };
                Label lblCoordsX = new Label() { Text = "X:", Anchor = AnchorStyles.Left | AnchorStyles.Top, Width = 20, Height = 30, Top = 113, Left = 20 };
                Label lblCoordsY = new Label() { Text = "Y:", Anchor = AnchorStyles.Left | AnchorStyles.Top, Width = 20, Height = 30, Top = 113, Left = 80 };
                Label lblCoordsZ = new Label() { Text = "Z:", Anchor = AnchorStyles.Left | AnchorStyles.Top, Width = 20, Height = 30, Top = 113, Left = 140 };
                Label objectId = new Label() { Text = "Id объект", Anchor = AnchorStyles.Left | AnchorStyles.Top, Width = 80, Height = 30, Top = 143, Left = 20 };
                Button buttonOk = new Button() { Text = "Ok", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 40, Height = 30, Top = 200, Left = 200, DialogResult = DialogResult.OK };
                comboBoxTargetObject.Items.AddRange( new object []
                {
                        "Объект",
                        "Точка"
                });
                comboBoxTargetObject.SelectedItem = targetEnum == TargetTrajectory.Point ? "Точка" : "Объект"; //Дефолтное значение Enum == Enum[0]
                TextBox textBoxX = new TextBox() { Enabled = checkBoxUseTrajectory.Checked && Convert.ToString(comboBoxTargetObject.SelectedItem) == "Точка", Text = Convert.ToString(target.X), Anchor = AnchorStyles.Left | AnchorStyles.Top, Width = 40, Height = 30, Top = 110, Left = 40 };
                TextBox textBoxY = new TextBox() { Enabled = checkBoxUseTrajectory.Checked && Convert.ToString(comboBoxTargetObject.SelectedItem) == "Точка", Text = Convert.ToString(target.Y), Anchor = AnchorStyles.Left | AnchorStyles.Top, Width = 40, Height = 30, Top = 110, Left = 100 };
                TextBox textBoxZ = new TextBox() { Enabled = checkBoxUseTrajectory.Checked && Convert.ToString(comboBoxTargetObject.SelectedItem) == "Точка", Text = Convert.ToString(target.Z), Anchor = AnchorStyles.Left | AnchorStyles.Top, Width = 40, Height = 30, Top = 110, Left = 160 };
                textBoxX.KeyPress += textBox1_KeyPress;
                textBoxY.KeyPress += textBox1_KeyPress;
                textBoxZ.KeyPress += textBox1_KeyPress;
                ComboBox comboBoxIds = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList, Enabled = checkBoxUseTrajectory.Checked && Convert.ToString(comboBoxTargetObject.SelectedItem) == "Объект", Anchor = AnchorStyles.Left | AnchorStyles.Top, Width = 180, Height = 30, Top = 175, Left = 20 };
                comboBoxTargetObject.SelectedValueChanged += (senderT, eT) =>
                {
                    comboBoxIds.Enabled = checkBoxUseTrajectory.Checked && Convert.ToString(comboBoxTargetObject.SelectedItem) == "Объект";
                    textBoxX.Enabled = textBoxY.Enabled = textBoxZ.Enabled = checkBoxUseTrajectory.Checked && Convert.ToString(comboBoxTargetObject.SelectedItem) == "Точка";
                };
                checkBoxUseTrajectory.CheckedChanged += (senderT, eT) =>
                {
                    comboBoxTargetObject.Enabled = checkBoxUseTrajectory.Checked;
                    comboBoxIds.Enabled = checkBoxUseTrajectory.Checked && Convert.ToString(comboBoxTargetObject.SelectedItem) == "Объект";
                    textBoxX.Enabled = textBoxY.Enabled = textBoxZ.Enabled = checkBoxUseTrajectory.Checked && Convert.ToString(comboBoxTargetObject.SelectedItem) == "Точка";
                };
                List<IdRenderObject> idsRenderObject = new List<IdRenderObject>();
                foreach (var renderObect in _renderObjects.Select((r, i) => new { Row = r, Index = i }))
                {
                    idsRenderObject.Add(new IdRenderObject {
                        colorChoices = renderObect.Row.ColorСhoice,
                        Text = Convert.ToString(renderObect.Row.ColorСhoice.Xyz).Replace("(", "").Replace(")", "").Replace(";", "").Replace(" ", "") + "   №" + Convert.ToString(renderObect.Index)
                    });
                }
                comboBoxIds.Items.AddRange(idsRenderObject.Cast<object>().ToArray());
                comboBoxIds.DisplayMember = "Text";
                comboBoxIds.ValueMember = "colorChoices";
                comboBoxIds.SelectedItem = (idsRenderObject).Where(x => x.colorChoices == colorObject).FirstOrDefault();
                
                dlgChangeTrajectory.Controls.Add(checkBoxUseTrajectory);
                dlgChangeTrajectory.Controls.Add(lblCoords);
                dlgChangeTrajectory.Controls.Add(comboBoxTargetObject);
                dlgChangeTrajectory.Controls.Add(lblTargetObject);
                dlgChangeTrajectory.Controls.Add(lblCoordsX);
                dlgChangeTrajectory.Controls.Add(lblCoordsY);
                dlgChangeTrajectory.Controls.Add(lblCoordsZ);
                dlgChangeTrajectory.Controls.Add(textBoxX);
                dlgChangeTrajectory.Controls.Add(textBoxY);
                dlgChangeTrajectory.Controls.Add(textBoxZ);
                dlgChangeTrajectory.Controls.Add(objectId);
                dlgChangeTrajectory.Controls.Add(comboBoxIds);
                dlgChangeTrajectory.Controls.Add(buttonOk);
                if (dlgChangeTrajectory.ShowDialog() == DialogResult.OK)
                {
                    if (checkBoxUseTrajectory.Checked)
                    {
                        TrajectoryFunctionsForm trajectoryFunctionsForm = new TrajectoryFunctionsForm(_renderObjects[_SelectID].trajctoryRenderObject.trajectoryFunctionsX, _renderObjects[_SelectID].trajctoryRenderObject.trajectoryFunctionsY, _renderObjects[_SelectID].trajctoryRenderObject.trajectoryFunctionsZ);
                        if (trajectoryFunctionsForm.ShowDialog() == DialogResult.OK)
                        {
                            if(trajectoryFunctionsForm.trajectoryFunctionsX.ValidateTrajectoryFunc() && trajectoryFunctionsForm.trajectoryFunctionsY.ValidateTrajectoryFunc() && trajectoryFunctionsForm.trajectoryFunctionsZ.ValidateTrajectoryFunc())
                            {
                                _renderObjects[_SelectID].trajctoryRenderObject.useTrajectory = true;
                                _renderObjects[_SelectID].trajctoryRenderObject.target = Convert.ToString(comboBoxTargetObject.SelectedItem) == "Точка" ? TargetTrajectory.Point : TargetTrajectory.Object;
                                _renderObjects[_SelectID].trajctoryRenderObject.trajectoryFunctionsX = trajectoryFunctionsForm.trajectoryFunctionsX;
                                _renderObjects[_SelectID].trajctoryRenderObject.trajectoryFunctionsY = trajectoryFunctionsForm.trajectoryFunctionsY;
                                _renderObjects[_SelectID].trajctoryRenderObject.trajectoryFunctionsZ = trajectoryFunctionsForm.trajectoryFunctionsZ;
                                if (Convert.ToString(comboBoxTargetObject.SelectedItem) == "Точка")
                                    _renderObjects[_SelectID].trajctoryRenderObject.SetPoint(new Vector4(float.Parse(textBoxX.Text), float.Parse(textBoxY.Text), float.Parse(textBoxZ.Text), 1.0f));
                                else
                                    _renderObjects[_SelectID].trajctoryRenderObject.SetObject(((IdRenderObject)comboBoxIds.SelectedItem).colorChoices);
                            }
                            else
                            {
                                _renderObjects[_SelectID].trajctoryRenderObject.useTrajectory = false;
                            }
                        }
                        else
                        {
                            _renderObjects[_SelectID].trajctoryRenderObject.useTrajectory = false;
                        }
                    }
                    _renderObjects[_SelectID].trajctoryRenderObject.useTrajectory = checkBoxUseTrajectory.Checked;
                }
            }
        }
    }
}
