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
    class LightObject : RenderObject
    {
        public Vector3 Position;
        public Vector3 Angel_speed;
        public Vector4 Attribute;
        public Vector3 LightVecNormalized;
        public Vector3 AmbirntIntensity;
        public Vector4 ColorRadiation;
        public LightObject(Vertex[] vertices, Color4 color, Color4 color_choice, Vector3 position, Vector4 attribute, Vector3 lighVecNormalized, Vector3 ambientIntensity, Vector3 angle_speed) : base(vertices, color, color_choice)
        {
            Position = position;
            Angel_speed = angle_speed;
            Attribute = attribute;
            LightVecNormalized = lighVecNormalized;
            AmbirntIntensity = ambientIntensity;
            ColorRadiation.X = color.R;
            ColorRadiation.Y = color.G;
            ColorRadiation.Z = color.B;
            ColorRadiation.W = color.A;
        }
        public void SetColorRadiation(Color4 color)
        {
            ColorRadiation.X = color.R;
            ColorRadiation.Y = color.G;
            ColorRadiation.Z = color.B;
            ColorRadiation.W = color.A;
        }
        public void PositionLightUniform()
        {
            GL.Uniform4(5, new Vector4(Position, 1));
        }
    }
}
