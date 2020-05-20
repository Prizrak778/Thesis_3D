﻿using System;
using System.Collections.Generic;
using OpenTK;

namespace Thesis_3D
{
    public enum TypeObjectCreate
    {
        NonTypeObject,
        SolidCube,
        Plane,
        Sphere
    };
    public struct Vertex
    {
        public const int Size = (4 + 4 + 2) * 4; // size of struct in bytes

        public Vector4 _Position;
        public Vector4 _NormalCoord;
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
    public struct GeometricInfo
    {
        public static readonly GeometricInfo DefaultGeometricInfo = new GeometricInfo();
        //Его расстояние от центра до границы
        public float side;
        //Его стартовое смещение
        public Vector3 StartPosition { get; private set; }
        public int colBreakX;
        public int colBreakY;
        public int coeffSX;
        public int coeffSY;
        //Углы поворота по осямs
        public int angleX; 
        public int angleY;
        public int angleZ;
        //Тип созданного объекта
        public TypeObjectCreate typeObjectCreate;
        //Цвет объектаs
        public Vector4 ColorObj; 

        public GeometricInfo(GeometricInfo geometricInfo)
        {
            side = geometricInfo.side;
            StartPosition = geometricInfo.StartPosition;
            colBreakX = geometricInfo.colBreakX;
            colBreakY = geometricInfo.colBreakY;
            coeffSX = geometricInfo.coeffSX;
            coeffSY = geometricInfo.coeffSX;
            angleX = geometricInfo.angleX;
            angleY = geometricInfo.angleY;
            angleZ = geometricInfo.angleZ;
            typeObjectCreate = geometricInfo.typeObjectCreate;
            ColorObj = geometricInfo.ColorObj;
        }
        public GeometricInfo(Vector3 locStartPosition, Vector4 locColorObj, float locSide = 1.0f, int locColBreakX = 1, int locColBreakY = 1, int locCoeffSX = 1, int locCoeffSY = 1, int locAngleX = 0, int locAngleY = 0, int locAngleZ = 0, TypeObjectCreate locTypeObjectCreate = TypeObjectCreate.NonTypeObject)
        {
            side = locSide;
            StartPosition = locStartPosition;
            colBreakX = locColBreakX;
            colBreakY = locColBreakY;
            coeffSX = locCoeffSX;
            coeffSY = locCoeffSY;
            angleX = locAngleX;
            angleY = locAngleY;
            angleZ = locAngleZ;
            typeObjectCreate = locTypeObjectCreate;
            ColorObj = locColorObj;
        }
        public GeometricInfo(Vector4 locColorObj, float shiftx = 0, float shifty = 0, float shiftz = 0, float locSide = 1.0f, int locColBreakX = 1, int locColBreakY = 1, int locCoeffSX = 1, int locCoeffSY = 1, int locAngleX = 0, int locAngleY = 0, int locAngleZ = 0, TypeObjectCreate locTypeObjectCreate = TypeObjectCreate.NonTypeObject)
        {
            side = locSide;
            StartPosition = new Vector3(shiftx, shifty, shiftz);
            colBreakX = locColBreakX;
            colBreakY = locColBreakY;
            coeffSX = locCoeffSX;
            coeffSY = locCoeffSY;
            angleX = locAngleX;
            angleY = locAngleY;
            angleZ = locAngleZ;
            typeObjectCreate = locTypeObjectCreate;
            ColorObj = locColorObj;
        }
        public GeometricInfo(Vector3 locStartPosition, float colorR = 0, float colorG = 0, float colorB = 0, float colorA = 0,float locSide = 1.0f, int locColBreakX = 1, int locColBreakY = 1, int locCoeffSX = 1, int locCoeffSY = 1, int locAngleX = 0, int locAngleY = 0, int locAngleZ = 0, TypeObjectCreate locTypeObjectCreate = TypeObjectCreate.NonTypeObject)
        {
            side = locSide;
            StartPosition = locStartPosition;
            colBreakX = locColBreakX;
            colBreakY = locColBreakY;
            coeffSX = locCoeffSX;
            coeffSY = locCoeffSY;
            angleX = locAngleX;
            angleY = locAngleY;
            angleZ = locAngleZ;
            typeObjectCreate = locTypeObjectCreate;
            ColorObj = new Vector4(colorR, colorG, colorB, colorA);
        }
        public GeometricInfo(float colorR = 0, float colorG = 0, float colorB = 0, float colorA = 0, float shiftx = 0, float shifty = 0, float shiftz = 0, float locSide = 1.0f, int locColBreakX = 1, int locColBreakY = 1, int locCoeffSX = 1, int locCoeffSY = 1, int locAngleX = 0, int locAngleY = 0, int locAngleZ = 0, TypeObjectCreate locTypeObjectCreate = TypeObjectCreate.NonTypeObject)
        {
            side = locSide;
            StartPosition = new Vector3(shiftx, shifty, shiftz);
            colBreakX = locColBreakX;
            colBreakY = locColBreakY;
            coeffSX = locCoeffSX;
            coeffSY = locCoeffSY;
            angleX = locAngleX;
            angleY = locAngleY;
            angleZ = locAngleZ;
            typeObjectCreate = locTypeObjectCreate;
            ColorObj = new Vector4(colorR, colorG, colorB, colorA);
        }
    }
    class ObjectCreate
    {
        public Vertex[] vertices;
        public GeometricInfo geometricInfo;

