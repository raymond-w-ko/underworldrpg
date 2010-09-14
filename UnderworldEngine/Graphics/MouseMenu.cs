using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace UnderworldEngine.Graphics
{
    class MouseMenu
    {
        private MouseState _mouseStateCurrent;
        private MouseState _mouseStatePrevious;
        private bool _isVisible;

        private bool _isLeftClicking;
        private Vector2 _clickPosition;

        public MouseMenu()
        {
            _mouseStateCurrent = _mouseStatePrevious = Mouse.GetState();
            _isVisible = false;
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            MouseState _mouseStateCurrent = Mouse.GetState();
            // Move the sprite to the current mouse position when the left button is pressed
            if (_mouseStateCurrent.LeftButton == ButtonState.Pressed &&
                _mouseStatePrevious.LeftButton == ButtonState.Released) {
                _clickPosition = new Vector2(_mouseStateCurrent.X, _mouseStateCurrent.Y);
            }

            // Change the horizontal direction of the sprite when the right mouse button is clicked
            if (_mouseStateCurrent.RightButton == ButtonState.Pressed &&
                _mouseStatePrevious.RightButton == ButtonState.Released) {
                _isVisible = !_isVisible;
            }     

            _mouseStatePrevious = _mouseStateCurrent;

            if (!_isVisible) {
                return;
            }
        }

        public void Draw()
        {
            if (!_isVisible) {
                return;
            }
        }
    }
}
