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
        public Vertex(Vertex vertex)
        {
            _Position = vertex._Position;
            _NormalCoord = vertex._NormalCoord;
            _TexCoord = vertex._TexCoord;
        }
    }
    class ObjectCreate
    {
        public static Vertex[] CreateSolidCube(float side, float shift_lr, float shift_y, float shift_ud)//размер куба, смещение y/-y, смещение x/-x, смещение z/-z цвет
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

        public static Vertex[] CreatePlane(float side, float shift_lr, float shift_y, float shift_ud, int alpha_x = 90, int alpha_y = 90, int alpha_z = 90)
        {
            alpha_x %= 360;
            alpha_y %= 360;
            alpha_z %= 360;
            float angle_x = MathHelper.DegreesToRadians(alpha_x);
            float angle_y = MathHelper.DegreesToRadians(alpha_y);
            float angle_z = MathHelper.DegreesToRadians(alpha_z);
            Vector3 translation = new Vector3(shift_lr, shift_y, shift_ud);

            Matrix4 matrix4RX = Matrix4.Zero;
            Matrix4 matrix4RY = Matrix4.Zero;
            Matrix4 matrix4RZ = Matrix4.Zero;
            Matrix4.CreateRotationX(angle_x, out matrix4RX);
            Matrix4.CreateRotationY(angle_y, out matrix4RY);
            Matrix4.CreateRotationZ(angle_z, out matrix4RZ);
            Matrix4 transform = matrix4RX * matrix4RY * matrix4RZ;
            Vector4 tnorm = transform * new Vector4(-1.0f, -1.0f, 1.0f, 0.0f);
            List<Vertex> vertices = new List<Vertex>
            {
                new Vertex(new Vector4((transform * new Vector4( 0, 0, 0, 1.0f)).Xyz + translation, 1.0f), tnorm, new Vector2(0.5f, 0.5f)),
                new Vertex(new Vector4((transform * new Vector4( side + 0, 0, -side + 0, 1.0f)).Xyz + translation, 1.0f), tnorm, new Vector2(1f, 0f)),
                new Vertex(new Vector4((transform * new Vector4(-side + 0, 0, -side + 0, 1.0f)).Xyz + translation, 1.0f), tnorm, new Vector2(0, 0)),

                new Vertex(new Vector4((transform * new Vector4(-side + 0, 0,  side + 0, 1.0f)).Xyz + translation, 1.0f), tnorm, new Vector2(0f, 1f)),
                new Vertex(new Vector4((transform * new Vector4( 0, 0, 0, 1.0f)).Xyz + translation, 1.0f),  tnorm, new Vector2(0.5f, 0.5f)),
                new Vertex(new Vector4((transform * new Vector4(-side + 0, 0, -side + 0, 0.0f)).Xyz + translation, 1.0f),  tnorm, new Vector2(0, 0)),

                new Vertex(new Vector4((transform * new Vector4( side + 0, 0, -side + 0, 1.0f)).Xyz + translation, 1.0f),  tnorm, new Vector2(0f, 1f)),
                new Vertex(new Vector4((transform * new Vector4( 0, 0, 0, 1.0f)).Xyz + translation, 1.0f),  tnorm, new Vector2(0.5f, 0.5f)),
                new Vertex(new Vector4((transform * new Vector4( side + 0, 0,  side + 0, 1.0f)).Xyz + translation, 1.0f), tnorm, new Vector2(0, 0)),

                new Vertex(new Vector4((transform * new Vector4(-side + 0, 0,  side + 0, 0.0f)).Xyz + translation, 1.0f), tnorm, new Vector2(0f, 1f)),
                new Vertex(new Vector4((transform * new Vector4( side + 0, 0,  side + 0, 1.0f)).Xyz + translation, 1.0f),  tnorm, new Vector2(0, 0)),
                new Vertex(new Vector4((transform * new Vector4( 0, 0, 0, 1.0f)).Xyz + translation, 1.0f),  tnorm, new Vector2(0.5f, 0.5f)),
            };
            return vertices.ToArray();
        }

        public static Vertex[] CreateSphere(float side, float shift_lr, float shift_y, float shift_ud, int nx, int ny, float k1, float k2)
        {
            Vector4[] coord_s = new Vector4[(nx + 1) * 2 * ny];
            Vector2[] texcoord = new Vector2[(nx + 1) * 2 * ny];
            Vector4[] normal_s = new Vector4[(nx + 1) * 2 * ny];
            Spherecoord(coord_s, normal_s, texcoord, side / 2, nx, ny, k1, k2);
            Vertex[] vertices = new Vertex[(nx + 1) * 6 * ny];
            for (int i = 0; i < (nx + 1) * 2 * ny; i++)
            {
                coord_s[i].X += shift_lr;
                coord_s[i].Y += shift_y;
                coord_s[i].Z += shift_ud;
            }
            for (int i = 0, j = 0; j < (nx + 1) * 2 * ny - 3; i += 6, j += 2)
            {

                vertices[i] = new Vertex(coord_s[j], normal_s[j], texcoord[j]);
                vertices[i + 1] = new Vertex(coord_s[j + 2], normal_s[j + 2], texcoord[j + 2]);
                vertices[i + 2] = new Vertex(coord_s[j + 1], normal_s[j + 1], texcoord[j + 1]);
            }
            for (int i = 3, j = 1; j < (nx + 1) * 2 * ny - 3; i += 6, j += 2)
            {

                vertices[i] = new Vertex(coord_s[j], normal_s[j], texcoord[j]);
                vertices[i + 1] = new Vertex(coord_s[j + 1], normal_s[j + 1], texcoord[j + 1]);
                vertices[i + 2] = new Vertex(coord_s[j + 2], normal_s[j + 2], texcoord[j + 2]);
            }
            return vertices;
        }
        //side - растояние от цента куба до центра грани(в случае шара радиус)
        //shift_lr - смещение по оси х
        //shift_y - смещение по оси у
        //shift_ud - смещение по оси z
        //для шара nx - кол полигонов между полюсами, ny - кол полигонов в 
        //k1, k2 - коэф для получение эллиса, шара или диска
        //приемлимый шар получается если nx>16 и ny > 16 при небольшом радиусе
        private static void Spherecoord(Vector4[] coord_s, Vector4[] normal_s, Vector2[] texcoord, float r, int nx, int ny, float k1, float k2)
        {
            int ix, iy, i = 0;
            double x, y, z, sy, cy, sy1, cy1, sx, cx, piy, pix, ay, ay1, ax, tx, ty, ty1, dnx, dny, diy;
            dnx = 1.0 / (double)nx;
            dny = 1.0 / (double)ny;
            piy = Math.PI * dny;
            pix = Math.PI * dnx;
            for (iy = 0; iy < ny; iy++)
            {
                diy = (double)iy;
                ay = diy * piy;
                sy = Math.Sin(ay);
                cy = Math.Cos(ay);
                ty = diy * dny;
                ay1 = ay + piy;
                sy1 = Math.Sin(ay1);
                cy1 = Math.Cos(ay1);
                ty1 = ty + dny;
                for (ix = 0; ix <= nx; ix++)
                {
                    ax = 2.0 * ix * pix;
                    sx = k1 * Math.Sin(ax);
                    cx = k2 * Math.Cos(ax);
                    x = r * sy * cx;
                    y = r * sy * sx;
                    z = -r * cy;
                    tx = (double)ix * dnx;
                    // Координаты нормали в текущей вершине
                    normal_s[i] = new Vector4((float)x, (float)y, (float)z, 0.0f); // Нормаль направлена от центра
                    coord_s[i] = new Vector4((float)x, (float)y, (float)z, 1.0f);
                    texcoord[i] = new Vector2((float)tx, (float)ty);
                    i++;
                    // Координаты текстуры в текущей вершине
                    x = r * sy1 * cx;
                    y = r * sy1 * sx;
                    z = -r * cy1;
                    normal_s[i] = new Vector4((float)x, (float)y, (float)z, 0.0f);
                    coord_s[i] = new Vector4((float)x, (float)y, (float)z, 1.0f);
                    texcoord[i] = new Vector2((float)tx, (float)ty1);
                    i++;
                }
            }

        }
    }
}
