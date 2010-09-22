using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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

        public BattleCursor(Grid grid,
            string cursorTextureName, string gridOverlayTextureName)
        {
            _grid = grid;
            Vector2 dimensions = _grid.Dimensions;
            _xLimit = (int)dimensions.X;
            _zLimit = (int)dimensions.Y;

            // We want camera to lag behind a little bit, similar to Disgaea
            MovementSpeed = Game1.Camera.MoveSpeed * 1.10f;
        }

        public void Update(GameTime gameTime)
        {
            ;
        }

        public void Draw(GameTime gameTime)
        {
            ;
        }
    }
}
