using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Xml;

namespace UnderworldEngine.Game
{
    public class GridSquare
    {
        public bool IsVisible = true;
        public bool IsWalkable = true;
        public bool IsSelected = false;

        private int _xIndex;
        public int XIndex
        {
            get
            {
                return _xIndex;
            }
        }

        private int _zIndex;
        public int ZIndex
        {
            get
            {
                return _zIndex;
            }
        }
        public float Height = 0;
        public static float MIN_FLOOR_HEIGHT = 0;

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
        
        public VertexPositionTexture[] Vertices;
        public int[] Indices;

        private BoundingBox _boundingBox;
        public BoundingBox BoundingBox
        {
            get
            {
                return _boundingBox;
            }
        }

        public GridSquare(int xIndex, int zIndex, float height)
        {
            Height = height;
            this._xIndex = xIndex;
            this._zIndex = zIndex;
        }

        public void CompileVertices()
        {
            Vector3 origin = new Vector3(_xIndex + 0.5f, Height, _zIndex + 0.5f);
            Quad Top = new Quad(origin, Vector3.Up, Vector3.Forward, 1.0f, 1.0f);
            
            float extraHeight = (float)Math.Tan(MathHelper.ToRadians(TopAngle));
            if (TopUp == Vector3.Forward) {
                Top.UpperLeft.Y += extraHeight;
                Top.UpperRight.Y += extraHeight;
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

            Vertices = new VertexPositionTexture[4];
            Indices = new int[6];

            convertToVerticesAndIndices(Top, Vertices, 4 * 0, Indices, 6 * 0, _xIndex, _zIndex);

            // calculate bounding box
            Vector3[] points = new Vector3[4];
            for (int ii = 0; ii < 4; ii++) {
                points[ii] = Vertices[ii].Position;
            }
            _boundingBox = BoundingBox.CreateFromPoints(points);
            //Game1.Debug.WriteLine(_boundingBox.ToString());
        }

        private void convertToVerticesAndIndices(Quad quad,
            VertexPositionTexture[] vert, int vertOffset,
            int[] index, int indexOffset,
            int u, int v)
        {
            Vertices[vertOffset + 0] = new VertexPositionTexture(quad.LowerLeft, new Vector2(u, v));
            Vertices[vertOffset + 1] = new VertexPositionTexture(quad.UpperLeft, new Vector2(u, v + 1));
            Vertices[vertOffset + 2] = new VertexPositionTexture(quad.UpperRight, new Vector2(u + 1, v + 1));
            Vertices[vertOffset + 3] = new VertexPositionTexture(quad.LowerRight, new Vector2(u + 1, v));

            if (IsVisible) {
                Indices[indexOffset + 0] = 0 + indexOffset;
                Indices[indexOffset + 1] = 1 + indexOffset;
                Indices[indexOffset + 2] = 3 + indexOffset;
                Indices[indexOffset + 3] = 3 + indexOffset;
                Indices[indexOffset + 4] = 1 + indexOffset;
                Indices[indexOffset + 5] = 2 + indexOffset;
            }
            else {
                Indices[indexOffset + 0] = 3 + indexOffset;
                Indices[indexOffset + 1] = 1 + indexOffset;
                Indices[indexOffset + 2] = 0 + indexOffset;
                Indices[indexOffset + 3] = 2 + indexOffset;
                Indices[indexOffset + 4] = 1 + indexOffset;
                Indices[indexOffset + 5] = 3 + indexOffset;
            }
        }

        public void Save(XmlDocument xmlDocument, XmlNode rootNode)
        {
            XmlNode gridSquareNode = xmlDocument.CreateElement("GridSquare");

            XmlAttribute xIndexAttribute = xmlDocument.CreateAttribute("xIndex");
            xIndexAttribute.Value = _xIndex.ToString();
            XmlAttribute zIndexAttribute = xmlDocument.CreateAttribute("zIndex");
            zIndexAttribute.Value = _zIndex.ToString();
            gridSquareNode.Attributes.Append(xIndexAttribute);
            gridSquareNode.Attributes.Append(zIndexAttribute);

            XmlNode IsWalkableNode = xmlDocument.CreateElement("IsWalkable");
            IsWalkableNode.AppendChild(xmlDocument.CreateTextNode(IsWalkable.ToString()));
            gridSquareNode.AppendChild(IsWalkableNode);

            XmlNode heightNode = xmlDocument.CreateElement("Height");
            heightNode.AppendChild(xmlDocument.CreateTextNode(Height.ToString()));
            gridSquareNode.AppendChild(heightNode);

            XmlNode topUpNode = xmlDocument.CreateElement("TopUp");
            topUpNode.AppendChild(xmlDocument.CreateTextNode(
                TopUp.X.ToString() + " " + TopUp.Y.ToString() + " " + TopUp.Z.ToString()
                ));
            gridSquareNode.AppendChild(topUpNode);

            XmlNode topAngleNode = xmlDocument.CreateElement("TopAngle");
            topAngleNode.AppendChild(xmlDocument.CreateTextNode(
                TopAngle.ToString()
                ));
            gridSquareNode.AppendChild(topAngleNode);

            rootNode.AppendChild(gridSquareNode);
        }
    }
}
