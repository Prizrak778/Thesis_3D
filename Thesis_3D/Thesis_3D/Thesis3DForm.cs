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

        private int pointShasowFBO = -1;
        private int pointShasowPositionFBO = -1;
        private int shadowFBO;
        private int depthTex;
        private float far_plane = 1024;
        private int _program = -1;
        private int _program_contour = -1;
        private int _program_some_light = -1;
        private int _program_Lgh_directed_solar_effect = -1;
        private int _program_Fong_directed = -1;
        private int _program_Fong_fog = -1;
        private int _program_shadow_project = -1;
        private int _program_Fong = -1;
        private int _program_shadow_map = -1;
        private int _program_shadow_map_L = -1;
        private int _program_shadow_map_test = -1;
        private int _program_shadow_map_new = -1;
        private int _program_shadow_map_PCF = -1;
        private int _program_shadow_map_PCF_new = -1;
        private int _program_shadow_point = -1;
        private int _program_shadow_point_noshaders = -1;

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

        private string defaultTitle = string.Empty;

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
        private int CompileShaders(ref string error, String VertexString, String FragmentString, String GeometricString = "")
        {
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, File.ReadAllText(VertexString));
            GL.CompileShader(vertexShader);
            error += GL.GetShaderInfoLog(vertexShader);
            if (!string.IsNullOrWhiteSpace(error)) error += "\r\n";
            int geometryShader = 0;
            if (GeometricString != "")
            {
                geometryShader = GL.CreateShader(ShaderType.GeometryShader);
                GL.ShaderSource(geometryShader, File.ReadAllText(GeometricString));
                GL.CompileShader(geometryShader);
                error += GL.GetShaderInfoLog(geometryShader);
                if (!string.IsNullOrWhiteSpace(error)) error += "\r\n";
            }
            
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, File.ReadAllText(FragmentString));
            GL.CompileShader(fragmentShader);
            error += GL.GetShaderInfoLog(fragmentShader) + "\r\n";
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
            error += GL.GetProgramInfoLog(program);
            if (!string.IsNullOrWhiteSpace(error)) error += "\r\n";
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
            // В плане матриц перемножение проихсодит в обратном порядке в шейдерах это P * V * M здесь это M * V * P
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
            string GeometryShader = string.Empty;
            if ((_program_contour = _program = CompileShaders(ref error, VertexShader, FragentShader)) == -1)
            {
                error += "Ошибка при компиляции обычного шейдера\n===============================\n";
                return false;
            }
            VertexShader = @"Components\Shaders\vertexShader.vert";
            FragentShader = @"Components\Shaders\fragmentShader.frag";
            listProgram.Add(_program);

            if ((_program = CompileShaders(ref error, VertexShader, FragentShader)) == -1)
            {
                error += "Ошибка при компиляции шейдера т.и. без отражения\n===============================\n";
                return false;
            }
            listProgram.Add(_program);

            VertexShader = @"Components\Shaders\vertexShader_mirror.vert";
            FragentShader = @"Components\Shaders\fragmentShader.frag";
            if ((_program = CompileShaders(ref error, VertexShader, FragentShader)) == -1)
            {
                error += "Ошибка при компиляции шейдера т.и. с отражением\n===============================\n";
                return false;
            }
            listProgram.Add(_program);

            VertexShader = @"Components\Shaders\vertexShader_double_mirror.vert";
            FragentShader = @"Components\Shaders\fragmentShader_double_mirror.frag";
            if ((_program = CompileShaders(ref error, VertexShader, FragentShader)) == -1)
            {
                error += "Ошибка при компиляции шейдера т.и. с двойным отражением\n===============================\n";
                return false;
            }
            listProgram.Add(_program);

            VertexShader = @"Components\Shaders\vertexShader_flatshadow.vert";
            FragentShader = @"Components\Shaders\fragmentShader.frag";
            if ((_program = CompileShaders(ref error, VertexShader, FragentShader)) == -1)
            {
                error += "Ошибка при компиляции шейдера т.и. с плоским затенением\n===============================\n";
                return false;
            }
            listProgram.Add(_program);

            VertexShader = @"Components\Shaders\vertexShader_some_light.vert";
            FragentShader = @"Components\Shaders\fragmentShader.frag";
            if ((_program = CompileShaders(ref error, VertexShader, FragentShader)) == -1)
            {
                error += "Ошибка при компиляции шейдера несколько т.и.\n===============================\n";
                return false;
            }
            _program_some_light = _program;
            listProgram.Add(_program);

            VertexShader = @"Components\Shaders\vertexShader_Lgh_directed.vert";
            FragentShader = @"Components\Shaders\fragmentShader.frag";
            if ((_program = CompileShaders(ref error, VertexShader, FragentShader)) == -1)
            {
                error += "Ошибка при компиляции шейдера направленного т.и\n===============================\n";
                return false;
            }
            listProgram.Add(_program);

            VertexShader = @"Components\Shaders\vertexShader_Fong.vert";
            FragentShader = @"Components\Shaders\fragmentShader_Fong.frag";
            if ((_program = CompileShaders(ref error, VertexShader, FragentShader)) == -1)
            {
                error += "Ошибка при компиляции шейдера затенение по Фонгу\n===============================\n";
                return false;
            }
            _program_Fong = _program;
            listProgram.Add(_program);

            VertexShader = @"Components\Shaders\vertexShader_Fong.vert";
            FragentShader = @"Components\Shaders\fragmentShader_Fong_half.frag";
            if ((_program = CompileShaders(ref error, VertexShader, FragentShader)) == -1)
            {
                error += "Ошибка при компиляции шейдера затенение по Фонгу с использованием вектора полпути\n===============================\n";
                return false;
            }
            listProgram.Add(_program);

            VertexShader = @"Components\Shaders\vertexShader_Fong.vert";
            FragentShader = @"Components\Shaders\fragmentShader_Fong_directed.frag";
            if ((_program = CompileShaders(ref error, VertexShader, FragentShader)) == -1)
            {
                error += "Ошибка при компиляции шейдера узконаправленый источник\n===============================\n";
                return false;
            }
            _program_Fong_directed = _program;
            listProgram.Add(_program);

            VertexShader = @"Components\Shaders\vertexShader_Fong.vert";
            FragentShader = @"Components\Shaders\fragmentShader_Fong_fog.frag";
            if ((_program = CompileShaders(ref error, VertexShader, FragentShader)) == -1)
            {
                error += "Ошибка при компиляции шейдера туман\n===============================\n";
                return false;
            }
            _program_Fong_fog = _program;
            listProgram.Add(_program);

            VertexShader = @"Components\Shaders\vertexShader_shadow.vert";
            FragentShader = @"Components\Shaders\fragmentShader_shadow.frag";
            if ((_program = CompileShaders(ref error, VertexShader, FragentShader)) == -1)
            {
                error += "Ошибка при компиляции шейдера плоских теней\n===============================\n";
                return false;
            }
            _program_shadow_project = _program;
            listProgram.Add(_program);

            VertexShader = @"Components\Shaders\vertexShader_Lgh_directed_solar_effect.vert";
            FragentShader = @"Components\Shaders\fragmentShader_Lgh_directed_solar_effect.frag";
            if ((_program = CompileShaders(ref error, VertexShader, FragentShader)) == -1)
            {
                error += "Ошибка при компиляции шейдера плоских теней\n===============================\n";
                return false;
            }
            _program_Lgh_directed_solar_effect = _program;

            VertexShader = @"Components\Shaders\vertexShader_shadow_map_test.vert";
            FragentShader = @"Components\Shaders\fragmentShader_shadow_map_test.frag";
            if ((_program = CompileShaders(ref error, VertexShader, FragentShader)) == -1)
            {
                error += "Ошибка при компиляции шейдера глубины карты теней\n===============================\n";
                return false;
            }
            _program_shadow_map_test = _program;
            listProgram.Add(_program);

            VertexShader = @"Components\Shaders\vertexShader_shadow_map.vert";
            FragentShader = @"Components\Shaders\fragmentShader_shadow_map.frag";
            if ((_program = CompileShaders(ref error, VertexShader, FragentShader)) == -1)
            {
                error += "Ошибка при компиляции шейдера карты теней\n===============================\n";
                return false;
            }
            _program_shadow_map = _program;
            listProgram.Add(_program);

            VertexShader = @"Components\Shaders\vertexShader_shadow_map.vert";
            FragentShader = @"Components\Shaders\fragmentShader_shadow_map_new.frag";
            if ((_program = CompileShaders(ref error, VertexShader, FragentShader)) == -1)
            {
                error += "Ошибка при компиляции шейдера старой карты теней\n===============================\n";
                return false;
            }
            _program_shadow_map_new = _program;
            listProgram.Add(_program);

            VertexShader = @"Components\Shaders\vertexShader_shadow_map.vert";
            FragentShader = @"Components\Shaders\fragmentShader_shadow_map_PCF.frag";
            if ((_program = CompileShaders(ref error, VertexShader, FragentShader)) == -1)
            {
                error += "Ошибка при компиляции шейдера старой карты теней PCF\n===============================\n";
                return false;
            }
            _program_shadow_map_PCF = _program;
            listProgram.Add(_program);

            VertexShader = @"Components\Shaders\vertexShader_shadow_map.vert";
            FragentShader = @"Components\Shaders\fragmentShader_shadow_map_PCF_new.frag";
            if ((_program = CompileShaders(ref error, VertexShader, FragentShader)) == -1)
            {
                error += "Ошибка при компиляции шейдера старой карты теней PCF\n===============================\n";
                return false;
            }
            _program_shadow_map_PCF_new = _program;
            listProgram.Add(_program);

            VertexShader = @"Components\Shaders\vertexShader_shadow_map_L.vert";
            GeometryShader = @"Components\Shaders\geometryShader_shadow_map_L.geom";
            FragentShader = @"Components\Shaders\fragmentShader_shadow_map_L.frag";
            if ((_program = CompileShaders(ref error, VertexShader, FragentShader, GeometryShader)) == -1)
            {
                error += "Ошибка при компиляции шейдера текстурный карты теней\n===============================\n";
                return false;
            }
            _program_shadow_map_L = _program;
            
            VertexShader = @"Components\Shaders\vertexShader_shadow_point.vert";
            FragentShader = @"Components\Shaders\fragmentShader_shadow_point.frag";
            if ((_program = CompileShaders(ref error, VertexShader, FragentShader)) == -1)
            {
                error += "Ошибка при компиляции шейдера реберной трассировка\n===============================\n";
                return false;
            }
            _program_shadow_point = _program;
            listProgram.Add(_program);

            VertexShader = @"Components\Shaders\vertexShader_Fong.vert";
            FragentShader = @"Components\Shaders\fragmentShader_shadow_point_noshaders.frag";
            if ((_program = CompileShaders(ref error, VertexShader, FragentShader)) == -1)
            {
                error += "Ошибка при компиляции шейдера реберной трассировка без шейдеров\n===============================\n";
                return false;
            }
            _program_shadow_point_noshaders = _program;
            listProgram.Add(_program);
            return true;
        }
        #region BufferPointShadowns
        private void ResetBufferPointShadownsPosition()
        {
            if (pointShasowPositionFBO == -1)
            {
                setBufferPointShadownsPosition();
                return;
            }
            if (_renderObjects.Count < 1) return;
            var sizeBuffer = 4 * _renderObjects.Where(x => x.TypeObject != TypeObjectRenderLight.LightSourceObject).Count();
            if (sizeBuffer < 0) return;
            var offsetVertex = 0;
            float[] allPosition = new float[sizeBuffer];
            foreach (var renderObject in _renderObjects.Where(x => x.TypeObject != TypeObjectRenderLight.LightSourceObject))
            {

                Vector3 positionObject = renderObject.ModelMatrix.ExtractTranslation();
                allPosition[offsetVertex] = positionObject.X;
                allPosition[offsetVertex + 1] = positionObject.Y;
                allPosition[offsetVertex + 2] = positionObject.Z;
                allPosition[offsetVertex + 3] = renderObject.BufferSize(); //Костыль или оптимизация??? 
                offsetVertex += 4;
            }
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, pointShasowPositionFBO);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, allPosition.Length, allPosition, BufferUsageHint.DynamicDraw);
        }
        private void setBufferPointShadownsPosition()
        {
            if (_renderObjects.Count < 1) return;
            var sizeBuffer = 4 * _renderObjects.Where(x => x.TypeObject != TypeObjectRenderLight.LightSourceObject).Count();
            if (sizeBuffer < 0) return;
            var offsetVertex = 0;
            float[] allPosition = new float[sizeBuffer];
            foreach (var renderObject in _renderObjects.Where(x => x.TypeObject != TypeObjectRenderLight.LightSourceObject))
            {

                Vector3 positionObject = renderObject.ModelMatrix.ExtractTranslation();
                allPosition[offsetVertex] = positionObject.X;
                allPosition[offsetVertex + 1] = positionObject.Y;
                allPosition[offsetVertex + 2] = positionObject.Z;
                allPosition[offsetVertex + 3] = renderObject.BufferSize(); //Костыль или оптимизация??? 
                offsetVertex += 4;
            }
            GL.GenBuffers(1, out pointShasowPositionFBO);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, pointShasowPositionFBO);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, allPosition.Length, allPosition, BufferUsageHint.DynamicDraw);
        }

        private void setBufferPointShadowns()
        {
            if (_renderObjects.Count < 1) return;
            var colVertex = 0;
            foreach(var renderObject in _renderObjects.Where(x => x.TypeObject != TypeObjectRenderLight.LightSourceObject))
            {
                colVertex += renderObject.BufferSize();
            }
            if (colVertex < 0) return;
            var offsetVertex = 0;
            float[] allVertex = new float[colVertex * 4];
            foreach (var renderObject in _renderObjects.Where(x=>x.TypeObject != TypeObjectRenderLight.LightSourceObject))
            {
                Vertex[] vertexObject = new Vertex[renderObject.BufferSize()];
                renderObject.ReadBuffer(vertexObject);
                for(int i = 0; i < vertexObject.Length; i++)
                {
                    allVertex[offsetVertex + i * 4] = (renderObject.RotationMatrix * renderObject.ModelMatrix * vertexObject[i]._Position).X;
                    allVertex[offsetVertex + i * 4 + 1] = (renderObject.RotationMatrix * renderObject.ModelMatrix * vertexObject[i]._Position).Y;
                    allVertex[offsetVertex + i * 4 + 2] = (renderObject.RotationMatrix * renderObject.ModelMatrix * vertexObject[i]._Position).Z;
                    allVertex[offsetVertex + i * 4 + 3] = (renderObject.RotationMatrix * renderObject.ModelMatrix * vertexObject[i]._Position).W;
                }
                offsetVertex += renderObject.BufferSize() * 4;
            }
            GL.GenBuffers(1, out pointShasowFBO);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, pointShasowFBO);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, allVertex.Length, allVertex, BufferUsageHint.DynamicCopy);
        }
        private void ResetBufferPointShadowns()
        {
            if(pointShasowFBO == -1)
            {
                setBufferPointShadowns();
                return;
            }
            if (_renderObjects.Count < 1) return;
            var colVertex = 0;
            foreach (var renderObject in _renderObjects.Where(x => x.TypeObject != TypeObjectRenderLight.LightSourceObject))
            {
                colVertex += renderObject.BufferSize();
            }
            if (colVertex < 0) return;
            var offsetVertex = 0;
            float[] allVertex = new float[colVertex * 4];
            foreach (var renderObject in _renderObjects.Where(x => x.TypeObject != TypeObjectRenderLight.LightSourceObject))
            {
                Vertex[] vertexObject = new Vertex[renderObject.BufferSize()];
                renderObject.ReadBuffer(vertexObject);
                for (int i = 0; i < vertexObject.Length; i++)
                {
                    allVertex[offsetVertex + i * 4] = (renderObject.RotationMatrix * renderObject.ModelMatrix * vertexObject[i]._Position).X;
                    allVertex[offsetVertex + i * 4 + 1] = (renderObject.RotationMatrix * renderObject.ModelMatrix * vertexObject[i]._Position).Y;
                    allVertex[offsetVertex + i * 4 + 2] = (renderObject.RotationMatrix * renderObject.ModelMatrix * vertexObject[i]._Position).Z;
                    allVertex[offsetVertex + i * 4 + 3] = (renderObject.RotationMatrix * renderObject.ModelMatrix * vertexObject[i]._Position).W;
                }
                offsetVertex += renderObject.BufferSize() * 4;
            }
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, pointShasowFBO);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, allVertex.Length, allVertex, BufferUsageHint.DynamicDraw);
        }
        #endregion

        #region shadow_map_tex
        private void init_tex_shadow()
        {
            float[] border = { 1.0f, 0.0f, 0.0f, 0.0f };

            //Текстура с картой теней

            GL.GenTextures(1, out depthTex);
            GL.BindTexture(TextureTarget.TextureCubeMap, depthTex);
            for (int i = 0; i < 6; i++)
            {
                //GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Depth32fStencil8, glControl.Height, glControl.Width, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.DepthComponent32f, 1024, 1024, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
            }
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
            //GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureBorderColor, border);
            //GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureCompareMode, (int)TextureCompareMode.CompareRefToTexture);
            //GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureCompareFunc, (int)DepthFunction.Less);

            //GL.ActiveTexture(TextureUnit.Texture0);
            //GL.BindTexture(TextureTarget.TextureCubeMap, depthTex);

            GL.GenFramebuffers(1, out shadowFBO);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, shadowFBO);
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, depthTex, 0);
            var check = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            //GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthTex, 0);
            GL.DrawBuffer(DrawBufferMode.None);
            GL.ReadBuffer(ReadBufferMode.None);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        #endregion

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
                MessageBox.Show(ErrorText);
                throw new Exception(ErrorText);
            }
            init_tex_shadow();
            comboBoxShaders.Items.AddRange(new object[] 
              {
                  "Обычные цвета",
                  "Т.И. без отражения",
                  "Т.И. с отражением",
                  "Т.И. с двойным отражением",
                  "Т.И. с плоским затенением",
                  "Несколько Т.И.",
                  "Направленный источник",
                  "Затенение по Фонгу",
                  "Затенение по Фонгу с использованием вектора полпути",
                  "Узконаправленный источник",
                  "Туман",
                  "Плоское затенение для одного элемента",
                  "Карта теней тест",
                  "Карта теней",
                  "Карта теней улучшенный",
                  "Карта теней PFC",
                  "Карта теней PFC улучшенный",
                  "Рёберная трассировка",
                  "Рёберная трассировка не шейдеры"
            });
            comboBoxShaders.SelectedIndex = 0;
            Vector3 positionObject = new Vector3(-1.0f, 1.0f, 0.0f);
            primarySphereAt = new RenderObject(ObjectCreate.CreateSphere(40f, positionObject, 60, 60, 1, 1), positionObject, Color4.DeepSkyBlue, RandomColor(), locSide: 40f, locTypeObjectCreate: TypeObjectCreate.Sphere, locColBreakX: 60, locColBreakY: 60, locCoeffSX: 1, locCoeffSY: 1);
            _renderObjects.Add(new RenderObject(ObjectCreate.CreatePlane(1.5f, positionObject, 0, 0, 45), positionObject, Color4.LightCyan, RandomColor(), plane: true, locSide: 1.5f, locTypeObjectCreate: TypeObjectCreate.Plane, locAngleZ: 45));
            primaryRenderObject = _renderObjects[0];
            positionObject = new Vector3(0.0f, 0.0f, 0.0f);
            //_renderObjects.Add(new RenderObject(ObjectCreate.CreatePlane(15f, positionObject, 0, 0, 0), positionObject, Color4.Green, RandomColor(), plane: true, locSide: 40f, locTypeObjectCreate: TypeObjectCreate.Plane));
            positionObject = new Vector3(0.0f, 2.0f, 0.0f);
            _renderObjects.Add(new RenderObject(ObjectCreate.CreateSolidCube(0.5f, positionObject), positionObject, Color4.LightCoral, RandomColor(), locSide: 0.5f, locTypeObjectCreate: TypeObjectCreate.SolidCube));
            /*for (int i = 0; i < 10; i++)
            {
                positionObject = new Vector3((float)i + 1, 2.0f, 0.0f);
                _renderObjects.Add(new RenderObject(ObjectCreate.CreateSolidCube(0.5f, positionObject), positionObject, Color4.LightCoral, RandomColor(), locSide: 0.5f, locTypeObjectCreate: TypeObjectCreate.SolidCube));
            }
            for (int i = 0; i < 10; i++)
            {
                positionObject = new Vector3(1, -(float)i + 2.0f, 0.0f);
                _renderObjects.Add(new RenderObject(ObjectCreate.CreateSolidCube(0.5f, positionObject), positionObject, Color4.LightCoral, RandomColor(), locSide: 0.5f, locTypeObjectCreate: TypeObjectCreate.SolidCube));
            }*/
            //positionObject = new Vector3(1.0f);
            //_renderObjects.Add(new RenderObject(ObjectCreate.CreateSphere(1.5f, positionObject, 60, 60, 1, 1), positionObject, Color4.Brown, RandomColor(), locSide: 1.5f, locTypeObjectCreate: TypeObjectCreate.Sphere, locColBreakX: 60, locColBreakY: 60, locCoeffSX: 1, locCoeffSY: 1));
            Vector3 positionLight = new Vector3(-3, 1.0f, 0.0f);
            _lightObjects.Add(new LightObject(ObjectCreate.CreateSphere(1.0f, positionLight, 10, 10, 1, 1), Color4.Yellow, RandomColor(), positionLight, new Vector4(5.0f, 5.0f, 1.0f, 1.0f), new Vector3(-0.2f, -1f, -0.3f), new Vector3(0.3f, 0.3f, 0.0f), new Vector3(1.0f, 0.0f, 5f), _program_Fong_directed, side: 1f, locTypeObjectCreate: TypeObjectCreate.SolidCube, locColBreakX: 10, locColBreakY: 10, locCoeffSX: 1, locCoeffSY: 1));
            primaryLightObject = _lightObjects[0];
            
            /*primaryLightObject.trajctoryRenderObject = new TrajctoryRenderObject(
                new TrajectoryFunctions(300, (double x) => (Math.Cos(x)), 0.03f, -180, 180, 0, "cos(x)", true),
                new TrajectoryFunctions(300, (double x) => (Math.Sin(x)), 0.03f, -180, 180, 0, "sin(y)", true),
                new TrajectoryFunctions(100, (double x) => (x), 0.001f, -1, 1, 0, "z", false),
                TargetTrajectory.Point,
                new Vector4(0, 0, 0, 1f)
                );*/
            primaryLightObject.Ambient = new Vector3(0.0f, 0.15f, 0.0f);
            /*positionLight = new Vector3(4.0f, 3.0f, 1.0f);                                                                                                                                                                                     
            _lightObjects.Add(new LightObject(ObjectCreate.CreateSolidCube(0.1f, positionLight), Color4.Yellow, RandomColor(), positionLight, new Vector4(5.0f, 5.0f, 1.0f, 1.0f), new Vector3(-0.2f, -1f, -0.3f), new Vector3(0.0f, 0.3f, 0.3f), new Vector3(1.0f, 0.0f, 5f), side: 0.1f, locTypeObjectCreate: TypeObjectCreate.SolidCube));
            positionLight = new Vector3(7.0f, 3.0f, 1.0f);                                                                                                                                                                                     
            _lightObjects.Add(new LightObject(ObjectCreate.CreateSolidCube(0.1f, positionLight), Color4.Yellow, RandomColor(), positionLight, new Vector4(5.0f, 5.0f, 1.0f, 1.0f), new Vector3(-0.2f, -1f, -0.3f), new Vector3(0.3f, 0.0f, 0.3f), new Vector3(1.0f, 0.0f, 5f), side: 0.1f, locTypeObjectCreate: TypeObjectCreate.SolidCube));*/
            /*for (int i = 0; i < 147; i++)
            {
                positionLight = new Vector3(10.0f + 3*i, 3.0f, 1.0f);
                _lightObjects.Add(new LightObject(ObjectCreate.CreateSolidCube(0.1f, positionLight), Color4.Yellow, RandomColor(), positionLight, new Vector4(5.0f, 5.0f, 1.0f, 1.0f), new Vector3(-0.2f, -1f, -0.3f), new Vector3(0.3f, 0.0f, 0.3f), new Vector3(1.0f, 0.0f, 5f), side: 0.1f, locTypeObjectCreate: TypeObjectCreate.SolidCube));
            }*/
            foreach (var obj in _lightObjects)
            {
                _renderObjects.Add(obj);
            }
            setBufferPointShadowns();
            setBufferPointShadownsPosition();
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
            defaultTitle =
                GL.GetString(StringName.Vendor) + " " +
                GL.GetString(StringName.Renderer) + " " +
                GL.GetString(StringName.Version);
            Text = defaultTitle + $" (Vsync: {glControlThesis3D.VSync})";
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
                case Keys.V:
                    glControlThesis3D.VSync = glControlThesis3D.VSync ? false : true;
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
            Matrix4 ModelView = renderObject.RotationMatrix * renderObject.ModelMatrix;
            renderObject.Bind();
            GL.UniformMatrix4(20, false, ref _MVP);
            //GL.UniformMatrix4(21, false, ref _projectionMatrix);
            GL.UniformMatrix4(22, false, ref ModelView);
            renderObject.PolygonMode_now(polygon);
        }
        private void Render()
        {
            var startTime = System.Diagnostics.Stopwatch.StartNew();
            GL.ClearColor(new Color4(0.3f, 0.3f, 0.3f, 1.0f));
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            CreateProjection();

            
            if ((_program == _program_shadow_map || _program == _program_shadow_map_test || _program == _program_shadow_map_new || _program == _program_shadow_map_PCF || _program == _program_shadow_map_PCF_new) && primaryLightObject != null)
            {
                var programSave = _program;
                Matrix4 lightProjection = Matrix4.CreatePerspectiveFieldOfView(90f * (float)Math.PI / 180f, 1f, 0.01f, far_plane);
                Matrix4[] shadowTransforms = new Matrix4[6];
                Vector3 positionLight = primaryLightObject.getPositionRenderObject().Xyz * new Vector3(1.0f, 1.0f, 1.0f);
                shadowTransforms[0] = Matrix4.LookAt(positionLight, positionLight + new Vector3( 1.0f,  0.0f,  0.0f), new Vector3(0, -1,  0)) * lightProjection;
                shadowTransforms[1] = Matrix4.LookAt(positionLight, positionLight + new Vector3(-1.0f,  0.0f,  0.0f), new Vector3(0, -1,  0)) * lightProjection;
                shadowTransforms[2] = Matrix4.LookAt(positionLight, positionLight + new Vector3( 0.0f,  1.0f,  0.0f), new Vector3(0,  0,  1)) * lightProjection;
                shadowTransforms[3] = Matrix4.LookAt(positionLight, positionLight + new Vector3( 0.0f, -1.0f,  0.0f), new Vector3(0,  0, -1)) * lightProjection;
                shadowTransforms[4] = Matrix4.LookAt(positionLight, positionLight + new Vector3( 0.0f,  0.0f,  1.0f), new Vector3(0, -1,  0)) * lightProjection;
                shadowTransforms[5] = Matrix4.LookAt(positionLight, positionLight + new Vector3( 0.0f,  0.0f, -1.0f), new Vector3(0, -1,  0)) * lightProjection;
                
                GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, shadowFBO);
                GL.Clear(ClearBufferMask.DepthBufferBit);
                GL.UseProgram(_program_shadow_map_L);
                GL.Uniform1(7, far_plane);
                for (int i = 0; i < 6; i++)
                {
                    var index_shadow_mat = GL.GetUniformLocation(_program_shadow_map_L, "shadowMatrices");
                    GL.UniformMatrix4(index_shadow_mat + i, false, ref shadowTransforms[i]);
                }
                primaryLightObject.PositionLightUniform(18);
                foreach (var renderObject in _renderObjects.Where(x=>x.TypeObject != TypeObjectRenderLight.LightSourceObject))
                {
                    RenderFigure(renderObject, PolygonMode.Fill);
                    renderObject.Render();
                }
                _program = programSave;
            }
            if(_program == _program_shadow_point)
            {
                ResetBufferPointShadowns();
            }
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.UseProgram(_program);
            cameraFirstFace.SetPositionCamerShader(23);
            var offset = 0;
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
                else if (_program == _program_shadow_point)
                {
                    GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 3, pointShasowFBO);
                    GL.Uniform1(30, (int)renderObject.TypeObject);
                    GL.Uniform1(31, offset);
                    GL.Uniform1(32, renderObject.BufferSize());
                    GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
                    offset += renderObject.BufferSize();
                }
                
                if ((_program == _program_shadow_map || _program == _program_shadow_map_new || _program == _program_shadow_map_PCF || _program == _program_shadow_map_PCF_new) && renderObject.TypeObject != TypeObjectRenderLight.LightSourceObject)
                {
                    GL.ActiveTexture(TextureUnit.Texture0);
                    GL.BindTexture(TextureTarget.TextureCubeMap, depthTex);
                    GL.Uniform1(7, far_plane);
                    primaryLightObject.PositionLightUniform(18);
                    primaryLightObject.IntensityLightVectorUniform(24);
                    primaryLightObject.IntensityAmbient(26);
                    primaryLightObject.IntensityMirror(28);
                    renderObject.mirrorUnifrom(29);
                    renderObject.ambientUnifrom(27);
                    renderObject.diffusionUnifrom(25);
                }
                else if(_program == _program_shadow_point_noshaders)
                {
                    primaryLightObject.PositionLightUniform(18);
                    primaryLightObject.IntensityLightVectorUniform(24);
                    primaryLightObject.IntensityAmbient(26);
                    primaryLightObject.IntensityMirror(28);
                    renderObject.mirrorUnifrom(29);
                    renderObject.ambientUnifrom(27);
                    renderObject.diffusionUnifrom(25);
                    Vertex[] vertexObject = new Vertex[renderObject.BufferSize()];
                    renderObject.ReadBuffer(vertexObject);
                    bool flag = true;
                    foreach (var renderOtherObject in  _renderObjects.Where(x=>x.ColorСhoice != renderObject.ColorСhoice && x.TypeObject != TypeObjectRenderLight.LightSourceObject))
                    {
                        Vertex[] vertexOtherObject = new Vertex[renderOtherObject.BufferSize()];
                        renderOtherObject.ReadBuffer(vertexOtherObject);
                        for (int i = 0; i < vertexObject.Length && flag; i+=3)
                        {

                            for(int j = 0; j < vertexOtherObject.Length && flag; j+=3)
                            {
                                Vector3 pa = (renderOtherObject.ModelMatrix * vertexOtherObject[j]._Position).Xyz;
                                Vector3 pb = (renderOtherObject.ModelMatrix * vertexOtherObject[j + 1]._Position).Xyz;
                                Vector3 pc = (renderOtherObject.ModelMatrix * vertexOtherObject[j + 2]._Position).Xyz;
                                Vector3 p2 = (renderOtherObject.ModelMatrix * vertexObject[i]._Position).Xyz;
                                Vector3 N = Vector3.Normalize(Vector3.Cross((pb - pa), (pc - pa)));
                                float D = -N.X * pa.X - N.Y * pa.Y - N.Z * pa.Z;
                                float mu = N.X * primaryLightObject.Position.X + N.Y * primaryLightObject.Position.Y + N.Z * primaryLightObject.Position.Z;

                                mu = -(D + mu);
                                float mu_znam = N.X * (p2.X - primaryLightObject.Position.X) + N.Y * (p2.Y - primaryLightObject.Position.Y) + N.Z * (p2.Z - primaryLightObject.Position.Z);
                                if (Math.Abs(mu_znam) < 0.00000001)
                                {
                                    continue;
                                }
                                mu = mu / mu_znam;
                                if (mu < 0 || mu >= 1)
                                {
                                    continue;
                                }
                                Vector3 p = primaryLightObject.Position + mu * (p2 - primaryLightObject.Position);
                                Vector3 pa1 = Vector3.Normalize(pa - p);
                                Vector3 pa2 = Vector3.Normalize(pb - p);
                                Vector3 pa3 = Vector3.Normalize(pc - p);
                                float a1 = pa1.X * pa2.X + pa1.Y * pa2.Y + pa1.Z * pa2.Z;
                                float a2 = pa2.X * pa3.X + pa2.Y * pa3.Y + pa2.Z * pa3.Z;
                                float a3 = pa3.X * pa1.X + pa3.Y * pa1.Y + pa3.Z * pa1.Z;
                                float total = (float)((Math.Acos(a1) + Math.Acos(a2) + Math.Acos(a3)) * 180 / Math.PI);
                                if (Math.Abs(total - 360) < 0.00000001)
                                {
                                    flag = false;
                                }
                            }
                        }
                        if (!flag) break;
                    }
                    int flagUn = flag ? 1 : 0;
                    GL.Uniform1(30, flagUn);
                }
                else if(_program == _program_shadow_map_test)
                {
                    GL.ActiveTexture(TextureUnit.Texture0);
                    GL.BindTexture(TextureTarget.TextureCubeMap, depthTex);
                    GL.Uniform1(7, far_plane);
                    primaryLightObject.PositionLightUniform(18);
                    primaryLightObject.IntensityLightVectorUniform(24);
                    primaryLightObject.IntensityAmbient(26);
                    primaryLightObject.IntensityMirror(28);
                    renderObject.mirrorUnifrom(29);
                    renderObject.ambientUnifrom(27);
                    renderObject.diffusionUnifrom(25);
                }
                else if (_program == _program_some_light)
                {
                    foreach (var light in _lightObjects.Cast<LightObject>().Select((r, i) => new { Row = r, Index = i }))
                    {
                        light.Row.PositionLightUniform(175 + light.Index);
                        light.Row.IntensityLightVectorUniform(27 + light.Index);
                        renderObject.mirrorUnifrom(24);
                        renderObject.ambientUnifrom(25);
                        renderObject.diffusionUnifrom(26);
                    }
                }
                else if(_program == _program_Fong_fog && countLightObj > 0)
                {
                    primaryLightObject.PositionLightUniform(18);
                    primaryLightObject.SetAttrFog(31, 1f, 30, 9f, 32, new Vector3(0.3f, 0.3f, 0.3f));
                    primaryLightObject.IntensityLightVectorUniform(24);
                    renderObject.mirrorUnifrom(29);
                    renderObject.ambientUnifrom(27);
                    renderObject.diffusionUnifrom(25);
                }
                else if(_program == _program_Fong_directed && countLightObj > 0)
                {
                    if(primaryLightObject.uboLightInfo != -1) GL.BindBufferRange(BufferRangeTarget.UniformBuffer, 24, primaryLightObject.uboLightInfo, (IntPtr)0, primaryLightObject.blockSizeLightInfo);
                    renderObject.mirrorUnifrom(29);
                    renderObject.ambientUnifrom(27);
                    renderObject.diffusionUnifrom(25);
                }
                else if(_program != _program_shadow_map_L)
                {
                    if (countLightObj > 0)
                    {
                        primaryLightObject.PositionLightUniform(18);
                        primaryLightObject.IntensityLightVectorUniform(24);
                        primaryLightObject.IntensityAmbient(26);
                        primaryLightObject.IntensityMirror(28);
                        renderObject.mirrorUnifrom(29);
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
                RenderFigure(renderObject, PolygonMode.Fill);
                Vector4 color = renderObject.geometricInfo.ColorObj;
                GL.Uniform4(19, ref color);
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
                Vector4 color = primarySphereAt.geometricInfo.ColorObj;
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
            Text = defaultTitle + $" (Vsync: {glControlThesis3D.VSync})";
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
            DlgAddEditAnFigure dlgNewAn = new DlgAddEditAnFigure();
            if (dlgNewAn.ShowDialog() == DialogResult.OK)
            {
                Vertex[] figure_vertex = dlgNewAn.figureVertex;
                _renderObjects.Add(new RenderObject(figure_vertex, dlgNewAn.geometricInfo, RandomColor()));
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
                Vector4 CurrentPosition = _renderObjects[_SelectID].getPositionRenderObject();
                var typeObject = _renderObjects[_SelectID].TypeObject;
                Vector3 diff = _renderObjects[_SelectID].getDiffusion();
                Vector3 ambient = _renderObjects[_SelectID].getAmbient();
                Vector3 mirror = _renderObjects[_SelectID].getMirror();
                if (typeObject == TypeObjectRenderLight.LightSourceObject)
                {
                    var lightObject = _lightObjects.Where(x => x.ColorСhoice == _renderObjects[_SelectID].ColorСhoice).FirstOrDefault();
                    diff = lightObject.DiffusionIntensity;
                    ambient = lightObject.Ambient;
                    mirror = lightObject.Mirror;
                }
                int changeNonAnalitik = -1;
                var typeFigureChange = _renderObjects[_SelectID].geometricInfo.typeObjectCreate;
                if (typeFigureChange != TypeObjectCreate.NonTypeObject)
                {
                    Form dlgChangeTypeFigure = new Form()
                    {
                        Text = "Изменение объекта",
                        Width = 420,
                        Height = 130,
                        MinimumSize = new Size(420, 130),
                        FormBorderStyle = FormBorderStyle.Sizable,
                        StartPosition = FormStartPosition.CenterScreen,
                    };
                    Label labelText = new Label() { Text = "Данная фигура аналитическая. Изменить её структуру фигуры как аналитическу?", Anchor = AnchorStyles.Left | AnchorStyles.Top, Width = 420, Height = 30, Top = 20, Left = 20 };
                    Button buttonYes = new Button() { Text = "Да" , Top = 60, Left = 190, Width = 100, Height = 25, Anchor = AnchorStyles.Bottom | AnchorStyles.Right, DialogResult = DialogResult.Yes };
                    Button buttonNo  = new Button() { Text = "Нет", Top = 60, Left = 290, Width = 100, Height = 25, Anchor = AnchorStyles.Bottom | AnchorStyles.Right, DialogResult = DialogResult.No  };
                    dlgChangeTypeFigure.Controls.Add(labelText);
                    dlgChangeTypeFigure.Controls.Add(buttonYes);
                    dlgChangeTypeFigure.Controls.Add(buttonNo);
                    DialogResult dialogResult = dlgChangeTypeFigure.ShowDialog();
                    changeNonAnalitik = dialogResult == DialogResult.Cancel ? -1 : dialogResult == DialogResult.Yes ? 1 : 0;
                }
                if (changeNonAnalitik == 0)
                {
                    Form dlgChangeFigure = new Form()
                    {
                        Text = "Изменение объекта",
                        Width = 680,
                        Height = 650,
                        FormBorderStyle = FormBorderStyle.Sizable,
                        StartPosition = FormStartPosition.CenterScreen,
                    };
                    Label lblDiffs = new Label() { Text = typeObject == TypeObjectRenderLight.LightSourceObject ? "Интенсивность освещения:" : "Коэффициенты рассеивание:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 145, Height = 30, Top = 380, Left = 190 };
                    Label lblDiffR = new Label() { Text = "R:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 385, Left = 315 };
                    Label lblDiffG = new Label() { Text = "G:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 385, Left = 375 };
                    Label lblDiffB = new Label() { Text = "B:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 385, Left = 435 };
                    TextBox textBoxDiffR = new TextBox() { Text = diff.X.ToString(), Width = 40, Height = 30, Top = 383, Left = 335, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    TextBox textBoxDiffG = new TextBox() { Text = diff.Y.ToString(), Width = 40, Height = 30, Top = 383, Left = 395, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    TextBox textBoxDiffB = new TextBox() { Text = diff.Z.ToString(), Width = 40, Height = 30, Top = 383, Left = 455, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    Label lblAmbients = new Label() { Text = typeObject == TypeObjectRenderLight.LightSourceObject ? "Интенсивность фонового света:" : "Коэффициенты отражения фоного света:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 145, Height = 30, Top = 417, Left = 190 };
                    Label lblAmbientR = new Label() { Text = "R:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 423, Left = 315 };
                    Label lblAmbientG = new Label() { Text = "G:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 423, Left = 375 };
                    Label lblAmbientB = new Label() { Text = "B:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 423, Left = 435 };
                    TextBox textBoxAmbientR = new TextBox() { Text = ambient.X.ToString(), Width = 40, Height = 30, Top = 421, Left = 335, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    TextBox textBoxAmbientG = new TextBox() { Text = ambient.Y.ToString(), Width = 40, Height = 30, Top = 421, Left = 395, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    TextBox textBoxAmbientB = new TextBox() { Text = ambient.Z.ToString(), Width = 40, Height = 30, Top = 421, Left = 455, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    Label lblMirrors = new Label() { Text = typeObject == TypeObjectRenderLight.LightSourceObject ? "Интенсивность отраженного света:" : "Коэффициент зеркального отражения:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 145, Height = 30, Top = 452, Left = 190 };
                    Label lblMirrorR = new Label() { Text = "R:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 458, Left = 315 };
                    Label lblMirrorG = new Label() { Text = "G:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 458, Left = 375 };
                    Label lblMirrorB = new Label() { Text = "B:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 458, Left = 435 };
                    TextBox textBoxMirrorR = new TextBox() { Text = mirror.X.ToString(), Width = 40, Height = 30, Top = 456, Left = 335, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    TextBox textBoxMirrorG = new TextBox() { Text = mirror.Y.ToString(), Width = 40, Height = 30, Top = 456, Left = 395, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    TextBox textBoxMirrorB = new TextBox() { Text = mirror.Z.ToString(), Width = 40, Height = 30, Top = 456, Left = 455, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    Color4 colorSurface = new Color4();
                    colorSurface.R = _renderObjects[_SelectID].geometricInfo.ColorObj.X;
                    colorSurface.G = _renderObjects[_SelectID].geometricInfo.ColorObj.Y;
                    colorSurface.B = _renderObjects[_SelectID].geometricInfo.ColorObj.Z;
                    colorSurface.A = _renderObjects[_SelectID].geometricInfo.ColorObj.W;
                    ColorDialog colorDialog = new ColorDialog();
                    Label lblButtonColor = new Label() { Text = "Цвет объекта:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 170, Height = 30, Top = 423, Left = 20 };
                    Button buttonColor = new Button() { Text = "", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 70, Height = 30, Top = 415, Left = 100, BackColor = (System.Drawing.Color)colorSurface };
                    buttonColor.Click += (sender1, e1) => { if (colorDialog.ShowDialog() == DialogResult.OK) { colorSurface = buttonColor.BackColor = colorDialog.Color; } };
                    Label lblCoords = new Label() { Text = "Смещение по осям:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 120, Height = 30, Top = 528, Left = 20 };
                    Label lblCoordX = new Label() { Text = "X:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 528, Left = 150 };
                    Label lblCoordY = new Label() { Text = "Y:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 528, Left = 210 };
                    Label lblCoordZ = new Label() { Text = "Z:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 528, Left = 270 };
                    TextBox textBoxCoordX = new TextBox() { Text = "0", Width = 40, Height = 30, Top = 525, Left = 170, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    TextBox textBoxCoordY = new TextBox() { Text = "0", Width = 40, Height = 30, Top = 525, Left = 230, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    TextBox textBoxCoordZ = new TextBox() { Text = "0", Width = 40, Height = 30, Top = 525, Left = 290, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    Label lblAngles = new Label() { Text = "Вращение по осям:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 110, Height = 30, Top = 528, Left = 335 };
                    Label lblAngleX = new Label() { Text = "X:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 528, Left = 450 };
                    Label lblAngleY = new Label() { Text = "Y:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 528, Left = 510 };
                    Label lblAngleZ = new Label() { Text = "Z:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 528, Left = 570 };
                    TextBox textBoxAngleX = new TextBox() { Text = "0", Width = 40, Height = 30, Top = 525, Left = 470, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    TextBox textBoxAngleY = new TextBox() { Text = "0", Width = 40, Height = 30, Top = 525, Left = 530, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    TextBox textBoxAngleZ = new TextBox() { Text = "0", Width = 40, Height = 30, Top = 525, Left = 590, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    Label lblCurrentPosition = new Label() { Text = "Текущая позиция:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 110, Height = 30, Top = 500, Left = 20  };
                    Label lblCurrentPositionX = new Label() { Text = "X:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 500, Left = 150 };
                    Label lblCurrentPositionY = new Label() { Text = "Y:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 500, Left = 210 };
                    Label lblCurrentPositionZ = new Label() { Text = "Z:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 20, Height = 30, Top = 500, Left = 270 };
                    TextBox textBoxCurrentPositionX = new TextBox() { Enabled = false, Text = CurrentPosition.X.ToString(), Width = 40, Height = 30, Top = 498, Left = 170, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    TextBox textBoxCurrentPositionY = new TextBox() { Enabled = false, Text = CurrentPosition.Y.ToString(), Width = 40, Height = 30, Top = 498, Left = 230, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    TextBox textBoxCurrentPositionZ = new TextBox() { Enabled = false, Text = CurrentPosition.Z.ToString(), Width = 40, Height = 30, Top = 498, Left = 290, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    Label lblTrackBar = new Label() { Text = "Прозрачность объекта:", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 170, Height = 30, Top = 555, Left = 20 };
                    TrackBar trackBar = new TrackBar() { Value = (int)(_renderObjects[_SelectID].geometricInfo.ColorObj.W * 10f), Minimum = 0, Maximum = 10, Anchor = AnchorStyles.Left | AnchorStyles.Bottom, Width = 170, Height = 10, Top = 555, Left = 150 };
                    CheckBox checkBoxChangeStruct = new CheckBox() { Checked = false, Text = "Изменить структуру фигуры", Width = 170, Height = 30, Top = 375, Left = 20, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
                    TextBox textBoxChangeCoord = new TextBox() { Enabled = false, Multiline = true, Width = 250, Height = 350, Top = 10, Left = 10, Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom, ScrollBars = ScrollBars.Vertical };
                    TextBox textBoxChangeFinit = new TextBox() { Enabled = false, Multiline = true, Width = 250, Height = 350, Top = 10, Left = 10, Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom, ScrollBars = ScrollBars.Vertical };
                    SplitContainer splitterText = new SplitContainer() { Width = 540, Height = 360, Left = 10, Top = 10, Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom, SplitterDistance = 260 };
                    Button buttonSave = new Button() { Text = "Save", Top = 20, Left = 550, Width = 100, Height = 25, Anchor = AnchorStyles.Top | AnchorStyles.Right };
                    SaveFileDialog saveFileCoord = new SaveFileDialog();
                    SaveFileDialog saveFileFinit = new SaveFileDialog();
                    checkBoxChangeStruct.CheckedChanged += (sender1, e1) => { textBoxChangeCoord.Enabled = textBoxChangeFinit.Enabled = checkBoxChangeStruct.Checked; };
                    buttonSave.Click += (sender1, e1) => { saveFileCoord.ShowDialog(); saveFileFinit.ShowDialog(); };
                    checkBoxChangeStruct.CheckedChanged += (sender1, e1) =>
                        {
                        if(checkBoxChangeStruct.Checked && typeFigureChange != TypeObjectCreate.NonTypeObject)
                        {
                            MessageBox.Show("Данная фигура аналитическая, при изменении её структуры она перестанет быть аналитической фигуры");
                        }
                    };
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
                    dlgChangeFigure.Controls.Add(checkBoxChangeStruct);
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
                    dlgChangeFigure.Controls.Add(lblCurrentPosition);
                    dlgChangeFigure.Controls.Add(textBoxCurrentPositionX);
                    dlgChangeFigure.Controls.Add(textBoxCurrentPositionY);
                    dlgChangeFigure.Controls.Add(textBoxCurrentPositionZ);
                    dlgChangeFigure.Controls.Add(lblCurrentPositionX);
                    dlgChangeFigure.Controls.Add(lblCurrentPositionY);
                    dlgChangeFigure.Controls.Add(lblCurrentPositionZ);
                    dlgChangeFigure.Controls.Add(lblAmbients);
                    dlgChangeFigure.Controls.Add(lblAmbientR);
                    dlgChangeFigure.Controls.Add(lblAmbientG);
                    dlgChangeFigure.Controls.Add(lblAmbientB);
                    dlgChangeFigure.Controls.Add(textBoxAmbientR);
                    dlgChangeFigure.Controls.Add(textBoxAmbientG);
                    dlgChangeFigure.Controls.Add(textBoxAmbientB);
                    dlgChangeFigure.Controls.Add(lblMirrors);
                    dlgChangeFigure.Controls.Add(lblMirrorR);
                    dlgChangeFigure.Controls.Add(lblMirrorG);
                    dlgChangeFigure.Controls.Add(lblMirrorB);
                    dlgChangeFigure.Controls.Add(textBoxMirrorR);
                    dlgChangeFigure.Controls.Add(textBoxMirrorG);
                    dlgChangeFigure.Controls.Add(textBoxMirrorB);
                    dlgChangeFigure.Controls.Add(lblAngles);
                    dlgChangeFigure.Controls.Add(lblAngleX);
                    dlgChangeFigure.Controls.Add(lblAngleY);
                    dlgChangeFigure.Controls.Add(lblAngleZ);
                    dlgChangeFigure.Controls.Add(textBoxAngleX);
                    dlgChangeFigure.Controls.Add(textBoxAngleY);
                    dlgChangeFigure.Controls.Add(textBoxAngleZ);

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
                        if (checkBoxChangeStruct.Checked)
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
                                _renderObjects[_SelectID].geometricInfo.typeObjectCreate = TypeObjectCreate.NonTypeObject;
                            }
                            else
                            {
                                MessageBox.Show("Ошибка во формате входных данных", "Ошибка");
                            }
                        }
                        _renderObjects[_SelectID].geometricInfo.ColorObj = new Vector4(colorSurface.R, colorSurface.G, colorSurface.B, trackBar.Value / 10f);
                        _renderObjects[_SelectID].changeModelMstrix(new Vector3(float.Parse(textBoxCoordX.Text), float.Parse(textBoxCoordY.Text), float.Parse(textBoxCoordZ.Text)));
                        _renderObjects[_SelectID].changeRotateMstrix(new Vector3(float.Parse(textBoxAngleX.Text), float.Parse(textBoxAngleY.Text), float.Parse(textBoxAngleZ.Text)));
                        if (typeObject == TypeObjectRenderLight.LightSourceObject)
                        {
                            var lightObject = _lightObjects.Where(x => x.ColorСhoice == _renderObjects[_SelectID].ColorСhoice).FirstOrDefault();
                            lightObject.DiffusionIntensity = new Vector3(float.Parse(textBoxDiffR.Text), float.Parse(textBoxDiffG.Text), float.Parse(textBoxDiffB.Text));
                            lightObject.Ambient = new Vector3(float.Parse(textBoxAmbientR.Text), float.Parse(textBoxAmbientG.Text), float.Parse(textBoxAmbientB.Text));
                            lightObject.Mirror = new Vector3(float.Parse(textBoxMirrorR.Text), float.Parse(textBoxMirrorG.Text), float.Parse(textBoxMirrorB.Text));
                            if (lightObject != null)
                            {
                                lightObject.SetPositionLight(_renderObjects[_SelectID].ModelMatrix);
                                if (_program_Fong_directed != -1 && lightObject.uboLightInfo != -1) lightObject.UpdatePositionForBlock(_program_Fong_directed);
                            }
                        }
                        else
                        {
                            _renderObjects[_SelectID].setDiffusion(new Vector3(float.Parse(textBoxDiffR.Text), float.Parse(textBoxDiffG.Text), float.Parse(textBoxDiffB.Text)));
                            _renderObjects[_SelectID].setAmbient(new Vector3(float.Parse(textBoxAmbientR.Text), float.Parse(textBoxAmbientG.Text), float.Parse(textBoxAmbientB.Text)));
                            _renderObjects[_SelectID].setMirror(new Vector3(float.Parse(textBoxMirrorR.Text), float.Parse(textBoxMirrorG.Text), float.Parse(textBoxMirrorB.Text)));
                        }
                    }
                }
                else if(changeNonAnalitik == 1)
                {
                    Vector3 position = _renderObjects[_SelectID].getStartPosition();
                    DlgAddEditAnFigure dlgNewAn = new DlgAddEditAnFigure(_renderObjects[_SelectID].geometricInfo);
                    dlgNewAn.SetColor(new Color4(_renderObjects[_SelectID].geometricInfo.ColorObj.X, _renderObjects[_SelectID].geometricInfo.ColorObj.Y, _renderObjects[_SelectID].geometricInfo.ColorObj.Z, _renderObjects[_SelectID].geometricInfo.ColorObj.W));
                    if (dlgNewAn.ShowDialog() == DialogResult.OK)
                    {
                        Vertex[] figure_vertex = dlgNewAn.figureVertex;
                        _renderObjects[_SelectID].WriteBuffer(figure_vertex);
                        _renderObjects[_SelectID].geometricInfo = dlgNewAn.geometricInfo;
                    }
                }
            }
            else
            {
                MessageBox.Show("Этот объект нельзя изменить");
            }
            ResetBufferPointShadowns();
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
                ResetBufferPointShadowns();
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
