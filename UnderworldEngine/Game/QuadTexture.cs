using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace UnderworldEngine.Game
{
    public class QuadTexture : Quad
    {
        private VertexPositionNormalTexture[] vertices;
        private int[] indexes;

        private Texture2D texture;

        private VertexDeclaration vertexDeclaration;

        public QuadTexture(Vector3 origin, Vector3 normal, Vector3 up, float width, float height,
            string textureName)
            : base(origin, normal, up, width, height)
        {
            this.vertices = new VertexPositionNormalTexture[4];
            this.indexes = new int[6];

            FillVertices();

            texture = Game1.DefaultContent.Load<Texture2D>("Textures/ground");
            this.effect.EnableTexture();
            this.effect.SetTexture(texture);

            vertexDeclaration = new VertexDeclaration(Game1.DefaultGraphics.GraphicsDevice,
                VertexPositionNormalTexture.VertexElements);

            Game1.DefaultGraphicsDevice.VertexDeclaration = vertexDeclaration;
        }

        private void FillVertices()
        {
            // Fill in texture coordinates to display full texture on quad
            Vector2 textureUpperLeft = new Vector2(0.0f, 0.0f);
            Vector2 textureUpperRight = new Vector2(1.0f, 0.0f);
            Vector2 textureLowerLeft = new Vector2(0.0f, 1.0f);
            Vector2 textureLowerRight = new Vector2(1.0f, 1.0f);

            // Provide a normal for each vertex
            for (int i = 0; i < vertices.Length; i++) {
                vertices[i].Normal = normal;
            }

            // Set the position and texture coordinate for each vertex
            vertices[0].Position = lowerLeft;
            vertices[0].TextureCoordinate = textureLowerLeft;
            vertices[1].Position = upperLeft;
            vertices[1].TextureCoordinate = textureUpperLeft;
            vertices[2].Position = lowerRight;
            vertices[2].TextureCoordinate = textureLowerRight;
            vertices[3].Position = upperRight;
            vertices[3].TextureCoordinate = textureUpperRight;

            // Set the index buffer for each vertex, using
            // clockwise winding, otherwise will be culled
            indexes[0] = 0;
            indexes[1] = 1;
            indexes[2] = 2;

            indexes[3] = 2;
            indexes[4] = 1;
            indexes[5] = 3;
        }

        public override void Draw()
        {
            if (!this.IsVisible) {
                return;
            }

            CompileTransformations();
            this.effect.UpdatePresentationMatrices(this.worldMatrix);

            this.effect.BasicEffect.Begin();

            foreach (EffectPass pass in this.effect.BasicEffect.CurrentTechnique.Passes) {
                pass.Begin();

                Game1.DefaultGraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(
                    PrimitiveType.TriangleList,
                    this.vertices, 0, 4,
                    this.indexes, 0, 2);

                pass.End();
            }

            this.effect.BasicEffect.End();
        }

        public class ScaleByZeroException : ApplicationException { }
        public void ScaleUvMap(float scale)
        {
            if (scale == 0) {
                throw new ScaleByZeroException();
            }

            for (int ii = 0; ii < 4; ii++) {
                this.vertices[ii].TextureCoordinate *= scale;
            }
        }
    }
}
