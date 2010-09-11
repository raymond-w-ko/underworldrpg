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
    class Quad
    {
        public VertexPositionNormalTexture[] Vertices;
        public int[] Indexes;
        Vector3 Origin;
        Vector3 Normal;
        Vector3 Up;

        Vector3 Left;
        Vector3 UpperLeft;
        Vector3 UpperRight;
        Vector3 LowerLeft;
        Vector3 LowerRight;

        public Quad(Vector3 origin, Vector3 normal, Vector3 up, float width, float height)
        {
            this.Vertices = new VertexPositionNormalTexture[4];
            this.Indexes = new int[6];
            this.Origin = origin;
            this.Normal = normal;
            this.Up = up;

            Left = Vector3.Cross(Normal, Up);
            Vector3 uppercenter = (Up * height / 2) + Origin;

            this.UpperLeft = uppercenter + (Left * width / 2);
            this.UpperRight = uppercenter - (Left * width / 2);
            this.LowerLeft = UpperLeft - (Up * height);
            this.LowerRight = UpperRight - (Up * height);

            FillVertices();
        }

        private void FillVertices()
        {
            // Fill in texture coordinates to display full texture
            // on quad
            Vector2 textureUpperLeft = new Vector2(0.0f, 0.0f);
            Vector2 textureUpperRight = new Vector2(5.0f, 0.0f);
            Vector2 textureLowerLeft = new Vector2(0.0f, 5.0f);
            Vector2 textureLowerRight = new Vector2(5.0f, 5.0f);

            // Provide a normal for each vertex
            for (int i = 0; i < Vertices.Length; i++) {
                Vertices[i].Normal = Normal;
            }

            // Set the position and texture coordinate for each
            // vertex
            Vertices[0].Position = LowerLeft;
            Vertices[0].TextureCoordinate = textureLowerLeft;
            Vertices[1].Position = UpperLeft;
            Vertices[1].TextureCoordinate = textureUpperLeft;
            Vertices[2].Position = LowerRight;
            Vertices[2].TextureCoordinate = textureLowerRight;
            Vertices[3].Position = UpperRight;
            Vertices[3].TextureCoordinate = textureUpperRight;

            // Set the index buffer for each vertex, using
            // clockwise winding
            Indexes[0] = 0;
            Indexes[1] = 1;
            Indexes[2] = 2;
            Indexes[3] = 2;
            Indexes[4] = 1;
            Indexes[5] = 3;
        }
    }
}
