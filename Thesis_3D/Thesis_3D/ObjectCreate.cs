using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Thesis_3D
{
    public struct Vertex
    {
        public const int Size = (4 + 4 + 2) * 4; // size of struct in bytes

        public Vector4 _Position;
        private Vector4 _NormalCoord;
        private Vector2 _TexCoord;

        public Vertex(Vector4 position, Vector4 normal, Vector2 texcoord)
        {
            _Position = position;
            _NormalCoord = normal;
            _TexCoord = texcoord;
        }
    }
    class ObjectCreate
    {
        public static Vertex[] CreateSolidCube(float side, float shift_lr, float shift_y, float shift_ud)//размер куба, смещение y/-y, смещение x/-x,цвет
        {
            side = side / 2f;
            Vertex[] vertices =
            {
                //координаты для треуголника, нормаль, текстурные координаты
                //на каждый куб по 12 треугольников(36 точек)
                
                new Vertex(new Vector4(-side+shift_lr, -side+shift_y, -side+shift_ud, 1.0f), new Vector4(-1.0f, 0f, 0f, 0.0f), new Vector2(0, 0)),
                new Vertex(new Vector4(-side+shift_lr,  side+shift_y, -side+shift_ud, 1.0f), new Vector4(-1.0f, 0f, 0f, 0.0f), new Vector2(0, 1)),
                new Vertex(new Vector4(-side+shift_lr, -side+shift_y,  side+shift_ud, 1.0f), new Vector4(-1.0f, 0f, 0f, 0.0f), new Vector2(1, 0)),

                new Vertex(new Vector4(-side+shift_lr, -side+shift_y,  side+shift_ud, 1.0f), new Vector4(-1.0f, 0f, 0f, 0.0f), new Vector2(1, 0)),
                new Vertex(new Vector4(-side+shift_lr,  side+shift_y, -side+shift_ud, 1.0f), new Vector4(-1.0f, 0f, 0f, 0.0f), new Vector2(0, 1)),
                new Vertex(new Vector4(-side+shift_lr,  side+shift_y,  side+shift_ud, 1.0f), new Vector4(-1.0f, 0f, 0f, 0.0f), new Vector2(1, 1)),

                //+x
                new Vertex(new Vector4( side+shift_lr, -side+shift_y, -side+shift_ud, 1.0f), new Vector4( 1.0f, 0f, 0f, 0.0f), new Vector2(0, 0)),
                new Vertex(new Vector4( side+shift_lr,  side+shift_y, -side+shift_ud, 1.0f), new Vector4( 1.0f, 0f, 0f, 0.0f), new Vector2(0, 1)),
                new Vertex(new Vector4( side+shift_lr, -side+shift_y,  side+shift_ud, 1.0f), new Vector4( 1.0f, 0f, 0f, 0.0f), new Vector2(1, 0)),

                new Vertex(new Vector4( side+shift_lr, -side+shift_y,  side+shift_ud, 1.0f), new Vector4( 1.0f, 0f, 0f, 0.0f), new Vector2(1, 0)),
                new Vertex(new Vector4( side+shift_lr,  side+shift_y, -side+shift_ud, 1.0f), new Vector4( 1.0f, 0f, 0f, 0.0f), new Vector2(0, 1)),
                new Vertex(new Vector4( side+shift_lr,  side+shift_y,  side+shift_ud, 1.0f), new Vector4( 1.0f, 0f, 0f, 0.0f), new Vector2(1, 1)),

                //-z
                new Vertex(new Vector4(-side+shift_lr, -side+shift_y, -side+shift_ud, 1.0f), new Vector4(0f, -1.0f, 0f, 0.0f), new Vector2(0, 0)),
                new Vertex(new Vector4( side+shift_lr, -side+shift_y, -side+shift_ud, 1.0f), new Vector4(0f, -1.0f, 0f, 0.0f), new Vector2(1, 0)),
                new Vertex(new Vector4(-side+shift_lr, -side+shift_y,  side+shift_ud, 1.0f), new Vector4(0f, -1.0f, 0f, 0.0f), new Vector2(0, 1)),

                new Vertex(new Vector4(-side+shift_lr, -side+shift_y,  side+shift_ud, 1.0f), new Vector4(0f, -1.0f, 0f, 0.0f), new Vector2(0, 1)),
                new Vertex(new Vector4( side+shift_lr, -side+shift_y, -side+shift_ud, 1.0f), new Vector4(0f, -1.0f, 0f, 0.0f), new Vector2(1, 0)),
                new Vertex(new Vector4( side+shift_lr, -side+shift_y,  side+shift_ud, 1.0f), new Vector4(0f, -1.0f, 0f, 0.0f), new Vector2(1, 1)),

                //z+
                new Vertex(new Vector4(-side+shift_lr,  side+shift_y, -side+shift_ud, 1.0f), new Vector4(0f,  1.0f,  0f, 0.0f), new Vector2(0, 0)),
                new Vertex(new Vector4(-side+shift_lr,  side+shift_y,  side+shift_ud, 1.0f), new Vector4(0f,  1.0f,  0f, 0.0f), new Vector2(0, 1)),
                new Vertex(new Vector4( side+shift_lr,  side+shift_y, -side+shift_ud, 1.0f), new Vector4(0f,  1.0f,  0f, 0.0f), new Vector2(1, 0)),

                new Vertex(new Vector4( side+shift_lr,  side+shift_y, -side+shift_ud, 1.0f), new Vector4(0f,  1.0f,  0f, 0.0f), new Vector2(1, 0)),
                new Vertex(new Vector4(-side+shift_lr,  side+shift_y,  side+shift_ud, 1.0f), new Vector4(0f,  1.0f,  0f, 0.0f), new Vector2(0, 1)),
                new Vertex(new Vector4( side+shift_lr,  side+shift_y,  side+shift_ud, 1.0f), new Vector4(0f,  1.0f,  0f, 0.0f), new Vector2(1, 1)),

                //y-
                new Vertex(new Vector4(-side+shift_lr, -side+shift_y, -side+shift_ud, 1.0f), new Vector4(0f, 0f, -1.0f, 0.0f), new Vector2(0, 0)),
                new Vertex(new Vector4(-side+shift_lr,  side+shift_y, -side+shift_ud, 1.0f), new Vector4(0f, 0f, -1.0f, 0.0f), new Vector2(0, 1)),
                new Vertex(new Vector4( side+shift_lr, -side+shift_y, -side+shift_ud, 1.0f), new Vector4(0f, 0f, -1.0f, 0.0f), new Vector2(1, 0)),

                new Vertex(new Vector4( side+shift_lr, -side+shift_y, -side+shift_ud, 1.0f), new Vector4(0f, 0f, -1.0f, 0.0f), new Vector2(1, 0)),
                new Vertex(new Vector4(-side+shift_lr,  side+shift_y, -side+shift_ud, 1.0f), new Vector4(0f, 0f, -1.0f, 0.0f), new Vector2(0, 1)),
                new Vertex(new Vector4( side+shift_lr,  side+shift_y, -side+shift_ud, 1.0f), new Vector4(0f, 0f, -1.0f, 0.0f), new Vector2(1, 1)),

                //y+
                new Vertex(new Vector4(-side+shift_lr, -side+shift_y,  side+shift_ud, 1.0f), new Vector4(0f, 0f,  1.0f, 0.0f), new Vector2(0, 0)),
                new Vertex(new Vector4( side+shift_lr, -side+shift_y,  side+shift_ud, 1.0f), new Vector4(0f, 0f,  1.0f, 0.0f), new Vector2(1, 0)),
                new Vertex(new Vector4(-side+shift_lr,  side+shift_y,  side+shift_ud, 1.0f), new Vector4(0f, 0f,  1.0f, 0.0f), new Vector2(0, 1)),

                new Vertex(new Vector4(-side+shift_lr,  side+shift_y,  side+shift_ud, 1.0f), new Vector4(0f, 0f,  1.0f, 0.0f), new Vector2(0, 1)),
                new Vertex(new Vector4( side+shift_lr, -side+shift_y,  side+shift_ud, 1.0f), new Vector4(0f, 0f,  1.0f, 0.0f), new Vector2(1, 0)),
                new Vertex(new Vector4( side+shift_lr,  side+shift_y,  side+shift_ud, 1.0f), new Vector4(0f, 0f,  1.0f, 0.0f), new Vector2(1, 1)),
            };
            return vertices;
        }
    }
}
