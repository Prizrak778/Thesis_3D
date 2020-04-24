﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace Thesis_3D
{
    public enum TypeObjectRender
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
    public class RenderObject : IDisposable
    {
        private bool _initialized;
        private int _vertexArray;
        private int _buffer; //Буффер в котором хранится объект
        private int ssbo; //Буффер для плоских теней
        private int _verticeCount;
        private PolygonMode _polygon;
        private Vector4 diffusion = Vector4.One;
        public TypeObjectRender TypeObject = TypeObjectRender.SimpleObject;
        public Matrix4 ModelMatrix = Matrix4.CreateTranslation(0, 0, 0);
        public Vector4 Color_obj; //Цвет объекта
        public Vector4 Color_choice; //Цвет объекта для буффера выбора
        public RenderObject(Vertex[] vertices, Color4 color, Color4 color_choice, TypeObjectRender typeObject = TypeObjectRender.SimpleObject, bool plane = false)
        {
            _verticeCount = vertices.Length;
            _vertexArray = GL.GenVertexArray();
            TypeObject = typeObject;
            GL.GenBuffers(1, out _buffer);
            //PolygonMode.Line
            GL.BindVertexArray(_vertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _buffer);
            GL.NamedBufferStorage(_buffer, Vertex.Size * _verticeCount,          // the size needed by this buffer
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
            if (plane) bufferProjectionShadow(vertices);
        }
        public void Bind()//Сохранение буфера для дальнейшей отрисовки
        {
            GL.BindVertexArray(_vertexArray);
        }
        public int RenderBuffer()//вернуть номер буффера
        {
            return _buffer;
        }
        public int ShadowProjectBuffer()//вернуть номер буффера
        {
            return ssbo;
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
            return _verticeCount;
        }
        public void ReadBuffer(Vertex[] vertices)
        {
            GL.GetNamedBufferSubData(_buffer, IntPtr.Zero, Vertex.Size * BufferSize(), vertices);
        }
        public void WriteBuffer(Vertex[] vertices)
        {
            GL.DeleteVertexArray(_vertexArray);
            GL.DeleteBuffer(_buffer);
            _verticeCount = vertices.Length;
            _vertexArray = GL.GenVertexArray();
            GL.GenBuffers(1, out _buffer);

            GL.BindVertexArray(_vertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _buffer);
            GL.NamedBufferStorage(_buffer, Vertex.Size * vertices.Length,        // the size needed by this buffer
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
        }
        public Vector4 getDiffusion()
        {
            return diffusion;
        }

        public void setDiffusion(Vector4 value)
        {
            diffusion = new Vector4(Math.Abs(value.X * 10) / 10, Math.Abs(value.Y * 10) / 10, Math.Abs(value.Z * 10) / 10, 1f);
        }
        public void diffusionUnifrom(int location)
        {
            GL.Uniform4(location, diffusion);
        }
    }
}
