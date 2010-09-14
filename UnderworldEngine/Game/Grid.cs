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
        private uint _xSize;
        private uint _zSize;

        private GridSquare[,] _grid;

        private Random _rand;
        private bool useRandomY = false;

        /// <summary>
        /// Creates an empty map of X by Z size
        /// </summary>
        /// <param name="xSize">Number of unit grids along the X axis</param>
        /// <param name="zSize">Number of unit grids along the Z axis</param>
        public Grid(uint xSize, uint zSize)
        {
            _rand = new Random(31337);

            _xSize = xSize;
            _zSize = zSize;

            _grid = new GridSquare[_xSize, zSize];
            for (uint xx = 0; xx < xSize; xx++) {
                for (uint zz = 0; zz < zSize; zz++) {
                    float height = useRandomY ? (int)Math.Ceiling((_rand.NextDouble() * 2)) : 1;
                    _grid[xx, zz] = new GridSquare(xx, zz, height);
                    _grid[xx, zz].TopAngle = 0.0f;
                    _grid[xx, zz].CompileVertices();
                }
            }
        }

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

        public void Draw()
        {
            BasicEffect bem = new BasicEffect(Game1.DefaultGraphicsDevice, null);
            bem.TextureEnabled = true;
            bem.Texture = Game1.DefaultContent.Load<Texture2D>("Textures/red");

            bem.Alpha = 0.5f;

            bem.View = Game1.Camera.ViewMatrix;
            bem.Projection = Game1.Camera.ProjectionMatrix;
            bem.World = Matrix.Identity;

            // enable alpha blending
            Game1.DefaultGraphics.GraphicsDevice.RenderState.AlphaBlendEnable = true;
            Game1.DefaultGraphics.GraphicsDevice.RenderState.AlphaBlendOperation = BlendFunction.Add;
            Game1.DefaultGraphics.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            Game1.DefaultGraphics.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            Game1.DefaultGraphics.GraphicsDevice.RenderState.SeparateAlphaBlendEnabled = false;

            Game1.DefaultGraphicsDevice.VertexDeclaration = new VertexDeclaration(
                Game1.DefaultGraphics.GraphicsDevice,
                VertexPositionTexture.VertexElements);

            bem.Begin();
            foreach (EffectPass pass in bem.CurrentTechnique.Passes) {
                pass.Begin();

                for (uint xx = 0; xx < _xSize; xx++) {
                    for (uint zz = 0; zz < _zSize; zz++) {
                        Game1.DefaultGraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(
                            PrimitiveType.TriangleList,
                            _grid[xx, zz].Vertices, 0, 4,
                            _grid[xx, zz].Indices, 0, 2
                            );
                    }
                }

                pass.End();
            }

            Game1.DefaultGraphics.GraphicsDevice.RenderState.AlphaBlendEnable = false;
        }
    }
}
