/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace UnderworldEngine.Game
{
    class MapGridUnit
    {
        public bool IsVisible = true;
        public bool IsWalkable = true;

        private uint _xIndex;
        private uint _zIndex;
        public float Height = 0;
        public static float MIN_FLOOR_HEIGHT = 0;
        public string TopTexture = "Texture/ground";

        /// <summary>
        /// Specify the slant of the top square in degrees
        /// </summary>
        private float _topAngle = 0.0f;
        public class TopAngleException : ApplicationException { }
        public float TopAngle
        {
            get
            {
                return _topAngle;
            }
            set
            {
                if ((value < 0) || (value >= 90)) {
                    throw new TopAngleException();
                }
                _topAngle = value;
            }
        }

        private Vector3 _topUp = Vector3.Forward;
        public Vector3 TopUp
        {
            get
            {
                return _topUp;
            }
            set
            {
                if (value != Vector3.Forward &&
                    value != Vector3.Backward &&
                    value != Vector3.Left &&
                    value != Vector3.Right) {
                    throw new TopAngleException();
                }

                _topUp = value;
            }
        }

        public string FrontTexture = "Texture/dirt_01";
        public string BackTexture = "Texture/dirt_01";
        public string LeftTexture = "Texture/dirt_01";
        public string RightTexture = "Texture/dirt_01";

        VertexPositionTexture[] Vertices;
        int[] Indices;

        public MapGridUnit(uint xIndex, uint zIndex, float height)
        {
            Height = height;
            this._xIndex = xIndex;
            this._zIndex = zIndex;
        }

        public void CompileVertices()
        {
            Vector3 origin = new Vector3(_xIndex - 0.5f, Height, _zIndex - 0.5f);

            Vector3 frontOrigin =
                new Vector3(origin.X, MIN_FLOOR_HEIGHT + (Height / 2.0f), origin.Z - 0.5f);
            Vector3 backOrigin =
                new Vector3(origin.X, MIN_FLOOR_HEIGHT + (Height / 2.0f), origin.Z + 0.5f);
            Vector3 leftOrigin =
                new Vector3(origin.X - 0.5f, MIN_FLOOR_HEIGHT + (Height / 2.0f), origin.Z);
            Vector3 rightOrigin =
                new Vector3(origin.X + 0.5f, MIN_FLOOR_HEIGHT + (Height / 2.0f), origin.Z);

            // Figure out position of base vertices
            Quad Top = new Quad(origin, Vector3.Up, Vector3.Forward, 1.0f, Height);
            Quad Front = new Quad(frontOrigin, Vector3.Backward, Vector3.Up, 1.0f, Height);
            Quad Back = new Quad(backOrigin, Vector3.Forward, Vector3.Up, 1.0f, Height);
            Quad Left = new Quad(leftOrigin, Vector3.Left, Vector3.Up, 1.0f, Height);
            Quad Right = new Quad(rightOrigin, Vector3.Right, Vector3.Up, 1.0f, Height);

            // Figure out slope
            float extraHeight = (float)Math.Tan(MathHelper.ToRadians(TopAngle));
            if (TopUp == Vector3.Forward) {
                Top.UpperLeft.Y += extraHeight;
                Top.UpperRight.Y += extraHeight;

                Back.UpperLeft.Y += extraHeight;
                Back.UpperRight.Y += extraHeight;


            }
            else if (TopUp == Vector3.Backward) {
                Top.LowerLeft.Y += extraHeight;
                Top.LowerRight.Y += extraHeight;
            }
            else if (TopUp == Vector3.Left) {
                Top.UpperLeft.Y += extraHeight;
                Top.LowerLeft.Y += extraHeight;
            }
            else if (TopUp == Vector3.Right) {
                Top.UpperRight.Y += extraHeight;
                Top.LowerRight.Y += extraHeight;
            }

            Vertices = new VertexPositionTexture[20];
            Indices = new int[30];

            convertToVerticesAndIndices(Top, Vertices, 4 * 0, Indices, 6 * 0);
            convertToVerticesAndIndices(Front, Vertices, 4 * 1, Indices, 6 * 1);
            convertToVerticesAndIndices(Back, Vertices, 4 * 2, Indices, 6 * 2);
            convertToVerticesAndIndices(Left, Vertices, 4 * 3, Indices, 6 * 3);
            convertToVerticesAndIndices(Right, Vertices, 4 * 4, Indices, 6 * 4);
        }

        private void convertToVerticesAndIndices(Quad quad,
            VertexPositionTexture[] vert, int vertOffset,
            int[] index, int indexOffset)
        {
            Vertices[vertOffset + 0] = new VertexPositionTexture(quad.LowerLeft, new Vector2(0, 0));
            Vertices[vertOffset + 1] = new VertexPositionTexture(quad.UpperLeft, new Vector2(0, 1));
            Vertices[vertOffset + 2] = new VertexPositionTexture(quad.UpperRight, new Vector2(1, 1));
            Vertices[vertOffset + 3] = new VertexPositionTexture(quad.LowerRight, new Vector2(1, 0));

            Indices[indexOffset + 0] = 0 + indexOffset;
            Indices[indexOffset + 1] = 1 + indexOffset;
            Indices[indexOffset + 2] = 2 + indexOffset;
            Indices[indexOffset + 3] = 3 + indexOffset;
            Indices[indexOffset + 4] = 1 + indexOffset;
            Indices[indexOffset + 5] = 2 + indexOffset;
        }
    }
}
*/