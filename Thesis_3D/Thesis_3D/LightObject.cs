using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using Buffer = System.Buffer;

namespace Thesis_3D
{
    class LightObject : RenderObject
    {
        public int uboHandle;
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
        public void PositionLightUniform(int location)
        {
            Vector4 position_v4 = new Vector4(Position, 1);
            GL.Uniform4(location, position_v4);
        }
        public void MatrixViewUnifomr(int location)
        {
            Matrix4 modelview = Matrix4.CreateTranslation(
                0,
                0, 
                0 
                );
            GL.UniformMatrix4(location, false, ref modelview);
        }

        public void IntensityLightUniform(int location)
        {
            GL.Uniform3(location, AmbirntIntensity);
        }
        public void SendParmInShader(int location)
        {
            SendParmInShader(location, "SpotLightInfo");
        }

        public void SendParmInShader(int location, string nameBlock)
        {
            int index_SLI = GL.GetUniformBlockIndex(location, nameBlock);
            if (index_SLI != -1)
            {
                int blockSize;
                GL.GetActiveUniformBlock(location, index_SLI, ActiveUniformBlockParameter.UniformBlockDataSize, out blockSize);
                byte[] blockBuffer = new byte[blockSize];
                string[] names = { nameBlock + ".position_lgh", nameBlock + ".intensity_lgh", nameBlock + ".direction_lgh", nameBlock + ".exponent_lgh", nameBlock + ".cutoff_lgh" };
                int[] indices = new int[5];
                GL.GetUniformIndices(location, 5, names, indices);
                int[] offset = new int[5];
                GL.GetActiveUniforms(location, 5, indices, ActiveUniformParameter.UniformOffset, offset);

                float[] position_lgh = { Position.X, Position.Y, Position.Z, 0.0f };
                float[] intensity_lgh = { 1.0f, 1.0f, 1.0f }; //интенсивность света
                float[] direction_lgh = { 0f, -1.0f, 0f }; //направление света
                float[] exponent = { 1.0f }; // Экспанента углового ослабления света
                float[] cutoff = { 30f }; //угол отсечения 

                Buffer.BlockCopy(position_lgh, 0, blockBuffer, offset[0], position_lgh.Length * sizeof(float));
                Buffer.BlockCopy(intensity_lgh, 0, blockBuffer, offset[1], intensity_lgh.Length * sizeof(float));
                Buffer.BlockCopy(direction_lgh, 0, blockBuffer, offset[2], direction_lgh.Length * sizeof(float));
                Buffer.BlockCopy(exponent, 0, blockBuffer, offset[3], exponent.Length * sizeof(float));
                Buffer.BlockCopy(cutoff, 0, blockBuffer, offset[4], cutoff.Length * sizeof(float));

                GL.GenBuffers(1, out uboHandle);
                GL.BindBuffer(BufferTarget.UniformBuffer, uboHandle);
                GL.BufferData(BufferTarget.UniformBuffer, blockSize, blockBuffer, BufferUsageHint.DynamicDraw);
            }
        }
    }
}
