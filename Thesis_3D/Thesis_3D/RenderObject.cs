using System;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace Thesis_3D
{
    public enum TargetTrajectory
    {
        Point,
        Object
    };
    public enum TypeObjectRenderLight
    {
        SimpleObject,
        LightSourceObject,
        FlatObject
    };
    public struct Triangls
    {
        public Vector4[] point;
        public Triangls(Vector4 point_1, Vector4 point_2, Vector4 point_3)
        {
            point = new Vector4[3];
            point[0] = point_1;
            point[1] = point_2;
            point[2] = point_3;
        }
    }
    public struct PointUnik
    {
        public Vector4 point;
    }
    public class TrajctoryRenderObject
    {
        public bool useTrajectory;
        public TrajectoryFunctions trajectoryFunctionsX;
        public TrajectoryFunctions trajectoryFunctionsY;
        public TrajectoryFunctions trajectoryFunctionsZ;
        Vector4 colorObject;
        Vector4 point;
        public TargetTrajectory target;
        public TrajctoryRenderObject()
        {
            useTrajectory = false;
        }
        public TrajctoryRenderObject(TrajectoryFunctions locTrajectoryFunctionsX, TrajectoryFunctions locTrajectoryFunctionsY, TrajectoryFunctions locTrajectoryFunctionsZ, TargetTrajectory locTarget, Vector4 vectorTarget)
        {
            useTrajectory = true;
            trajectoryFunctionsX = locTrajectoryFunctionsX;
            trajectoryFunctionsY = locTrajectoryFunctionsY;
            trajectoryFunctionsZ = locTrajectoryFunctionsZ;
            target = locTarget;
            if (locTarget == TargetTrajectory.Object)
            {
                colorObject = vectorTarget;
            }
            else
            {
                point = vectorTarget;
            }
        }
        public void SetObject(Vector4 colorChoice)
        {
            colorObject = colorChoice;
        }
        public Vector4 GetObject()
        {
            return colorObject;
        }
        public void SetPoint(Vector4 vectorTarget)
        {
            point = vectorTarget;
        }
        public Vector4 GetPoint()
        {
            return point;
        }
        public Vector3 getValue()
        {
            if (useTrajectory)
            {
                float X = (float)(trajectoryFunctionsX.getValue());
                float Y = (float)(trajectoryFunctionsY.getValue());
                float Z = (float)(trajectoryFunctionsZ.getValue());
                return new Vector3(X, Y, Z);
            }
            return Vector3.Zero;
        }
    }

    public class RenderObject : IDisposable
    {
        //Геоментрическая информация 
        public float side; //Его расстояние от центра до границы
        private Vector3 _StartPosition = Vector3.Zero; //Его стартовое смещение
        public int colBreakX;
        public int colBreakY;
        public int coeffSX;
        public int coeffSY;
        public int angleX;
        public int angleY;
        public int angleZ;
        public TypeObjectCreate typeObjectCreate; //Тип созданного объекта
        public Vector4 ColorObj; //Цвет объекта

        //Состояние объекта
        private bool _Initialized;
        //Информация по буферам
        private int _VertexArray; //vaobj "имя" вершинного объекта
        private int _Buffer; //Буффер в котором хранится объект
        private int ssbo; //Буффер для плоских теней
        private int _VerticeCount; //Количество вершин

        //Информация для отрисовки
        private PolygonMode _Polygon;
        private Vector3 Diffusion = Vector3.One;
        private Vector3 Ambient = Vector3.One;
        public TypeObjectRenderLight TypeObject = TypeObjectRenderLight.SimpleObject;
        public TrajctoryRenderObject trajctoryRenderObject; //траектория движения объекта
        public Matrix4 ModelMatrix = Matrix4.CreateTranslation(0, 0, 0); //Матрица смещения от стартовой позиции
        public Vector4 ColorСhoice; //Цвет объекта для буффера выбора
        public RenderObject(Vertex[] vertices,  Vector3 startPosition, Color4 color, Color4 locСolorСhoice, TypeObjectRenderLight typeObject = TypeObjectRenderLight.SimpleObject, bool plane = false, float locSide = 1, TypeObjectCreate locTypeObjectCreate = TypeObjectCreate.SolidCube, int locColBreakX = 1, int locColBreakY = 1, int locCoeffSX = 1, int locCoeffSY = 1, int locAngleX = 0, int locAngleY = 0, int locAngleZ = 0)
        {
            colBreakX = locColBreakX;
            colBreakY = locColBreakY;
            coeffSX = locCoeffSX;
            coeffSY = locCoeffSY;
            angleX = locAngleX;
            angleY = locAngleY;
            angleZ = locAngleZ;
            trajctoryRenderObject = new TrajctoryRenderObject();
            _VerticeCount = vertices.Length;
            _VertexArray = GL.GenVertexArray();
            _StartPosition = startPosition;
            TypeObject = typeObject;
            side = locSide;
            typeObjectCreate = locTypeObjectCreate;
            GL.GenBuffers(1, out _Buffer);
            //PolygonMode.Line
            GL.BindVertexArray(_VertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _Buffer);
            GL.NamedBufferStorage(_Buffer, Vertex.Size * _VerticeCount,          // the size needed by this buffer
                vertices,                                                        // data to initialize with
                BufferStorageFlags.MapWriteBit);                                 // at this point we will only write to the buffer
                                                                                 // create vertex array and buffer here

            GL.VertexArrayAttribBinding(_VertexArray, 1, 0);
            GL.EnableVertexArrayAttrib(_VertexArray, 1);
            GL.VertexArrayAttribFormat(
                _VertexArray,
                1,                                                               // attribute index, from the shader location = 0
                4,                                                               // size of attribute, vec4
                VertexAttribType.Float,                                          // contains floats
                false,                                                           // does not need to be normalized as it is already, floats ignore this flag anyway
                0);                                                              // relative offset, first item

            GL.VertexArrayAttribBinding(_VertexArray, 2, 0);
            GL.EnableVertexArrayAttrib(_VertexArray, 2);
            GL.VertexArrayAttribFormat(
                _VertexArray,
                2,                                                               // attribute index, from the shader location = 1
                4,                                                               // size of attribute, vec4
                VertexAttribType.Float,                                          // contains floats
                false,                                                           // does not need to be normalized as it is already, floats ignore this flag anyway
                16);                                                             // relative offset after a vec4
            GL.VertexArrayAttribBinding(_VertexArray, 3, 0);
            GL.EnableVertexArrayAttrib(_VertexArray, 3);
            GL.VertexArrayAttribFormat(
                _VertexArray,
                3,                                                               // attribute index, from the shader location = 2
                4,                                                               // size of attribute, vec4
                VertexAttribType.Float,                                          // contains floats
                false,                                                           // does not need to be normalized as it is already, floats ignore this flag anyway
                32);                                                             // relative offset after a vec4 + vec4

            _Initialized = true;
            GL.VertexArrayVertexBuffer(_VertexArray, 0, _Buffer, IntPtr.Zero, Vertex.Size);
            ColorObj.X = color.R;
            ColorObj.Y = color.G;
            ColorObj.Z = color.B;
            ColorObj.W = color.A;
            ColorСhoice.X = locСolorСhoice.R;
            ColorСhoice.Y = locСolorСhoice.G;
            ColorСhoice.Z = locСolorСhoice.B;
            ColorСhoice.W = locСolorСhoice.A;
            if (plane) bufferProjectionShadow(vertices);
        }
        public void Bind()//Сохранение буфера для дальнейшей отрисовки
        {
            GL.BindVertexArray(_VertexArray);
        }
        public int RenderBuffer()//вернуть номер буффера
        {
            return _Buffer;
        }
        public int ShadowProjectBuffer()//вернуть номер буффера
        {
            return ssbo;
        }
        public void Render()//Отрисовка(пока только треугольником)
        {
            GL.DrawArrays(PrimitiveType.Triangles, 0, _VerticeCount);
        }
        public void PolygonMode_now(PolygonMode polygon)//тип полигона для отрисовки(Fill, Line, Point)
        {
            _Polygon = polygon;
            GL.PolygonMode(MaterialFace.FrontAndBack, polygon);
        }
        public void Render_line()//Отрисовка(пока только треугольником)
        {
            //GL.DrawArrays(PrimitiveType.Lines, 0, _verticeCount);
            GL.DrawArrays(PrimitiveType.Triangles, 0, _VerticeCount);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_Initialized)
                {
                    GL.DeleteVertexArray(_VertexArray);
                    GL.DeleteBuffer(_Buffer);
                    _Initialized = false;
                }
            }
        }
        public void changeModelMstrix(Vector3 tr)
        {
            Vector3 translation = ModelMatrix.ExtractTranslation();
            ModelMatrix.ClearTranslation();
            translation += tr;
            ModelMatrix = Matrix4.CreateTranslation(translation);
        }
        private void bufferProjectionShadow(Vertex[] vertices)
        {
            
            float[] temp = new float[vertices.Length * 4 + 4];
            temp[0] = vertices[0]._NormalCoord.X;
            temp[1] = vertices[0]._NormalCoord.Y;
            temp[2] = vertices[0]._NormalCoord.Z;
            temp[3] = vertices[0]._NormalCoord.W;
            for (int i = 0; i < vertices.Length; i++)
            {
                temp[4 * i + 4] = vertices[i]._Position.X;
                temp[4 * i + 5] = vertices[i]._Position.Y;
                temp[4 * i + 6] = vertices[i]._Position.Z;
                temp[4 * i + 7] = vertices[i]._Position.W;
            }
            

            GL.GenBuffers(1, out ssbo);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, ssbo);
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 5, ssbo);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, temp.Length * 4 + 4 * 4, temp, BufferUsageHint.DynamicDraw);
        }
        public int BufferSize()
        {
            return _VerticeCount;
        }
        public void ReadBuffer(Vertex[] vertices)
        {
            GL.GetNamedBufferSubData(_Buffer, IntPtr.Zero, Vertex.Size * BufferSize(), vertices);
        }
        public void WriteBuffer(Vertex[] vertices)
        {
            GL.DeleteVertexArray(_VertexArray);
            GL.DeleteBuffer(_Buffer);
            _VerticeCount = vertices.Length;
            _VertexArray = GL.GenVertexArray();
            GL.GenBuffers(1, out _Buffer);

            GL.BindVertexArray(_VertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _Buffer);
            GL.NamedBufferStorage(_Buffer, Vertex.Size * vertices.Length,        // the size needed by this buffer
                vertices,                                                        // data to initialize with
                BufferStorageFlags.MapWriteBit);                                 // at this point we will only write to the buffer
                                                                                 // create vertex array and buffer here

            GL.VertexArrayAttribBinding(_VertexArray, 1, 0);
            GL.EnableVertexArrayAttrib(_VertexArray, 1);
            GL.VertexArrayAttribFormat(
                _VertexArray,
                1,                                                               // attribute index, from the shader location = 0
                4,                                                               // size of attribute, vec4
                VertexAttribType.Float,                                          // contains floats
                false,                                                           // does not need to be normalized as it is already, floats ignore this flag anyway
                0);                                                              // relative offset, first item

            GL.VertexArrayAttribBinding(_VertexArray, 2, 0);
            GL.EnableVertexArrayAttrib(_VertexArray, 2);
            GL.VertexArrayAttribFormat(
                _VertexArray,
                2,                                                               // attribute index, from the shader location = 1
                4,                                                               // size of attribute, vec4
                VertexAttribType.Float,                                          // contains floats
                false,                                                           // does not need to be normalized as it is already, floats ignore this flag anyway
                16);                                                             // relative offset after a vec4
            GL.VertexArrayAttribBinding(_VertexArray, 3, 0);
            GL.EnableVertexArrayAttrib(_VertexArray, 3);
            GL.VertexArrayAttribFormat(
                _VertexArray,
                3,                                                               // attribute index, from the shader location = 2
                4,                                                               // size of attribute, vec4
                VertexAttribType.Float,                                          // contains floats
                false,                                                           // does not need to be normalized as it is already, floats ignore this flag anyway
                32);                                                             // relative offset after a vec4 + vec4

            _Initialized = true;
            GL.VertexArrayVertexBuffer(_VertexArray, 0, _Buffer, IntPtr.Zero, Vertex.Size);
        }
        public Vector3 getDiffusion()
        {
            return Diffusion;
        }

        public void setDiffusion(Vector3 value)
        {
            Diffusion = new Vector3(Math.Abs(value.X * 10) / 10, Math.Abs(value.Y * 10) / 10, Math.Abs(value.Z * 10) / 10);
        }
        public void diffusionUnifrom(int location)
        {
            GL.Uniform3(location, Diffusion);
        }
        public Vector3 getAmbient()
        {
            return Ambient;
        }
        public void setAmbient(Vector3 value)
        {
            Ambient = new Vector3(Math.Abs(value.X * 10) / 10, Math.Abs(value.Y * 10) / 10, Math.Abs(value.Z * 10) / 10);
        }
        public void ambientUnifrom(int location)
        {
            GL.Uniform3(location, Diffusion);
        }
        public Vector4 getPositionRenderObject()
        {
            Vector3 translation = ModelMatrix.ExtractTranslation();
            translation += _StartPosition;
            return new Vector4(translation, 1.0f);
        }
        public Vector3 getStartPosition()
        {
            return _StartPosition;
        }
    }
}
