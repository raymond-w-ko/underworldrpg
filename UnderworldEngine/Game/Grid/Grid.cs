﻿using System;
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
using System.Xml;

namespace UnderworldEngine.Game
{
    class Grid
    {
        private int _xSize;
        private int _zSize;
        private string _textureName;

        private GridSquare[,] _grid;

        private bool _useRandomY = false;

        private BasicEffect _basicEffect;
        private VertexDeclaration _vertexDeclaration;

        public bool NeedCompile = false;

        private VertexPositionTexture[] _vertices;
        private int[] _indices;

        /// <summary>
        /// Creates an empty map of X by Z size
        /// </summary>
        /// <param name="xSize">Number of unit grids along the X axis</param>
        /// <param name="zSize">Number of unit grids along the Z axis</param>
        public Grid(int xSize, int zSize, string texture)
        {
            _xSize = xSize;
            _zSize = zSize;
            _textureName = texture;

            _grid = new GridSquare[_xSize, _zSize];
            for (int xx = 0; xx < xSize; xx++) {
                for (int zz = 0; zz < zSize; zz++) {
                    float height = 0.01f;
                    _grid[xx, zz] = new GridSquare(xx, zz, height);
                    _grid[xx, zz].TopAngle = 0.0f;
                    _grid[xx, zz].CompileVertices();
                }
            }

            this.initializeGraphics(texture);
            this.compileVertices();
        }

        private void initializeGraphics(string texture)
        {
            _basicEffect = new BasicEffect(Game1.DefaultGraphicsDevice, null);
            _basicEffect.TextureEnabled = true;
            _basicEffect.Texture = Game1.DefaultContent.Load<Texture2D>(texture);
            _basicEffect.Alpha = 0.6f;

            _vertexDeclaration = new VertexDeclaration(
                Game1.DefaultGraphics.GraphicsDevice,
                VertexPositionTexture.VertexElements
                );
        }

        public Grid(int xSize, int zSize) : this(xSize, zSize, "Textures/red") { }

        /// <summary>
        /// Loads a map from the Content Pipeline
        /// </summary>
        /// <param name="mapName"></param>
        public Grid(string mapName)
        {
            ;
        }

        /// <summary>
        /// Saves the entire map to a file
        /// </summary>
        /// <param name="mapName"></param>
        public void Save(XmlDocument xmlDocument, XmlNode rootNode)
        {
            XmlNode gridNode = xmlDocument.CreateElement("Grid");

            XmlAttribute xSizeAttribute = xmlDocument.CreateAttribute("xSize");
            xSizeAttribute.Value = _xSize.ToString();
            XmlAttribute zSizeAttribute = xmlDocument.CreateAttribute("zSize");
            zSizeAttribute.Value = _zSize.ToString();
            XmlAttribute textureAttribute = xmlDocument.CreateAttribute("texture");
            textureAttribute.Value = _textureName;
            gridNode.Attributes.Append(xSizeAttribute);
            gridNode.Attributes.Append(zSizeAttribute);
            gridNode.Attributes.Append(textureAttribute);

            for (int xx = 0; xx < _xSize; xx++) {
                for (int zz = 0; zz < _zSize; zz++) {
                    _grid[xx, zz].Save(xmlDocument, gridNode);
                }
            }

            rootNode.AppendChild(gridNode);
        }

        private void compileVertices()
        {
            _vertices = new VertexPositionTexture[_xSize * _zSize * 4];
            _indices = new int[_xSize * _zSize * 6];
            int vertCounter = 0;
            int indexCounter = 0;
            for (uint xx = 0; xx < _xSize; xx++) {
                for (uint zz = 0; zz < _zSize; zz++) {
                    for (int jj = 0; jj < 6; jj++, indexCounter++) {
                        _indices[indexCounter] = _grid[xx, zz].Indices[jj] + vertCounter;
                    }
                    for (int ii = 0; ii < 4; ii++, vertCounter++) {
                        _vertices[vertCounter] = _grid[xx, zz].Vertices[ii];
                    }
                }
            }
        }

