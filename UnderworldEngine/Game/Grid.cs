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
    class Grid
    {
        private int _xSize;
        private int _zSize;

        private GridSquare[,] _grid;

        private Random _rand;
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
            _rand = new Random(31337);

            _xSize = xSize;
            _zSize = zSize;

            _grid = new GridSquare[_xSize, zSize];
            for (uint xx = 0; xx < xSize; xx++) {
                for (uint zz = 0; zz < zSize; zz++) {
                    float height = _useRandomY ? (int)Math.Ceiling((_rand.NextDouble() * 2)) : 2;
                    _grid[xx, zz] = new GridSquare(xx, zz, height);
                    _grid[xx, zz].TopAngle = 0.0f;
                    _grid[xx, zz].CompileVertices();
                }
            }

            _basicEffect = new BasicEffect(Game1.DefaultGraphicsDevice, null);
            _basicEffect.TextureEnabled = true;
            _basicEffect.Texture = Game1.DefaultContent.Load<Texture2D>("Textures/red");
            _basicEffect.Alpha = 0.6f;

            _vertexDeclaration = new VertexDeclaration(
                Game1.DefaultGraphics.GraphicsDevice,
                VertexPositionTexture.VertexElements
                );

            this.compileVertices();
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
        public void Save(string mapName)
        {
            ;
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
            /*
            Game1.DefaultGraphics.GraphicsDevice.RenderState.AlphaBlendOperation = BlendFunction.Add;
            Game1.DefaultGraphics.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            Game1.DefaultGraphics.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            Game1.DefaultGraphics.GraphicsDevice.RenderState.SeparateAlphaBlendEnabled = false;
            */

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
    }
}
