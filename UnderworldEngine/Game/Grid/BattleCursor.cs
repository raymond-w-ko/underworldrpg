using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UnderworldEngine.Game
{
    public class BattleCursor
    {
        private Grid _grid;

        private int _xLimit;
        private int _zLimit;

        private float _movementSpeed;
        /// <summary>
        /// Battle cursor movement speed is in terms of Direct X units per second.
        /// </summary>
        public float MovementSpeed
        {
            get
            {
                return _movementSpeed;
            }
            set
            {
                _movementSpeed = (value / 60.0f);
            }
        }

        private QuadTexture _cursor;
        private QuadTexture _overlay;

        float _timer;
        float _minAlpha;
        float _maxAlpha;
        float _direction;

        public BattleCursor(Grid grid,
            string cursorTextureName, string gridOverlayTextureName)
        {
            _grid = grid;
            Vector2 dimensions = _grid.Dimensions;
            _xLimit = (int)dimensions.X;
            _zLimit = (int)dimensions.Y;

            // We want camera to lag behind a little bit, similar to Disgaea
            MovementSpeed = Game1.Camera.MoveSpeed * 1.10f;

            _cursor = new QuadTexture(
                new Vector3(0, 0, 0),
                Vector3.Up,
                Vector3.Forward,
                1.0f,
                1.0f,
                cursorTextureName
                );

            _overlay = new QuadTexture(
                new Vector3(6.5f, .01f, 3.5f),
                Vector3.Up,
                Vector3.Forward,
                1.0f, 1.0f,
                gridOverlayTextureName
                );
            _overlay.EnableAlphaBlending(
                BlendFunction.Add,
                Blend.SourceAlpha,
                Blend.One
                );
            _overlay.Alpha = 1.0f;

            _timer = 0;
            _minAlpha = .2f;
            _maxAlpha = .6f;
            _direction = +1f;
        }

        private void tick()
        {
            if (_direction > 0) {
                if (_timer > 30) {
                    _direction = -1f;
                }
                else {
                    _timer++;
                }
            }
            else if (_direction < 0) {
                if (_timer < 0) {
                    _direction = 1f;
                }
                else {
                    _timer--;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            tick();
            _overlay.Alpha = ((_maxAlpha - _minAlpha) * (float)(_timer / 30.0f)) + _minAlpha;
        }

        public void Draw(GameTime gameTime)
        {
            _overlay.Draw(gameTime);
        }

        public void Unload()
        {
            ;
        }
    }
}
