using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using UnderworldEngine.Graphics;

namespace UnderworldEngine.Game
{
    public class QuadTexture : Quad
    {
        private VertexPositionTexture[] _vertices;
        private int[] _indices;

        private Texture2D _texture;
        private BasicEffect _basicEffect;
        private bool _alphaBlendState;
        private BlendFunction _blendFunction;
        private Blend _sourceBlend;
        private Blend _destBlend;
        public float Alpha
        {
            get
            {
                return _basicEffect.Alpha;
            }
            set
            {
                if (value < 0.0f || value > 1.0f) {
                    throw new ApplicationException("Invalid alpha value");
                }
                _basicEffect.Alpha = value;
            }
        }

        private VertexDeclaration _vertexDeclaration;

        public QuadTexture(Vector3 origin, Vector3 normal, Vector3 up, float width, float height,
            string textureName)
            : base(origin, normal, up, width, height)
        {
            this._vertices = new VertexPositionTexture[4];
            this._indices = new int[6];

            FillTextureCoordinates();
            FillVertices();
            FillIndices();

            _texture = Game1.DefaultContent.Load<Texture2D>(textureName);
            _basicEffect = new BasicEffect(Game1.DefaultGraphicsDevice, null);
            _basicEffect.TextureEnabled = true;
            _basicEffect.Texture = _texture;

            _vertexDeclaration = new VertexDeclaration(Game1.DefaultGraphics.GraphicsDevice,
                VertexPositionTexture.VertexElements);
        }

        private void FillTextureCoordinates()
        {
            // Fill in texture coordinates to display full texture on quad
            Vector2 textureUpperLeft = new Vector2(0.0f, 0.0f);
            Vector2 textureUpperRight = new Vector2(1.0f, 0.0f);
            Vector2 textureLowerLeft = new Vector2(0.0f, 1.0f);
            Vector2 textureLowerRight = new Vector2(1.0f, 1.0f);


            _vertices[0].TextureCoordinate = textureLowerLeft;
            _vertices[1].TextureCoordinate = textureUpperLeft;
            _vertices[2].TextureCoordinate = textureLowerRight;
            _vertices[3].TextureCoordinate = textureUpperRight;
        }

        public void FillVertices()
        {
            // Set the position and texture coordinate for each vertex
            _vertices[0].Position = LowerLeft;
            _vertices[1].Position = UpperLeft;
            _vertices[2].Position = LowerRight;
            _vertices[3].Position = UpperRight;
        }

        private void FillIndices()
        {
            // Set the index buffer for each vertex, using
            // clockwise winding, otherwise will be culled
            _indices[0] = 0;
            _indices[1] = 1;
            _indices[2] = 2;

            _indices[3] = 2;
            _indices[4] = 1;
            _indices[5] = 3;
        }

        public override void Draw(GameTime gameTime)
        {
            if (!this.Visible) {
                return;
            }

            Game1.DefaultGraphicsDevice.VertexDeclaration = _vertexDeclaration;

            CompileTransformations();

            _basicEffect.View = Game1.Camera.ViewMatrix;
            _basicEffect.Projection = Game1.Camera.ProjectionMatrix;
            _basicEffect.World = _worldMatrix;

            bool oldAlphaBlendingState = Game1.DefaultGraphicsDevice.RenderState.AlphaBlendEnable;

            Game1.DefaultGraphicsDevice.RenderState.AlphaBlendEnable = _alphaBlendState;
            Game1.DefaultGraphicsDevice.RenderState.BlendFunction = _blendFunction;
            Game1.DefaultGraphicsDevice.RenderState.SourceBlend = _sourceBlend;
            Game1.DefaultGraphicsDevice.RenderState.DestinationBlend = _destBlend;

            _basicEffect.Begin();

            foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes) {
                pass.Begin();

                Game1.DefaultGraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(
                    PrimitiveType.TriangleList,
                    this._vertices, 0, 4,
                    this._indices, 0, 2);

                pass.End();
            }

            _basicEffect.End();

            Game1.DefaultGraphicsDevice.RenderState.AlphaBlendEnable = oldAlphaBlendingState;
        }

        public void EnableAlphaBlending(
            BlendFunction blendFunction, 
            Blend sourceBlend, Blend destBlend
            )
        {
            _alphaBlendState = true;

            _basicEffect.Alpha = 1.0f;

            _blendFunction = blendFunction;
            _sourceBlend = sourceBlend;
            _destBlend = destBlend;
        }

        public void DisableAlphaBlending()
        {
            _alphaBlendState = false;
        }

        public void ScaleUvMap(float scale)
        {
            if (scale == 0) {
                throw new ApplicationException("The UV map can't be scaled by a factor of 0.");
            }

            for (int ii = 0; ii < 4; ii++) {
                this._vertices[ii].TextureCoordinate *= scale;
            }
        }

        public void ImportVertices(GridSquare gridSquare)
        {
            for (int ii = 0; ii < 4; ii++) {
                this._vertices[ii] = gridSquare.Vertices[ii];
            }

            for (int ii = 0; ii < 6; ii++) {
                this._indices[ii] = gridSquare.Indices[ii];
            }
        }
    }
}