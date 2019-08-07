using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace Thesis_3D
{
    public class RenderObject : IDisposable
    {
        private bool _initialized;
        private int _vertexArray;
        private int _buffer; //Буффер в котором хранится объект
        private int _verticeCount;
        private PolygonMode _polygon;
        public Matrix4 ModelMatrix = Matrix4.CreateTranslation(0, 0, 0);
        public Vector4 Color_obj; //Цвет объекта
        public Vector4 Color_choice; //Цвет объекта для буффера выбора
        public RenderObject(Vertex[] vertices, Color4 color, Color4 color_choice)
        {
            _verticeCount = vertices.Length;
            _vertexArray = GL.GenVertexArray();
            GL.GenBuffers(1, out _buffer);
            //PolygonMode.Line
            GL.BindVertexArray(_vertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _buffer);
            GL.NamedBufferStorage(_buffer, Vertex.Size * _verticeCount,        // the size needed by this buffer
                vertices,                                                        // data to initialize with
                BufferStorageFlags.MapWriteBit);                                 // at this point we will only write to the buffer
                                                                                 // create vertex array and buffer here

            GL.VertexArrayAttribBinding(_vertexArray, 1, 0);
            GL.EnableVertexArrayAttrib(_vertexArray, 1);
            GL.VertexArrayAttribFormat(
                _vertexArray,
                1,                                                               // attribute index, from the shader location = 0
                4,                                                               // size of attribute, vec4
                VertexAttribType.Float,                                          // contains floats
                false,                                                           // does not need to be normalized as it is already, floats ignore this flag anyway
                0);                                                              // relative offset, first item

            GL.VertexArrayAttribBinding(_vertexArray, 2, 0);
            GL.EnableVertexArrayAttrib(_vertexArray, 2);
            GL.VertexArrayAttribFormat(
                _vertexArray,
                2,                                                               // attribute index, from the shader location = 1
                4,                                                               // size of attribute, vec4
                VertexAttribType.Float,                                          // contains floats
                false,                                                           // does not need to be normalized as it is already, floats ignore this flag anyway
                16);                                                             // relative offset after a vec4
            GL.VertexArrayAttribBinding(_vertexArray, 3, 0);
            GL.EnableVertexArrayAttrib(_vertexArray, 3);
            GL.VertexArrayAttribFormat(
                _vertexArray,
                3,                                                               // attribute index, from the shader location = 2
                4,                                                               // size of attribute, vec4
                VertexAttribType.Float,                                          // contains floats
                false,                                                           // does not need to be normalized as it is already, floats ignore this flag anyway
                32);                                                             // relative offset after a vec4 + vec4

            _initialized = true;
            GL.VertexArrayVertexBuffer(_vertexArray, 0, _buffer, IntPtr.Zero, Vertex.Size);
            Color_obj.X = color.R;
            Color_obj.Y = color.G;
            Color_obj.Z = color.B;
            Color_obj.W = color.A;
            Color_choice.X = color_choice.R;
            Color_choice.Y = color_choice.G;
            Color_choice.Z = color_choice.B;
            Color_choice.W = color_choice.A;
        }
        public void Bind()//Сохранение буфера для дальнейшей отрисовки
        {
            GL.BindVertexArray(_vertexArray);
        }
        public int RenderBuffer()//вернуть номер буффера
        {
            return _buffer;
        }
        public void Render()//Отрисовка(пока только треугольником)
        {
            GL.DrawArrays(PrimitiveType.Triangles, 0, _verticeCount);
        }
        public void PolygonMode_now(PolygonMode polygon)//тип полигона для отрисовки(Fill, Line, Point)
        {
            _polygon = polygon;
            GL.PolygonMode(MaterialFace.FrontAndBack, polygon);
        }
        public void Render_line()//Отрисовка(пока только треугольником)
        {
            //GL.DrawArrays(PrimitiveType.Lines, 0, _verticeCount);
            GL.DrawArrays(PrimitiveType.Triangles, 0, _verticeCount);
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
                if (_initialized)
                {
                    GL.DeleteVertexArray(_vertexArray);
                    GL.DeleteBuffer(_buffer);
                    _initialized = false;
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
    }
}
