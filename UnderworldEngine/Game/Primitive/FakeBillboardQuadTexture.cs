using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace UnderworldEngine.Game
{
    public class FakeBillboardQuadTexture
    {
        private static SpriteBatch _spriteBatch = null;
        private uint _count = 0;
        private uint _drawCount;

        private Vector3 _position;
        public Vector3 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        private Vector2 _dimensions;
        public Vector2 Dimensions
        {
            get
            {
                return _dimensions;
            }
            set
            {
                _dimensions = value;
            }
        }
        private float _aspectRatio;

        private Texture2D _texture;

        private Vector3 _viewportPosition;
        public Vector3 ViewportPosition
        {
            get
            {
                return _viewportPosition;
            }
        }

        public Vector2 Offset;

        private int _centerX;
        private int _centerY;

        public FakeBillboardQuadTexture(Vector3 position, Vector2 dimensions, string textureName)
        {
            if (_spriteBatch == null) {
                _spriteBatch = new SpriteBatch(Game1.DefaultGraphicsDevice);
                _drawCount = 0;
            }

            _position = position;
            _dimensions = dimensions;
            _aspectRatio = dimensions.Y / dimensions.X;
            Offset = new Vector2(0, 0);

            _texture = Game1.DefaultContent.Load<Texture2D>(textureName);

            _count++;
        }

        public void CalculateViewportPosition()
        {
            _viewportPosition = Game1.DefaultGraphicsDevice.Viewport.Project(
                _position,
                Game1.Camera.ProjectionMatrix,
                Game1.Camera.ViewMatrix,
                Matrix.Identity
                );
        }

        public void ResizeToGridLength(float scale)
        {
            Vector3 corner1 = Game1.DefaultGraphicsDevice.Viewport.Project(
                new Vector3(_position.X + .5f, _position.Y, _position.Z),
                Game1.Camera.ProjectionMatrix,
                Game1.Camera.ViewMatrix,
                Matrix.Identity
                );

            Vector3 corner2 = Game1.DefaultGraphicsDevice.Viewport.Project(
                new Vector3(_position.X, _position.Y + .5f, _position.Z),
                Game1.Camera.ProjectionMatrix,
                Game1.Camera.ViewMatrix,
                Matrix.Identity
                );

            Vector3 corner3 = Game1.DefaultGraphicsDevice.Viewport.Project(
                new Vector3(_position.X + .5f, _position.Y + .5f, _position.Z),
                Game1.Camera.ProjectionMatrix,
                Game1.Camera.ViewMatrix,
                Matrix.Identity
                );

            Vector3 corner4 = Game1.DefaultGraphicsDevice.Viewport.Project(
                new Vector3(_position.X -.5f, _position.Y -.5f, _position.Z),
                Game1.Camera.ProjectionMatrix,
                Game1.Camera.ViewMatrix,
                Matrix.Identity
                );

            float distance1 = Vector3.Distance(corner1, corner2);
            float distance2 = Vector3.Distance(corner3, corner4);

            float maxDistance = distance1 > distance2 ? distance1 : distance2;

            float newWidth = scale * maxDistance;
            float newHeight = _aspectRatio * newWidth;

            _dimensions.X = newWidth;
            _dimensions.Y = newHeight;
        }

        public void Update(GameTime gameTime)
        {
            this.CalculateViewportPosition();
            
            _centerX = (int)(Math.Round(_viewportPosition.X) - (_dimensions.X / 2.0f) + Offset.X);
            _centerY = (int)(Math.Round(_viewportPosition.Y) - (_dimensions.Y / 2.0f) + Offset.Y);
        }

        public void Draw(GameTime gameTime)
        {
            if (_drawCount == 0) {
                _spriteBatch.Begin();
            }
            _drawCount++;


            _spriteBatch.Draw(
                _texture, 
                new Rectangle(_centerX, _centerY, (int)_dimensions.X, (int)_dimensions.Y),
                Color.White
                );

            if (_drawCount == _count) {
                _spriteBatch.End();
                _spriteBatch.ResetFor3d();
                _drawCount = 0;
            }
        }
    }
}