        //Задел на будущее
        ObjectCreate()
        {
            Vector3 startPosition = Vector3.Zero;
            Vector4 colorObj = new Vector4(0, 0, 0, 1);
            geometricInfo = new GeometricInfo(startPosition, colorObj);
            vertices = CreateSolidCube(geometricInfo.side, startPosition);
        }
        ObjectCreate(GeometricInfo locGeometricInfo, Vertex[] locVertices)
        {
            geometricInfo = locGeometricInfo;
            vertices = locVertices;
        }
        ObjectCreate(GeometricInfo locGeometricInfo)
        {
            geometricInfo = locGeometricInfo;
            if(locGeometricInfo.typeObjectCreate == TypeObjectCreate.SolidCube)
                vertices = CreateSolidCube(locGeometricInfo);
            if (locGeometricInfo.typeObjectCreate == TypeObjectCreate.Sphere)
                vertices = CreateSphere(locGeometricInfo);
            if (locGeometricInfo.typeObjectCreate == TypeObjectCreate.Plane)
                vertices = CreatePlane(locGeometricInfo);
        }
        public static Vertex[] CreateSolidCube(GeometricInfo geometricInfo)//размер куба, смещение y/-y, смещение x/-x, смещение z/-z цвет
        {
            return CreateSolidCube(geometricInfo.side, geometricInfo.StartPosition.X, geometricInfo.StartPosition.Y, geometricInfo.StartPosition.Z, geometricInfo.angleX, geometricInfo.angleY, geometricInfo.angleZ);
        }
        public static Vertex[] CreateSolidCube(float side, Vector3 shift, int alphaX = 0, int alphaY = 0, int alphaZ = 0)//размер куба, смещение y/-y, смещение x/-x, смещение z/-z цвет
        {
            return CreateSolidCube(side, shift.X, shift.Y, shift.Z, alphaX, alphaY, alphaZ);
        }
        public static Vertex[] CreateSolidCube(float side, float shift_lr, float shift_y, float shift_ud, int alphaX = 0, int alphaY = 0, int alphaZ = 0)//размер куба, смещение y/-y, смещение x/-x, смещение z/-z цвет
        {
            alphaX %= 360;
            alphaY %= 360;
            alphaZ %= 360;
            float angle_x = MathHelper.DegreesToRadians(alphaX);
            float angle_y = MathHelper.DegreesToRadians(alphaY);
            float angle_z = MathHelper.DegreesToRadians(alphaZ);
            Vector3 translation = new Vector3(shift_lr, shift_y, shift_ud);

            Matrix4 matrix4RX = Matrix4.Zero;
            Matrix4 matrix4RY = Matrix4.Zero;
            Matrix4 matrix4RZ = Matrix4.Zero;
            Matrix4.CreateRotationX(angle_x, out matrix4RX);
            Matrix4.CreateRotationY(angle_y, out matrix4RY);
            Matrix4.CreateRotationZ(angle_z, out matrix4RZ);
            Matrix4 transform = matrix4RX * matrix4RY * matrix4RZ;

            //Чуть позже допишу задание с поворотом
            Vector3 tnormXYZ;
            CalcNormals(new Vector3((transform * new Vector4(0, 0, 0, 1.0f)).Xyz), new Vector3((transform * new Vector4(side + 0, 0, -side + 0, 1.0f)).Xyz), new Vector3((transform * new Vector4(-side + 0, 0, -side + 0, 1.0f)).Xyz), out tnormXYZ);

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
        public static Vertex[] CreatePlane(GeometricInfo geometricInfo)//размер куба, смещение y/-y, смещение x/-x, смещение z/-z цвет
        {
            return CreatePlane(geometricInfo.side, geometricInfo.StartPosition.X, geometricInfo.StartPosition.Y, geometricInfo.StartPosition.Z, geometricInfo.angleX, geometricInfo.angleY, geometricInfo.angleZ);
        }
        public static Vertex[] CreatePlane(float side, Vector3 shift, int alpha_x = 90, int alpha_y = 90, int alpha_z = 90)
        {
            return CreatePlane(side, shift.X, shift.Y, shift.Z, alpha_x, alpha_y, alpha_z);
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
            Vector3 tnormXYZ;
            CalcNormals(new Vector3((transform * new Vector4(0, 0, 0, 1.0f)).Xyz), new Vector3((transform * new Vector4(side + 0, 0, -side + 0, 1.0f)).Xyz), new Vector3((transform * new Vector4(-side + 0, 0, -side + 0, 1.0f)).Xyz), out tnormXYZ);
            Vector4 tnorm = new Vector4(tnormXYZ, 1.0f);
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
        public static Vertex[] CreateSphere(GeometricInfo geometricInfo)//размер куба, смещение y/-y, смещение x/-x, смещение z/-z цвет
        {
            return CreateSphere(geometricInfo.side, geometricInfo.StartPosition.X, geometricInfo.StartPosition.Y, geometricInfo.StartPosition.Z, geometricInfo.coeffSX, geometricInfo.coeffSY, geometricInfo.colBreakX, geometricInfo.colBreakY);
        }
        public static Vertex[] CreateSphere(float side, Vector3 shift, int nx, int ny, float k1, float k2)
        {
            return CreateSphere(side, shift.X, shift.Y, shift.Z, nx, ny, k1, k2);
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
        private static void CalcNormals(Vector3 a, Vector3 b, Vector3 c, out Vector3 n)
        {
            float wrki;
            Vector3 v1, v2;

            v1.X = a.X - b.X;
            v1.Y = a.Y - b.Y;
            v1.Z = a.Z - b.Z;

            v2.X = b.X - c.X;
            v2.Y = b.Y - c.Y;
            v2.Z = b.Z - c.Z;

            wrki = (float)Math.Sqrt((v1.Y * v2.Z - v1.Z * v2.Y) * (v1.Y * v2.Z - v1.Z * v2.Y) + (v1.Z * v2.X - v1.X * v2.Z) * (v1.Z * v2.X - v1.X * v2.Z) + (v1.X * v2.Y - v1.Y * v2.X) * (v1.X * v2.Y - v1.Y * v2.X));
            n.X = (v1.Y * v2.Z - v1.Z * v2.Y) / wrki;
            n.Y = (v1.Z * v2.X - v1.X * v2.Z) / wrki;
            n.Z = (v1.X * v2.Y - v1.Y * v2.X) / wrki;
        }
    }
}
