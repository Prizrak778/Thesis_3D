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
        private int _verticeLenght;
        private PolygonMode _polygon;
        public Vector4 Color4; //Цвет объекта
        public Vector4 Color_choice; //Цвет объекта для буффера выбора
        public RenderObject(Color4 color, Color4 color_choice)
        {
            Color4.X = color.R;
            Color4.Y = color.G;
            Color4.Z = color.B;
            Color4.W = color.A;
            Color_choice.X = color_choice.R;
            Color_choice.Y = color_choice.G;
            Color_choice.Z = color_choice.B;
            Color_choice.W = color_choice.A;
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
    }
}