        public void Draw()
        {
            _basicEffect.View = Game1.Camera.ViewMatrix;
            _basicEffect.Projection = Game1.Camera.ProjectionMatrix;
            _basicEffect.World = Matrix.Identity;

            // enable alpha blending
            Game1.DefaultGraphics.GraphicsDevice.RenderState.AlphaBlendEnable = true;
            Game1.DefaultGraphics.GraphicsDevice.RenderState.AlphaBlendOperation = BlendFunction.Add;
            Game1.DefaultGraphics.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            Game1.DefaultGraphics.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            Game1.DefaultGraphics.GraphicsDevice.RenderState.SeparateAlphaBlendEnabled = false;

            Game1.DefaultGraphicsDevice.VertexDeclaration = _vertexDeclaration;

            _basicEffect.Begin();
            foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes) {
                pass.Begin();

                /*
                for (uint xx = 0; xx < _xSize; xx++) {
                    for (uint zz = 0; zz < _zSize; zz++) {
                        Game1.DefaultGraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(
                            PrimitiveType.TriangleList,
                            _grid[xx, zz].Vertices, 0, 4,
                            _grid[xx, zz].Indices, 0, 2
                            );
                    }
                }
                */

                Game1.DefaultGraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(
                    PrimitiveType.TriangleList,
                    _vertices, 0, 4 * _xSize * _zSize,
                    _indices, 0, 2 * _xSize * _zSize
                    );

                pass.End();
            }
            _basicEffect.End();

            Game1.DefaultGraphics.GraphicsDevice.RenderState.AlphaBlendEnable = false;
        }

        public Vector2 FindIntersection(Ray ray)
        {
            foreach (GridSquare gs in _grid) {
                if (ray.Intersects(gs.BoundingBox) != null) {
                    return new Vector2(gs.XIndex, gs.ZIndex);
                }
            }
            return new Vector2(-1, -1);
        }

        public static Grid Load(XmlDocument xmlDocument, XmlNode rootNode)
        {
            int xSize = Convert.ToInt32(rootNode.Attributes["xSize"].Value);
            int zSize = Convert.ToInt32(rootNode.Attributes["zSize"].Value);
            string textureName = rootNode.Attributes["texture"].Value;
            Grid NewGrid = new Grid(xSize, zSize, "Textures/red");

            NewGrid._grid = new GridSquare[xSize, zSize];
            XmlNodeList gridSquareList = rootNode.ChildNodes;
            foreach (XmlNode node in gridSquareList) {
                int xIndex = Convert.ToInt32(node.Attributes["xIndex"].Value);
                int zIndex = Convert.ToInt32(node.Attributes["zIndex"].Value);

                bool isWalkable = Convert.ToBoolean(node["IsWalkable"].InnerText);
                float height = (float)Convert.ToDouble(node["Height"].InnerText);
                Vector3 topUp = (new Vector3()).GetFromString(node["TopUp"].InnerText);
                float topAngle = (float)Convert.ToDouble(node["TopAngle"].InnerText);

                GridSquare curGS = new GridSquare(xIndex, zIndex, height);
                NewGrid._grid[xIndex, zIndex] = curGS;

                curGS.IsWalkable = isWalkable;
                curGS.Height = height;
                curGS.TopUp = topUp;
                curGS.TopAngle = topAngle;

                curGS.CompileVertices();
            }

            NewGrid.initializeGraphics(textureName);
            NewGrid.compileVertices();

            return NewGrid;
        }
    }

    public static class Extensions
    {
        public static Vector3 GetFromString(this Vector3 vec, string s)
        {
            string[] values = s.Split(' ');
            vec.X = (float)Convert.ToDouble(values[0]);
            vec.Y = (float)Convert.ToDouble(values[1]);
            vec.Z = (float)Convert.ToDouble(values[2]);
            return vec;
        }
    }
}
