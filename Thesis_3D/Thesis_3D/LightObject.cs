﻿using System;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using Buffer = System.Buffer;

namespace Thesis_3D
{
    class LightObject : RenderObject
    {
        public int uboLightInfo = -1;
        public int blockSizeLightInfo = -1; 
        public Vector3 Position;
        public Vector3 Angel_speed;
        public Vector4 Attribute;
        public Vector3 LightVecNormalized;
        public Vector3 DiffusionIntensity;
        public Vector3 Ambient = Vector3.One;
        public Vector3 Mirror = Vector3.One;
        public Vector4 ColorRadiation;
        private Vector3 StartPosition;
        public LightObject(Vertex[] vertices, Color4 color, Color4 color_choice, Vector3 position, Vector4 attribute, Vector3 lighVecNormalized, Vector3 diffusionIntensity, Vector3 angle_speed, int programBlock = -1, string nameBlock = null, float side = 1, TypeObjectCreate locTypeObjectCreate = TypeObjectCreate.SolidCube, int locColBreakX = 1, int locColBreakY = 1, int locCoeffSX = 1, int locCoeffSY = 1, int locAngleX = 0, int locAngleY = 0, int locAngleZ = 0) : base(vertices, position, color, color_choice, TypeObjectRenderLight.LightSourceObject, locSide: side, locTypeObjectCreate: locTypeObjectCreate, locColBreakX: locColBreakX, locColBreakY: locColBreakY, locCoeffSX: locCoeffSX, locCoeffSY: locCoeffSY, locAngleX: locAngleX, locAngleY: locAngleY, locAngleZ: locAngleZ)
        {
            Position = position; //Позиция источника
            StartPosition = position;
            Angel_speed = angle_speed; //X - Скорость и направление Y и Z - угол поворота по соответствующим осям
            Attribute = attribute; //Я ещё придумаю зачем я это добавил
            LightVecNormalized = lighVecNormalized;
            DiffusionIntensity = diffusionIntensity;
            ColorRadiation.X = color.R;
            ColorRadiation.Y = color.G;
            ColorRadiation.Z = color.B;
            ColorRadiation.W = color.A;
            if(programBlock != -1)
            {
                if (!string.IsNullOrWhiteSpace(nameBlock)) InitBufferForBlock(programBlock, nameBlock);
                else InitBufferForBlock(programBlock);
            }
        }
        public void SetAttrFog(int localMinDist, float MinDist, int localMaxDist, float MaxDist, int localColorFog, Vector3 ColorFog)
        {
            GL.Uniform1(localMaxDist, MaxDist);
            GL.Uniform1(localMinDist, MinDist);
            GL.Uniform3(localColorFog, ColorFog);
        }
        public void SetPositionLight(Matrix4 ModelMatrix)
        {
            Position = ModelMatrix.ExtractTranslation();
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
        public void IntensityLightVectorUniform(int location)
        {
            GL.Uniform3(location, DiffusionIntensity);
        }
        public void IntensityAmbient(int location)
        {
            GL.Uniform3(location, Ambient);
        }
        public void IntensityMirror(int location)
        {
            GL.Uniform3(location, Mirror);
        }
        public void lighVecNormalizedUniform(int location)
        {
            GL.Uniform3(location, LightVecNormalized);
        }
        private void InitBufferForBlock(int program)
        {
            InitBufferForBlock(program, "SpotLightInfo");
        }

        private void InitBufferForBlock(int program, string nameBlock)
        {
            int index_SLI = GL.GetUniformBlockIndex(program, nameBlock);
            if (index_SLI != -1)
            {
                GL.GetActiveUniformBlock(program, index_SLI, ActiveUniformBlockParameter.UniformBlockDataSize, out blockSizeLightInfo);
                byte[] blockBuffer = new byte[blockSizeLightInfo];
                string[] names = { nameBlock + ".position_lgh", nameBlock + ".intensity_lgh", nameBlock + ".direction_lgh", nameBlock + ".exponent_lgh", nameBlock + ".cutoff_lgh" };
                int[] indices = new int[5];
                GL.GetUniformIndices(program, 5, names, indices);
                int[] offset = new int[5];
                GL.GetActiveUniforms(program, 5, indices, ActiveUniformParameter.UniformOffset, offset);

                float[] position_lgh = { Position.X, Position.Y, Position.Z, 0.0f };
                float[] intensity_lgh = { DiffusionIntensity.X, DiffusionIntensity.Y, DiffusionIntensity.Z }; //интенсивность света
                float[] direction_lgh = { LightVecNormalized.X, LightVecNormalized.Y, LightVecNormalized.Z }; //направление света
                float[] exponent = { 1.0f }; // Экспанента углового ослабления света
                float[] cutoff = { 30f }; //угол отсечения 

                Buffer.BlockCopy(position_lgh, 0, blockBuffer, offset[0], position_lgh.Length * sizeof(float));
                Buffer.BlockCopy(intensity_lgh, 0, blockBuffer, offset[1], intensity_lgh.Length * sizeof(float));
                Buffer.BlockCopy(direction_lgh, 0, blockBuffer, offset[2], direction_lgh.Length * sizeof(float));
                Buffer.BlockCopy(exponent, 0, blockBuffer, offset[3], exponent.Length * sizeof(float));
                Buffer.BlockCopy(cutoff, 0, blockBuffer, offset[4], cutoff.Length * sizeof(float));

                if (uboLightInfo != -1) GL.DeleteBuffer(uboLightInfo);
                GL.GenBuffers(1, out uboLightInfo);
                GL.BindBuffer(BufferTarget.UniformBuffer, uboLightInfo);
                GL.BufferData(BufferTarget.UniformBuffer, blockSizeLightInfo, blockBuffer, BufferUsageHint.DynamicDraw);
            }
        }
        public void UpdatePositionForBlock(int program)
        {
            UpdatePositionForBlock(program, "SpotLightInfo");
        }
        public void UpdatePositionForBlock(int program, string nameBlock)
        {
            float[] position_lgh = { Position.X, Position.Y, Position.Z, 0.0f };
            GL.BindBuffer(BufferTarget.UniformBuffer, uboLightInfo);
            GL.BufferSubData(BufferTarget.UniformBuffer, (IntPtr)0, sizeof(float) * position_lgh.Length, position_lgh);
        }

        public void UpdateBufferForBlock(int program)
        {
            UpdateBufferForBlock(program, "SpotLightInfo");
        }
        public void UpdateBufferForBlock(int program, string nameBlock)
        {
            int index_SLI = GL.GetUniformBlockIndex(program, nameBlock);
            if (index_SLI != -1)
            {
                GL.GetActiveUniformBlock(program, index_SLI, ActiveUniformBlockParameter.UniformBlockDataSize, out blockSizeLightInfo);
                byte[] blockBuffer = new byte[blockSizeLightInfo];
                string[] names = { nameBlock + ".position_lgh", nameBlock + ".intensity_lgh", nameBlock + ".direction_lgh", nameBlock + ".exponent_lgh", nameBlock + ".cutoff_lgh" };
                int[] indices = new int[5];
                GL.GetUniformIndices(program, 5, names, indices);
                int[] offset = new int[5];
                GL.GetActiveUniforms(program, 5, indices, ActiveUniformParameter.UniformOffset, offset);

                float[] position_lgh = { Position.X, Position.Y, Position.Z, 0.0f };
                float[] intensity_lgh = { DiffusionIntensity.X, DiffusionIntensity.Y, DiffusionIntensity.Z }; //интенсивность света
                float[] direction_lgh = { LightVecNormalized.X, LightVecNormalized.Y, LightVecNormalized.Z }; //направление света
                float[] exponent = { 1.0f }; // Экспанента углового ослабления света
                float[] cutoff = { 30f }; //угол отсечения 

                Buffer.BlockCopy(position_lgh, 0, blockBuffer, offset[0], position_lgh.Length * sizeof(float));
                Buffer.BlockCopy(intensity_lgh, 0, blockBuffer, offset[1], intensity_lgh.Length * sizeof(float));
                Buffer.BlockCopy(direction_lgh, 0, blockBuffer, offset[2], direction_lgh.Length * sizeof(float));
                Buffer.BlockCopy(exponent, 0, blockBuffer, offset[3], exponent.Length * sizeof(float));
                Buffer.BlockCopy(cutoff, 0, blockBuffer, offset[4], cutoff.Length * sizeof(float));

                GL.BindBuffer(BufferTarget.UniformBuffer, uboLightInfo);
                GL.BufferSubData(BufferTarget.UniformBuffer, (IntPtr)0, blockSizeLightInfo, blockBuffer);
            }
        }

    }
}
