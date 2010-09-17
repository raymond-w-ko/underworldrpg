using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UnderworldEngine.Graphics
{
    class MouseMenu : Menu
    {
        private MouseState _mouseStateCurrent;
        private MouseState _mouseStatePrevious;

        private bool _isLeftClicking;
        private Vector2 _clickPosition;

        public MouseMenu(string fontName,
            Color defaultTextColor,
            Color backgroundColor, float backgroundAlpha,
            Color borderColor,
            float width, float height)
            : base(fontName, defaultTextColor,
                backgroundColor, backgroundAlpha,
                borderColor,
                width, height,
                0, 0)
        {
            _mouseStateCurrent = _mouseStatePrevious = Mouse.GetState();
            IsVisible = false;
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            MouseState _mouseStateCurrent = Mouse.GetState();
            // Move the sprite to the current mouse position when the left button is pressed
            if (_mouseStateCurrent.LeftButton == ButtonState.Pressed &&
                _mouseStatePrevious.LeftButton == ButtonState.Released) {
                _isLeftClicking = true;
                _clickPosition = new Vector2(_mouseStateCurrent.X, _mouseStateCurrent.Y);
            }

            // Change the horizontal direction of the sprite when the right mouse button is clicked
            if (_mouseStateCurrent.RightButton == ButtonState.Pressed &&
                _mouseStatePrevious.RightButton == ButtonState.Released) {
                IsVisible = !IsVisible;
                _clickPosition = new Vector2(_mouseStateCurrent.X, _mouseStateCurrent.Y);
                _position = _clickPosition;
            }

            _mouseStatePrevious = _mouseStateCurrent;

            if (!IsVisible) {
                return;
            }
        }

        public override void Draw()
        {
            if (!IsVisible) {
                return;
            }

            base.Draw();
        }
    }
}
